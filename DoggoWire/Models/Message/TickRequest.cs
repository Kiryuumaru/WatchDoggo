using Newtonsoft.Json;
using System.Collections.Generic;
using DoggoWire.Abstraction;

namespace DoggoWire.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class TickRequest : Request
    {
        [JsonProperty("ticks")]
        public List<string> Ticks { get; set; } = new List<string>();

        [JsonProperty("subscribe")]
        private string subscribe = "1";
        public bool Subscribe
        {
            get
            {
                return subscribe == null ? false : subscribe.Equals("1");
            }
            set
            {
                subscribe = value ? "1" : null;
            }
        }
    }
}
