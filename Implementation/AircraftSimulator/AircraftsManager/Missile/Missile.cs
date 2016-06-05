using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace AircraftsManager.Missile
{
    sealed class Missile : Initializer
    {
        private MissileType missileType;
        private Common.Context missileFlightContext;

        internal MissileType MissileType
        {
            get
            {
                return missileType;
            }
        }

        internal Common.Context MissileFlightContext
        {
            get
            {
                return missileFlightContext;
            }
        }

        internal Missile(MissileType missileType, int targetId)
        {
            this.missileType = missileType;
            Initialize();
            ((this.missileFlightContext).Strategies[0] as Strategy.MissileStrategy).TargetID = targetId;
        }

        protected override void Initialize()
        {
            this.missileFlightContext = new Strategy.MissileFlightContext(this.missileType);
        }
    }
}
