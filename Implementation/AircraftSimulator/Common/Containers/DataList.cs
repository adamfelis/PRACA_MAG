using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Common.Containers
{
    [Serializable]
    public class DataList : IDataList
    {
        public Data[] DataArray { get; set; }
    }
}
