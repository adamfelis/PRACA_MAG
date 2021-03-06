﻿using System;
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
        protected Common.Context context;
        protected Shooters.ShooterType shooterType;

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

        internal void AddMissile(Missile.MissileType missileType, int missile_id, int targetId)
        {
            this.activeMissiles.Add(missile_id, new Missile.Missile(missileType, targetId));
        }

        internal Missile.Missile GetMissile(int missileId)
        {
            if (!this.activeMissiles.ContainsKey(missileId))
                throw new Missile.InvalidMissileIdException();
            return activeMissiles[missileId];
        }
    }
}
