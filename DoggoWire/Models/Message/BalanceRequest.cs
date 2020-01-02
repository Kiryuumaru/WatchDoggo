﻿using Newtonsoft.Json;
using DoggoWire.Abstraction;

namespace DoggoWire.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class BalanceRequest : Request
    {
        [JsonProperty("balance")]
        public string Balance { get; set; } = "1";

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
