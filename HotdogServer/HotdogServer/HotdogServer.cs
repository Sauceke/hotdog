using Alchemy;
using Alchemy.Classes;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading;

namespace HotdogServer
{
    internal class HotdogServer
    {
        private WebSocketServer server;
        private HotdogSerialMonitor serialMonitor;
        private HotdogBLEMonitor bleMonitor;

        public void Start(int port)
        {
            server = new WebSocketServer(port, IPAddress.Any)
            {
                OnReceive = _ => { },
                OnConnected = OnConnect,
                OnDisconnect = OnDisconnect,
                TimeOut = new TimeSpan(1, 0, 0),
                FlashAccessPolicyEnabled = true
            };
            server.Start();
        }

        public void Stop()
        {
            server?.Stop();
            serialMonitor?.Close();
        }

        private void OnDisconnect(UserContext _)
        {
            serialMonitor?.Close();
            bleMonitor?.Close();
            Console.WriteLine("Client disconnected");
        }

        private void OnConnect(UserContext context)
        {
            DepthReadingEventArgs depthArgs = null;
            serialMonitor?.Close();
            bleMonitor?.Close();
            serialMonitor = new HotdogSerialMonitor();
            bleMonitor = new HotdogBLEMonitor();
            serialMonitor.NewReading += (s, a) => depthArgs = a;
            bleMonitor.NewReading += (s, a) => depthArgs = a;
            serialMonitor.Open();
            bleMonitor.Open();
            Console.WriteLine("Client connected");
            while (true)
            {
                Thread.Sleep(30);
                if (depthArgs != null)
                {
                    context.Send(JsonConvert.SerializeObject(depthArgs));
                }
            }
        }
    }
}
