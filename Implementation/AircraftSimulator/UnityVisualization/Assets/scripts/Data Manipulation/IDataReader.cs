using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.Priveleges;
using Common.EventArgs;

namespace Assets.scripts.Data_Manipulation
{
    public interface IDataReader : IClientPrivileges
    {
        void ReadDataSentFromServer();
    }
}
