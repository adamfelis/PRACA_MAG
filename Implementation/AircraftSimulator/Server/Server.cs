using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Common;
using Common.Connection;
using Common.Containers;
using Common.EventArgs;
using DataStorageNamespace;
using Server.Executors;

namespace Server
{
    public sealed class Server : Initializer, IServer
    {
        private readonly IServerOutputPriveleges _serverOutputPriveleges;
        private IServerInputPriveleges _serverInputPriveleges;
        private IDataStorage dataStorage;
        private IDictionary<int, IClientConnection> clients;
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
            StartServer();
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
                    var clientConnection = new ClientConnection(_listener.AcceptTcpClient(), onMessageReceived, _serverOutputPriveleges.OnClientRemoved, clients);
                } while (true);
            });
            _listenerThread.Start();
        }

        private void onMessageReceived(IConnector sender, string data)
        {
            var client = sender as IClientConnection;
            IDataList readableData = dataStorage.PrepareDataReceivedFromClient(client.Id, data);
            interpretClientMessages(client, new DataEventArgs() {DataList = readableData, Id = client.Id});
        }

        private void interpretClientMessages(IClientConnection client, DataEventArgs dataEventArgs)
        { 
            switch (dataEventArgs.DataList.DataArray.First().MessageType)
            {
                case MessageType.ClientJoinRequest:
                    _serverOutputPriveleges.OnClientAdded(dataEventArgs, new ClientAddedExecutor(ref client, ref dataEventArgs, ref clients, ref dataStorage));
                    break;
                case MessageType.ClientDisconnected:
                    _serverOutputPriveleges.OnClientRemoved(dataEventArgs, new ClientRemovedExecutor(ref client, ref dataEventArgs, ref clients));
                    break;
                case MessageType.ClientDataRequest:
                    _serverOutputPriveleges.OnClientDataPresented(dataEventArgs, new ClientDataPresentedExecutor(_serverInputPriveleges, ref dataEventArgs));

                    //TOREMOVE
                //{
                //    dataEventArgs.Data.MessageType = MessageType.ClientDataResponse;
                //    _serverInputPriveleges.RespondToClient(dataEventArgs);
                //}
                    break;
                default:
                    break;
            }
        }

        public void StopServer()
        {
            _listener.Stop();
            _listenerThread.Abort();
        }

        protected override void Initialize()
        {
            clients = new Dictionary<int, IClientConnection>();
            dataStorage = new DataStorage();
            _serverInputPriveleges = new ServerInputPrivileges(ref clients, ref dataStorage);
        }
    }
}
