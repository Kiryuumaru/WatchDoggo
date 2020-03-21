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

            #endregion

            #region Properties

            private readonly List<Quote> quotes = new List<Quote>();
            private readonly List<Quote> initQuotes = new List<Quote>();
            private ITradingInstanceUI tradingInstanceUI;
            private string tickStreamId = "";
            private bool isTickHistoryInitialized = false;
            private bool isTickInitialized = false;

            public readonly ActiveSymbol ActiveSymbol;
            public List<Purchase> Purchases = new List<Purchase>();

            public int MarkCounter { get; private set; } = 0;

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
            public double Accuracy
            {
                get
                {
                    int win = Purchases.FindAll(w => w.PurchaseType == PurchaseType.Win).Count;
                    int loss = Purchases.FindAll(l => l.PurchaseType == PurchaseType.Lose).Count;
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

            #endregion

            #region NonVolatile Properties

            public int Size
            {
                get
                {
                    object blob = storage.GetValue(ActiveSymbol.Symbol + "_" + RegKey.LRSize);
                    if (blob == null) return 30;
                    return Convert.ToInt32(blob);
                }
                set
                {
                    storage.SetValue(ActiveSymbol.Symbol + "_" + RegKey.LRSize, value);
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
            public bool CutoffLoseEnable
            {
                get
                {
                    object blob = storage.GetValue(ActiveSymbol.Symbol + "_" + RegKey.CutoffLoseEnable);
                    if (blob == null) return false;
                    return blob.ToString().Equals("1");
                }
                set
                {
                    storage.SetValue(ActiveSymbol.Symbol + "_" + RegKey.CutoffLoseEnable, value ? "1" : "0");
                }
            }
            public bool CutoffWinEnable
            {
                get
                {
                    object blob = storage.GetValue(ActiveSymbol.Symbol + "_" + RegKey.CutoffWinEnable);
                    if (blob == null) return false;
                    return blob.ToString().Equals("1");
                }
                set
                {
                    storage.SetValue(ActiveSymbol.Symbol + "_" + RegKey.CutoffWinEnable, value ? "1" : "0");
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
                        SendMsg(new ForgetRequest()
                        {
                            Forget = tickStreamId
                        });
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

            private void LinearRegression(int count, int offset, out double rSquared, out double slope)
            {
                int[] xVals = new int[count];
                double[] yVals = new double[count];
                double[] yValsNorm = new double[count];

                for (int i = 0; i < xVals.Length; i++)
                {
                    xVals[i] = i;
                    yVals[i] = quotes[offset + i].Value;
                }
                for (int i = 0; i < xVals.Length; i++)
                {
                    xVals[i] = i;
                    yValsNorm[i] = (double)((yVals[i] - yVals.Min()) / (yVals.Max() - yVals.Min())) * 100;
                }

                int sumOfX = 0;
                int sumOfXSq = 0;
                double sumOfY = 0;
                double sumOfYSq = 0;
                double sumCodeviates = 0;

                for (var i = 0; i < xVals.Length; i++)
                {
                    int x = xVals[i];
                    double y = yValsNorm[i];
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

                rSquared = dblR * dblR * 100;
                slope = (count - 1) * (sCo / ssX);
            }

            private int ConvertToTicks(DurationUnit durationUnit, int value)
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

            #endregion

            #region Methods

            public void Start()
            {
                SendMsg(new TickRequest()
                {
                    Ticks = new List<string>() { ActiveSymbol.Symbol }
                });
            }

            public void Stop()
            {
                isTickInitialized = false;
                isTickHistoryInitialized = false;
                quotes.Clear();
            }

            public void SetTradingInstanceUI(ITradingInstanceUI instanceUI)
            {
                tradingInstanceUI = instanceUI;
            }

            public void Clear()
            {
                quotes.Clear();
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

            public void Mark(int ticksDuration)
            {
                MarkCounter = ticksDuration;
            }

            #endregion

            #region Methods

            public void Buy(ContractType contractType)
            {
                if (!isTickHistoryInitialized) return;

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
                    quotes.Add(new Quote(
                        obj.Request.Symbol,
                        obj.History.Prices[i],
                        obj.History.Times[i]));
                }
                for (int i = 0; i < initQuotes.Count; i++)
                {
                    if (!quotes.Any(t => t.DateTime.Equals(initQuotes[i].DateTime)))
                    {
                        quotes.Add(initQuotes[i]);
                    }
                }
                MarkCounter = 0;
                initQuotes.Clear();

                tradingInstanceUI?.OnQuoteHistoryReady(new QuoteHistoryEventArgs(quotes.ToArray()));
                isTickHistoryInitialized = true;
            }

            private void TradingInstance_OnTickResponse(TickResponse obj)
            {
                if (obj.Error != null) return;
                if (!obj.Tick.Symbol.Equals(ActiveSymbol.Symbol)) return;

                tickStreamId = obj.Tick.Id;

                Quote quote = new Quote(obj.Tick);

                if (isTickHistoryInitialized)
                {
                    tradingInstanceUI?.OnQuote(new QuoteEventArgs(quote));
                    if (quotes.Any(t => t.DateTime.Equals(quote.DateTime))) return;
                    quotes.Add(quote);
                    while (quotes.Count > Size)
                    {
                        quotes.RemoveAt(0);
                    }
                    if (MarkCounter > 0) MarkCounter--;
                }
                else
                {
                    initQuotes.Add(quote);
                }

                if (!isTickInitialized)
                {
                    isTickInitialized = true;
                    SendMsg(new TickHistoryRequest()
                    {
                        Symbol = ActiveSymbol.Symbol,
                        Count = Size
                    });
                }

                //if (SymbolQuotes == null) return;
                //if (tickStreamResponse.Error != null) return;
                //SymbolQuotes.AddQuote(new Quote(tickStreamResponse.Tick));

                //if (((CutoffLoseEnable && TotalSessionAmount - BuyAmount <= -Math.Abs(CutoffLoseAmount)) ||
                //    (CutoffWinEnable && TotalSessionAmount + BuyAmount >= Math.Abs(CutoffWinAmount))))
                //{
                //    onCutoff?.Invoke(new CutoffEventArgs(TotalSessionAmount + BuyAmount >= Math.Abs(CutoffWinAmount)));
                //}
                //decimal ongoingAmount = 0;
                //foreach (Purchase ongoing in Purchases.FindAll(p => p.SellTransaction == null))
                //{
                //    ongoingAmount += -ongoing.BuyTransaction.Amount;
                //}
                //if (!SymbolQuotes.Marked && Pinger.IsTradeSafe &&
                //    SymbolQuotes.Count >= LRTailSize + LRHeadSize &&
                //    Math.Abs(SymbolQuotes.TailR2) >= LRTailR2Barrier &&
                //    Math.Abs(SymbolQuotes.HeadR2) >= LRHeadR2Barrier &&
                //    Math.Abs(SymbolQuotes.HeadSlope) >= LRHeadSlopeBarrier &&
                //    Math.Abs(SymbolQuotes.TailSlope) >= LRTailSlopeBarrier &&
                //    (CutoffLoseEnable ? ((TotalSessionAmount - ongoingAmount - BuyAmount) > -Math.Abs(CutoffLoseAmount)) : true) &&
                //    (CutoffWinEnable ? ((TotalSessionAmount + ongoingAmount + BuyAmount) < Math.Abs(CutoffWinAmount)) : true) &&
                //    (AllowStraight ? true :
                //        (SymbolQuotes.TailSlope > 0 && SymbolQuotes.HeadSlope < 0) ||
                //        (SymbolQuotes.TailSlope < 0 && SymbolQuotes.HeadSlope > 0)))
                //{
                //    SymbolQuotes.Mark(ConvertToTicks(BuyDurationUnit, BuyDuration));
                //    SendMsg(new BuyRequest()
                //    {
                //        Price = BuyAmount,
                //        Parameters = new BuyParameters()
                //        {
                //            Amount = BuyAmount,
                //            Basis = ParameterBasis.Stake,
                //            ContractType = ReverseLogic ? (SymbolQuotes.HeadSlope > 0 ? ContractType.Put : ContractType.Call) : (SymbolQuotes.HeadSlope > 0 ? ContractType.Call : ContractType.Put),
                //            Currency = Current.Currency,
                //            Duration = BuyDuration,
                //            DurationUnit = BuyDurationUnit,
                //            Symbol = ActiveSymbol.Symbol
                //        }
                //    });
                //}
                //onQuote?.Invoke(new QuoteEventArgs(SymbolQuotes));
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
                }
            }

            #endregion
        }
    }
}
