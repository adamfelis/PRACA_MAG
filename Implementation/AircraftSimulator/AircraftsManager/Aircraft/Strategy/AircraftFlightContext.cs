using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftsManager.Aircraft.Strategy
{
    sealed class AircraftFlightContext : Common.Context
    {
        public AircraftFlightContext(Shooter.ShooterType shooterType)
        {
            this.Initialize();
            this.AddStrategy(Common.Strategy.GetSpecificShooterStrategy(shooterType));
        }
    }
}
