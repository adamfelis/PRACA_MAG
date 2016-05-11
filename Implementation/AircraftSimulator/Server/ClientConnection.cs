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
using Common.Exceptions;
using DataStorageNamespace;
using Server.Executors;

namespace Server
{
    public delegate void ClientConnectionInterruptedHandler(DataEventArgs eventArgs, IClientRemovedExecutor clientRemovedExecutor);
    public class ClientConnection : Connector, IClientConnection
    {
        private static int id_counter = 0;
        private ClientConnectionInterruptedHandler onClientDisconnected;
        private IDictionary<int, IClientConnection> clients;
        private IDataStorage dataStorage;

        public ClientConnection(TcpClient client, MessageReceivedHandler onMessageReceived, ClientConnectionInterruptedHandler onClientRemoved, ref IDictionary<int, IClientConnection> clients, ref IDataStorage dataStorage)
        {
            Id = id_counter++;
            onClientDisconnected = onClientRemoved;
            this.clients = clients;
            this.dataStorage = dataStorage;
            base.client = client;
            base.onMessageReceived = onMessageReceived;
            base.ReadingWithBlocking = true;
            base.Disconnected = new DataParser<DataList>().Serialize(
            new DataList()
            {
                DataArray = new[]
                {
                    new Data()
                    {
                        MessageType = MessageType.ServerDisconnected,
                        Response = ActionType.NoResponse
                    }
                }
            });
            base.Initialize();
        }

        public int Id { get; }

        public string ClientName { get; set; }

        public override void onDisconnected(ErrorCodeException e)
        {
            //if stream has been already closed it means client had been informed if possible 
            if (!client.Connected)
                return;

            DataEventArgs dataEventArgs = new DataEventArgs()
            {
                Id = this.Id
            };
            IClientConnection connection = this;
            //if there was law layer exception we can only remove client on server side
            if (e.GetType() == typeof (LawLayerException))
            {
                onClientDisconnected(dataEventArgs, new ClientRemovedExecutor(ref connection, ref dataEventArgs, ref clients, ref dataStorage));
                if (client.Connected)
                    client.Close();
                return;
            }

            //if there was any other exception inform client first
            string toSend = new DataParser<DataList>().Serialize(
            new DataList()
            {
                DataArray = new[]
                    {
                        new Data()
                        {
                            MessageType = MessageType.ServerDisconnected,
                            Response = ActionType.NoResponse,
                            Error = e.Error
                        }
                    }

            });
            SendMessage(toSend);
        }
    }
}
