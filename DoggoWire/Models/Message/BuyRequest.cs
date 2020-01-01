using Newtonsoft.Json;
using DoggoWire.Abstraction;

namespace DoggoWire.Models
{
    public class BuyParameters
    {
        public enum ParameterBasis
        {
            Payout,
            Stake
        }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("basis")]
        private string basis = "";

        [JsonIgnore]
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
        public string ContractType { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("duration")]
        public int Duration { get; set; }

        [JsonProperty("duration_unit")]
        public string DurationUnit { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }
    }

    public class BuyRequest : Request
    {
        [JsonProperty("buy")]
        public string Buy { get; set; } = "1";

        [JsonProperty("subscribe")]
        private string subscribe = "1";
        public bool Subscribe
        {
            get
            {
                return subscribe.Equals("1");
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
