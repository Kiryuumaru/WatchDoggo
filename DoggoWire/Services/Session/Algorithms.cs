using System;
using System.Collections.Generic;
using System.Text;

namespace DoggoWire.Services
{
    public static partial class Session
    {
        public static class Algorithm
        {

            private static void CalculateTickHistoryResponse()
            {
                //testBuys.Clear();
                //TestWins = 0;
                //TestLoses = 0;

                //List<TickHistoryResponse> responses = new List<TickHistoryResponse>(tickHistoryResponses);

                //foreach (SymbolQuotes symbolQuotes in SymbolsQuotes)
                //{
                //    symbolQuotes.Clear();
                //}

                //for (int i = 0; i < LRSize; i++)
                //{
                //    CalculatingProgress = i;
                //    foreach (TickHistoryResponse response in responses)
                //    {
                //        if (!Calculating)
                //        {
                //            foreach (SymbolQuotes interruptedSymbol in SymbolsQuotes)
                //            {
                //                interruptedSymbol.Clear();
                //            }
                //            return;
                //        }
                //        SymbolQuotes symbolQuotes = SymbolsQuotes.Find(t => t.ActiveSymbol.Symbol == response.Request.Symbol);
                //        if (symbolQuotes == null) continue;
                //        if (response.History.Prices.Length <= i) continue;
                //        Quote quote = new Quote(
                //            response.Request.Symbol,
                //            response.History.Prices[i],
                //            Helpers.ConvertEpoch(response.History.Times[i]));
                //        symbolQuotes.AddQuote(quote);
                //        if (symbolQuotes.Count >= LRTailSize + LRHeadSize)
                //        {
                //            TestBuy testBuy = testBuys.Find(b => b.Symbol.Equals(symbolQuotes.ActiveSymbol.Symbol));
                //            if (symbolQuotes.MarkCounter <= 1 && testBuy != null)
                //            {
                //                testBuys.Remove(testBuy);
                //                if (testBuy.ContractType.Equals("CALL"))
                //                {
                //                    if (testBuy.Quote < quote.Value) TestWins++;
                //                    else TestLoses++;
                //                }
                //                else
                //                {
                //                    if (testBuy.Quote > quote.Value) TestWins++;
                //                    else TestLoses++;
                //                }
                //            }
                //            if (!symbolQuotes.Marked &&
                //                symbolQuotes.Count >= LRTailSize + LRHeadSize &&
                //                Math.Abs(symbolQuotes.TailR2) >= LRTailR2Barrier &&
                //                Math.Abs(symbolQuotes.HeadR2) >= LRHeadR2Barrier &&
                //                Math.Abs(symbolQuotes.HeadSlope) >= LRHeadSlopeBarrier &&
                //                Math.Abs(symbolQuotes.TailSlope) >= LRTailSlopeBarrier &&
                //                (AllowStraight ? true :
                //                    (symbolQuotes.TailSlope > 0 && symbolQuotes.HeadSlope < 0) ||
                //                    (symbolQuotes.TailSlope < 0 && symbolQuotes.HeadSlope > 0)))
                //            {
                //                int durationInTicks = ConvertToTicks(BuyDurationUnit, BuyDuration);
                //                symbolQuotes.Mark(durationInTicks);
                //                testBuys.Add(new TestBuy(
                //                    symbolQuotes.ActiveSymbol.Symbol,
                //                    ReverseLogic ? (symbolQuotes.HeadSlope > 0 ? "PUT" : "CALL") : (symbolQuotes.HeadSlope > 0 ? "CALL" : "PUT"),
                //                    quote.Value,
                //                    durationInTicks));
                //            }
                //        }
                //        onQuote?.Invoke(new QuoteEventArgs(symbolQuotes));
                //    }
                //}
                //Calculating = false;
            }
        }
    }
}
