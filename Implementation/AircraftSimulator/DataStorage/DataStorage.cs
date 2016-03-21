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
    public class DataStorage : IDataStorage
    {
        public IDictionary<int, Queue<IData>> Messages { get; }
        public IData ClientDataReceived(int id, string data)
        {
            return null;
        }

        public string MathToolDataReceived(int id, IData data)
        {
            return null;
        }

        public DataStorage()
        {
            Messages = new Dictionary<int, Queue<IData>>();
        }
        public void ClientAdded(object sender, ClientEventArgs args)
        {
            Messages.Add(new KeyValuePair<int, Queue<IData>>(args.Id, new Queue<IData>()));
        }

        public void ClientRemoved(object sender, ClientEventArgs args)
        {
            Messages.Remove(args.Id);
        }
    }
}
