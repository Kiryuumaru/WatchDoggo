using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using DoggoWire.Models;
using WebSocketSharp.Net;

namespace DoggoWire.Services
{
    public static partial class Session
    {
        public static class Current
        {
            #region HelperClass

            public struct DurationUnit
            {
                public const string Ticks = "t";
                public const string Seconds = "s";
                public const string Minutes = "m";
                public const string Hours = "h";
                public const string Days = "d";
            }

            public class TestBuy
            {
                public string Symbol { get; private set; }
                public string ContractType { get; private set; }
                public decimal Quote { get; private set; }
                public int TickExpiry { get; private set; }
                public TestBuy(string symbol, string contractType, decimal quote, int tickExpiry)
                {
                    Symbol = symbol;
                    ContractType = contractType;
                    Quote = quote;
                    TickExpiry = tickExpiry;
                }
            }

            public class StateChangesEventArgs : EventArgs
            {
                public bool LoggedIn { get; private set; }
                public bool Reset { get; private set; }
                public StateChangesEventArgs(bool loggedIn, bool reset)
                {
                    LoggedIn = loggedIn;
                    Reset = reset;
                }
            }

            public class BalanceChangesEventArgs : EventArgs
            {
                public decimal Balance { get; private set; }
                public string Currency { get; private set; }
                public BalanceChangesEventArgs(decimal balance, string currency)
                {
                    Balance = balance;
                    Currency = currency;
                }
            }

            public class ActiveSymbolAvaliableEventArgs : EventArgs
            {
                public List<ActiveSymbol> ActiveSymbols { get; private set; }
                public ActiveSymbolAvaliableEventArgs(List<ActiveSymbol> activeSymbols)
                {
                    ActiveSymbols = activeSymbols;
                }
            }

            public class TransactionEventArgs : EventArgs
            {
                public Transaction Transaction { get; private set; }
                public TransactionEventArgs(Transaction transaction)
                {
                    Transaction = transaction;
                }
            }

            public class PurchaseEventArgs : EventArgs
            {
                public Purchase Purchase { get; private set; }
                public PurchaseEventArgs(Purchase purchase)
                {
                    Purchase = purchase;
                }
            }

            public class QuoteEventArgs : EventArgs
            {
                public SymbolQuotes SymbolQuotes { get; private set; }
                public QuoteEventArgs(SymbolQuotes symbolQuotes)
                {
                    SymbolQuotes = symbolQuotes;
                }
            }

            public class CalculateEventArgs : EventArgs
            {
                public bool Calculating { get; private set; }
                public CalculateEventArgs(bool calculating)
                {
                    Calculating = calculating;
                }
            }

            public class CutoffEventArgs : EventArgs
            {
                public bool WasWin { get; private set; }
                public CutoffEventArgs(bool wasWin)
                {
                    WasWin = wasWin;
                }
            }

            #endregion

            #region Properties

            private static readonly Stopwatch tickResponseTimer = new Stopwatch();
            private static readonly List<TickHistoryResponse> tickHistoryResponses = new List<TickHistoryResponse>();
            private static readonly List<TestBuy> testBuys = new List<TestBuy>();

            private static Action<StateChangesEventArgs> onStateChanges;
            private static Action<BalanceChangesEventArgs> onBalanceChanges;
            private static Action<ActiveSymbolAvaliableEventArgs> onActiveSymbolAvailable;
            private static Action<TransactionEventArgs> onTransaction;
            private static Action<PurchaseEventArgs> onPurchase;
            private static Action<QuoteEventArgs> onQuote;
            private static Action<CalculateEventArgs> onCalculateStateChange;
            private static Action<CutoffEventArgs> onCutoff;

            public static List<Transaction> Transactions = new List<Transaction>();
            public static List<Purchase> Purchases = new List<Purchase>();
            public static List<SymbolQuotes> SymbolsQuotes = new List<SymbolQuotes>();

