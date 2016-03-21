﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.EventArgs;

namespace Server
{
    public delegate void AddClientHandler(object sender, ClientEventArgs eventHandler);
    public delegate void RemoveClientHandler(object sender, ClientEventArgs eventHandler);
    public delegate void PresentDataOfTheClientHandler(object sender, DataEventArgs eventHandler);
    public interface IServerPrivileges
    {
        AddClientHandler OnClientAdded { get; set; }
        RemoveClientHandler OnClientRemoved { get; set; }
        PresentDataOfTheClientHandler OnClientDataPresented { get; set; }
    }
}