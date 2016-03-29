using System;
using Common.EventArgs;
using Server;

namespace Main.ServerCommunication
{
    partial class ServerCommunication : Server.IServerPrivileges
    {
        public AddClientHandler OnClientAdded => onClientAdded;

        void onClientAdded(object sender, DataEventArgs dataEventArgs)
        {
          
        }

        public PresentDataOfTheClientHandler OnClientDataPresented => onClientDataPresented;

        void onClientDataPresented(object sender, DataEventArgs dataEventArgs)
        {

        }

        public RemoveClientHandler OnClientRemoved => onClientRemoved;

        void onClientRemoved(object sender, DataEventArgs dataEventArgs)
        {

        }
    }
}
