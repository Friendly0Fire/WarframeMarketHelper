using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using vtortola.WebSockets;
using vtortola.WebSockets.Http;
using vtortola.WebSockets.Rfc6455;
using System.Windows.Automation;

namespace WarframeMarketHelper
{
    internal class Helper
    {
        private readonly WebSocketClient _wsClient;
        private string _token = "";
        private readonly object _tokenLock = new object();
        private CancellationTokenSource _cancellationSource;
        private CancellationToken _cancellation;

        private bool _currentlyOnline = false;

        public string Token
        {
            get
            {
                lock (_tokenLock)
                {
                    return _token;
                }
            }
            set
            {
                lock (_tokenLock)
                {
                    _token = value;
                }
            }
        }

        public Helper()
        {
            var opts = new WebSocketListenerOptions
            {
                NegotiationTimeout = TimeSpan.FromSeconds(10)
            };
            opts.Standards.RegisterRfc6455();
            _wsClient = new WebSocketClient(opts);

        }

        ~Helper()
        {
            if (_wsClient == null) return;

            try
            {
                var task = _wsClient.CloseAsync();
                task.RunSynchronously();
            }
            catch(Exception) {}
        }

        public void Start()
        {
            _cancellationSource = new CancellationTokenSource();
            _cancellation = _cancellationSource.Token;

            SetOnlineState(false);

            Automation.AddAutomationFocusChangedEventHandler(OnFocusChanged);
        }

        public void Stop()
        {
            _cancellationSource?.Cancel();

            Automation.RemoveAutomationFocusChangedEventHandler(OnFocusChanged);
        }

        private void OnFocusChanged(object sender, AutomationFocusChangedEventArgs e)
        {
            AutomationElement focusedElement = sender as AutomationElement;
            if (focusedElement != null)
            {
                int processId = focusedElement.Current.ProcessId;
                using (Process process = Process.GetProcessById(processId))
                {
                    Debug.WriteLine(process.ProcessName);
                    if(process.ProcessName == "Warframe.x64" && !_currentlyOnline)
                        SetOnlineState(true);
                    else if (process.ProcessName != "Warframe.x64" && _currentlyOnline)
                        SetOnlineState(false);
                }
            }
        }

        private async void SetOnlineState(bool online)
        {
            try
            {
                var auth = "JWT ";
                lock (_tokenLock)
                {
                    auth += _token;
                }

                var ws = await _wsClient.ConnectAsync(new Uri("wss://warframe.market/socket"), new Headers<RequestHeader>(
                    new Dictionary<string, string>
                    {
                        {"Authorization", auth}
                    }), _cancellation);

                await ws.WriteStringAsync("{\"type\": \"@WS/USER/SET_STATUS\", \"payload\": \""
                                          + (online ? "ingame" : "invisible")
                                          + "\"}", _cancellation);

                await ws.CloseAsync();

                _currentlyOnline = online;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
