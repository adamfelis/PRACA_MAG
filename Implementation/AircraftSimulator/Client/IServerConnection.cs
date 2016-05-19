using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Connection;

namespace Client
{
    public interface IServerConnection : IConnector
    {
        string ConnectToServer(string passedIp);
        /// <summary>
        /// Client wants to disconnect from server due to internal client's error
        /// </summary>
        void DisconnectFromServer();
        /// <summary>
        /// Clients received information about server error and accepts disconnection
        /// </summary>
        void AcceptDisconnection();
    }
}
