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
        public Common.Context context;
        protected ShooterType shooterType;
    }
}
