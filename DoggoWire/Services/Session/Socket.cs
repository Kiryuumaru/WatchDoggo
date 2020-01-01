using Newtonsoft.Json;
using DoggoWire.Models;
using DoggoWire.Abstraction;

namespace DoggoWire.Services
{
    public static partial class Session
    {
        private static void SendMsg(Request request)
        {
            string s = JsonConvert.SerializeObject(request);
            ws.Send(s);
        }

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
                    Current.AuthorizeResponse(JsonConvert.DeserializeObject<AuthorizeResponse>(json));
                    break;
                case BalanceResponse.MsgType:
                    Current.BalanceResponse(JsonConvert.DeserializeObject<BalanceResponse>(json));
                    break;
                case ActiveSymbolsResponse.MsgType:
                    Current.ActiveSymbolsResponse(JsonConvert.DeserializeObject<ActiveSymbolsResponse>(json));
                    break;
                case TickHistoryResponse.MsgType:
                    Current.TickHistoryResponse(JsonConvert.DeserializeObject<TickHistoryResponse>(json));
                    break;
                case TransactionResponse.MsgType:
                    Current.TransactionResponse(JsonConvert.DeserializeObject<TransactionResponse>(json));
                    break;
                case TickResponse.MsgType:
                    Current.TickResponse(JsonConvert.DeserializeObject<TickResponse>(json));
                    break;
                case BuyResponse.MsgType:
                    //Current.BuyResponse(JsonConvert.DeserializeObject<BuyResponse>(json));
                    break;
            }
        }
    }
}
