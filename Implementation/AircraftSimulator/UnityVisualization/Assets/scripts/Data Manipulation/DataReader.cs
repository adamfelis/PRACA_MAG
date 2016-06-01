using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.scripts.Data_Manipulation;
using Client.Priveleges;
using Common.Containers;
using Common.EventArgs;
using Common.Exceptions;

namespace Assets.scripts
{
    public class DataReader : UnityNotifier, IDataReader
    {
        private IDataHandler dataHandler;
        private Queue<DataEventArgs> communicatesFromServer;
        public DataReader(IDataHandler dataHandler, ICommunication communication)
        {
            base.unityShellNotifier = communication;
            this.dataHandler = dataHandler;
            communicatesFromServer = new Queue<DataEventArgs>();
        }

        public void ReadDataSentFromServer()
        {
            lock (communicatesFromServer)
            {
                while (communicatesFromServer.Count > 0)
                {
                    var eventArgs = communicatesFromServer.Dequeue();
                    IData data = eventArgs.DataList.DataArray.First();
                    if (data.Error != ErrorCode.None)
                    {
                        ServerDisconnected(null, data.Error);
                    }
                    switch (data.MessageType)
                    {
                        case MessageType.ClientJoinResponse:
                            dataHandler.OnClientJoinResponse(eventArgs.DataList.DataArray.First());
                            break;
                        case MessageType.ServerDisconnected:
                            dataHandler.OnServerDisconnected();
                            break;
                        case MessageType.ClientDataResponse:
                            dataHandler.OnServerDataResponse(eventArgs.DataList);

                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public ServerDataReceivedHandler ServerDataReceived
        {
            get
            {
                return (object sender, DataEventArgs eventArgs) =>
                {
                    lock (communicatesFromServer)
                    {
                        communicatesFromServer.Enqueue(eventArgs);
                    }
                };
            }
        }

        public ServerDisconnectedHandler ServerDisconnected
        {
            get
            {
                return delegate (object sender, ErrorCode e)
                {
                    string toOuput = String.Empty;
                    if (e == ErrorCode.WriteOperation || e == ErrorCode.ReadOperation
                    || e == ErrorCode.StreamClosed)
                        toOuput = "unity connection side closing: " + e;
                    else
                    {
                        toOuput = "server connection side closing: " + e;
                    }
                    unityShellNotifier.NotifyUnityShell(toOuput);
                };
            }
        }
    }
}
