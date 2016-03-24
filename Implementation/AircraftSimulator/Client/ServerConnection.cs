using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Connection;
using System.Net.Sockets;

namespace Client
{
    public class ServerConnection : Connector, IClient, IServerConnection
    {
        private const int PortNum = 10000;
        private const string NetIP = "127.0.0.13";

        public ServerConnection(MessageReceivedHandler onMessageReceivedHandler)
        {
            base.onMessageReceived = onMessageReceivedHandler;
            base.ReadingWithBlocking = false;
        }

        public string ConnectToServer()
        {
            base.client = new TcpClient(NetIP, PortNum);
            base.Initialize();
            return String.Empty;
        }
    }
}
