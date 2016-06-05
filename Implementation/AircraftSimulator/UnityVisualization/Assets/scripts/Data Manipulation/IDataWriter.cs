using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Containers;

namespace Assets.scripts.Data_Manipulation
{
    public interface IDataWriter
    {
        void SendJoinMessage(string localIPAddress);
        void SendAircraftRequestToServer();
        void SendMissileRequestToServer(int shooterId, int targetId, int missileId);
        void SendMissileFiredToServer(int shooterId, int targetId, int missileId);
        void DisconnectFromServer();
    }
}