            private static bool symbolQuotesReady = false;
            private static bool started = false;
            public static bool Started
            {
                get
                {
                    return started;
                }
                set
                {
                    started = value;
                    onStateChanges?.Invoke(new StateChangesEventArgs(true, false));
                }
            }
            public static bool Calculating { get; set; }
            public static int CalculatingProgress { get; private set; }
            public static decimal Balance { get; private set; }
            public static string Country { get; private set; }
            public static string Currency { get; private set; }
            public static string Email { get; private set; }
            public static string Fullname { get; private set; }
            public static bool Virtual { get; private set; }
            public static string LandingCompanyFullname { get; private set; }
            public static string LandingCompanyName { get; private set; }
            public static string LoginId { get; private set; }
            public static int TestWins { get; private set; }
            public static int TestLoses { get; private set; }
            public static double ExpectedAccuracy
            {
                get
                {
                    if (TestWins + TestLoses == 0) return 0;
                    return (double)TestWins / (TestWins + TestLoses);
                }
            }
            public static int ExpectedPurchases
            {
                get
                {
                    return TestWins + TestLoses;
                }
            }
            public static bool Exist
            {
                get
                {
                    if (Accounts == null) return false;
                    object blob = storage.GetValue(RegKey.SelectedAuth);
                    if (blob == null) return false;
                    string value = (string)blob;
                    Account account = Array.Find(Accounts, acct => acct.Name == value);
                    if (account == null) return false;
                    return true;
                }
            }
            public static double Accuracy
            {
                get
                {
                    int win = Purchases.FindAll(w => w.PurchaseType == PurchaseType.Win).Count;
                    int loss = Purchases.FindAll(l => l.PurchaseType == PurchaseType.Lose).Count;
                    if (win + loss == 0) return 0;
                    return (double)win / (win + loss);
                }
            }
            public static int PurchasesCount
            {
                get
                {
                    return Purchases.FindAll(w => w.PurchaseType == PurchaseType.Win).Count +
                        Purchases.FindAll(l => l.PurchaseType == PurchaseType.Lose).Count;
                }
            }
            public static decimal TotalSessionAmount
            {
                get
                {
                    decimal amount = 0;
                    IReadOnlyList<Purchase> currentPurchases = new List<Purchase>(Purchases);
                    foreach (Purchase purchase in currentPurchases)
                    {
                        if (purchase.PurchaseType == PurchaseType.Ongoing) continue;
                        amount += purchase.Amount;
                    }
                    return amount;
                }
            }

