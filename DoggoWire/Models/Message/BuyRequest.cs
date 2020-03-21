using Newtonsoft.Json;
using DoggoWire.Abstraction;

namespace DoggoWire.Models
{
    public enum ParameterBasis
    {
        Payout,
        Stake
    }

    public enum ContractType
    {
        Call,
        Put
    }

    public enum DurationUnit
    {
        Ticks,
        Seconds,
        Minutes,
        Hours,
        Days
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class BuyParameters
    {
        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("basis")]
        private string basis = "";
        public ParameterBasis Basis
        {
            get
            {
                return basis.Equals("payout") ? ParameterBasis.Payout : ParameterBasis.Stake;
            }
            set
            {
                basis = value == ParameterBasis.Payout ? "payout" : "stake";
            }
        }

        [JsonProperty("contract_type")]
        private string contractType = "";
        public ContractType ContractType
        {
            get
            {
                return contractType switch
                {
                    "CALL" => ContractType.Call,
                    "PUT" => ContractType.Put,
                    _ => ContractType.Call,
                };
            }
            set
            {
                switch (value)
                {
                    case ContractType.Call:
                        contractType = "CALL";
                        break;
                    case ContractType.Put:
                        contractType = "PUT";
                        break;
                }
            }
        }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("duration")]
        public int Duration { get; set; }

        [JsonProperty("duration_unit")]
        private string durationUnit = "";
        public DurationUnit DurationUnit
        {
            get
            {
                return durationUnit switch
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
                        durationUnit = "t";
                        break;
                    case DurationUnit.Seconds:
                        durationUnit = "s";
                        break;
                    case DurationUnit.Minutes:
                        durationUnit = "m";
                        break;
                    case DurationUnit.Hours:
                        durationUnit = "h";
                        break;
                    case DurationUnit.Days:
                        durationUnit = "d";
                        break;
                }
            }
        }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class BuyRequest : Request
    {
        [JsonProperty("buy")]
        public string Buy { get; set; } = "1";

        [JsonProperty("subscribe")]
        private string subscribe = null;
        public bool Subscribe
        {
            get
            {
                return subscribe == null ? false : subscribe.Equals("1");
            }
            set
            {
                subscribe = value ? "1" : null;
            }
        }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("parameters")]
        public BuyParameters Parameters { get; set; }
    }
}
