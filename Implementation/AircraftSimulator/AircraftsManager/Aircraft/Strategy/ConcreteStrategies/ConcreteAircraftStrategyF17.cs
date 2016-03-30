﻿using Common.Containers;
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
            this.lateralDataFileName = "F17_lateral.xml";
            this.longitudinalDataFileName = "F17_longitudinal.xml";
            Initialize();
        }
        internal override IData GetLateralData()
        {
            throw new NotImplementedException();
        }

        internal override IData GetLongitudinalData()
        {
            throw new NotImplementedException();
        }
    }
}
