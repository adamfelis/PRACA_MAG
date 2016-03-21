using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftsManager.Missile.Strategy
{
    sealed class MissileFlightContext : Common.Context
    {
        public MissileFlightContext(MissileType missileType)
        {
            this.Initialize();
            this.AddStrategy(MissileStrategy.GetSpecificMissileStrategy(missileType));
        }
    }
}
