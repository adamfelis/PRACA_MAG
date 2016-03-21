using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public interface IServer
    {
        IServerInputPriveleges ServerInputPrivileges { get; }
        void StartServer();
        void StopServer();
    }
}
