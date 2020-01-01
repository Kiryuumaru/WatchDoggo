using DoggoWire.Abstraction;
using Microsoft.Win32;
using System;
using System.Threading.Tasks;
using WebSocketSharp;

namespace DoggoWire.Services
{
    public static partial class Session
    {
        #region Struct

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

        #endregion

        #region HelperClass

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

        private static IStorage storage = null;

        private static WebSocket ws;
        private static bool clearCurrentSession = true;
        private static Action<ConnectionChangesEventArgs> onConnectionChanges;

        public static bool Alive { get { return ws.IsAlive; } }
        public static long TradeSafeMillis
        {
            get
            {
                object blob = storage.GetValue(RegKey.TradeSafeMillis);
                if (blob == null) return 1000;
                return Convert.ToInt32(blob);
            }
            set
            {
                storage.SetValue(RegKey.TradeSafeMillis, value);
            }
        }

        #endregion

        #region Initializers

        public static async void Init(IStorage _storage)
        {
            storage = _storage;
            while (true)
            {
                if (ws == null)
                {
                    Start();
                }
                else if (Pinger.Delay > 30000)
                {
                    if (!ws.IsAlive)
                    {
                        onConnectionChanges?.Invoke(new ConnectionChangesEventArgs(false));
                        Start(false);
                    }
                }
                else if (Current.LastTickResponse > 30000 && Current.Started)
                {
                    onConnectionChanges?.Invoke(new ConnectionChangesEventArgs(false));
                    Current.Refresh();
                }
                await Task.Delay(30000);
            }
        }

        public static void Start(bool clearCurrent = true)
        {
            clearCurrentSession = clearCurrent;
            Task.Run(delegate
            {
                ws = new WebSocket("wss://ws.binaryws.com/websockets/v3?app_id=21644");
                ws.OnOpen += OnOpen;
                ws.OnMessage += OnMessage;
                ws.Connect();
                Pinger.Update();
            });
        }

        public static void Stop()
        {
            ws.CloseAsync();
        }

        #endregion

        #region Callback

        private static void OnOpen(object sender, EventArgs e)
        {
            onConnectionChanges?.Invoke(new ConnectionChangesEventArgs(true));
            if (clearCurrentSession)
            {
                Current.Initialize();
            }
            else
            {
                Current.Refresh();
            }
        }

        private static void OnMessage(object sender, MessageEventArgs e)
        {
            CrunchData(e.Data);
        }

        #endregion

        #region Methods

        public static void SetOnConnectionChanges(Action<ConnectionChangesEventArgs> eventHandler)
        {
            onConnectionChanges = eventHandler;
        }

        #endregion
    }
}
