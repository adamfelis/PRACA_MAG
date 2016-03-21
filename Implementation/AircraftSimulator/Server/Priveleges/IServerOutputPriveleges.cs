using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Common.Containers;

namespace Server
{
    public interface IServerOutputPriveleges
    {
        event AddClientHandler ClientAdded;
        event RemoveClientHandler ClientRemoved;
        event PresentDataOfTheClientHandler PresentDataOfTheClient;
        void Subscribe(IServerPrivileges serverPrivileges);

        void OnClientAdded(int id);
        void OnClientRemoved(int id);
        void OnClientDataPresented(int id, IData data);
    }
}
