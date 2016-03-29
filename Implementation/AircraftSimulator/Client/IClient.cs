﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.Priveleges;
using Common.Containers;

namespace Client
{
    public interface IClient
    {
        IClientInputPriveleges ClientInputPriveleges { get; }
        string ConnectToServer();
        void DisconnectFromServer();
    }
}
