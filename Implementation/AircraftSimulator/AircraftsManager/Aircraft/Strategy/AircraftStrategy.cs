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
        private float U_e;
        private float W_e;
        private float Q_e;
        private float theta_e;
        private float ni;
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
            int simulation_Index = (int)additionalInformation.Array[2][0];
            if (simulation_Index == 1 || additionalInformation.Array[1][0] != ni)
            {
                U_e = additionalInformation.Array[0][0];
                W_e = additionalInformation.Array[0][1];
                Q_e = additionalInformation.Array[0][2];
                theta_e = additionalInformation.Array[0][3];
                ni = additionalInformation.Array[1][0];
            }
            
            float tau = additionalInformation.Array[1][1];

            


            List<IData> preparedLongitudinalData = new List<IData>();

            //const int A_size = 4;
            //const float g = 9.81f;
            //float[][] A_dataArray = new float[A_size][];
            //for(int i = 0; i < A_size; i++)
            //{
            //    A_dataArray[i] = new float[A_size];
            //}

            //A_dataArray[0][0] = (1.0f / aircraftParameters.m) * 
            //    (aircraftParameters.X_dot_u + 
            //    (aircraftParameters.X_dot_w_dot * aircraftParameters.Z_dot_u) / (aircraftParameters.m - aircraftParameters.Z_dot_w_dot)
            //    );
            //A_dataArray[0][1] = (1.0f / aircraftParameters.m) *
            //    (aircraftParameters.X_dot_w +
            //    (aircraftParameters.X_dot_w_dot * aircraftParameters.Z_dot_w) / (aircraftParameters.m - aircraftParameters.Z_dot_w_dot)
            //    );
            //A_dataArray[0][2] = (1.0f / aircraftParameters.m) *
            //    (aircraftParameters.X_dot_q - aircraftParameters.m * W_e+
            //    (aircraftParameters.Z_dot_q + aircraftParameters.m * U_e) * aircraftParameters.X_dot_w_dot / (aircraftParameters.m - aircraftParameters.Z_dot_w_dot)
            //    );
            //A_dataArray[0][3] =
            //    (-g * (float)Math.Cos(theta_e) -
            //    (aircraftParameters.X_dot_w_dot * g * (float)Math.Sin(theta_e) / (aircraftParameters.m - aircraftParameters.Z_dot_w_dot))
            //    );

            //A_dataArray[1][0] = aircraftParameters.Z_dot_u / (aircraftParameters.m - aircraftParameters.Z_dot_w_dot);
            //A_dataArray[1][1] = aircraftParameters.Z_dot_w / (aircraftParameters.m - aircraftParameters.Z_dot_w_dot);
            //A_dataArray[1][2] = (aircraftParameters.Z_dot_q + aircraftParameters.m * U_e) / (aircraftParameters.m - aircraftParameters.Z_dot_w_dot);
            //A_dataArray[1][3] = (- aircraftParameters.m * g * (float)Math.Sin(theta_e)) / (aircraftParameters.m - aircraftParameters.Z_dot_w_dot);

            //A_dataArray[2][0] =
            //    (aircraftParameters.M_dot_w_dot * aircraftParameters.Z_dot_u) / (aircraftParameters.I_y * (aircraftParameters.m - aircraftParameters.Z_dot_w_dot)) +
            //    (aircraftParameters.M_dot_u) / (aircraftParameters.I_y);
            //A_dataArray[2][1] =
            //    (aircraftParameters.M_dot_w_dot * aircraftParameters.Z_dot_w) / (aircraftParameters.I_y * (aircraftParameters.m - aircraftParameters.Z_dot_w_dot)) +
            //    (aircraftParameters.M_dot_w) / (aircraftParameters.I_y);
            //A_dataArray[2][2] =
            //    (aircraftParameters.M_dot_w_dot * (aircraftParameters.Z_dot_q + aircraftParameters.m * U_e)) / (aircraftParameters.I_y * (aircraftParameters.m - aircraftParameters.Z_dot_w_dot)) +
            //    (aircraftParameters.M_dot_q) / (aircraftParameters.I_y);
            //A_dataArray[2][3] =
            //    (-aircraftParameters.M_dot_w_dot * aircraftParameters.m * g * (float)Math.Sin(theta_e)) / (aircraftParameters.I_y * (aircraftParameters.m - aircraftParameters.Z_dot_w_dot));

            //A_dataArray[3][0] = 0.0f;
            //A_dataArray[3][1] = 0.0f;
            //A_dataArray[3][2] = 1.0f;
            //A_dataArray[3][3] = 0.0f;

            //const int B_size = 2;
            //float[][] B_dataArray = new float[A_size][];
            //for (int i = 0; i < A_size; i++)
            //{
            //    B_dataArray[i] = new float[B_size];
            //}


            //B_dataArray[0][0] = (1 / aircraftParameters.m) *
            //    (aircraftParameters.X_dot_ni + (aircraftParameters.X_dot_w_dot * aircraftParameters.Z_dot_ni) / (aircraftParameters.m - aircraftParameters.Z_dot_w_dot));
            //B_dataArray[0][1] = (1 / aircraftParameters.m) *
            //    (aircraftParameters.X_dot_tau + (aircraftParameters.X_dot_w_dot * aircraftParameters.Z_dot_tau) / (aircraftParameters.m - aircraftParameters.Z_dot_w_dot));

            //B_dataArray[1][0] = aircraftParameters.Z_dot_ni / (aircraftParameters.m - aircraftParameters.Z_dot_w_dot);
            //B_dataArray[1][1] = aircraftParameters.Z_dot_tau / (aircraftParameters.m - aircraftParameters.Z_dot_w_dot);

            //B_dataArray[2][0] = (1 / aircraftParameters.I_y) *
            //    ((aircraftParameters.M_dot_w_dot * aircraftParameters.Z_dot_ni) / (aircraftParameters.m - aircraftParameters.Z_dot_w_dot) + aircraftParameters.M_dot_ni);
            //B_dataArray[2][1] = (1 / aircraftParameters.I_y) *
            //    ((aircraftParameters.M_dot_w_dot * aircraftParameters.Z_dot_tau) / (aircraftParameters.m - aircraftParameters.Z_dot_w_dot) + aircraftParameters.M_dot_tau);

            //B_dataArray[3][0] = 0.0f;
            //B_dataArray[3][1] = 0.0f;

            const int A_size = 4;
            const float g = 9.81f;
            float S = 49.239f;
            float c = 4.889f;
            float V0 = (float)Math.Sqrt(U_e * U_e + W_e * W_e);
            float m_prim = aircraftParameters.m / (0.5f * aircraftParameters.p * S);
            float I_y_prim = aircraftParameters.I_y / (0.5f * aircraftParameters.p * S * c);

            float[][] A_dataArray = new float[A_size][];
            for (int i = 0; i < A_size; i++)
            {
                A_dataArray[i] = new float[A_size];
            }

            A_dataArray[0][0] = (1.0f / m_prim) *
                (aircraftParameters.X_dot_u +
                (aircraftParameters.X_dot_w_dot * aircraftParameters.Z_dot_u * c) / (V0 * m_prim - aircraftParameters.Z_dot_w_dot * c)
                );
            A_dataArray[0][1] = (1.0f / m_prim) *
                (aircraftParameters.X_dot_w +
                (aircraftParameters.X_dot_w_dot * aircraftParameters.Z_dot_w * c) / (V0 * m_prim - aircraftParameters.Z_dot_w_dot *c)
                );
            A_dataArray[0][2] = (1.0f / m_prim) *
                (aircraftParameters.X_dot_q * c - m_prim * W_e +
                (aircraftParameters.Z_dot_q * c + m_prim * U_e) * aircraftParameters.X_dot_w_dot * c / (V0 * m_prim - aircraftParameters.Z_dot_w_dot * c)
                );
            A_dataArray[0][3] =
                (-g * (float)Math.Cos(theta_e) -
                (aircraftParameters.X_dot_w_dot * c * g * (float)Math.Sin(theta_e) / (V0 * m_prim - aircraftParameters.Z_dot_w_dot * c))
                );

            A_dataArray[1][0] = aircraftParameters.Z_dot_u * V0 / (V0 * m_prim - aircraftParameters.Z_dot_w_dot * c);
            A_dataArray[1][1] = aircraftParameters.Z_dot_w * V0 / (V0 * m_prim - aircraftParameters.Z_dot_w_dot * c);
            A_dataArray[1][2] = (aircraftParameters.Z_dot_q * c + m_prim * U_e) * V0 / (V0 * m_prim - aircraftParameters.Z_dot_w_dot * c);
            A_dataArray[1][3] = (-V0 * g * m_prim * (float)Math.Sin(theta_e)) / (V0 * m_prim - aircraftParameters.Z_dot_w_dot * c);

            A_dataArray[2][0] =
                (aircraftParameters.M_dot_w_dot * aircraftParameters.Z_dot_u * c) / (I_y_prim * (V0 * m_prim - aircraftParameters.Z_dot_w_dot * c)) +
                (aircraftParameters.M_dot_u) / (I_y_prim);
            A_dataArray[2][1] =
                (aircraftParameters.M_dot_w_dot * aircraftParameters.Z_dot_w * c) / (I_y_prim * (V0 * m_prim - aircraftParameters.Z_dot_w_dot * c)) +
                (aircraftParameters.M_dot_w) / (I_y_prim);
            A_dataArray[2][2] =
                (aircraftParameters.M_dot_w_dot * c * (aircraftParameters.Z_dot_q * c + m_prim * U_e)) / (I_y_prim * (V0 * m_prim - aircraftParameters.Z_dot_w_dot * c)) +
                (aircraftParameters.M_dot_q * c) / (I_y_prim);
            A_dataArray[2][3] =
                (-aircraftParameters.M_dot_w_dot * c * g * m_prim * (float)Math.Sin(theta_e)) / (I_y_prim * (V0 * m_prim - aircraftParameters.Z_dot_w_dot * c));

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


            B_dataArray[0][0] = (1 / m_prim) *
                (V0 * aircraftParameters.X_dot_ni + (V0 * aircraftParameters.X_dot_w_dot * aircraftParameters.Z_dot_ni * c) / (V0 * m_prim - aircraftParameters.Z_dot_w_dot * c));
            B_dataArray[0][1] = (1 / m_prim) *
                (V0 * aircraftParameters.X_dot_tau + (V0 * aircraftParameters.X_dot_w_dot * aircraftParameters.Z_dot_tau * c) / (V0 * m_prim - aircraftParameters.Z_dot_w_dot * c));

            B_dataArray[1][0] = V0 * V0 * aircraftParameters.Z_dot_ni / (V0 * m_prim - aircraftParameters.Z_dot_w_dot * c);
            B_dataArray[1][1] = V0 * V0 * aircraftParameters.Z_dot_tau / (V0 * m_prim - aircraftParameters.Z_dot_w_dot * c);

            B_dataArray[2][0] = (1 / I_y_prim) *
                ((aircraftParameters.M_dot_w_dot * V0 * aircraftParameters.Z_dot_ni * c) / (V0 * m_prim - aircraftParameters.Z_dot_w_dot * c) + aircraftParameters.M_dot_ni * V0);
            B_dataArray[2][1] = (1 / I_y_prim) *
                ((aircraftParameters.M_dot_w_dot * V0 * aircraftParameters.Z_dot_tau * c) / (V0 * m_prim - aircraftParameters.Z_dot_w_dot * c) + aircraftParameters.M_dot_tau * V0);

            B_dataArray[3][0] = 0.0f;
            B_dataArray[3][1] = 0.0f;

            float[][] x0_dataArray = new float[1][] { new float[4] { U_e, W_e, Q_e, theta_e } };//additionalInformation.Array[0][0], additionalInformation.Array[0][1], additionalInformation.Array[0][2], additionalInformation.Array[0][3] } };
            float[][] u_dataArray = new float[1][] { new float[2] { ni, tau } };
            float[][] simulation_Index_dataArray = new float[1][] { new float[1] { simulation_Index } };
            preparedLongitudinalData.Add(new Data() { InputType = DataType.Matrix, Array = A_dataArray, Sender="A" });
            preparedLongitudinalData.Add(new Data() { InputType = DataType.Matrix, Array = B_dataArray, Sender = "B" });
            preparedLongitudinalData.Add(new Data() { InputType = DataType.Matrix, Array = x0_dataArray, Sender = "x0" });
            preparedLongitudinalData.Add(new Data() { InputType = DataType.Matrix, Array = u_dataArray, Sender = "u" });
            preparedLongitudinalData.Add(new Data() { InputType = DataType.Matrix, Array = simulation_Index_dataArray, Sender = "simulation_index" });

            return preparedLongitudinalData;
        }
    }
}
