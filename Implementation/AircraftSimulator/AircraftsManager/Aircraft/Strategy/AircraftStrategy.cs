using Common.Containers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AircraftsManager.Aircraft.Strategy
{
    abstract class AircraftStrategy : Common.Strategy
    {
        protected string longitudinalDataFileName;
        protected string lateralDataFileName;
        protected IData longitudinalData;
        protected IData lateralData;
        internal abstract IData GetLongitudinalData();
        internal abstract IData GetLateralData();

        protected override void Initialize()
        {
            lateralData = GetData(lateralDataFileName);
            longitudinalData = GetData(longitudinalDataFileName);

        }

        private IData GetData(string dataFileName)
        {
            IData data = null;
            try
            {
                using (TextReader reader = new StreamReader(@"..\..\PRACA_MAG\Implementation\AircraftSimulator\AppInput\XMLFiles\" + dataFileName))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Data));
                    data = (IData)serializer.Deserialize(reader);
                }
            }
            catch (Exception e)
            {
                throw new global::Common.Exceptions.XMLFileNotFoundException();
            }
            return data;
        }
    }
}
