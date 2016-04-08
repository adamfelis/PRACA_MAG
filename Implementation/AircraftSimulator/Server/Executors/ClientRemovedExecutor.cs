using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Common.EventArgs;
using Patterns.Executors;

namespace Server.Executors
{
    public class ClientRemovedExecutor : Executor, IClientRemovedExecutor
    {
        private Dispatcher dispatcher;
        private Delegate action;
        private IClientConnection client;
        private DataEventArgs dataEventArgs;
        private IDictionary<int, IClientConnection> clients;

        public ClientRemovedExecutor(ref IClientConnection client, ref DataEventArgs dataEventArgs,
            ref IDictionary<int, IClientConnection> clients)
        {
            this.client = client;
            this.dataEventArgs = dataEventArgs;
            this.clients = clients;
        }

        public void SetupAndRun(Dispatcher dispatcher, Delegate action)
        {
            this.dispatcher = dispatcher;
            this.action = action;
            Execute();
        }

        protected override void InExecute()
        {
            DispatcherOperation result = dispatcher.BeginInvoke(action);
            base.InExecute();
        }

        protected override void PostExecute()
        {
            clients.Remove(client.Id);
        }
    }
}
