using Newtonsoft.Json;
using System;
using DoggoWire.Abstraction;
using DoggoWire.Services;

namespace DoggoWire.Models
{
    public enum TransactionAction
    {
        Buy,
        Sell,
        Deposit,
        Withdrawal,
        Escrow,
        Unknown
    }

    public class Transaction
    {
        [JsonProperty("action")]
        private readonly string action = "";
        public TransactionAction Action
        {
            get
            {
                switch (action)
                {
                    case "buy":
                        return TransactionAction.Buy;
                    case "sell":
                        return TransactionAction.Sell;
                    case "deposit":
                        return TransactionAction.Deposit;
                    case "withdrawal":
                        return TransactionAction.Withdrawal;
                    case "escrow":
                        return TransactionAction.Escrow;
                    default:
                        return TransactionAction.Unknown;
                }
            }
        }

        [JsonProperty("amount")]
        public decimal Amount { get; private set; }

        [JsonProperty("balance")]
        public decimal Balance { get; private set; }

        [JsonProperty("barrier")]
        public string Barrier { get; private set; }

        [JsonProperty("contract_id")]
        public long ContractId { get; private set; }

        [JsonProperty("currency")]
        public string Currency { get; private set; }

        [JsonProperty("date_expiry")]
        private readonly int dateExpiry = 0;
        public DateTime DateExpiry
        {
            get
            {
                return Helpers.ConvertEpoch(dateExpiry);
            }
        }

        [JsonProperty("display_name")]
        public string DisplayName { get; private set; }

        [JsonProperty("id")]
        public string Id { get; private set; }

        [JsonProperty("longcode")]
        public string LongCode { get; private set; }

        [JsonProperty("purchase_time")]
        private readonly int purchaseTime = 0;
        public DateTime PurchaseTime
        {
            get
            {
                return Helpers.ConvertEpoch(purchaseTime);
            }
        }

        [JsonProperty("symbol")]
        public string Symbol { get; private set; }

        [JsonProperty("transaction_id")]
        public long TransactionId { get; private set; }

        [JsonProperty("transaction_time")]
        private readonly int transactionTime = 0;
        public DateTime TransactionTime
        {
            get
            {
                return Helpers.ConvertEpoch(transactionTime);
            }
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class TransactionResponse : Response
    {
        public const string MsgType = "transaction";

        [JsonProperty("echo_req")]
        public TransactionRequest Request { get; private set; }

        [JsonProperty("transaction")]
        public Transaction Transaction { get; private set; }
    }
}
