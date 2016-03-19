using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftsManager.Common
{
    abstract class Strategy
    {
        public static Common.Strategy GetSpecificStrategy(Shooter.ShooterType shooterType)
        {
            Common.Strategy strategy = null;
            switch (shooterType)
            {
                case Shooter.ShooterType.F16:
                    strategy = new Aircraft.Strategy.ConcreteStrategies.ConcreteAircraftStrategyF16();
                    break;
                case Shooter.ShooterType.F17:
                    strategy = new Aircraft.Strategy.ConcreteStrategies.ConcreteAircraftStrategyF17();
                    break;
                default:
                    throw new Common.InvalidShooterTypeException();
            }
            return strategy;
        }
    }
}
