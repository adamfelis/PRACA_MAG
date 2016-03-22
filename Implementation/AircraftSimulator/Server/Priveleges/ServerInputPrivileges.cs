using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Containers;
using Common.EventArgs;
using DataStorageNamespace;

namespace Server
{
    public class ServerInputPrivileges : IServerInputPriveleges
    {
        private IDictionary<int, IClientConnection> clients;
        private IDataStorage dataStorage;

        public ServerInputPrivileges(ref IDictionary<int, IClientConnection> clients, ref IDataStorage dataStorage)
        {
            this.clients = clients;
            this.dataStorage = dataStorage;
        }

        public void RespondToClient(DataEventArgs dataEventArgs)
        {
            interpretResponseMessages(clients[dataEventArgs.Id], dataEventArgs);
        }

        private void interpretResponseMessages(IClientConnection client, DataEventArgs eventHandler)
        {
            switch (eventHandler.Data.MessageType)
            {
                case MessageType.ClientDataResponse:
                    try
                    {
                        string data = dataStorage.PrepareDataForClient(eventHandler.Id, eventHandler.Data);
                        client.SendMessage(data);
                    }
                    catch(MessageIgnoreException e) { }
                    break;
                default:
                break;
            }
        }
    }
}
