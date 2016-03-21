using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftsManager.Missile.Strategy
{
    abstract class MissileStrategy : Common.Strategy
    {
        private MissileType missileType;

        public MissileType MissileType
        {
            get
            {
                return missileType;
            }
        }

        public static Common.Strategy GetSpecificMissileStrategy(MissileType missileType)
        {
            Common.Strategy strategy = null;
            switch (missileType)
            {
                case MissileType.M1:
                    strategy = new Strategy.ConcreteStrategies.ConcreteMissileStrategyM1();
                    (strategy as Strategy.ConcreteStrategies.ConcreteMissileStrategyM1).missileType = missileType;
                    break;
                case MissileType.M2:
                    strategy = new Strategy.ConcreteStrategies.ConcreteMissileStrategyM2();
                    (strategy as Strategy.ConcreteStrategies.ConcreteMissileStrategyM2).missileType = missileType;
                    break;
                default:
                    throw new InvalidMissileTypeException();
            }

            return strategy;
        }
    }
}
