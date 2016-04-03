using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftsManager.Aircraft
{
    sealed class Aircraft : Shooter.Shooter
    {
        internal Shooters.ShooterType AircraftType
        {
            get
            {
                return shooterType;
            }
        }

        internal Aircraft(Shooters.ShooterType aircraftType)
        {
            this.shooterType = aircraftType;
            Initialize();
        }

        protected override void Initialize()
        {
            base.Initialize();
            this.context = new Strategy.AircraftFlightContext(shooterType);
        }
    }
}
