using DoggoWire.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoggoWire.Models
{
    public class Quote
    {
        public List<Transaction> Transactions { get; private set; } = new List<Transaction>();
        public List<MockBuy> MockBuys { get; private set; } = new List<MockBuy>();
        public string Symbol { get; private set; }
        public double Value { get; private set; }
        public long Epoch { get; private set; }
        public DateTime DateTime
        {
            get
            {
                return Helpers.ConvertEpoch(Epoch);
            }
        }
        public Quote(string symbol, double value, long epoch)
        {
            Symbol = symbol;
            Value = value;
            Epoch = epoch;
        }
        public Quote(Tick tick)
        {
            Symbol = tick.Symbol;
            Value = tick.Quote;
            Epoch = tick.Epoch;
        }
    }
}
