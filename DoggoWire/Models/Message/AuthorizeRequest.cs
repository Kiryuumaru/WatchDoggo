using Newtonsoft.Json;
using DoggoWire.Abstraction;

namespace DoggoWire.Models
{
    public class AuthorizeRequest : Request
    {
        [JsonProperty("authorize")]
        public string AuthorizeToken { get; set; }
    }
}