            public static int LRSize
            {
                get
                {
                    object blob = storage.GetValue(RegKey.LRSize);
                    if (blob == null) return 150;
                    return Convert.ToInt32(blob);
                }
                set
                {
                    storage.SetValue(RegKey.LRSize, value);
                }
            }
            public static int LRTailSize
            {
                get
                {
                    object blob = storage.GetValue(RegKey.LRTailSize);
                    if (blob == null) return 5;
                    return Convert.ToInt32(blob);
                }
                set
                {
                    storage.SetValue(RegKey.LRTailSize, value);
                }
            }
            public static int LRHeadSize
            {
                get
                {
                    object blob = storage.GetValue(RegKey.LRHeadSize);
                    if (blob == null) return 5;
                    return Convert.ToInt32(blob);
                }
                set
                {
                    storage.SetValue(RegKey.LRHeadSize, value);
                }
            }
            public static double LRTailR2Barrier
            {
                get
                {
                    object blob = storage.GetValue(RegKey.LRTailR2Barrier);
                    if (blob == null) return 100;
                    return Convert.ToDouble(blob);
                }
                set
                {
                    storage.SetValue(RegKey.LRTailR2Barrier, value);
                }
            }
            public static double LRHeadR2Barrier
            {
                get
                {
                    object blob = storage.GetValue(RegKey.LRHeadR2Barrier);
                    if (blob == null) return 100;
                    return Convert.ToDouble(blob);
                }
                set
                {
                    storage.SetValue(RegKey.LRHeadR2Barrier, value);
                }
            }
            public static double LRTailSlopeBarrier
            {
                get
                {
                    object blob = storage.GetValue(RegKey.LRTailSlopeBarrier);
                    if (blob == null) return 100;
                    return Convert.ToDouble(blob);
                }
                set
                {
                    storage.SetValue(RegKey.LRTailSlopeBarrier, value);
                }
            }
            public static double LRHeadSlopeBarrier
            {
                get
                {
                    object blob = storage.GetValue(RegKey.LRHeadSlopeBarrier);
                    if (blob == null) return 100;
                    return Convert.ToDouble(blob);
                }
                set
                {
                    storage.SetValue(RegKey.LRHeadSlopeBarrier, value);
                }
            }
            public static decimal BuyAmount
            {
                get
                {
                    object blob = storage.GetValue(RegKey.BuyAmount);
                    if (blob == null) return 1;
                    return Convert.ToDecimal(blob);
                }
                set
                {
                    storage.SetValue(RegKey.BuyAmount, value);
                }
            }
            public static int BuyDuration
            {
                get
                {
                    object blob = storage.GetValue(RegKey.BuyDuration);
                    if (blob == null) return 5;
                    return Convert.ToInt32(blob);
                }
                set
                {
                    storage.SetValue(RegKey.BuyDuration, value);
                }
            }
            public static string BuyDurationUnit
            {
                get
                {
                    object blob = storage.GetValue(RegKey.BuyDurationUnit);
                    if (blob == null) return "t";
                    return blob.ToString();
                }
                set
                {
                    storage.SetValue(RegKey.BuyDurationUnit, value);
                }
            }
            public static decimal CutoffLoseAmount
            {
                get
                {
                    object blob = storage.GetValue(RegKey.CutoffLoseAmount);
                    if (blob == null) return 0;
                    return Convert.ToDecimal(blob);
                }
                set
                {
                    storage.SetValue(RegKey.CutoffLoseAmount, value);
                }
            }
            public static decimal CutoffWinAmount
            {
                get
                {
                    object blob = storage.GetValue(RegKey.CutoffWinAmount);
                    if (blob == null) return 0;
                    return Convert.ToDecimal(blob);
                }
                set
                {
                    storage.SetValue(RegKey.CutoffWinAmount, value);
                }
            }
            public static bool ReverseLogic
            {
                get
                {
                    object blob = storage.GetValue(RegKey.ReverseLogic);
                    if (blob == null) return false;
                    return blob.ToString().Equals("1");
                }
                set
                {
                    storage.SetValue(RegKey.ReverseLogic, value ? "1" : "0");
                }
            }
            public static bool AllowStraight
            {
                get
                {
                    object blob = storage.GetValue(RegKey.AllowStraight);
                    if (blob == null) return false;
                    return blob.ToString().Equals("1");
                }
                set
                {
                    storage.SetValue(RegKey.AllowStraight, value ? "1" : "0");
                }
            }
            public static bool CutoffLoseEnable
            {
                get
                {
                    object blob = storage.GetValue(RegKey.CutoffLoseEnable);
                    if (blob == null) return false;
                    return blob.ToString().Equals("1");
                }
                set
                {
                    storage.SetValue(RegKey.CutoffLoseEnable, value ? "1" : "0");
                }
            }
            public static bool CutoffWinEnable
            {
                get
                {
                    object blob = storage.GetValue(RegKey.CutoffWinEnable);
                    if (blob == null) return false;
                    return blob.ToString().Equals("1");
                }
                set
                {
                    storage.SetValue(RegKey.CutoffWinEnable, value ? "1" : "0");
                }
            }
            public static long LastTickResponse
            {
                get
                {
                    return tickResponseTimer.ElapsedMilliseconds;
                }
            }

            #endregion

            #region Setters

            public static void Set(Account account)
            {
                if (account == null) return;
                if (Array.Find(Accounts, acct => acct.Name == account.Name) != null)
                {
                    storage.SetValue(RegKey.SelectedAuth, account.Name);
                }
            }

            public static void SetOnStateChanges(Action<StateChangesEventArgs> eventHandler)
            {
                onStateChanges = eventHandler;
            }

            public static void SetOnBalanceChanges(Action<BalanceChangesEventArgs> eventHandler)
            {
                onBalanceChanges = eventHandler;
            }

            public static void SetOnActiveSymbolAvailable(Action<ActiveSymbolAvaliableEventArgs> eventHandler)
            {
                onActiveSymbolAvailable = eventHandler;
            }

            public static void SetOnTransaction(Action<TransactionEventArgs> eventHandler)
            {
                onTransaction = eventHandler;
            }

            public static void SetOnPurchase(Action<PurchaseEventArgs> eventHandler)
            {
                onPurchase = eventHandler;
            }

            public static void SetOnQuote(Action<QuoteEventArgs> eventHandler)
            {
                onQuote = eventHandler;
            }

            public static void SetOnCalculateStateChange(Action<CalculateEventArgs> eventHandler)
            {
                onCalculateStateChange = eventHandler;
            }

