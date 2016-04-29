using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Containers;
using Common.EventArgs;
using Common.DataParser;
using Patterns.Executors;

namespace DataStorageNamespace
{
    public class MessageIgnoreException : Exception
    {
        public override string ToString()
        {
            return "Message should be ignored";
        }
    }
    public interface IDataStorage
    {
        /// <summary>
        /// stores communication messages for each client connected to the server
        /// </summary>
        IDictionary<int, Queue<IData>> ClientRequests { get; }
        /// <summary>
        /// Converts client data into IData
        /// </summary>
        /// <param name="id">client's id</param>
        /// <param name="data">data received through the socket</param>
        /// <returns>converted data list into IDataList object</returns>
        IDataList PrepareDataReceivedFromClient(int id, string data);
        /// <summary>
        /// Prepares math tool data to be sent through the socket
        /// </summary>
        /// <param name="id">id of the client who requested data</param>
        /// <param name="data">data list received from math tool</param>
        /// <returns>converted data into string</returns>
        string PrepareDataForClient(int id, IDataList data);
        void ClientAdded(object sender, DataEventArgs args);
        void ClientRemoved(object sender, DataEventArgs args);
    }
}
