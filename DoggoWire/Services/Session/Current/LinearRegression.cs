using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoggoWire.Services
{
    public class LinearRegression
    {
        #region s

        /// <summary>
        /// Fits a line to a collection of (x,y) points.
        /// </summary>
        /// <param name="xVals">The x-axis values.</param>
        /// <param name="yVals">The y-axis values.</param>
        /// <param name="rSquared">The r^2 value of the line.</param>
        /// <param name="yIntercept">The y-intercept value of the line (i.e. y = ax + b, yIntercept is b).</param>
        /// <param name="slope">The slop of the line (i.e. y = ax + b, slope is a).</param>
        public static void Solv(
            decimal[] yVals,
            out double rSquared,
            out double yIntercept,
            out double slope)
        {
            int[] xVals = new int[yVals.Length];
            double[] yValsNorm = new double[yVals.Length];
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

            int count = xVals.Length;
            int ssX = sumOfXSq - ((sumOfX * sumOfX) / count);

            double rNumerator = (count * sumCodeviates) - (sumOfX * sumOfY);
            double rDenom = (count * sumOfXSq - (sumOfX * sumOfX)) * (count * sumOfYSq - (sumOfY * sumOfY));
            var sCo = sumCodeviates - ((sumOfX * sumOfY) / count);

            double meanX = sumOfX / count;
            double meanY = sumOfY / count;
            double dblR = rNumerator / Math.Sqrt(rDenom);

            rSquared = dblR * dblR;
            yIntercept = meanY - (sCo / ssX * meanX);
            slope = sCo / ssX;
        }

        #endregion

        public IEnumerable<decimal> YValues { get; private set; }

        public LinearRegression(IEnumerable<decimal> yValues)
        {
            YValues = yValues;
        }

        public void Insert()
        {
        }
    }
}
