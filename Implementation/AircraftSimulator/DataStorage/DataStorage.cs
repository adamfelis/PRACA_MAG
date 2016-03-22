using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Containers;
using Common.EventArgs;
using DataParser;
using DataParser = DataParser.DataParser;

namespace DataStorageNamespace
{
    public class DataStorage : Initializer, IDataStorage
    {
        private IDataParser _dataParser;
        public IDictionary<int, Queue<IData>> ClientRequests { get; }
        public IData PrepareDataReceivedFromClient(int id, string data)
        {
            IData readableData = _dataParser.Deserialize(data);
            ClientRequests[id].Enqueue(readableData);
            return readableData;
        }

        public string PrepareDataForClient(int id, IData data)
        {
            IData request = ClientRequests[id].Dequeue();
            if (request.Response == ActionType.NoResponse)
                throw new MessageIgnoreException();
            data.InputType = request.OutputType;
            string toSend = _dataParser.Serialize(data);
            return toSend;
        }

        public DataStorage()
        {
            ClientRequests = new Dictionary<int, Queue<IData>>();
            Initialize();
        }
        public void ClientAdded(object sender, DataEventArgs args)
        {
            ClientRequests.Add(new KeyValuePair<int, Queue<IData>>(args.Id, new Queue<IData>()));
        }

        public void ClientRemoved(object sender, DataEventArgs args)
        {
            ClientRequests.Remove(args.Id);
        }

        protected override void Initialize()
        {
            _dataParser = new global::DataParser.DataParser();
            //TOREMOVE
            var a = _dataParser.Serialize(new Data()
            {
                Array = new float[][]
                {
                    new float[3]
                    {
                        1, 2, 3
                    },
                    new float[3]
                    {
                        4, 5, 6
                    }
                }
            });
            var b = _dataParser.Deserialize(a);
            var n = b.N;
            var m = b.M;
            var d = b.Get2DimArray();
            b.Set2DimArray(d);
            var c = false;
        }
    }
}
