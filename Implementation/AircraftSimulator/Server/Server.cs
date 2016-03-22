using System.Net;
using System.Net.Sockets;
using System.Threading;
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
        private TcpListener _listener;
        private Thread _listenerThread;
        const int PORT_NUM = 10000;

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
            _listenerThread = new Thread(() =>
            {
                _listener = new TcpListener(IPAddress.Any, PORT_NUM);
                _listener.Start();
                do
                {
                    ClientConnection client = new ClientConnection(_listener.AcceptTcpClient(), onMessageReceived);

                } while (true);
            });
            _listenerThread.Start();
        }

        private void onMessageReceived(ClientConnection sender, string data)
        {
            
        }

        public void StopServer()
        {
            _listener.Stop();
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
