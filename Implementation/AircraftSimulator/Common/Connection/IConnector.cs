using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Containers;
using Common.Exceptions;

namespace Common.Connection
{
    public interface IConnector
    {
        void SendMessage(string data);

        void onDisconnected(ErrorCodeException e);

        void CloseConnection();
    }
}
