using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.Collections.ObjectModel;

namespace AircraftsManager.Common
{
    abstract class Context : Initializer
    {
        protected List<Strategy> strategies;

        internal List<Strategy> Strategies
        {
            get
            {
                return strategies;
            }
        }

        internal void AddStrategy(Common.Strategy strategy)
        {
            foreach (Strategy s in strategies)
            {
                if (s.ShooterType == strategy.ShooterType)
                    throw new InvalidShooterTypeException();
            }
            this.strategies.Add(strategy);
        }

        internal void RemoveStrategy(Shooters.ShooterType shooterType)
        {
            if (strategies[0].ShooterType == shooterType)
                return;
            Strategy strategyToRemove = null;
            foreach (Strategy strategy in strategies)
            {
                if (strategy.ShooterType == shooterType)
                {
                    strategyToRemove = strategy;
                    break;
                }
            }
            if (strategyToRemove == null)
                throw new InvalidShooterTypeException();
            this.strategies.Remove(strategyToRemove);
        }

        protected override void Initialize()
        {
            this.strategies = new List<Strategy>();
        }
    }
}
