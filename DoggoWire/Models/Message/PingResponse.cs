using Newtonsoft.Json;
using DoggoWire.Abstraction;

namespace DoggoWire.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class PingResponse : Response
    {
        public const string MsgType = "ping";

        [JsonProperty("echo_req")]
        public PingRequest Request { get; private set; }

        [JsonProperty("ping")]
        public string Ping { get; private set; } = "";
    }
}
