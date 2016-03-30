using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Connection;

namespace Server
{
    public interface IClientConnection : IConnector
    {
        int Id { get; }

        string ClientName { get; set; }
    }
}