            public static void SetOnCutoff(Action<CutoffEventArgs> eventHandler)
            {
                onCutoff = eventHandler;
            }

            #endregion

            #region Methods

            private static int ConvertToTicks(string durationUnit, int value)
            {
                switch (durationUnit)
                {
                    case DurationUnit.Seconds:
                        return BuyDuration / 2;
                    case DurationUnit.Minutes:
                        return BuyDuration * 30;
                    case DurationUnit.Hours:
                        return BuyDuration * 1800;
                    case DurationUnit.Days:
                        return BuyDuration * 43200;
                    default:
                        return value;
                }
            }

            public static void Initialize()
            {
                if (Accounts == null) return;
                object blob = storage.GetValue(RegKey.SelectedAuth);
                if (blob == null) return;
                string value = (string)blob;
                Account account = Array.Find(Accounts, acct => acct.Name == value);
                if (account == null) return;
                symbolQuotesReady = false;
                clearCurrentSession = true;
                Calculating = true;
                onCalculateStateChange?.Invoke(new CalculateEventArgs(true));
                SendMsg(new AuthorizeRequest() { AuthorizeToken = account.Token });
            }

            public static void Reset()
            {
                if (Accounts == null) return;
                object blob = storage.GetValue(RegKey.SelectedAuth);
                if (blob == null) return;
                string value = (string)blob;
                Account account = Array.Find(Accounts, acct => acct.Name == value);
                if (account == null) return;
                symbolQuotesReady = false;
                clearCurrentSession = true;
                Calculating = false;
                onCalculateStateChange?.Invoke(new CalculateEventArgs(false));
                SendMsg(new AuthorizeRequest() { AuthorizeToken = account.Token });
            }

            public static void Refresh()
            {
                if (Accounts == null) return;
                object blob = storage.GetValue(RegKey.SelectedAuth);
                if (blob == null) return;
                string value = (string)blob;
                Account account = Array.Find(Accounts, acct => acct.Name == value);
                if (account == null) return;
                symbolQuotesReady = false;
                clearCurrentSession = false;
                Calculating = false;
                onCalculateStateChange?.Invoke(new CalculateEventArgs(false));
                SendMsg(new AuthorizeRequest() { AuthorizeToken = account.Token });
            }

            public static void Calculate()
            {
                if (Accounts == null) return;
                if (Calculating) return;
                clearCurrentSession = false;
                Calculating = true;
                onCalculateStateChange?.Invoke(new CalculateEventArgs(true));
                SendMsg(new ActiveSymbolsRequest());
            }

            #endregion

            #region Responses

            public static void AuthorizeResponse(AuthorizeResponse authorizeResponse)
            {
                if (authorizeResponse.Error == null)
                {
                    if (clearCurrentSession)
                    {
                        started = false;
                        Transactions.Clear();
                        Purchases.Clear();
                        SymbolsQuotes.Clear();
                    }

                    Balance = authorizeResponse.Authorize.Balance;
                    Country = authorizeResponse.Authorize.Country;
                    Currency = authorizeResponse.Authorize.Currency;
                    Email = authorizeResponse.Authorize.Email;
                    Fullname = authorizeResponse.Authorize.Fullname;
                    Virtual = authorizeResponse.Authorize.IsVirtual;
                    LandingCompanyFullname = authorizeResponse.Authorize.LandingCompanyFullname;
                    LandingCompanyName = authorizeResponse.Authorize.LandingCompanyName;
                    LoginId = authorizeResponse.Authorize.LoginId;

                    SendMsg(new BalanceRequest());
                    SendMsg(new TransactionRequest());
                    SendMsg(new ActiveSymbolsRequest());

                    onStateChanges?.Invoke(new StateChangesEventArgs(true, clearCurrentSession));
                    onBalanceChanges?.Invoke(new BalanceChangesEventArgs(authorizeResponse.Authorize.Balance, authorizeResponse.Authorize.Currency));
                }
                else
                {
                    onStateChanges?.Invoke(new StateChangesEventArgs(false, clearCurrentSession));
                }
            }

            public static void BalanceResponse(BalanceResponse balanceStreamResponse)
            {
                if (balanceStreamResponse.Error == null)
                {
                    Balance = balanceStreamResponse.Balance.Amount;

                    onBalanceChanges?.Invoke(new BalanceChangesEventArgs(balanceStreamResponse.Balance.Amount, balanceStreamResponse.Balance.Currency));
                }
                else
                {
                    if (!balanceStreamResponse.Error.Code.Equals("AlreadySubscribed"))
                    {
                        onBalanceChanges?.Invoke(new BalanceChangesEventArgs(Balance, Currency));
                    }
                }
            }

