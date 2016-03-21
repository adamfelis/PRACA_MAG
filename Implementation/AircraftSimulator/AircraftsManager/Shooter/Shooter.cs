using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftsManager.Shooter
{
    abstract class Shooter : Common.Initializer
    {
        protected Dictionary<int, Missile.Missile> activeMissiles;
        protected Common.Context context;
        protected ShooterType shooterType;

        internal Common.Context Context
        {
            get
            {
                return context;
            }
        }

        protected override void Initialize()
        {
            this.activeMissiles = new Dictionary<int, Missile.Missile>();
        }

        internal void AddMissile(Missile.MissileType missileType)
        {
            this.activeMissiles.Add(activeMissiles.Count, new Missile.Missile(missileType));
        }

        internal Missile.Missile GetMissile(int missileId)
        {
            if (!this.activeMissiles.ContainsKey(missileId))
                throw new Missile.InvalidMissileIdException();
            return activeMissiles[missileId];
        }
    }
}
