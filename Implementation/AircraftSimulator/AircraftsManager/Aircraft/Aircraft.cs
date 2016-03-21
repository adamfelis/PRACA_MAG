using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftsManager.Aircraft
{
    sealed class Aircraft : Shooter.Shooter
    {
        public Shooter.ShooterType AircraftType
        {
            get
            {
                return shooterType;
            }
        }

        public Aircraft(Shooter.ShooterType aircraftType)
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
