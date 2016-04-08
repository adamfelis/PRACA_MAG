using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Common.Containers;
using Common.EventArgs;
using Patterns.Executors;
using Server.Executors;

namespace Server
{
    public interface IServerOutputPriveleges
    {
        event AddClientHandler ClientAdded;
        event RemoveClientHandler ClientRemoved;
        event PresentDataOfTheClientHandler PresentDataOfTheClient;
        void Subscribe(IServerPrivileges serverPrivileges);

        void OnClientAdded(DataEventArgs eventHandler, IClientAddedExecutor clientAddedExecutor);
        void OnClientRemoved(DataEventArgs eventHandler, IClientRemovedExecutor clientRemovedExecutor);
        void OnClientDataPresented(DataEventArgs eventHandler, IClientDataPresentedExecutor clientDataPresentedExecutor);
    }
}
