using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using DoggoWire.Abstraction;
using DoggoWire.Services;

namespace DoggoWire.Models
{
    public class ActiveSymbol
    {
        [JsonProperty("allow_forward_starting")]
        private readonly int allowForwardStarting = 0;
        public bool AllowForwardStarting
        {
            get
            {
                return allowForwardStarting == 1;
            }
        }

        [JsonProperty("delay_amount")]
        public int DelayAmount { get; private set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; private set; }

        [JsonProperty("exchange_is_open")]
        private readonly int exchangeIsOpen = 0;
        public bool ExchangeIsOpen
        {
            get
            {
                return exchangeIsOpen == 1;
            }
        }

        [JsonProperty("exchange_name")]
        public string ExchangeName { get; private set; }

        [JsonProperty("intraday_interval_minutes")]
        public int IntradayIntervalMinutes { get; private set; }

        [JsonProperty("is_trading_suspended")]
        private readonly int isTradingSuspended = 1;
        public bool IsTradingSuspended
        {
            get
            {
                return isTradingSuspended == 1;
            }
        }

        [JsonProperty("market")]
        public string Market { get; private set; }

        [JsonProperty("market_display_name")]
        public string MarketDisplayName { get; private set; }

        [JsonProperty("pip")]
        public double Pip { get; private set; }

        [JsonProperty("quoted_currency_symbol")]
        public string QuotedCurrencySymbol { get; private set; }

        [JsonProperty("spot")]
        public double Spot { get; private set; }

        [JsonProperty("spot_age")]
        public string SpotAge { get; private set; }

        [JsonProperty("spot_time")]
        public long SpotTime { get; private set; }

        [JsonProperty("submarket")]
        public string Submarket { get; private set; }

        [JsonProperty("submarket_display_name")]
        public string SubmarketDisplayName { get; private set; }

        [JsonProperty("symbol")]
        public string Symbol { get; private set; }

        [JsonProperty("symbol_type")]
        public string SymbolType { get; private set; }

    }

    [JsonObject(MemberSerialization.OptIn)]
    public class ActiveSymbolsResponse : Response
    {
        public const string MsgType = "active_symbols";

        [JsonProperty("echo_req")]
        public ActiveSymbolsRequest Request { get; private set; }

        [JsonProperty("active_symbols")]
        public List<ActiveSymbol> ActiveSymbols { get; private set; } = new List<ActiveSymbol>();
    }
}
