using Newtonsoft.Json;
using DoggoWire.Abstraction;

namespace DoggoWire.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ForgetResponse : Response
    {
        public const string MsgType = "ping";

        [JsonProperty("echo_req")]
        public ForgetRequest Request { get; private set; }

        [JsonProperty("forget")]
        private readonly int forget = 0;
        public bool Forget
        {
            get
            {
                return forget == 1;
            }
        }
    }
}
