using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Common;
using Common.Containers;

namespace DataParser
{
    public class DataParser : Initializer, IDataParser
    {
        private XmlSerializer serializer;
        public DataParser()
        {
            Initialize();
        }


        protected override void Initialize()
        {
            serializer = new XmlSerializer(typeof(Data));
        }

        public string Serialize(IData data)
        {
            string toRet = String.Empty;
            using (StringWriter dataWriter = new StringWriter())
            {
                serializer.Serialize(dataWriter, data);
                toRet = dataWriter.ToString();
            }
            return toRet;
        }

        public IData Deserialize(string data)
        {
            IData toRet;
            using (StringReader dataReader = new StringReader(data))
            {
                toRet = serializer.Deserialize(dataReader) as IData;
            }
            return toRet;
        }

    }
}
