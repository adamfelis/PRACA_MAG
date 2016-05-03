using Common.Containers;
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
            this.aircraftDataFileName = "F17.xml";
            //this.lateralDataFileName = "F17_lateral.xml";
            //this.longitudinalDataFileName = "F17_longitudinal.xml";
            Initialize();
        }
        internal override List<IData> GetLateralData(IData additionalInformation = null)
        {
            return this.lateralData;
        }

        internal override List<IData> GetLongitudinalData(IData additionalInformation = null)
        {
            return this.lateralData;
        }

        internal override List<IData> GetLateralInitialData(IData additionalInformation)
        {
            return PrepareLateralMatrixes(additionalInformation);
        }

        internal override List<IData> GetLongitudinalInitialData(IData additionalInformation)
        {
            return PrepareLongitudinalMatrixes(additionalInformation);
        }
    }
}
