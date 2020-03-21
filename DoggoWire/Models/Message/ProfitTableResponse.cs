using Newtonsoft.Json;
using DoggoWire.Abstraction;
using System;
using DoggoWire.Services;

namespace DoggoWire.Models
{
    public class ProfitTableTransaction
    {
        [JsonProperty("app_id")]
        public long AppId { get; private set; }

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

        [JsonProperty("sell_price")]
        public decimal SellPrice { get; private set; }

        [JsonProperty("sell_time")]
        public long SellTime { get; private set; }

        [JsonProperty("shortcode")]
        public string Shortcode { get; private set; }

        [JsonProperty("transaction_id")]
        public long TransactionId { get; private set; }
    }

    public class ProfitTable
    {
        [JsonProperty("count")]
        public int Count { get; private set; }

        [JsonProperty("transactions")]
        public ProfitTableTransaction[] Transactions { get; private set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class ProfitTableResponse : Response
    {
        public const string MsgType = "profit_table";

        [JsonProperty("echo_req")]
        public ProfitTableRequest Request { get; private set; }

        [JsonProperty("profit_table")]
        public ProfitTable ProfitTable { get; private set; }
    }
}
