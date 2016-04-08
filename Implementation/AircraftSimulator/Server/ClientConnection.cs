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
using Common.DataParser;
using Server.Executors;

namespace Server
{
    public delegate void ClientConnectionInterruptedHandler(DataEventArgs eventArgs, IClientRemovedExecutor clientRemovedExecutor);
    public class ClientConnection : Connector, IClientConnection
    {
        private static int id_counter = 0;
        private ClientConnectionInterruptedHandler onClientDisconnected;
        private IDictionary<int, IClientConnection> clients;

        public ClientConnection(TcpClient client, MessageReceivedHandler onMessageReceived, ClientConnectionInterruptedHandler onClientRemoved, IDictionary<int, IClientConnection> clients)
        {
            Id = id_counter++;
            onClientDisconnected = onClientRemoved;
            this.clients = clients;
            base.client = client;
            base.onMessageReceived = onMessageReceived;
            base.ReadingWithBlocking = true;
            base.Disconnected = new DataParser<Data>().Serialize(new Data
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
            IClientConnection connection = this;
            onClientDisconnected(clientData, new ClientRemovedExecutor(ref connection, ref clientData, ref clients));
        }
    }
}
