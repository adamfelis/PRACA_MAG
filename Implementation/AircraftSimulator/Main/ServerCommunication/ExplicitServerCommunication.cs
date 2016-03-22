using System;
using Common.EventArgs;
using Server;

namespace Main.ServerCommunication
{
    partial class ServerCommunication : Server.IServerPrivileges
    {
        public AddClientHandler OnClientAdded => a;

        void a(object b, ClientEventArgs c)
        {
            
        }

        public PresentDataOfTheClientHandler OnClientDataPresented
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public RemoveClientHandler OnClientRemoved
        {
            get
            {
                throw new NotImplementedException();
            }
        }

    }
}
