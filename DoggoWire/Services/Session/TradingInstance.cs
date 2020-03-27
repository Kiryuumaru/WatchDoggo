using DoggoWire.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoggoWire.Services
{
    public static partial class Session
    {
        #region Properties

        private static event Action<TickHistoryResponse> OnTickHistoryResponse;
        private static event Action<TickResponse> OnTickResponse;
        private static event Action<BuyResponse> OnBuyResponse;
        private static event Action<TransactionResponse> OnTransactionResponse;

        #endregion

        public class TradingInstance : IDisposable
        {
            #region HelperClass

            public interface ITradingInstanceUI
            {
                void OnQuoteHistoryReady(QuoteHistoryEventArgs args);
                void OnQuote(QuoteEventArgs args);
                void OnTransaction(PurchaseEventArgs args);
                void OnCutoff(CutoffEventArgs args);
                void OnAutoBuyChanges(AutoBuyChangesArgs args);
            }

            public class PurchaseEventArgs : EventArgs
            {
                public Purchase Purchase { get; private set; }
                public PurchaseEventArgs(Purchase purchase)
                {
                    Purchase = purchase;
                }
            }

            public class QuoteHistoryEventArgs : EventArgs
            {
                public Quote[] Quotes { get; private set; }
                public QuoteHistoryEventArgs(Quote[] quotes)
                {
                    Quotes = quotes;
                }
            }

            public class QuoteEventArgs : EventArgs
            {
                public Quote Quote { get; private set; }
                public QuoteEventArgs(Quote quote)
                {
                    Quote = quote;
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

            public class AutoBuyChangesArgs : EventArgs
            {
                public bool AutoBuyEnabled { get; private set; }
                public AutoBuyChangesArgs(bool autoBuyEnabled)
                {
                    AutoBuyEnabled = autoBuyEnabled;
                }
            }

            #endregion

            #region Properties

            private readonly List<Transaction> transactionsToSeg = new List<Transaction>();
            private readonly List<Quote> quotes = new List<Quote>();
            private ITradingInstanceUI tradingInstanceUI;
            private string tickStreamId = "";
            private bool isTickHistoryInitialized = false;

            public readonly ActiveSymbol ActiveSymbol;
            public List<Purchase> Purchases = new List<Purchase>();
            public double LRSlope { get; private set; } = 0;
            public double LRR2 { get; private set; } = 0;
            public bool LRReady { get; private set; } = false;
            public int MarkCounter { get; private set; } = 0;

            private bool autoBuyEnable = false;
            public bool AutoBuyEnable
            {
                get
                {
                    return autoBuyEnable;
                }
                set
                {
                    autoBuyEnable = value;
                    tradingInstanceUI?.OnAutoBuyChanges(new AutoBuyChangesArgs(value));
                }
            }
            public Quote this[int i]
            {
                get
                {
                    return quotes[i];
                }
            }
            public int Count
            {
                get
                {
                    return quotes.Count;
                }
            }
            public bool Marked
            {
                get
                {
                    return MarkCounter > 0;
                }
            }
            public int WinCount
            {
                get
                {
                    return Purchases.FindAll(w => w.PurchaseType == PurchaseType.Win).Count;
                }
            }
            public int LoseCount
            {
                get
                {
                    return Purchases.FindAll(w => w.PurchaseType == PurchaseType.Lose).Count;
                }
            }
            public double Accuracy
            {
                get
                {
                    int win = WinCount;
                    int loss = LoseCount;
                    if (win + loss == 0) return 0;
                    return (double)win / (win + loss);
                }
            }
            public int PurchasesCount
            {
                get
                {
                    return Purchases.FindAll(w => w.PurchaseType == PurchaseType.Win).Count +
                        Purchases.FindAll(l => l.PurchaseType == PurchaseType.Lose).Count;
                }
            }
            public decimal TotalSessionAmount
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
            public int TickDuration
            {
                get
                {
                    if (quotes.Count > 1) return (int)(quotes[1].Epoch - quotes[0].Epoch);
                    else return -1;
                }
            }
            public long LastEpoch
            {
                get
                {
                    if (quotes.Count > 0) return quotes[quotes.Count - 1].Epoch;
                    else return -1;
                }
            }
            public long EpochToPredict
            {
                get
                {
                    return LastEpoch + (ConvertToTicks(BuyDurationUnit, BuyDuration) * TickDuration);
                }
            }

            #endregion

            #region NonVolatile Properties

            public int Size
            {
                get
                {
                    object blob = storage.GetValue(ActiveSymbol.Symbol + "_" + RegKey.Size);
                    if (blob == null) return 120;
                    return Convert.ToInt32(blob);
                }
                set
                {
                    storage.SetValue(ActiveSymbol.Symbol + "_" + RegKey.Size, value);
                }
            }
            public decimal BuyAmount
            {
                get
                {
                    object blob = storage.GetValue(ActiveSymbol.Symbol + "_" + RegKey.BuyAmount);
                    if (blob == null) return 1;
                    return Convert.ToDecimal(blob);
                }
                set
                {
                    storage.SetValue(ActiveSymbol.Symbol + "_" + RegKey.BuyAmount, value);
                }
            }
            public int BuyDuration
            {
                get
                {
                    object blob = storage.GetValue(ActiveSymbol.Symbol + "_" + RegKey.BuyDuration);
                    if (blob == null) return 5;
                    return Convert.ToInt32(blob);
                }
                set
                {
                    storage.SetValue(ActiveSymbol.Symbol + "_" + RegKey.BuyDuration, value);
                }
            }
            public DurationUnit BuyDurationUnit
            {
                get
                {
                    object blob = storage.GetValue(ActiveSymbol.Symbol + "_" + RegKey.BuyDurationUnit);
                    if (blob == null) return DurationUnit.Ticks;
                    return (blob.ToString()) switch
                    {
                        "t" => DurationUnit.Ticks,
                        "s" => DurationUnit.Seconds,
                        "m" => DurationUnit.Minutes,
                        "h" => DurationUnit.Hours,
                        "d" => DurationUnit.Days,
                        _ => DurationUnit.Ticks,
                    };
                }
                set
                {
                    switch (value)
                    {
                        case DurationUnit.Ticks:
                            storage.SetValue(ActiveSymbol.Symbol + "_" + RegKey.BuyDurationUnit, "t");
                            break;
                        case DurationUnit.Seconds:
                            storage.SetValue(ActiveSymbol.Symbol + "_" + RegKey.BuyDurationUnit, "s");
                            break;
                        case DurationUnit.Minutes:
                            storage.SetValue(ActiveSymbol.Symbol + "_" + RegKey.BuyDurationUnit, "m");
                            break;
                        case DurationUnit.Hours:
                            storage.SetValue(ActiveSymbol.Symbol + "_" + RegKey.BuyDurationUnit, "h");
                            break;
                        case DurationUnit.Days:
                            storage.SetValue(ActiveSymbol.Symbol + "_" + RegKey.BuyDurationUnit, "d");
                            break;
                    }
                }
            }
            public double AutoBuySlopeBarrier
            {
                get
                {
                    object blob = storage.GetValue(ActiveSymbol.Symbol + "_" + RegKey.LRSlopeBarrier);
                    if (blob == null) return 999;
                    return Convert.ToDouble(blob);
                }
                set
                {
                    storage.SetValue(ActiveSymbol.Symbol + "_" + RegKey.LRSlopeBarrier, value);
                }
            }
            public double AutoBuyR2Barrier
            {
                get
                {
                    object blob = storage.GetValue(ActiveSymbol.Symbol + "_" + RegKey.LRR2Barrier);
                    if (blob == null) return 999;
                    return Convert.ToDouble(blob);
                }
                set
                {
                    storage.SetValue(ActiveSymbol.Symbol + "_" + RegKey.LRR2Barrier, value);
                }
            }
            public bool CutoffEnable
            {
                get
                {
                    object blob = storage.GetValue(ActiveSymbol.Symbol + "_" + RegKey.CutoffEnable);
                    if (blob == null) return false;
                    return blob.ToString().Equals("1");
                }
                set
                {
                    storage.SetValue(ActiveSymbol.Symbol + "_" + RegKey.CutoffEnable, value ? "1" : "0");
                }
            }
            public decimal CutoffWinAmount
            {
                get
                {
                    object blob = storage.GetValue(ActiveSymbol.Symbol + "_" + RegKey.CutoffWinAmount);
                    if (blob == null) return 0;
                    return Convert.ToDecimal(blob);
                }
                set
                {
                    storage.SetValue(ActiveSymbol.Symbol + "_" + RegKey.CutoffWinAmount, value);
                }
            }
            public decimal CutoffLoseAmount
            {
                get
                {
                    object blob = storage.GetValue(ActiveSymbol.Symbol + "_" + RegKey.CutoffLoseAmount);
                    if (blob == null) return 0;
                    return Convert.ToDecimal(blob);
                }
                set
                {
                    storage.SetValue(ActiveSymbol.Symbol + "_" + RegKey.CutoffLoseAmount, value);
                }
            }
            public int LRSize
            {
                get
                {
                    object blob = storage.GetValue(ActiveSymbol.Symbol + "_" + RegKey.LRSize);
                    if (blob == null) return 5;
                    return Convert.ToInt32(blob);
                }
                set
                {
                    storage.SetValue(ActiveSymbol.Symbol + "_" + RegKey.LRSize, value);
                }
            }
            public bool ReverseLogic
            {
                get
                {
                    object blob = storage.GetValue(ActiveSymbol.Symbol + "_" + RegKey.ReverseLogic);
                    if (blob == null) return false;
                    return blob.ToString().Equals("1");
                }
                set
                {
                    storage.SetValue(ActiveSymbol.Symbol + "_" + RegKey.ReverseLogic, value ? "1" : "0");
                }
            }
            public bool AllowStraight
            {
                get
                {
                    object blob = storage.GetValue(ActiveSymbol.Symbol + "_" + RegKey.AllowStraight);
                    if (blob == null) return false;
                    return blob.ToString().Equals("1");
                }
                set
                {
                    storage.SetValue(ActiveSymbol.Symbol + "_" + RegKey.AllowStraight, value ? "1" : "0");
                }
            }

            #endregion

            #region Initializers

            public TradingInstance(ActiveSymbol activeSymbol)
            {
                ActiveSymbol = activeSymbol;
                OnTickHistoryResponse += TradingInstance_OnTickHistoryResponse;
                OnTickResponse += TradingInstance_OnTickResponse;
                OnBuyResponse += TradingInstance_OnBuyResponse;
                OnTransactionResponse += TradingInstance_OnTransactionResponse;
            }

            public void Dispose()
            {
                if (!string.IsNullOrEmpty(tickStreamId))
                {
                    Task.Run(delegate
                    {
                        SendMsg(new ForgetRequest { Forget = tickStreamId });
                    });
                }
                Current.OpenTradingSymbols.Remove(ActiveSymbol.Symbol);
                OnTickHistoryResponse -= TradingInstance_OnTickHistoryResponse;
                OnTickResponse -= TradingInstance_OnTickResponse;
                OnBuyResponse -= TradingInstance_OnBuyResponse;
                OnTransactionResponse -= TradingInstance_OnTransactionResponse;
            }

            #endregion

            #region Helpers

            public void LinearRegression(int count, int offset, out double rSquared, out double slope)
            {
                int[] xVals = new int[count];
                double[] yVals = new double[count];

                for (int i = 0; i < xVals.Length; i++)
                {
                    xVals[i] = i;
                    yVals[i] = quotes[quotes.Count - count + i - offset].Value;
                }

                int sumOfX = 0;
                int sumOfXSq = 0;
                double sumOfY = 0;
                double sumOfYSq = 0;
                double sumCodeviates = 0;

                for (var i = 0; i < xVals.Length; i++)
                {
                    int x = xVals[i];
                    double y = yVals[i];
                    sumCodeviates += x * y;
                    sumOfX += x;
                    sumOfY += y;
                    sumOfXSq += x * x;
                    sumOfYSq += y * y;
                }

                int ssX = sumOfXSq - ((sumOfX * sumOfX) / count);
                var sCo = sumCodeviates - ((sumOfX * sumOfY) / count);

                var rNumerator = (count * sumCodeviates) - (sumOfX * sumOfY);
                var rDenom = (count * sumOfXSq - (sumOfX * sumOfX)) * (count * sumOfYSq - (sumOfY * sumOfY));

                var dblR = rNumerator / Math.Sqrt(rDenom);

                rSquared = dblR * dblR;
                slope = sCo / ssX;
            }

            public int ConvertToTicks(DurationUnit durationUnit, int value)
            {
                if (quotes.Count > 1)
                {
                    int diff = (int)(quotes[1].Epoch - quotes[0].Epoch);
                    return durationUnit switch
                    {
                        DurationUnit.Seconds => value / diff,
                        DurationUnit.Minutes => value * (60 / diff),
                        DurationUnit.Hours => value * (3600 / diff),
                        DurationUnit.Days => value * (86400 / diff),
                        _ => value,
                    };
                }
                else return -1;
            }

            #endregion

            #region Methods

            public void Start()
            {
                SendMsg(new TickRequest()
                {
                    Ticks = new List<string>() { ActiveSymbol.Symbol }
                });
                AutoBuyEnable = false;
            }

            public void Stop()
            {
                isTickHistoryInitialized = false;
                quotes.Clear();
            }

            public void SetTradingInstanceUI(ITradingInstanceUI instanceUI)
            {
                tradingInstanceUI = instanceUI;
            }

            public double MinValue()
            {
                return quotes.Min(q => q.Value);
            }

            public double MaxValue()
            {
                return quotes.Max(q => q.Value);
            }

            public DateTime MinDateTime()
            {
                return quotes.Min(q => q.DateTime);
            }

            public DateTime MaxDateTime()
            {
                return quotes.Max(q => q.DateTime);
            }

            public Quote First()
            {
                return quotes.First();
            }

            public Quote Last()
            {
                return quotes.Last();
            }

            public void Mark(int ticksDuration)
            {
                MarkCounter = ticksDuration;
            }

            private void AddQuote(Quote quote)
            {
                if (quotes.Any(item => item.Epoch == quote.Epoch)) return;
                Quote quoteBefore = quotes.LastOrDefault(item => item.Epoch < quote.Epoch);
                if (quotes.Contains(quoteBefore))
                {
                    quotes.Insert(quotes.IndexOf(quoteBefore) + 1, quote);
                }
                else
                {
                    quotes.Add(quote);
                }
                while (quotes.Count > Size) quotes.RemoveAt(0);
            }

            #endregion

            #region Methods

            public void Buy(ContractType contractType)
            {
                if (!isTickHistoryInitialized) return;
                Mark(ConvertToTicks(BuyDurationUnit, BuyDuration) + 2);
                SendMsg(new BuyRequest()
                {
                    Price = BuyAmount,
                    Subscribe = true,
                    Parameters = new BuyParameters()
                    {
                        Amount = BuyAmount,
                        Basis = ParameterBasis.Stake,
                        ContractType = contractType,
                        Currency = Current.Currency,
                        Duration = BuyDuration,
                        DurationUnit = BuyDurationUnit,
                        Symbol = ActiveSymbol.Symbol
                    }
                });
            }

            #endregion

            #region Responses

            private void TradingInstance_OnTickHistoryResponse(TickHistoryResponse obj)
            {
                if (obj.Error != null) return;
                if (!obj.Request.Symbol.Equals(ActiveSymbol.Symbol)) return;

                for (int i = 0; i < obj.History.Prices.Length; i++)
                {
                    AddQuote(new Quote(
                        obj.Request.Symbol,
                        obj.History.Prices[i],
                        obj.History.Times[i]));
                }
                MarkCounter = 0;

                tradingInstanceUI?.OnQuoteHistoryReady(new QuoteHistoryEventArgs(quotes.ToArray()));
                isTickHistoryInitialized = true;
            }

            private void TradingInstance_OnTickResponse(TickResponse obj)
            {
                if (obj.Error != null) return;
                if (!obj.Tick.Symbol.Equals(ActiveSymbol.Symbol)) return;
                Current.KickTheTickDog();

                tickStreamId = obj.Tick.Id;

                Quote quote = new Quote(obj.Tick);

                if (isTickHistoryInitialized)
                {
                    AddQuote(quote);
                    if (MarkCounter > 0) MarkCounter--;
                    if (Count > LRSize)
                    {
                        LinearRegression(LRSize, 0, out double r, out double s);
                        LRR2 = r;
                        LRSlope = s;
                        if (AutoBuyEnable)
                        {
                            if (!CutoffEnable ||
                                (CutoffWinAmount > TotalSessionAmount &&
                                -CutoffLoseAmount < TotalSessionAmount))
                            {
                                if (!Marked &&
                                    r > AutoBuyR2Barrier &&
                                    Math.Abs(s) > AutoBuySlopeBarrier)
                                {
                                    if (s > 0) Buy(ContractType.Call);
                                    else Buy(ContractType.Put);
                                }
                            }
                            else
                            {
                                AutoBuyEnable = false;
                                tradingInstanceUI?.OnCutoff(new CutoffEventArgs(TotalSessionAmount > 0));
                            }
                        }
                        LRReady = true;
                    }
                    else
                    {
                        LRReady = false;
                    }
                    tradingInstanceUI?.OnQuote(new QuoteEventArgs(quote));
                }
                else
                {
                    AddQuote(quote);
                    SendMsg(new TickHistoryRequest()
                    {
                        Symbol = ActiveSymbol.Symbol,
                        Count = Size
                    });
                }
                SegregateTransactions();
            }

            private void TradingInstance_OnBuyResponse(BuyResponse obj)
            {
                if (obj.Error != null) return;
                if (!obj.Request.Parameters.Symbol.Equals(ActiveSymbol.Symbol)) return;
                Purchases.Add(new Purchase(ActiveSymbol, obj));
            }

            private void TradingInstance_OnTransactionResponse(TransactionResponse obj)
            {
                if (obj.Error != null) return;
                if (!obj.Transaction.Symbol.Equals(ActiveSymbol.Symbol)) return;
                Purchase purchase = Purchases.Find(item => item.BuyResponse.Buy.ContractId == obj.Transaction.ContractId);
                if (purchase != null)
                {
                    if (obj.Transaction.Action == TransactionAction.Buy)
                    {
                        purchase.BuyTransaction = obj.Transaction;
                        tradingInstanceUI?.OnTransaction(new PurchaseEventArgs(purchase));
                    }
                    else if (obj.Transaction.Action == TransactionAction.Sell)
                    {
                        purchase.SellTransaction = obj.Transaction;
                        tradingInstanceUI?.OnTransaction(new PurchaseEventArgs(purchase));
                    }
                    transactionsToSeg.Add(obj.Transaction);
                }
                SegregateTransactions();
            }

            private void SegregateTransactions()
            {
                List<Transaction> transactionsToRem = new List<Transaction>();
                foreach (Transaction transaction in transactionsToSeg)
                {
                    if (transaction.Action == TransactionAction.Sell)
                    {
                        Quote quote = quotes.LastOrDefault(item => item.Epoch == transaction.DateExpiry);
                        if (quote == null && quotes[quotes.Count - 1].Epoch == transaction.DateExpiry)
                        {
                            quote = quotes.LastOrDefault(item => item.Epoch == transaction.DateExpiry + 1);
                        }
                        if (quote != null)
                        {
                            quote.Transactions.Add(transaction);
                        }
                    }
                    else
                    {
                        Purchase purchase = Purchases.LastOrDefault(item => item.ContractId == transaction.ContractId);
                        if (purchase != null)
                        {
                            Quote quote = quotes.LastOrDefault(item => item.Epoch == purchase.BuyResponse.Buy.StartTime);
                            if (quote == null)
                            {
                                quote = quotes.LastOrDefault(item => item.Epoch == purchase.BuyResponse.Buy.StartTime + 1);
                            }
                            if (quote != null)
                            {
                                quote.Transactions.Add(transaction);
                            }
                        }
                        else
                        {
                            Quote quote = quotes.LastOrDefault(item => item.Epoch == transaction.TransactionTime);
                            if (quote == null)
                            {
                                quote = quotes.LastOrDefault(item => item.Epoch == transaction.TransactionTime + 1);
                            }
                            if (quote != null)
                            {
                                quote.Transactions.Add(transaction);
                            }
                        }
                    }
                    transactionsToRem.Add(transaction);
                }
                foreach (Transaction transaction in transactionsToRem) transactionsToSeg.Remove(transaction);
            }

            #endregion
        }
    }
}
