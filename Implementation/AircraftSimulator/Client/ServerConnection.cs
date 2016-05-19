using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Connection;
using System.Net.Sockets;
using Common.Containers;
using System.Net;
using Common.DataParser;
using Common.Exceptions;

namespace Client
{
    public delegate void ServerConnectionInterruptedHandler(ErrorCode error);
    public class ServerConnection : Connector, IServerConnection
    {
        private const int PortNum = 10000;
        private string NetIP;
        private ServerConnectionInterruptedHandler onServerDisconnected;

        public ServerConnection(MessageReceivedHandler onMessageReceivedHandler, ServerConnectionInterruptedHandler onConnectionInterruptedHandler)
        {
            NetIP = getLocalIPAddress(); //"192.168.110.243";
            onServerDisconnected = onConnectionInterruptedHandler;
            base.onMessageReceived = onMessageReceivedHandler;
            base.ReadingWithBlocking = false;
            base.Disconnected = new DataParser<DataList>().Serialize(
            new DataList()
            {
                DataArray = new[]
                {
                    new Data
                    {
                        MessageType = MessageType.ClientDisconnected,
                        Response = ActionType.NoResponse,
                        Error =  ErrorCode.ClientException
                    }
                }
            });
        }

        private string getLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }

        public string ConnectToServer(string passedIp)
        {
            if (passedIp != String.Empty)
                NetIP = passedIp;
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                throw new Exception("Please connect to network.");
            base.client = new TcpClient(NetIP, PortNum);
            base.Initialize();
            return String.Empty;
        }

        public void DisconnectFromServer()
        {
            SendMessage(Disconnected);
        }

        public void AcceptDisconnection()
        {
            string toSend = new DataParser<DataList>().Serialize(
            new DataList()
            {
                DataArray = new[]
                {
                    new Data
                    {
                        MessageType = MessageType.ClientAcceptDisconnection,
                        Response = ActionType.NoResponse,
                    }
                }
            });
            SendMessage(toSend);
        }

        public override void onDisconnected(ErrorCodeException e)
        {
            if (client.Connected)
            {
                onServerDisconnected(e.Error);
                client.Close();
            }
        }
    }
}
