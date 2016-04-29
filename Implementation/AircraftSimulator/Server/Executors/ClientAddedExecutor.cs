using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;
using Patterns.Executors;
using Common.Containers;
using Common.EventArgs;
using DataStorageNamespace;

namespace Server.Executors
{

    public class ClientAddedExecutor : Executor, IClientAddedExecutor
    {
        private Dispatcher dispatcher;
        private Delegate action;
        private IClientConnection client;
        private DataEventArgs dataEventArgs;
        private IDictionary<int, IClientConnection> clients;
        private IDataStorage dataStorage;

        public ClientAddedExecutor(ref IClientConnection client, ref DataEventArgs dataEventArgs, ref IDictionary<int, IClientConnection> clients, ref IDataStorage dataStorage)
        {
            this.client = client;
            this.dataEventArgs = dataEventArgs;
            this.clients = clients;
            this.dataStorage = dataStorage;
        }

        public void SetupAndRun(Dispatcher dispatcher, Delegate action)
        {
            this.dispatcher = dispatcher;
            this.action = action;
            Execute();
        }

        protected override void PreExecute()
        {
            dataStorage.ClientAdded(this, dataEventArgs);
            client.ClientName = dataEventArgs.DataList.DataArray.First().Sender;
            clients.Add(new KeyValuePair<int, IClientConnection>(client.Id, client));
            base.PreExecute();
        }

        protected override void InExecute()
        {
           DispatcherOperation result = dispatcher.BeginInvoke(action);
           result.Completed += Result_Completed;
        }

        private void Result_Completed(object sender, EventArgs e)
        {
            PostExecute();
        }

        protected override void PostExecute()
        {
            DataList response = new DataList()
            {
                DataArray = new []
                {
                    new Data()
                    {
                        MessageType = MessageType.ClientJoinResponse
                    }
                }
            };
            client.SendMessage(dataStorage.PrepareDataForClient(client.Id, response));
        }
    }
}
