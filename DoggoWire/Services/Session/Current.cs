using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DoggoWire.Models;
using Newtonsoft.Json;
using WebSocketSharp;
using WebSocketSharp.Net;

namespace DoggoWire.Services
{
    public static partial class Session
    {
        public static class Current
        {
            #region HelperClass

            public class CurrentStateChangesEventArgs : EventArgs
            {
                public bool LoggedIn { get; private set; }
                public CurrentStateChangesEventArgs(bool loggedIn)
                {
                    LoggedIn = loggedIn;
                }
            }

            public class BalanceChangesEventArgs : EventArgs
            {
                public decimal Balance { get; private set; }
                public string Currency { get; private set; }
                public BalanceChangesEventArgs(decimal balance, string currency)
                {
                    Balance = balance;
                    Currency = currency;
                }
            }

            public class ActiveSymbolAvaliableEventArgs : EventArgs
            {
                public List<ActiveSymbol> ActiveSymbols { get; private set; }
                public ActiveSymbolAvaliableEventArgs(List<ActiveSymbol> activeSymbols)
                {
                    ActiveSymbols = activeSymbols ?? new List<ActiveSymbol>();
                }
            }

            public class ProfitTableResponseEventArgs : EventArgs
            {
                public ProfitTable ProfitTable { get; private set; }
                public ProfitTableResponseEventArgs(ProfitTable profitTable)
                {
                    ProfitTable = profitTable;
                }
            }

            #endregion

            #region Properties

            public static event Action<CurrentStateChangesEventArgs> OnStateChanges;
            public static event Action<BalanceChangesEventArgs> OnBalanceChanges;
            public static event Action<ActiveSymbolAvaliableEventArgs> OnActiveSymbolAvailable;
            public static event Action<ProfitTableResponseEventArgs> OnProfitTableResponse;

            public static List<ActiveSymbol> ActiveSymbols { get; private set; } = new List<ActiveSymbol>();
            public static List<string> OpenTradingSymbols { get; private set; } = new List<string>();
            public static decimal Balance { get; private set; }
            public static string Country { get; private set; }
            public static string Currency { get; private set; }
            public static string Email { get; private set; }
            public static string Fullname { get; private set; }
            public static bool Virtual { get; private set; }
            public static string LandingCompanyFullname { get; private set; }
            public static string LandingCompanyName { get; private set; }
            public static string LoginId { get; private set; }
            public static bool Exist
            {
                get
                {
                    if (Accounts == null) return false;
                    object blob = storage.GetValue(RegKey.SelectedAuth);
                    if (blob == null) return false;
                    string value = (string)blob;
                    Account account = Array.Find(Accounts, acct => acct.Name == value);
                    if (account == null) return false;
                    return true;
                }
            }

            #endregion

            #region Methods

            public static void Start()
            {
                Task.Run(delegate
                {
                    ws = new WebSocket("wss://ws.binaryws.com/websockets/v3?app_id=21644");
                    ws.OnOpen += OnOpen;
                    ws.OnMessage += OnMessage;
                    ws.Connect();
                });
            }

            public static void Stop()
            {
                ws.CloseAsync();
            }

            public static void Set(Account account)
            {
                if (account == null) return;
                if (Array.Find(Accounts, acct => acct.Name == account.Name) != null)
                {
                    storage.SetValue(RegKey.SelectedAuth, account.Name);
                }
            }

            public static TradingInstance StartTrading(string symbol)
            {
                if (OpenTradingSymbols.Any(s => s.Equals(symbol))) return null;
                OpenTradingSymbols.Add(symbol);
                ActiveSymbol activeSymbol = ActiveSymbols.Find(s => s.Symbol.Equals(symbol));
                if (activeSymbol == null) return null;
                return new TradingInstance(activeSymbol);
            }

            #endregion

            #region Callback

            private static void OnOpen(object sender, EventArgs e)
            {
                OnConnectionChanges?.Invoke(new ConnectionChangesEventArgs(true));
                Pinger.Update();
            }

            private static void OnMessage(object sender, MessageEventArgs e)
            {
                CrunchData(e.Data);
            }

            #endregion

            #region Request

            public static void AuthorizeRequest()
            {
                if (Accounts == null) return;
                object blob = storage.GetValue(RegKey.SelectedAuth);
                if (blob == null) return;
                Account account = Array.Find(Accounts, acct => acct.Name == (string)blob);
                if (account == null) return;
                SendMsg(new AuthorizeRequest() { AuthorizeToken = account.Token });
            }

            public static void BalanceRequest(bool subscribe = false)
            {
                SendMsg(new BalanceRequest()
                {
                    Subscribe = subscribe
                });
            }

