using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Containers;
using Common.EventArgs;
using Common.DataParser;
using Common.DataParser;
using Patterns.Executors;

namespace DataStorageNamespace
{
    public class DataStorage : Initializer, IDataStorage
    {
        private IDataParser _dataParser;
        public IDictionary<int, Queue<IData>> ClientRequests { get; }
        public IDataList PrepareDataReceivedFromClient(int id, string data)
        {
            IDataList readableData = _dataParser.Deserialize(data);
            if (ClientRequests.ContainsKey(id))
                ClientRequests[id].Enqueue(readableData.DataArray.First());
            return readableData;
        }

        public string PrepareDataForClient(int id, IDataList dataList)
        {
            if (!ClientRequests.ContainsKey(id))
                throw new MessageIgnoreException();
            if (ClientRequests[id].Count > 0)
            {
                IData request = ClientRequests[id].Dequeue();
                if (request.Response == ActionType.NoResponse)
                    throw new MessageIgnoreException();
                foreach (IData data in dataList.DataArray)
                {
                    data.InputType = request.OutputType;
                }
            }
            string toSend="";
            try
            {
               toSend = _dataParser.Serialize(dataList);
            }
            catch (Exception e)
            {
                string a = e.Message;
            }
          
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
            ClientRequests[args.Id].Enqueue(args.DataList.DataArray.First());
        }

        public void ClientRemoved(object sender, DataEventArgs args)
        {
            //ClientRequests.Remove(args.Id);
        }

        protected override void Initialize()
        {
            _dataParser = new DataParser<DataList>();
            //TOREMOVE
            //var a = _dataParser.Serialize(new DataList()
            //{
            //    DataArray = new []
            //    {
            //        new Data()
            //        {
            //            Array = new float[][]
            //            {
            //                new float[3]
            //                {
            //                    1, 2, 3
            //                },
            //                new float[3]
            //                {
            //                    4, 5, 6
            //                }
            //            }
            //        }
            //    }
            //});
            //var k = _dataParser.Deserialize(a);
            //var b = _dataParser.Deserialize(a).DataArray.First();
            //var n = b.N;
            //var m = b.M;
            //var d = b.Get2DimArray();
            //b.Set2DimArray(d);
            //var c = false;
        }
    }
}
