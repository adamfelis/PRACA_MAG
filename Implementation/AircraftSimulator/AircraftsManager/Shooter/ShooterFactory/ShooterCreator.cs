using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftsManager.Shooter.ShooterFactory
{
    abstract class ShooterCreator
    {
        internal abstract Shooter ShooterFactoryMethod(Shooters.ShooterType shooterType);
    }
}