            public static void ActiveSymbolsResponse(ActiveSymbolsResponse activeSymbolsResponse)
            {
                if (activeSymbolsResponse.Error == null)
                {
                    tickHistoryResponses.Clear();
                    SymbolsQuotes.Clear();
                    List<string> symbols = new List<string>();
                    //List<ActiveSymbol> activeSymbols = activeSymbolsResponse.ActiveSymbols.FindAll(sym =>
                    //    sym.Market.Equals("synthetic_index") && sym.ExchangeIsOpen);
                    List<ActiveSymbol> activeSymbols = activeSymbolsResponse.ActiveSymbols.FindAll(sym =>
                        sym.Symbol.Equals("R_100") && sym.ExchangeIsOpen);
                    foreach (ActiveSymbol activeSymbol in activeSymbols)
                    {
                        SymbolsQuotes.Add(new SymbolQuotes(activeSymbol, LRTailSize, LRHeadSize));
                        symbols.Add(activeSymbol.Symbol);
                        if (Calculating)
                        {
                            SendMsg(new TickHistoryRequest()
                            {
                                Symbol = activeSymbol.Symbol,
                                Count = LRSize
                            });
                        }
                        else
                        {
                            SendMsg(new TickHistoryRequest()
                            {
                                Symbol = activeSymbol.Symbol,
                                Count = LRTailSize + LRHeadSize
                            });
                        }
                    }
                    onActiveSymbolAvailable?.Invoke(new ActiveSymbolAvaliableEventArgs(activeSymbols));
                }
                else
                {
                    onActiveSymbolAvailable?.Invoke(new ActiveSymbolAvaliableEventArgs(new List<ActiveSymbol>()));
                }
            }

            public static void TransactionResponse(TransactionResponse transactionStreamResponse)
            {
                if (transactionStreamResponse.Error == null && transactionStreamResponse.Transaction.Action != TransactionAction.Unknown)
                {
                    Purchase purchase = Purchases.Find(p => p.BuyResponse.Buy.ContractId == transactionStreamResponse.Transaction.ContractId);
                    SymbolQuotes symbolQuotes = SymbolsQuotes.Find(tick => tick.ActiveSymbol.Symbol.Equals(transactionStreamResponse.Transaction.Symbol));
                    Transactions.Add(transactionStreamResponse.Transaction);
                    if (transactionStreamResponse.Transaction.Action == TransactionAction.Sell)
                    {
                        if (purchase != null)
                        {
                            Purchases.Remove(purchase);
                            purchase = new Purchase(symbolQuotes?.ActiveSymbol, purchase.BuyResponse, purchase.BuyTransaction, transactionStreamResponse.Transaction);
                            Purchases.Add(purchase);
                            onPurchase?.Invoke(new PurchaseEventArgs(purchase));
                        }
                    }
                    else if (transactionStreamResponse.Transaction.Action == TransactionAction.Buy)
                    {
                        if (purchase != null)
                        {
                            Purchases.Remove(purchase);
                            purchase = new Purchase(symbolQuotes?.ActiveSymbol, purchase.BuyResponse, transactionStreamResponse.Transaction, purchase.SellTransaction);
                            Purchases.Add(purchase);
                            onPurchase?.Invoke(new PurchaseEventArgs(purchase));
                        }
                    }
                    onTransaction?.Invoke(new TransactionEventArgs(transactionStreamResponse.Transaction));
                }
            }

