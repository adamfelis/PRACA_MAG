using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.Priveleges;
using Common;
using Common.Containers;
using Common.EventArgs;
using Common.Connection;
using DataParser;
using DataParser = DataParser.DataParser;

namespace Client
{
    public class Client : Initializer, IClient
    {
        private IClientOutputPrivileges _clientOutputPrivileges;
        public IClientOutputPrivileges ClientOutputPrivileges => _clientOutputPrivileges;
       
        private ServerConnection serverConnection;
        private IDataParser dataParser;

        public Client(IClientPrivileges clientPrivileges)
        {
            _clientOutputPrivileges = new ClientOutputPrivileges();
            _clientOutputPrivileges.Subscribe(clientPrivileges);
            Initialize();
        }

        public Client()
        {
            _clientOutputPrivileges = new ClientOutputPrivileges();
            Initialize();
        }

        protected override void Initialize()
        {
            dataParser = new global::DataParser.DataParser();
            serverConnection = new ServerConnection(onMessageReceived);
        }

        public string ConnectToServer()
        {
            try
            {
                serverConnection.ConnectToServer();
                IData join = new Data()
                {
                    MessageType = MessageType.ClientJoinRequest,
                    Response = ActionType.ResponseRequired,
                    Sender = Environment.MachineName
                };
                string toSend = dataParser.Serialize(join);
                serverConnection.SendMessage(toSend);
            }
            catch (Exception e)
            {
                return "Server is unreachable, please trigger server";
            }
            return "Join request sent to server";
        }

        private void onMessageReceived(IConnector sender, string data)
        {
            var client = sender as IServerConnection;
            IData readableData = dataParser.Deserialize(data);
            _clientOutputPrivileges.OnServerDataPresented(new DataEventArgs() {Data = readableData});
        }
    }
}
