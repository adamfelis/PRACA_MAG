using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

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
            this.strategies.Add(strategy);
        }

        protected override void Initialize()
        {
            this.strategies = new List<Strategy>();
        }
    }
}
