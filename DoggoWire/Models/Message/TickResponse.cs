using Newtonsoft.Json;
using System;
using DoggoWire.Abstraction;
using DoggoWire.Services;

namespace DoggoWire.Models
{
    public class Tick
    {
        [JsonProperty("ask")]
        public decimal Ask { get; private set; }

        [JsonProperty("bid")]
        public decimal Bid { get; private set; }

        [JsonProperty("epoch")]
        private readonly int epoch = 0;
        public DateTime DateTime
        {
            get
            {
                return Helpers.ConvertEpoch(epoch);
            }
        }

        [JsonProperty("id")]
        public string Id { get; private set; }

        [JsonProperty("pip_size")]
        public int PipSize { get; private set; }

        [JsonProperty("quote")]
        public decimal Quote { get; private set; }

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
