using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftsManager.Aircraft.Strategy.ConcreteStrategies
{
    sealed class ConcreteAircraftStrategyF17 : Strategy.AircraftStrategy
    {
        internal ConcreteAircraftStrategyF17()
        {
            // TODO: file reader
            //this.longitudinalData = longitudinalData;
            //this.lateralData = lateralData;
        }
        internal override float[,] GetLateralData()
        {
            throw new NotImplementedException();
        }

        internal override float[,] GetLongitudinalData()
        {
            throw new NotImplementedException();
        }
    }
}
