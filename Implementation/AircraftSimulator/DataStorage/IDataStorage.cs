using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Containers;
using Common.EventArgs;
using DataParser;

namespace DataStorageNamespace
{
    public interface IDataStorage
    {
        /// <summary>
        /// stores communication messages for each client connected to the server
        /// </summary>
        IDictionary<int, Queue<IData>> Messages { get; }
        /// <summary>
        /// Converts client data into IData
        /// </summary>
        /// <param name="id">client's id</param>
        /// <param name="data">data received through the socket</param>
        /// <returns>converted data into IData object</returns>
        IData ClientDataReceived(int id, string data);
        /// <summary>
        /// Prepares math tool data to be sent through the socket
        /// </summary>
        /// <param name="id">id of the client who requested data</param>
        /// <param name="data">data received from math tool</param>
        /// <returns>converted data into string</returns>
        string MathToolDataReceived(int id, IData data);
        void ClientAdded(object sender, ClientEventArgs args);
        void ClientRemoved(object sender, ClientEventArgs args);
    }
}
