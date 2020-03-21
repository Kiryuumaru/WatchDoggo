using Newtonsoft.Json;
using System;
using DoggoWire.Abstraction;
using DoggoWire.Services;

namespace DoggoWire.Models
{
    public class Tick
    {
        [JsonProperty("ask")]
        public double Ask { get; private set; }

        [JsonProperty("bid")]
        public double Bid { get; private set; }

        [JsonProperty("epoch")]
        public long Epoch { get; private set; }

        [JsonProperty("id")]
        public string Id { get; private set; }

        [JsonProperty("pip_size")]
        public int PipSize { get; private set; }

        [JsonProperty("quote")]
        public double Quote { get; private set; }

        [JsonProperty("symbol")]
        public string Symbol { get; private set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class TickResponse : Response
    {
        public const string MsgType = "tick";

        [JsonProperty("echo_req")]
        public TickRequest Request { get; private set; }

        [JsonProperty("tick")]
        public Tick Tick { get; private set; }
    }
}
