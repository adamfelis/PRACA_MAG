using Common;
using DataStorageNamespace;
using Server;

namespace Server
{
    public sealed class Server : Initializer, IServer
    {
        private readonly IServerOutputPriveleges _serverOutputPriveleges;
        private IServerInputPriveleges _serverInputPriveleges;
        private IDataStorage _dataStorage; 

        public IServerInputPriveleges ServerInputPrivileges => _serverInputPriveleges;

        public Server(IServerPrivileges serverPrivileges)
        {
            _serverOutputPriveleges = new ServerOutputPrivileges();
            _serverOutputPriveleges.Subscribe(serverPrivileges);
            Initialize();
        }

        public Server()
        {
            _serverOutputPriveleges = new ServerOutputPrivileges();
            Initialize();
        }

        public void StartServer()
        {
            
        }

        public void StopServer()
        {
        }

        protected override void Initialize()
        {
            _serverInputPriveleges = new ServerInputPrivileges();
            _dataStorage = new DataStorage();
            _serverOutputPriveleges.ClientAdded += _dataStorage.ClientAdded;
            _serverOutputPriveleges.ClientRemoved += _dataStorage.ClientRemoved;
        }
    }
}
