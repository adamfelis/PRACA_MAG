﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    public interface IClient
    {
        string ConnectToServer();

        void DisconnectFromServer();
    }
}
