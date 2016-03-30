using AircraftsManager.Missile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftsManager.Common
{
    abstract class Strategy : global::Common.Initializer
    {
        protected Shooter.ShooterType shooterType;

        internal Shooter.ShooterType ShooterType
        {
            get
            {
                return shooterType;
            }
        }

        internal static Common.Strategy GetSpecificShooterStrategy(Shooter.ShooterType shooterType)
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
            strategy.shooterType = shooterType;
            return strategy;
        }

        internal static Common.Strategy GetSpecificMissileStrategy(MissileType missileType)
        {
            Common.Strategy strategy = null;
            switch (missileType)
            {
                case MissileType.M1:
                    strategy = new Missile.Strategy.ConcreteStrategies.ConcreteMissileStrategyM1();
                    (strategy as Missile.Strategy.ConcreteStrategies.ConcreteMissileStrategyM1).MissileType = missileType;
                    break;
                case MissileType.M2:
                    strategy = new Missile.Strategy.ConcreteStrategies.ConcreteMissileStrategyM2();
                    (strategy as Missile.Strategy.ConcreteStrategies.ConcreteMissileStrategyM2).MissileType = missileType;
                    break;
                default:
                    throw new InvalidMissileTypeException();
            }

            return strategy;
        }
    }
}
