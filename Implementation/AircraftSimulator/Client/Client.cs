using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Priveleges;
using Common;

namespace Client
{
    public class Client : Initializer, IClient
    {
        private IClientOutputPrivileges _clientOutputPrivileges;
        public IClientOutputPrivileges ClientOutputPrivileges => _clientOutputPrivileges;
        public Client(IClientPrivileges clientPrivileges)
        {
            _clientOutputPrivileges = new ClientOutputPrivileges();
            _clientOutputPrivileges.Subscribe(clientPrivileges);
            Initialize();
        }

        public Client()
        {
            Initialize();
        }

        protected override void Initialize()
        {

        }
    }
}
