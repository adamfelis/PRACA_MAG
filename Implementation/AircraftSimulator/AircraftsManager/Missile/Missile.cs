using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftsManager.Missile
{
    sealed class Missile : Common.Initializer
    {
        private MissileType missileType;
        private Common.Context missileFlightContext;

        public MissileType MissileType
        {
            get
            {
                return missileType;
            }
        }

        public Common.Context MissileFlightContext
        {
            get
            {
                return missileFlightContext;
            }
        }

        public Missile(MissileType missileType)
        {
            this.missileType = missileType;
            Initialize();
        }

        protected override void Initialize()
        {
            this.missileFlightContext = new Strategy.MissileFlightContext(this.missileType);
        }
    }
}
