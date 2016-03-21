using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Containers;
using Common.EventArgs;

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

        public void OnClientAdded(int id)
        {
            ClientAdded?.Invoke(this, new ClientEventArgs{Id = id});
        }

        public void OnClientRemoved(int id)
        {
            ClientRemoved?.Invoke(this, new ClientEventArgs{Id = id});
        }

        public void OnClientDataPresented(int id, IData data)
        {
            PresentDataOfTheClient?.Invoke(this, new DataEventArgs() {Id = id, Data = data});
        }
    }
}
