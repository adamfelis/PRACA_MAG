using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Common.Connection;
using Common.Containers;
using DataParser;

namespace Server
{
    public class ClientConnection : Connector, IClientConnection
    {
        private static int id_counter = 0;
        
        public ClientConnection(TcpClient client, MessageReceivedHandler onMessageReceivedHandler)
        {
            this.Id = id_counter++;
            base.client = client;
            base.onMessageReceived = onMessageReceivedHandler;
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

    }
}
