using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Common;
using Common.Connection;
using Common.Containers;
using Common.EventArgs;
using DataStorageNamespace;
using Server;

namespace Server
{
    public sealed class Server : Initializer, IServer
    {
        private readonly IServerOutputPriveleges _serverOutputPriveleges;
        private IServerInputPriveleges _serverInputPriveleges;
        private IDataStorage _dataStorage;
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
                    var clientConnection = new ClientConnection(_listener.AcceptTcpClient(), onMessageReceived);
                    var a = true;
                } while (true);
            });
            _listenerThread.Start();
        }

        private void onMessageReceived(IConnector sender, string data)
        {
            var client = sender as IClientConnection;
            IData readableData = _dataStorage.PrepareDataReceivedFromClient(client.Id, data);
            interpretClientMessages(client, new DataEventArgs() {Data = readableData, Id = client.Id});
        }

        private void interpretClientMessages(IClientConnection client, DataEventArgs eventHandler)
        { 
            switch (eventHandler.Data.MessageType)
            {
                case MessageType.ClientJoinRequest:
                    _serverOutputPriveleges.OnClientAdded(eventHandler);
                    connectClient(client, eventHandler.Data);
                    break;
                case MessageType.ClientDisconnected:
                    _serverOutputPriveleges.OnClientRemoved(eventHandler);
                    disconnectClient(client, eventHandler.Data);
                    break;
                case MessageType.ClientDataRequest:
                    _serverOutputPriveleges.OnClientDataPresented(eventHandler);
                    break;
                default:
                    break;
            }
        }

        private void connectClient(IClientConnection client, IData data)
        {
            client.ClientName = data.Sender;
            clients.Add(new KeyValuePair<int, IClientConnection>(client.Id, client));
            IData response = new Data()
            {
                MessageType = MessageType.ClientJoinResponse
            };
            client.SendMessage(_dataStorage.PrepareDataForClient(client.Id, response));
        }

        private void disconnectClient(IClientConnection client, IData data)
        {
            clients.Remove(client.Id);
        }

        public void StopServer()
        {
            _listener.Stop();
            _listenerThread.Abort();
        }

        protected override void Initialize()
        {
            clients = new Dictionary<int, IClientConnection>();
            _dataStorage = new DataStorage();
            _serverInputPriveleges = new ServerInputPrivileges(ref clients, ref _dataStorage);
            _serverOutputPriveleges.ClientAdded += _dataStorage.ClientAdded;
            _serverOutputPriveleges.ClientRemoved += _dataStorage.ClientRemoved;
        }
    }
}
