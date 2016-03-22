using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public interface IClientConnection
    {
        int Id { get; }

        string ClientName { get; set; }

        void SendMessage(string data);
    }
}
