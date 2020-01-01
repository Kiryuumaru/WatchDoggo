using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoggoWire.Services;

namespace DoggoWire.Models
{
    public enum Direction
    {
        Increase,
        Decrease,
        Equal
    }

    public class SymbolQuotes
    {
        private readonly List<Quote> quotes = new List<Quote>();

        public int TailSize { get; set; }
        public int HeadSize { get; set; }
        public double TailR2 { get; private set; } = 0;
        public double HeadR2 { get; private set; } = 0;
        public double TailSlope { get; private set; } = 0;
        public double HeadSlope { get; private set; } = 0;
        public double TailMaxSlope { get; private set; } = 0;
        public double HeadMaxSlope { get; private set; } = 0;
        public DateTime TailMaxSlopeDateTime { get; private set; } = DateTime.UtcNow;
        public DateTime HeadMaxSlopeDateTime { get; private set; } = DateTime.UtcNow;
        public int MarkCounter { get; private set; } = 0;
        public int Count
        {
            get { return quotes.Count; }
        }
        public bool Marked
        {
            get
            {
                return MarkCounter > 0;
            }
        }

        public ActiveSymbol ActiveSymbol { get; private set; }

        public SymbolQuotes(ActiveSymbol activeSymbol, int tailSize, int headSize)
        {
            ActiveSymbol = activeSymbol;
            TailSize = tailSize;
            HeadSize = headSize;
        }

        private void LinearRegression(int count, int offset, out double rSquared, out double slope)
        {
            int[] xVals = new int[count];
            decimal[] yVals = new decimal[count];
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

        public Quote this[int i]
        {
            get { return quotes[quotes.Count - i - 1]; }
        }

        public void Clear()
        {
            quotes.Clear();
        }

        public void Mark(int ticksDuration)
        {
            MarkCounter = ticksDuration;
        }

        public void AddQuote(Quote quote)
        {
            try
            {
                if (quotes.Any(t => t.DateTime.Equals(quote.DateTime))) return;
                quotes.Add(quote);
                while (quotes.Count > TailSize + HeadSize)
                {
                    quotes.RemoveAt(0);
                }
                if (MarkCounter > 0)
                {
                    MarkCounter--;
                }
                if (TailSize + HeadSize <= quotes.Count)
                {
                    double r2Tail = 0;
                    double r2Head = 0;
                    double sTail = 0;
                    double sHead = 0;

                    if (TailSize != 0) LinearRegression(TailSize, HeadSize, out r2Tail, out sTail);
                    if (HeadSize != 0) LinearRegression(HeadSize, 0, out r2Head, out sHead);

                    TailSlope = sTail;
                    TailR2 = r2Tail;
                    HeadSlope = sHead;
                    HeadR2 = r2Head;

                    if (TailMaxSlope <= Math.Abs(TailSlope))
                    {
                        TailMaxSlope = Math.Abs(TailSlope);
                        TailMaxSlopeDateTime = quote.DateTime;
                    }
                    if (HeadMaxSlope <= Math.Abs(HeadSlope))
                    {
                        HeadMaxSlope = Math.Abs(HeadSlope);
                        HeadMaxSlopeDateTime = quote.DateTime;
                    }
                }
            }
            catch { }
        }

        public List<Direction> GetDirections(int count)
        {
            List<Direction> directions = new List<Direction>();
            for (int i = 0; i < count - 1 && i < quotes.Count - 1; i++)
            {
                if (quotes[i + 1].Value < quotes[i].Value)
                {
                    directions.Add(Direction.Increase);
                }
                else if (quotes[i + 1].Value < quotes[i].Value)
                {
                    directions.Add(Direction.Decrease);
                }
                else
                {
                    directions.Add(Direction.Equal);
                }
            }
            return directions;
        }
    }
}
