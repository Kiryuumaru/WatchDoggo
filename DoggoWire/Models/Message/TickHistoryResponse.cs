using Newtonsoft.Json;
using System.Collections.Generic;
using DoggoWire.Abstraction;

namespace DoggoWire.Models
{
    public class History
    {
        [JsonProperty("prices")]
        public decimal[] Prices { get; private set; }

        [JsonProperty("times")]
        public int[] Times { get; private set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class TickHistoryResponse : Response
    {
        public const string MsgType = "history";

        [JsonProperty("echo_req")]
        public TickHistoryRequest Request { get; private set; }

        [JsonProperty("history")]
        public History History { get; private set; }

        [JsonProperty("pip_size")]
        public int PipSize { get; private set; }
    }
}
