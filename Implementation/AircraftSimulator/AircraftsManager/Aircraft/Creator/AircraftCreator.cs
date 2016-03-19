using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AircraftsManager.Shooter;

namespace AircraftsManager.Aircraft.Creator
{
    class AircraftCreator : Shooter.ShooterFactory.ShooterCreator
    {
        public override Shooter.Shooter ShooterFactoryMethod(ShooterType shooterType)
        {
            return new Aircraft(shooterType);
        }
    }
}
