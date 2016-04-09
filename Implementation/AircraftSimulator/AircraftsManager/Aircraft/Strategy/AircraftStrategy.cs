using Common.Containers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Common.DataParser;

namespace AircraftsManager.Aircraft.Strategy
{
    abstract class AircraftStrategy : Common.Strategy
    {
        protected string longitudinalDataFileName;
        protected string lateralDataFileName;
        protected IData longitudinalData;
        protected IData lateralData;
        protected AircraftParameters aircraftParameters;
        internal abstract IData GetLongitudinalData(IData additionalInformation = null);
        internal abstract IData GetLateralData(IData additionalInformation = null);

        protected override void Initialize()
        {
            //aircraftParameters = GetData()
            //lateralData = GetData(lateralDataFileName);
            //longitudinalData = GetData(longitudinalDataFileName);
        }

        private AircraftParameters GetData(string dataFileName)
        {
            AircraftParameters parameters = null;
            try
            {
                using (TextReader reader = new StreamReader(@"..\..\PRACA_MAG\Implementation\AircraftSimulator\AppInput\XMLFiles\" + dataFileName))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(AircraftParameters));
                    parameters = (AircraftParameters)serializer.Deserialize(reader);
                }
            }
            catch (Exception e)
            {
                throw new global::Common.Exceptions.XMLFileNotFoundException();
            }
            return parameters;
        }
    }
}
