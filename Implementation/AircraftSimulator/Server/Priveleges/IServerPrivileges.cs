using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.EventArgs;
using Patterns.Executors;
using Server.Executors;

namespace Server
{
    public delegate void OnActionCompleted();
    public delegate void AddClientHandler(object sender, DataEventArgs eventHandler, IClientAddedExecutor clientAddedExecutor);
    public delegate void RemoveClientHandler(object sender, DataEventArgs eventHandler, IClientRemovedExecutor clientRemovedExecutor);
    public delegate void PresentDataOfTheClientHandler(object sender, DataEventArgs eventHandler, IClientDataPresentedExecutor clientDataPresentedExecutor);
    public interface IServerPrivileges
    {
        AddClientHandler OnClientAdded { get; }
        RemoveClientHandler OnClientRemoved { get; }
        PresentDataOfTheClientHandler OnClientDataPresented { get; }
    }
}
