﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.scripts.Model;
using Common.Containers;

namespace Assets.scripts
{
    public delegate void ClientResponseHandler();
    public interface IDataHandler
    {
        event ClientResponseHandler ClientResponseHandler;
        event ClientResponseHandler ClientDisconnected;

        void OnClientJoinResponse(IData data);
        void OnServerDataResponse(IDataList dataList);
        void OnServerDisconnected();

        

    }
}
