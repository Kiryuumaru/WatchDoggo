using Newtonsoft.Json;
using DoggoWire.Abstraction;

namespace DoggoWire.Models
{
    public class Balance
    {
        [JsonProperty("balance")]
        public decimal Amount { get; private set; }

        [JsonProperty("currency")]
        public string Currency { get; private set; }

        [JsonProperty("id")]
        public string Id { get; private set; }

        [JsonProperty("loginid")]
        public string LoginId { get; private set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class BalanceResponse : Response
    {
        public const string MsgType = "balance";

        [JsonProperty("echo_req")]
        public BalanceRequest Request { get; private set; }

        [JsonProperty("balance")]
        public Balance Balance { get; private set; }
    }
}
