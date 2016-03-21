using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace AircraftsManager.Shooter
{
    abstract class Shooter : Initializer
    {
        protected Dictionary<int, Missile.Missile> activeMissiles;
        public Common.Context context;
        protected ShooterType shooterType;

        protected override void Initialize()
        {
            this.activeMissiles = new Dictionary<int, Missile.Missile>();
        }

        public void AddMissile(Missile.MissileType missileType)
        {
            this.activeMissiles.Add(activeMissiles.Count, new Missile.Missile(missileType));
        }

        public Missile.Missile GetMissile(int missileId)
        {
            if (!this.activeMissiles.ContainsKey(missileId))
                throw new Missile.InvalidMissileIdException();
            return activeMissiles[missileId];
        }
    }
}
