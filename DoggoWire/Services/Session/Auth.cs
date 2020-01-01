using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using DoggoWire.Models;

namespace DoggoWire.Services
{
    public static partial class Session
    {
        #region Properties

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
        public static bool LoggedIn
        {
            get
            {
                return (Accounts?.Length ?? 0) != 0;
            }
        }

        #endregion

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
            storage.SetValue(RegKey.LRSize, "");
            storage.SetValue(RegKey.LRTailSize, "");
            storage.SetValue(RegKey.LRHeadSize, "");
            storage.SetValue(RegKey.LRTailR2Barrier, "");
            storage.SetValue(RegKey.LRHeadR2Barrier, "");
            storage.SetValue(RegKey.LRTailSlopeBarrier, "");
            storage.SetValue(RegKey.LRHeadSlopeBarrier, "");
            storage.SetValue(RegKey.BuyAmount, "");
            storage.SetValue(RegKey.BuyDuration, "");
            storage.SetValue(RegKey.BuyDurationUnit, "");
            storage.SetValue(RegKey.CutoffLoseAmount, "");
            storage.SetValue(RegKey.CutoffWinAmount, "");
            storage.SetValue(RegKey.ReverseLogic, "");
            storage.SetValue(RegKey.AllowStraight, "");
            storage.SetValue(RegKey.CutoffLoseEnable, "");
            storage.SetValue(RegKey.CutoffWinEnable, "");
        }
    }
}
