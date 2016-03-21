using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Containers;

namespace DataParser
{
    public interface IDataParser
    {
        string Serialize(IData data);
        IData Deserialize(string data);
    }
}
