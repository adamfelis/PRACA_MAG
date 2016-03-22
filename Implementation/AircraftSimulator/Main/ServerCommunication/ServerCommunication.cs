using Server;

namespace Main.ServerCommunication
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