            public static void ActiveSymbolsRequest()
            {
                SendMsg(new ActiveSymbolsRequest());
            }

            public static void ProfitTableRequest(int count = 100)
            {
                SendMsg(new ProfitTableRequest() { Limit = count });
            }

            public static void TransactionStreamRequest()
            {
                SendMsg(new TransactionRequest());
            }

            #endregion

            #region Responses

            private static void CrunchData(string json)
            {
                var msg = new { msg_type = "" };
                var response = JsonConvert.DeserializeAnonymousType(json, msg);
                switch (response.msg_type)
                {
                    case PingResponse.MsgType:
                        Pinger.Update();
                        break;
                    case AuthorizeResponse.MsgType:
                        CrunchAuthorizeResponse(JsonConvert.DeserializeObject<AuthorizeResponse>(json, serializerSettings));
                        break;
                    case BalanceResponse.MsgType:
                        CrunchBalanceResponse(JsonConvert.DeserializeObject<BalanceResponse>(json, serializerSettings));
                        break;
                    case ActiveSymbolsResponse.MsgType:
                        CrunchActiveSymbolsResponse(JsonConvert.DeserializeObject<ActiveSymbolsResponse>(json, serializerSettings));
                        break;
                    case ProfitTableResponse.MsgType:
                        CrunchProfitTableResponse(JsonConvert.DeserializeObject<ProfitTableResponse>(json, serializerSettings));
                        break;
                    case TickHistoryResponse.MsgType:
                        OnTickHistoryResponse?.Invoke(JsonConvert.DeserializeObject<TickHistoryResponse>(json, serializerSettings));
                        break;
                    case TickResponse.MsgType:
                        OnTickResponse?.Invoke(JsonConvert.DeserializeObject<TickResponse>(json, serializerSettings));
                        break;
                    case BuyResponse.MsgType:
                        OnBuyResponse?.Invoke(JsonConvert.DeserializeObject<BuyResponse>(json, serializerSettings));
                        break;
                    case TransactionResponse.MsgType:
                        OnTransactionResponse?.Invoke(JsonConvert.DeserializeObject<TransactionResponse>(json, serializerSettings));
                        break;
                }
            }

            public static void CrunchAuthorizeResponse(AuthorizeResponse authorizeResponse)
            {
                if (authorizeResponse.Error == null)
                {
                    Balance = authorizeResponse.Authorize.Balance;
                    Country = authorizeResponse.Authorize.Country;
                    Currency = authorizeResponse.Authorize.Currency;
                    Email = authorizeResponse.Authorize.Email;
                    Fullname = authorizeResponse.Authorize.Fullname;
                    Virtual = authorizeResponse.Authorize.IsVirtual;
                    LandingCompanyFullname = authorizeResponse.Authorize.LandingCompanyFullname;
                    LandingCompanyName = authorizeResponse.Authorize.LandingCompanyName;
                    LoginId = authorizeResponse.Authorize.LoginId;

                    OnStateChanges?.Invoke(new CurrentStateChangesEventArgs(true));
                }
                else
                {
                    OnStateChanges?.Invoke(new CurrentStateChangesEventArgs(false));
                }
            }

            public static void CrunchBalanceResponse(BalanceResponse balanceStreamResponse)
            {
                if (balanceStreamResponse.Error == null)
                {
                    Balance = balanceStreamResponse.Balance.Amount;

                    OnBalanceChanges?.Invoke(new BalanceChangesEventArgs(balanceStreamResponse.Balance.Amount, balanceStreamResponse.Balance.Currency));
                }
                else
                {
                    if (!balanceStreamResponse.Error.Code.Equals("AlreadySubscribed"))
                    {
                        OnBalanceChanges?.Invoke(new BalanceChangesEventArgs(Balance, Currency));
                    }
                }
            }

            public static void CrunchActiveSymbolsResponse(ActiveSymbolsResponse activeSymbolsResponse)
            {
                if (activeSymbolsResponse.Error == null)
                {
                    ActiveSymbols = activeSymbolsResponse.ActiveSymbols.FindAll(sym =>
                        sym.Market.Equals("synthetic_index") && sym.ExchangeIsOpen);
                    OnActiveSymbolAvailable?.Invoke(new ActiveSymbolAvaliableEventArgs(ActiveSymbols));
                }
                else
                {
                    OnActiveSymbolAvailable?.Invoke(new ActiveSymbolAvaliableEventArgs(null));
                }
            }

            public static void CrunchProfitTableResponse(ProfitTableResponse profitTableResponse)
            {
                if (profitTableResponse.Error == null)
                {
                    OnProfitTableResponse?.Invoke(new ProfitTableResponseEventArgs(profitTableResponse.ProfitTable));
                }
            }

            #endregion
        }
    }
}
