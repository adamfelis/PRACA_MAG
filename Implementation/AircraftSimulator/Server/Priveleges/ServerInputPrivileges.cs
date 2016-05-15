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
            try
            {
                interpretResponseMessages(clients[dataEventArgs.Id], dataEventArgs);
            }
            //client already disconnected
            catch (KeyNotFoundException e)
            {
                bool a = false;
            }
        }

        private void interpretResponseMessages(IClientConnection client, DataEventArgs eventHandler)
        {
            //decision based on first Data message type
            switch (eventHandler.DataList.DataArray.First().MessageType)
            {
                case MessageType.ClientDataResponse:
                    try
                    {
                        string data = dataStorage.PrepareDataForClient(eventHandler.Id, eventHandler.DataList);
                        client.SendMessage(data);
                    }
                    catch(MessageIgnoreException e)
                    {
                        bool a = false;
                    }
                    break;
                default:
                break;
            }
        }
    }
}
