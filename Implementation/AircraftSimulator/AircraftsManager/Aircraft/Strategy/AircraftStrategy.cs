using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftsManager.Aircraft.Strategy
{
    abstract class AircraftStrategy : Common.Strategy
    {
        protected float[,] longitudinalData;
        protected float[,] lateralData;
        public abstract float[,] GetLongitudinalData();
        public abstract float[,] GetLateralData();
    }
}
