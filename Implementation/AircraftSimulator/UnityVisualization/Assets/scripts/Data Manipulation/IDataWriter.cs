using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.AircraftData;
using Common.Containers;

namespace Assets.scripts.Data_Manipulation
{
    public interface IDataWriter
    {
        void SendJoinMessage(string localIPAddress);
        void SendAircraftRequestToServer();
        void SendMissileRequestToServer(MissileData missileData);
        void SendMissileFiredToServer(MissileData missileData);
        void DisconnectFromServer();
    }
}
