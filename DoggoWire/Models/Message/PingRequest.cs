using Newtonsoft.Json;
using DoggoWire.Abstraction;

namespace DoggoWire.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class PingRequest : Request
    {
        [JsonProperty("ping")]
        public string Ping { get; set; } = "1";
    }
}
