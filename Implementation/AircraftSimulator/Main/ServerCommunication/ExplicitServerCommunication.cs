using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.EventArgs;
using Server;

namespace ApplicationManager.ServerCommunication
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
