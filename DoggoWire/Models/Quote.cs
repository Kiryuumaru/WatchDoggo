using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoggoWire.Models
{
    public class Quote
    {
        public string Symbol { get; private set; }
        public decimal Value { get; private set; }
        public DateTime DateTime { get; private set; }
        public Quote(string symbol, decimal value, DateTime dateTime)
        {
            Symbol = symbol;
            Value = value;
            DateTime = dateTime;
        }
        public Quote(Tick tick)
        {
            Symbol = tick.Symbol;
            Value = tick.Quote;
            DateTime = tick.DateTime;
        }
    }
}
