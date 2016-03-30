using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftsManager.Missile.Strategy
{
    abstract class MissileStrategy : Common.Strategy
    {
        private MissileType missileType;

        internal MissileType MissileType
        {
            get
            {
                return missileType;
            }
            set
            {
                missileType = value;
            }
        }

        protected override void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}
