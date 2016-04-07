using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Containers;

namespace Common.DataParser
{
    public interface IDataParser
    {
        string Serialize(IData data);
        IData Deserialize(string data);
    }
}
