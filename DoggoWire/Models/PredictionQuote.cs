using DoggoWire.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoggoWire.Models
{
    public class PredictionQuote
    {
        public double Value { get; private set; }
        public long Epoch { get; private set; }
        public DateTime DateTime
        {
            get
            {
                return Helpers.ConvertEpoch(Epoch);
            }
        }
        public PredictionQuote(double value, long epoch)
        {
            Value = value;
            Epoch = epoch;
        }
    }
}
