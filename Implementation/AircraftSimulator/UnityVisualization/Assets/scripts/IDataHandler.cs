using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Containers;

namespace Assets.scripts
{
    public delegate void ClientResponseHandler();
    public interface IDataHandler
    {
        event ClientResponseHandler ClientResponseHandler;
        event ClientResponseHandler ClientDisconnected;

        void OnClientJoinResponse();
        void OnServerDataResponse(IDataList dataList);
        void OnServerDisconnected();

        

    }
}
