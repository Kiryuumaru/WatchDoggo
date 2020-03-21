using System;
using System.Diagnostics;
using System.Threading.Tasks;
using DoggoWire.Models;

namespace DoggoWire.Services
{
    public static partial class Session
    {
        public static class Pinger
        {
            private static readonly Stopwatch pingStopwatch = new Stopwatch();

            private static bool pingReceived = false;

            private static long delay = 0;
            public static long Delay
            {
                get
                {
                    return pingReceived ? delay : pingStopwatch.ElapsedMilliseconds;
                }
            }
            public static bool IsTradeSafe
            {
                get
                {
                    return Delay < 1000;
                }
            }
            public static async void Update()
            {
                if (pingReceived) return;
                pingReceived = true;

                if (pingStopwatch.IsRunning)
                {
                    delay = pingStopwatch.ElapsedMilliseconds;
                    Console.WriteLine("ping: " + pingStopwatch.ElapsedMilliseconds);
                    await Task.Delay(5000);
                    SendMsg(new PingRequest());
                    pingStopwatch.Restart();
                }
                else
                {
                    SendMsg(new PingRequest());
                    pingStopwatch.Restart();
                }

                pingReceived = false;
            }
        }
    }
}
