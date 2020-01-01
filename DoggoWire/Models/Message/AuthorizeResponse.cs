using Newtonsoft.Json;
using DoggoWire.Abstraction;

namespace DoggoWire.Models
{
    public class Authorize
    {
        [JsonProperty("balance")]
        public decimal Balance { get; private set; }

        [JsonProperty("country")]
        public string Country { get; private set; }

        [JsonProperty("currency")]
        public string Currency { get; private set; }

        [JsonProperty("email")]
        public string Email { get; private set; }

        [JsonProperty("fullname")]
        public string Fullname { get; private set; }

        [JsonProperty("is_virtual")]
        private readonly int isVirtual = 1;
        public bool IsVirtual
        {
            get { return isVirtual == 1; }
        }

        [JsonProperty("landing_company_fullname")]
        public string LandingCompanyFullname { get; private set; }

        [JsonProperty("landing_company_name")]
        public string LandingCompanyName { get; private set; }

        [JsonProperty("loginid")]
        public string LoginId { get; private set; }
    }

    public class AuthorizeResponse : Response
    {
        public const string MsgType = "authorize";

        [JsonProperty("echo_req")]
        public AuthorizeRequest Request { get; private set; }

        [JsonProperty("authorize")]
        public Authorize Authorize { get; private set; }
    }
}
