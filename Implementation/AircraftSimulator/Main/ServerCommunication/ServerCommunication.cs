using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server;

namespace ApplicationManager.ServerCommunication
{
    partial class ServerCommunication : IServerCommunication
    {
        private IServer serverInstance;
        public IServer ServerInstance
        {
            get
            {
                if (serverInstance == null)
                    serverInstance = new Server.Server(this as IServerPrivileges);
                return serverInstance;
            }
        }
    }
}
