using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Common;
using Common.Containers;

namespace Common.DataParser
{
    public class DataParser<T> : Initializer, IDataParser
    {
        private XmlSerializer serializer;
        public DataParser()
        {
            Initialize();
        }


        protected override void Initialize()
        {
            serializer = new XmlSerializer(typeof(T));
        }

        public string Serialize(IDataList data)
        {
            string toRet = String.Empty;
            try
            {
                using (StringWriter stringWriter = new StringWriter())
                {
                    using (XmlWriter dataWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings() {Indent = false}))
                    //using (XmlWriter dataWriter = XmlWriter.Create(stringWriter))
                    {
                        serializer.Serialize(dataWriter, data);
                        toRet = stringWriter.ToString();
                    }
                }
            }
            catch (Exception e)
            {
                string a = e.Message;
            }
            return toRet;
        }

        public IDataList Deserialize(string data)
        {
            IDataList toRet = null;
            try
            {
                using (StringReader dataReader = new StringReader(data))
                {
                    toRet = serializer.Deserialize(dataReader) as IDataList;
                }
            }
            catch (Exception e)
            {
                string a = e.Message;
            }
            return toRet;
        }

    }
}
