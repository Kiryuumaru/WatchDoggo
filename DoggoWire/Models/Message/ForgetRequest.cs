using Newtonsoft.Json;
using DoggoWire.Abstraction;

namespace DoggoWire.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ForgetRequest : Request
    {
        [JsonProperty("forget")]
        public string Forget { get; set; }
    }
}
