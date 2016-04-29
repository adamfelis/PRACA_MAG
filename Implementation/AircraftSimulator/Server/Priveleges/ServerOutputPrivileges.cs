using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Containers;
using Common.EventArgs;
using Patterns.Executors;
using Server.Executors;

namespace Server
{

    public class ServerOutputPrivileges : IServerOutputPriveleges
    {
        public event AddClientHandler ClientAdded;
        public event RemoveClientHandler ClientRemoved;
        public event PresentDataOfTheClientHandler PresentDataOfTheClient;

        public void Subscribe(IServerPrivileges serverPrivileges)
        {
            ClientAdded += serverPrivileges.OnClientAdded;
            ClientRemoved += serverPrivileges.OnClientRemoved;
            PresentDataOfTheClient += serverPrivileges.OnClientDataPresented;
        }

        public void OnClientAdded(DataEventArgs eventArgs, IClientAddedExecutor clientAddedExecutor)
        {
            ClientAdded?.Invoke(this, new DataEventArgs { Id = eventArgs.Id, DataList = eventArgs.DataList }, clientAddedExecutor);
        }

        public void OnClientRemoved(DataEventArgs eventArgs, IClientRemovedExecutor clientRemovedExecutor)
        {
            ClientRemoved?.Invoke(this, new DataEventArgs { Id = eventArgs.Id, DataList = eventArgs.DataList }, clientRemovedExecutor);
        }

        public void OnClientDataPresented(DataEventArgs eventArgs, IClientDataPresentedExecutor clientDataPresentedExecutor)
        {
            PresentDataOfTheClient?.Invoke(this, new DataEventArgs() { Id = eventArgs.Id, DataList = eventArgs.DataList }, clientDataPresentedExecutor);
        }
    }
}
