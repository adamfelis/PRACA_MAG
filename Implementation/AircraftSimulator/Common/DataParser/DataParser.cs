﻿using System;
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

        public string Serialize(IData data)
        {
            string toRet = String.Empty;
            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlWriter dataWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings() {Indent = false}))
                {
                    serializer.Serialize(dataWriter, data);
                    toRet = stringWriter.ToString();
                }
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