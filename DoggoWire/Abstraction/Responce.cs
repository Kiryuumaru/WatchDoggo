using Newtonsoft.Json;

namespace DoggoWire.Abstraction
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class Response
    {
        public class ErrorResponse
        {
            [JsonProperty("code")]
            public string Code { get; private set; }

            [JsonProperty("message")]
            public string Message { get; private set; }
        }

        [JsonProperty("error")]
        public ErrorResponse Error { get; private set; }
    }
}
