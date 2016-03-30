using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Common.Connection;
using Common.Containers;
using Common.EventArgs;
using DataParser;

namespace Server
{
    public delegate void ClientConnectionInterruptedHandler(DataEventArgs eventArgs);
    public class ClientConnection : Connector, IClientConnection
    {
        private static int id_counter = 0;
        private ClientConnectionInterruptedHandler onClientDisconnected;
        
        public ClientConnection(TcpClient client, MessageReceivedHandler onMessageReceived, ClientConnectionInterruptedHandler onClientRemoved)
        {
            Id = id_counter++;
            onClientDisconnected = onClientRemoved;
            base.client = client;
            base.onMessageReceived = onMessageReceived;
            base.ReadingWithBlocking = true;
            base.Disconnected = new DataParser.DataParser().Serialize(new Data
            {
                MessageType = MessageType.ServerDisconnected,
                Response = ActionType.NoResponse
            });
            base.Initialize();
        }

        public int Id { get; }

        public string ClientName { get; set; }

        protected override void onDisconnected()
        {
            var clientData = new DataEventArgs()
            {
                Id = this.Id,
                Data = new Data()
                {
                    MessageType = MessageType.ClientDisconnected,
                    Response = ActionType.NoResponse
                }
            };
            onClientDisconnected(clientData);
        }
    }
}
