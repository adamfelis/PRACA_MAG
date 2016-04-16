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
        //protected string longitudinalDataFileName;
        //protected string lateralDataFileName;

        protected string aircraftDataFileName;
        protected List<IData> longitudinalData;
        protected List<IData> lateralData;
        protected AircraftParameters aircraftParameters;
        internal abstract List<IData> GetLongitudinalData(IData additionalInformation = null);
        internal abstract List<IData> GetLateralData(IData additionalInformation = null);

        protected override void Initialize()
        {
            aircraftParameters = GetData(aircraftDataFileName);
            lateralData = null;
            longitudinalData = null;
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

        protected List<IData> PrepareLongitudinalData(IData additionalInformation)
        {
            float U_e = additionalInformation.Array[0][0];
            float W_e = additionalInformation.Array[0][1];
            float theta_e = additionalInformation.Array[0][2];

            List<IData> preparedLongitudinalData = new List<IData>();

            const int A_size = 4;
            const float g = 9.81f;
            float[][] A_dataArray = new float[A_size][];
            for(int i = 0; i < A_size; i++)
            {
                A_dataArray[i] = new float[A_size];
            }

            A_dataArray[0][0] = (1.0f / aircraftParameters.m) * 
                (aircraftParameters.X_dot_u + 
                (aircraftParameters.X_dot_w_dot * aircraftParameters.Z_dot_u) / (aircraftParameters.m - aircraftParameters.Z_dot_w_dot)
                );
            A_dataArray[0][1] = (1.0f / aircraftParameters.m) *
                (aircraftParameters.X_dot_w +
                (aircraftParameters.X_dot_w_dot * aircraftParameters.Z_dot_w) / (aircraftParameters.m - aircraftParameters.Z_dot_w_dot)
                );
            A_dataArray[0][2] = (1.0f / aircraftParameters.m) *
                (aircraftParameters.X_dot_q - aircraftParameters.m * W_e+
                (aircraftParameters.Z_dot_q + aircraftParameters.m * U_e) * aircraftParameters.X_dot_w_dot / (aircraftParameters.m - aircraftParameters.Z_dot_w_dot)
                );
            A_dataArray[0][3] =
                (-g * (float)Math.Cos(theta_e) -
                (aircraftParameters.X_dot_w_dot * g * (float)Math.Sin(theta_e) / (aircraftParameters.m - aircraftParameters.Z_dot_w_dot))
                );

            A_dataArray[1][0] = aircraftParameters.Z_dot_u / (aircraftParameters.m - aircraftParameters.Z_dot_w_dot);
            A_dataArray[1][1] = aircraftParameters.Z_dot_w / (aircraftParameters.m - aircraftParameters.Z_dot_w_dot);
            A_dataArray[1][2] = (aircraftParameters.Z_dot_q + aircraftParameters.m * U_e) / (aircraftParameters.m - aircraftParameters.Z_dot_w_dot);
            A_dataArray[1][3] = (- aircraftParameters.m * g * (float)Math.Sin(theta_e)) / (aircraftParameters.m - aircraftParameters.Z_dot_w_dot);

            A_dataArray[2][0] =
                (aircraftParameters.M_dot_w_dot * aircraftParameters.Z_dot_u) / (aircraftParameters.I_y * (aircraftParameters.m - aircraftParameters.Z_dot_w_dot)) +
                (aircraftParameters.M_dot_u) / (aircraftParameters.I_y);
            A_dataArray[2][1] =
                (aircraftParameters.M_dot_w_dot * aircraftParameters.Z_dot_w) / (aircraftParameters.I_y * (aircraftParameters.m - aircraftParameters.Z_dot_w_dot)) +
                (aircraftParameters.M_dot_w) / (aircraftParameters.I_y);
            A_dataArray[2][2] =
                (aircraftParameters.M_dot_w_dot * (aircraftParameters.Z_dot_q + aircraftParameters.m * U_e)) / (aircraftParameters.I_y * (aircraftParameters.m - aircraftParameters.Z_dot_w_dot)) +
                (aircraftParameters.M_dot_q) / (aircraftParameters.I_y);
            A_dataArray[2][3] =
                (-aircraftParameters.M_dot_w_dot * aircraftParameters.m * g * (float)Math.Sin(theta_e)) / (aircraftParameters.I_y * (aircraftParameters.m - aircraftParameters.Z_dot_w_dot));

            A_dataArray[3][0] = 0.0f;
            A_dataArray[3][1] = 0.0f;
            A_dataArray[3][2] = 1.0f;
            A_dataArray[3][3] = 0.0f;

            const int B_size = 2;
            float[][] B_dataArray = new float[A_size][];
            for (int i = 0; i < A_size; i++)
            {
                B_dataArray[i] = new float[B_size];
            }


            B_dataArray[0][0] = (1 / aircraftParameters.m) *
                (aircraftParameters.X_dot_ni + (aircraftParameters.X_dot_w_dot * aircraftParameters.Z_dot_ni) / (aircraftParameters.m - aircraftParameters.Z_dot_w_dot));
            B_dataArray[0][1] = (1 / aircraftParameters.m) *
                (aircraftParameters.X_dot_tau + (aircraftParameters.X_dot_w_dot * aircraftParameters.Z_dot_tau) / (aircraftParameters.m - aircraftParameters.Z_dot_w_dot));

            B_dataArray[1][0] = aircraftParameters.Z_dot_ni / (aircraftParameters.m - aircraftParameters.Z_dot_w_dot);
            B_dataArray[1][1] = aircraftParameters.Z_dot_tau / (aircraftParameters.m - aircraftParameters.Z_dot_w_dot);

            B_dataArray[2][0] = (1 / aircraftParameters.I_y) *
                ((aircraftParameters.M_dot_w_dot * aircraftParameters.Z_dot_ni) / (aircraftParameters.m - aircraftParameters.Z_dot_w_dot) + aircraftParameters.M_dot_ni);
            B_dataArray[2][1] = (1 / aircraftParameters.I_y) *
                ((aircraftParameters.M_dot_w_dot * aircraftParameters.Z_dot_tau) / (aircraftParameters.m - aircraftParameters.Z_dot_w_dot) + aircraftParameters.M_dot_tau);

            B_dataArray[3][0] = 0.0f;
            B_dataArray[3][1] = 0.0f;

            preparedLongitudinalData.Add(new Data() { InputType = DataType.Matrix, Array = A_dataArray, Sender="A" });
            preparedLongitudinalData.Add(new Data() { InputType = DataType.Matrix, Array = B_dataArray, Sender="B" });

            return preparedLongitudinalData;
        }
    }
}
