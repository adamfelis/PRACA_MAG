using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server;

namespace ApplicationManager.ServerCommunication
{
    partial class ServerCommunication : Server.IServerPrivileges
    {
        public AddClientHandler OnClientAdded
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public PresentDataOfTheClientHandler OnClientDataPresented
        {
            get
            {
                throw new NotImplementedException();
            }

            set
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

            set
            {
                throw new NotImplementedException();
            }
        }

    }
}
