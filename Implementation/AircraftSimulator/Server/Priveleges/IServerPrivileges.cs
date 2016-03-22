using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.EventArgs;

namespace Server
{
    public delegate void AddClientHandler(object sender, DataEventArgs eventHandler);
    public delegate void RemoveClientHandler(object sender, DataEventArgs eventHandler);
    public delegate void PresentDataOfTheClientHandler(object sender, DataEventArgs eventHandler);
    public interface IServerPrivileges
    {
        AddClientHandler OnClientAdded { get; }
        RemoveClientHandler OnClientRemoved { get; }
        PresentDataOfTheClientHandler OnClientDataPresented { get; }
    }
}
