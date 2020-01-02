using Newtonsoft.Json;
using DoggoWire.Abstraction;

namespace DoggoWire.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ActiveSymbolsRequest : Request
    {
        [JsonProperty("active_symbols")]
        public string ActiveSymbols { get; set; } = "full";

        [JsonProperty("product_type")]
        public string ProductType { get; set; } = "basic";
    }
}
