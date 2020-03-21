using DoggoWire.Abstraction;
using DoggoWire.Models;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Net;

namespace DoggoWire.Services
{
    public static partial class Session
    {
        #region HelperClass

        private struct RegKey
        {
            public const string Auth = "state";
            public const string SelectedAuth = "state_selected";
            public const string TradeSafeMillis = "trade_safe_millis";
            public const string LRSize = "lr_size";
            public const string LRTailSize = "lr_tail_size";
            public const string LRHeadSize = "lr_head_size";
            public const string LRTailR2Barrier = "lr_tail_r2_barrier";
            public const string LRHeadR2Barrier = "lr_head_r2_barrier";
            public const string LRTailSlopeBarrier = "lr_tail_slope_barrier";
            public const string LRHeadSlopeBarrier = "lr_head_slope_barrier";
            public const string BuyAmount = "buy_amount";
            public const string BuyDuration = "buy_duration";
            public const string BuyDurationUnit = "buy_duration_unit";
            public const string CutoffLoseAmount = "cutoff_lose_amount";
            public const string CutoffWinAmount = "cutoff_win_amount";
            public const string ReverseLogic = "reverse_logic";
            public const string AllowStraight = "allow_straight";
            public const string CutoffLoseEnable = "cutoff_lose_enable";
            public const string CutoffWinEnable = "cutoff_win_enable";
        }

        public class ConnectionChangesEventArgs : EventArgs
        {
            public bool Connected { get; private set; }
            public ConnectionChangesEventArgs(bool connected)
            {
                Connected = connected;
            }
        }

        #endregion

        #region Properties

        private static readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
        private static IStorage storage = null;
        private static WebSocket ws;

        public static event Action<ConnectionChangesEventArgs> OnConnectionChanges;
        public static bool Alive { get { return ws.IsAlive; } }
        public static bool LoggedIn
        {
            get
            {
                return (Accounts?.Length ?? 0) != 0;
            }
        }

        #endregion

        #region Initializers

        public static void Init(IStorage _storage)
        {
            storage = _storage;
            if (ws == null)
            {
                Current.Start();
            }
        }

        #endregion

        #region Auth

        private const string Prefix = "http://+:80/";
        private static readonly HttpListener authListener = new HttpListener();

        public static Account[] Accounts
        {
            get
            {
                object blob = storage.GetValue(RegKey.Auth);
                if (blob == null) return null;
                string value = (string)blob;
                if (value.Contains("redirect.html?"))
                {
                    List<Account> accounts = new List<Account>();
                    string[] data = value.Substring(value.IndexOf("?") + 1).Split('&');
                    int acctIndex = 1;
                    while (true)
                    {
                        string acct = Array.Find(data, i => i.Contains("acct" + acctIndex));
                        if (string.IsNullOrEmpty(acct)) break;
                        string token = data.First(i => i.Contains("token" + acctIndex));
                        string cur = data.First(i => i.Contains("cur" + acctIndex));
                        accounts.Add(new Account(
                            acct.Substring(acct.IndexOf("=") + 1),
                            token.Substring(token.IndexOf("=") + 1),
                            cur.Substring(cur.IndexOf("=") + 1),
                            acct.Contains("VRT")));
                        acctIndex++;
                    }
                    return accounts.ToArray();
                }
                else
                {
                    return null;
                }
            }
        }

        public static void Login(Action onFinish)
        {
            string host = System.IO.File.ReadAllText(@"C:\\Windows\\System32\\Drivers\\etc\\hosts");
            if (!host.Contains("127.0.0.1 WatchDoggo.com"))
            {
                System.IO.File.AppendAllText(@"C:\\Windows\\System32\\Drivers\\etc\\hosts", "\n127.0.0.1 WatchDoggo.com");
            }
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("HttpListener is not supported on this platform.");
                return;
            }
            if (!authListener.IsListening)
            {
                if (!authListener.Prefixes.Contains(Prefix)) authListener.Prefixes.Add(Prefix);
                authListener.Start();
                authListener.BeginGetContext((result) =>
                {
                    HttpListenerContext context = authListener.EndGetContext(result);

                    byte[] buffer = Encoding.UTF8.GetBytes("<html><body>Redirecting . . .</body></html>");

                    context.Response.ContentType = "text/html";
                    context.Response.ContentLength64 = buffer.Length;
                    context.Response.StatusCode = 200;
                    context.Response.OutputStream.Write(buffer, 0, buffer.Length);
                    context.Response.OutputStream.Close();

                    storage.SetValue(RegKey.Auth, context.Request.RawUrl);
                    if (LoggedIn)
                    {
                        authListener.Stop();
                        authListener.Close();
                        onFinish?.Invoke();
                    }
                }, null);
            }
            Process.Start(new ProcessStartInfo("https://oauth.binary.com/oauth2/authorize?app_id=21644"));
        }

        public static void Logout()
        {
            storage.SetValue(RegKey.Auth, "");
            storage.SetValue(RegKey.SelectedAuth, "");
        }

        #endregion

        #region Socket

        private static void SendMsg(Request request)
        {
            if (ws.IsAlive)
            {
                ws.Send(JsonConvert.SerializeObject(request, Formatting.None, serializerSettings));
            }
        }

        #endregion
    }
}
