using Newtonsoft.Json;
using System.Collections.Generic;
using DoggoWire.Abstraction;

namespace DoggoWire.Models
{
    public class TickRequest : Request
    {
        [JsonProperty("ticks")]
        public List<string> Ticks { get; set; } = new List<string>();

        [JsonProperty("subscribe")]
        private string subscribe = "1";

        [JsonIgnore]
        public bool Subscribe
        {
            get
            {
                return subscribe.Equals("1");
            }
            set
            {
                subscribe = value ? "1" : null;
            }
        }
    }
}