            public static void TickHistoryResponse(TickHistoryResponse tickHistoryResponse)
            {
                if (tickHistoryResponse.Error == null)
                {
                    if (Calculating)
                    {
                        tickHistoryResponses.Add(tickHistoryResponse);
                        bool isCalcReady = true;
                        foreach (SymbolQuotes symbolQuotes in SymbolsQuotes)
                        {
                            if (tickHistoryResponses.Find(r => r.Request.Symbol.Equals(symbolQuotes.ActiveSymbol.Symbol)) == null)
                            {
                                isCalcReady = false;
                            }
                        }
                        if (isCalcReady)
                        {
                            CalculateTickHistoryResponse();
                            List<string> symbols = new List<string>();
                            foreach (SymbolQuotes symbolQuotes in SymbolsQuotes)
                            {
                                SendMsg(new TickHistoryRequest()
                                {
                                    Symbol = symbolQuotes.ActiveSymbol.Symbol,
                                    Count = LRTailSize + LRHeadSize
                                });
                            }
                        }
                    }
                    else
                    {
                        SymbolQuotes symbolQuotes = SymbolsQuotes.Find(s => s.ActiveSymbol.Symbol.Equals(tickHistoryResponse.Request.Symbol));
                        if (symbolQuotes != null)
                        {
                            for (int i = 0; i < tickHistoryResponse.History.Prices.Length; i++)
                            {
                                symbolQuotes.AddQuote(new Quote(
                                    tickHistoryResponse.Request.Symbol,
                                    tickHistoryResponse.History.Prices[i],
                                    Helpers.ConvertEpoch(tickHistoryResponse.History.Times[i])));
                            }
                            symbolQuotes.Mark(0);
                            SendMsg(new TickRequest() { Ticks = new List<string>() { symbolQuotes.ActiveSymbol.Symbol } });
                        }
                        symbolQuotesReady = true;
                        onCalculateStateChange?.Invoke(new CalculateEventArgs(Calculating));
                    }
                }
            }

            private static void CalculateTickHistoryResponse()
            {
                testBuys.Clear();
                TestWins = 0;
                TestLoses = 0;

                List<TickHistoryResponse> responses = new List<TickHistoryResponse>(tickHistoryResponses);

                foreach (SymbolQuotes symbolQuotes in SymbolsQuotes)
                {
                    symbolQuotes.Clear();
                }

                for (int i = 0; i < LRSize; i++)
                {
                    CalculatingProgress = i;
                    foreach (TickHistoryResponse response in responses)
                    {
                        if (!Calculating)
                        {
                            foreach (SymbolQuotes interruptedSymbol in SymbolsQuotes)
                            {
                                interruptedSymbol.Clear();
                            }
                            return;
                        }
                        SymbolQuotes symbolQuotes = SymbolsQuotes.Find(t => t.ActiveSymbol.Symbol == response.Request.Symbol);
                        if (symbolQuotes == null) continue;
                        if (response.History.Prices.Length <= i) continue;
                        Quote quote = new Quote(
                            response.Request.Symbol,
                            response.History.Prices[i],
                            Helpers.ConvertEpoch(response.History.Times[i]));
                        symbolQuotes.AddQuote(quote);
                        if (symbolQuotes.Count >= LRTailSize + LRHeadSize)
                        {
                            TestBuy testBuy = testBuys.Find(b => b.Symbol.Equals(symbolQuotes.ActiveSymbol.Symbol));
                            if (symbolQuotes.MarkCounter <= 1 && testBuy != null)
                            {
                                testBuys.Remove(testBuy);
                                if (testBuy.ContractType.Equals("CALL"))
                                {
                                    if (testBuy.Quote < quote.Value) TestWins++;
                                    else TestLoses++;
                                }
                                else
                                {
                                    if (testBuy.Quote > quote.Value) TestWins++;
                                    else TestLoses++;
                                }
                            }
                            if (!symbolQuotes.Marked &&
                                symbolQuotes.Count >= LRTailSize + LRHeadSize &&
                                Math.Abs(symbolQuotes.TailR2) >= LRTailR2Barrier &&
                                Math.Abs(symbolQuotes.HeadR2) >= LRHeadR2Barrier &&
                                Math.Abs(symbolQuotes.HeadSlope) >= LRHeadSlopeBarrier &&
                                Math.Abs(symbolQuotes.TailSlope) >= LRTailSlopeBarrier &&
                                (AllowStraight ? true :
                                    (symbolQuotes.TailSlope > 0 && symbolQuotes.HeadSlope < 0) ||
                                    (symbolQuotes.TailSlope < 0 && symbolQuotes.HeadSlope > 0)))
                            {
                                int durationInTicks = ConvertToTicks(BuyDurationUnit, BuyDuration);
                                symbolQuotes.Mark(durationInTicks);
                                testBuys.Add(new TestBuy(
                                    symbolQuotes.ActiveSymbol.Symbol,
                                    ReverseLogic ? (symbolQuotes.HeadSlope > 0 ? "PUT" : "CALL") : (symbolQuotes.HeadSlope > 0 ? "CALL" : "PUT"),
                                    quote.Value,
                                    durationInTicks));
                            }
                        }
                        onQuote?.Invoke(new QuoteEventArgs(symbolQuotes));
                    }
                }
                Calculating = false;
            }

