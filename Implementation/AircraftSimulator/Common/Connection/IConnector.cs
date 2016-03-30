using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Connection
{
    public interface IConnector
    {
        void SendMessage(string data);
    }
}
