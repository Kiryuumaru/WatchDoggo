using Newtonsoft.Json;
using DoggoWire.Abstraction;

namespace DoggoWire.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class TickHistoryRequest : Request
    {
        [JsonProperty("ticks_history")]
        public string Symbol { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("end")]
        public string End { get; set; } = "latest";
        //public string End { get; set; } = "1577726909";

        [JsonProperty("style")]
        public string Style { get; set; } = "ticks";
    }
}