            public static void TickResponse(TickResponse tickStreamResponse)
            {
                tickResponseTimer.Restart();
                if (tickStreamResponse.Error != null || Calculating || !symbolQuotesReady) return;
                SymbolQuotes symbolQuotes = SymbolsQuotes.Find(t => t.ActiveSymbol.Symbol == tickStreamResponse.Tick.Symbol);
                if (symbolQuotes == null) return;
                symbolQuotes.AddQuote(new Quote(tickStreamResponse.Tick));

                if (Started && 
                    ((CutoffLoseEnable && TotalSessionAmount - BuyAmount <= -Math.Abs(CutoffLoseAmount)) ||
                    (CutoffWinEnable && TotalSessionAmount + BuyAmount >= Math.Abs(CutoffWinAmount))))
                {
                    Started = false;
                    onCutoff?.Invoke(new CutoffEventArgs(TotalSessionAmount + BuyAmount >= Math.Abs(CutoffWinAmount)));
                }
                decimal ongoingAmount = 0;
                foreach (Purchase ongoing in Purchases.FindAll(p => p.SellTransaction == null))
                {
                    ongoingAmount += -ongoing.BuyTransaction.Amount;
                }
                if (Started && !symbolQuotes.Marked && Pinger.IsTradeSafe &&
                    symbolQuotes.Count >= LRTailSize + LRHeadSize &&
                    Math.Abs(symbolQuotes.TailR2) >= LRTailR2Barrier &&
                    Math.Abs(symbolQuotes.HeadR2) >= LRHeadR2Barrier &&
                    Math.Abs(symbolQuotes.HeadSlope) >= LRHeadSlopeBarrier &&
                    Math.Abs(symbolQuotes.TailSlope) >= LRTailSlopeBarrier &&
                    (CutoffLoseEnable ? ((TotalSessionAmount - ongoingAmount - BuyAmount) > -Math.Abs(CutoffLoseAmount)) : true) &&
                    (CutoffWinEnable ? ((TotalSessionAmount + ongoingAmount + BuyAmount) < Math.Abs(CutoffWinAmount)) : true) &&
                    (AllowStraight ? true :
                        (symbolQuotes.TailSlope > 0 && symbolQuotes.HeadSlope < 0) ||
                        (symbolQuotes.TailSlope < 0 && symbolQuotes.HeadSlope > 0)))
                {
                    symbolQuotes.Mark(ConvertToTicks(BuyDurationUnit, BuyDuration));
                    SendMsg(new BuyRequest()
                    {
                        Price = BuyAmount,
                        Parameters = new BuyParameters()
                        {
                            Amount = BuyAmount,
                            Basis = ParameterBasis.Stake,
                            ContractType = ReverseLogic ? (symbolQuotes.HeadSlope > 0 ? ContractType.Put : ContractType.Call) : (symbolQuotes.HeadSlope > 0 ? ContractType.Call : ContractType.Put),
                            Currency = Currency,
                            Duration = BuyDuration,
                            DurationUnit = BuyDurationUnit,
                            Symbol = symbolQuotes.ActiveSymbol.Symbol
                        }
                    });
                }
                onQuote?.Invoke(new QuoteEventArgs(symbolQuotes));
            }

            public static void BuyResponse(BuyResponse buyResponse)
            {
                Purchase purchase = Purchases.Find(p => p.BuyResponse.Buy.ContractId.Equals(buyResponse.Buy.ContractId));
                if (purchase != null)
                {
                    Purchases.Remove(purchase);
                }
                SymbolQuotes symbolQuotes = SymbolsQuotes.Find(s => s.ActiveSymbol.Symbol.Equals(buyResponse.Request.Parameters.Symbol));
                purchase = new Purchase(symbolQuotes?.ActiveSymbol, buyResponse, purchase?.BuyTransaction, purchase?.SellTransaction);
                Purchases.Add(purchase);
                onPurchase?.Invoke(new PurchaseEventArgs(purchase));
            }

            #endregion
        }
    }
}
