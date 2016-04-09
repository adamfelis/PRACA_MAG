using Common.Containers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AircraftsManager.Aircraft.Strategy.ConcreteStrategies
{
    sealed class ConcreteAircraftStrategyF16 : Strategy.AircraftStrategy
    {
        internal ConcreteAircraftStrategyF16()
        {
            this.lateralDataFileName = "F16_lateral.xml";
            this.longitudinalDataFileName = "F16_longitudinal.xml";
            Initialize();
            //data = new Data();
            //data.InputType = DataType.Matrix;
            //data.Set2DimArray(new float[,]
            //{
            //    { 1, 2, 3, 4 },
            //    { 5, 6, 7, 8 },
            //    { 9, 10, 11, 12 }
            //});
            //XmlSerializer serializer = new XmlSerializer(typeof(Data));
            //serializer.Serialize(File.Create(@"..\..\PRACA_MAG\Implementation\AircraftSimulator\AppInput\XMLFiles\F16_2.xml"), data);
        }

        internal override IData GetLateralData(IData additionalInformation = null)
        {
            return this.lateralData;
        }

        internal override IData GetLongitudinalData(IData additionalInformation = null)
        {
            return this.longitudinalData;
        }
    }
}
