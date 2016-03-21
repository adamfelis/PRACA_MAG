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
    }
}
