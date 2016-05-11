using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using Client.Priveleges;
using Common;
using Common.AircraftData;
using Common.Containers;
using Common.EventArgs;
using Common.Connection;
using Common.DataParser;
using Common.Exceptions;

namespace Client
{
    public class Client : Initializer, IClient
    {
        private IClientOutputPrivileges clientOutputPrivileges;
        public IClientOutputPrivileges ClientOutputPrivileges => clientOutputPrivileges;
        private IClientInputPriveleges clientInputPriveleges;
       
        private IServerConnection serverConnection;
        private IDataParser dataParser;

        public Client(IClientPrivileges clientPrivileges)
        {
            clientOutputPrivileges = new ClientOutputPrivileges();
            clientOutputPrivileges.Subscribe(clientPrivileges);
            Initialize();
        }

        public Client()
        {
            clientOutputPrivileges = new ClientOutputPrivileges();
            Initialize();
        }

        public IClientInputPriveleges ClientInputPriveleges => clientInputPriveleges;

        protected override void Initialize()
        {
            dataParser = new DataParser<DataList>();
            serverConnection = new ServerConnection(onMessageReceived, clientOutputPrivileges.OnServerDisconnected);
            clientInputPriveleges = new ClientInputPriveleges(ref dataParser, ref serverConnection);
        }

        public string ConnectToServer(AircraftData aircraftData)
        {
            string toSend;
            try
            {
                serverConnection.ConnectToServer();
                Data join = new Data()
                {
                    MessageType = MessageType.ClientJoinRequest,
                    Response = ActionType.ResponseRequired,
                    Sender = Environment.MachineName,
                    Array = aircraftData.GetData()
                };
                toSend = dataParser.Serialize(new DataList()
                {
                    DataArray = new []
                    {
                        join
                    }
                });
                serverConnection.SendMessage(toSend);
            }
            catch (SocketException e)
            {
                return "Server is unreachable, please trigger server";
            }
            catch (Exception e)
            {
                return e.Message;
            }
            //return toSend;
            return "Join request sent to server";
        }

        public void DisconnectFromServer()
        {
            serverConnection.DisconnectFromServer();
        }

        private void onMessageReceived(IConnector sender, string data)
        {
            var client = sender as IServerConnection;
            IDataList readableData = null;
            readableData = dataParser.Deserialize(data);
            if (readableData.DataArray.First().Error != ErrorCode.None)
                client.onDisconnected(
                    new ErrorCodeException()
                    {
                        Error = readableData.DataArray.First().Error
                    });
            else
            {
                clientOutputPrivileges.OnServerDataPresented(new DataEventArgs() { DataList = readableData });
            }
                //client.AcceptDisconnection();
        }
    }
}
