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
        Put,
        Unknown
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
                switch (contractType)
                {
                    case "CALL":
                        return ContractType.Call;
                    case "PUT":
                        return ContractType.Put;
                    default:
                        return ContractType.Unknown;
                }
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
        public string DurationUnit { get; set; }

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
