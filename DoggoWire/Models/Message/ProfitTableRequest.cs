using Newtonsoft.Json;
using DoggoWire.Abstraction;

namespace DoggoWire.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ProfitTableRequest : Request
    {
        [JsonProperty("profit_table")]
        public string ProfitTable { get; set; } = "1";

        [JsonProperty("description")]
        public string Description { get; set; } = "1";

        [JsonProperty("limit")]
        public int Limit { get; set; } = 100;

        [JsonProperty("offset")]
        public int Offset { get; set; } = 0;

        [JsonProperty("sort")]
        public string Sort { get; set; } = "DESC";
    }
}
