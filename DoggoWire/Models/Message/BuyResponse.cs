﻿using Newtonsoft.Json;
using System;
using DoggoWire.Abstraction;
using DoggoWire.Services;

namespace DoggoWire.Models
{
    public class Buy
    {
        [JsonProperty("balance_after")]
        public decimal BalanceAfter { get; private set; }

        [JsonProperty("buy_price")]
        public decimal BuyPrice { get; private set; }

        [JsonProperty("contract_id")]
        public long ContractId { get; private set; }

        [JsonProperty("longcode")]
        public string Longcode { get; private set; }

        [JsonProperty("payout")]
        public decimal Payout { get; private set; }

        [JsonProperty("purchase_time")]
        public long PurchaseTime { get; private set; }

        [JsonProperty("shortcode")]
        public string Shortcode { get; private set; }

        [JsonProperty("start_time")]
        public long StartTime { get; private set; }

        [JsonProperty("transaction_id")]
        public long TransactionId { get; private set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class BuyResponse : Response
    {
        public const string MsgType = "buy";

        [JsonProperty("echo_req")]
        public BuyRequest Request { get; private set; }

        [JsonProperty("buy")]
        public Buy Buy { get; private set; }
    }
}
