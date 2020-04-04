using System;
using System.Collections.Generic;
using System.Text;

namespace DoggoWire.Models
{
    public class MockBuy
    {
        public decimal Amount { get; private set; }
        public int Duration { get; private set; }
        public DurationUnit DurationUnit { get; private set; }
        public ContractType ContractType { get; private set; }
        public long HighlightTime { get; private set; }
        public MockBuy(decimal amount, int duration, DurationUnit durationUnit, ContractType contractType, long highlightTime)
        {
            Amount = amount;
            Duration = duration;
            DurationUnit = durationUnit;
            ContractType = contractType;
            HighlightTime = highlightTime;
        }
    }
}
