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
        private float V0;
        private float U_e;
        private float W_e;
        private float Q_e;
        private float theta_e;
        //longitudinal
        private float ni;
        private float tau;
        float[][] A_dataArray_longitudinal;
        float[][] B_dataArray_longitudinal;
        private const int longitudinal_simulation_time = 101;
        private static int longitudinal_simulation_index = 0;
        //lateral
        private float V_e;
        private float xi;
        private float zeta;
        private float phi;
        private float psi;
        private float P_e;
        private float R_e;
        float[][] A_dataArray_lateral;
        float[][] B_dataArray_lateral;
        private const int lateral_simulation_time = 101;
        private static int lateral_simulation_index = 0;

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

            longitudinal_simulation_index = (longitudinal_simulation_index + 1) % longitudinal_simulation_time;
            if (longitudinal_simulation_index == 0)
                longitudinal_simulation_index++;

            if (longitudinal_simulation_index == 1 || additionalInformation.Array[1][0] != ni || additionalInformation.Array[1][1] != tau)
            {
                longitudinal_simulation_index = 1; //needed when steering 'u' has changed - simulation is interruped and recalculated
                V0 = additionalInformation.Array[0][0];
                Q_e = additionalInformation.Array[0][1];
                theta_e = additionalInformation.Array[0][2];
                U_e = V0 * (float)Math.Cos(theta_e);
                W_e = V0 * (float)Math.Sin(theta_e);
                ni = additionalInformation.Array[1][0];
                tau = additionalInformation.Array[1][1];

                #region Old A, B calculations
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
                #endregion

                #region A, B calculations
                const int A_size = 4;
                float m_prim = aircraftParameters.m / (0.5f * aircraftParameters.p * aircraftParameters.S);
                float I_y_prim = aircraftParameters.I_y / (0.5f * aircraftParameters.p * aircraftParameters.S * aircraftParameters.c);

                A_dataArray_longitudinal = new float[A_size][];
                for (int i = 0; i < A_size; i++)
                {
                    A_dataArray_longitudinal[i] = new float[A_size];
                }

                A_dataArray_longitudinal[0][0] = (1.0f / m_prim) *
                    (aircraftParameters.X_dot_u +
                    (aircraftParameters.X_dot_w_dot * aircraftParameters.Z_dot_u * aircraftParameters.c) / (V0 * m_prim - aircraftParameters.Z_dot_w_dot * aircraftParameters.c)
                    );
                A_dataArray_longitudinal[0][1] = (1.0f / m_prim) *
                    (aircraftParameters.X_dot_w +
                    (aircraftParameters.X_dot_w_dot * aircraftParameters.Z_dot_w * aircraftParameters.c) / (V0 * m_prim - aircraftParameters.Z_dot_w_dot * aircraftParameters.c)
                    );
                A_dataArray_longitudinal[0][2] = (1.0f / m_prim) *
                    (aircraftParameters.X_dot_q * aircraftParameters.c - m_prim * W_e +
                    (aircraftParameters.Z_dot_q * aircraftParameters.c + m_prim * U_e) * aircraftParameters.X_dot_w_dot * aircraftParameters.c / (V0 * m_prim - aircraftParameters.Z_dot_w_dot * aircraftParameters.c)
                    );
                A_dataArray_longitudinal[0][3] =
                    (-aircraftParameters.g * (float)Math.Cos(theta_e) -
                    (aircraftParameters.X_dot_w_dot * aircraftParameters.c * aircraftParameters.g * (float)Math.Sin(theta_e) / (V0 * m_prim - aircraftParameters.Z_dot_w_dot * aircraftParameters.c))
                    );

                A_dataArray_longitudinal[1][0] = aircraftParameters.Z_dot_u * V0 / (V0 * m_prim - aircraftParameters.Z_dot_w_dot * aircraftParameters.c);
                A_dataArray_longitudinal[1][1] = aircraftParameters.Z_dot_w * V0 / (V0 * m_prim - aircraftParameters.Z_dot_w_dot * aircraftParameters.c);
                A_dataArray_longitudinal[1][2] = (aircraftParameters.Z_dot_q * aircraftParameters.c + m_prim * U_e) * V0 / (V0 * m_prim - aircraftParameters.Z_dot_w_dot * aircraftParameters.c);
                A_dataArray_longitudinal[1][3] = (-V0 * aircraftParameters.g * m_prim * (float)Math.Sin(theta_e)) / (V0 * m_prim - aircraftParameters.Z_dot_w_dot * aircraftParameters.c);

                A_dataArray_longitudinal[2][0] =
                    (aircraftParameters.M_dot_w_dot * aircraftParameters.Z_dot_u * aircraftParameters.c) / (I_y_prim * (V0 * m_prim - aircraftParameters.Z_dot_w_dot * aircraftParameters.c)) +
                    (aircraftParameters.M_dot_u) / (I_y_prim);
                A_dataArray_longitudinal[2][1] =
                    (aircraftParameters.M_dot_w_dot * aircraftParameters.Z_dot_w * aircraftParameters.c) / (I_y_prim * (V0 * m_prim - aircraftParameters.Z_dot_w_dot * aircraftParameters.c)) +
                    (aircraftParameters.M_dot_w) / (I_y_prim);
                A_dataArray_longitudinal[2][2] =
                    (aircraftParameters.M_dot_w_dot * aircraftParameters.c * (aircraftParameters.Z_dot_q * aircraftParameters.c + m_prim * U_e)) / (I_y_prim * (V0 * m_prim - aircraftParameters.Z_dot_w_dot * aircraftParameters.c)) +
                    (aircraftParameters.M_dot_q * aircraftParameters.c) / (I_y_prim);
                A_dataArray_longitudinal[2][3] =
                    (-aircraftParameters.M_dot_w_dot * aircraftParameters.c * aircraftParameters.g * m_prim * (float)Math.Sin(theta_e)) / (I_y_prim * (V0 * m_prim - aircraftParameters.Z_dot_w_dot * aircraftParameters.c));

                A_dataArray_longitudinal[3][0] = 0.0f;
                A_dataArray_longitudinal[3][1] = 0.0f;
                A_dataArray_longitudinal[3][2] = 1.0f;
                A_dataArray_longitudinal[3][3] = 0.0f;

                const int B_size = 2;
                B_dataArray_longitudinal = new float[A_size][];
                for (int i = 0; i < A_size; i++)
                {
                    B_dataArray_longitudinal[i] = new float[B_size];
                }


                B_dataArray_longitudinal[0][0] = (1 / m_prim) *
                    (V0 * aircraftParameters.X_dot_ni + (V0 * aircraftParameters.X_dot_w_dot * aircraftParameters.Z_dot_ni * aircraftParameters.c) / (V0 * m_prim - aircraftParameters.Z_dot_w_dot * aircraftParameters.c));
                B_dataArray_longitudinal[0][1] = (1 / m_prim) *
                    (V0 * aircraftParameters.X_dot_tau + (V0 * aircraftParameters.X_dot_w_dot * aircraftParameters.Z_dot_tau * aircraftParameters.c) / (V0 * m_prim - aircraftParameters.Z_dot_w_dot * aircraftParameters.c));

                B_dataArray_longitudinal[1][0] = V0 * V0 * aircraftParameters.Z_dot_ni / (V0 * m_prim - aircraftParameters.Z_dot_w_dot * aircraftParameters.c);
                B_dataArray_longitudinal[1][1] = V0 * V0 * aircraftParameters.Z_dot_tau / (V0 * m_prim - aircraftParameters.Z_dot_w_dot * aircraftParameters.c);

                B_dataArray_longitudinal[2][0] = (1 / I_y_prim) *
                    ((aircraftParameters.M_dot_w_dot * V0 * aircraftParameters.Z_dot_ni * aircraftParameters.c) / (V0 * m_prim - aircraftParameters.Z_dot_w_dot * aircraftParameters.c) + aircraftParameters.M_dot_ni * V0);
                B_dataArray_longitudinal[2][1] = (1 / I_y_prim) *
                    ((aircraftParameters.M_dot_w_dot * V0 * aircraftParameters.Z_dot_tau * aircraftParameters.c) / (V0 * m_prim - aircraftParameters.Z_dot_w_dot * aircraftParameters.c) + aircraftParameters.M_dot_tau * V0);

                B_dataArray_longitudinal[3][0] = 0.0f;
                B_dataArray_longitudinal[3][1] = 0.0f;

                #endregion

            }

            List<IData> preparedLongitudinalData = new List<IData>();

            float[][] x0_dataArray = new float[1][] { new float[4] { U_e, W_e, Q_e, theta_e } };
            float[][] u_dataArray = new float[1][] { new float[2] { ni, tau } };
            float[][] simulation_Index_dataArray = new float[1][] { new float[1] { longitudinal_simulation_index } };
            preparedLongitudinalData.Add(new Data() { InputType = DataType.Matrix, Array = A_dataArray_longitudinal, Sender = "A_longitudinal" });
            preparedLongitudinalData.Add(new Data() { InputType = DataType.Matrix, Array = B_dataArray_longitudinal, Sender = "B_longitudinal" });
            preparedLongitudinalData.Add(new Data() { InputType = DataType.Matrix, Array = x0_dataArray, Sender = "x0_longitudinal" });
            preparedLongitudinalData.Add(new Data() { InputType = DataType.Matrix, Array = u_dataArray, Sender = "u_longitudinal" });
            preparedLongitudinalData.Add(new Data() { InputType = DataType.Matrix, Array = simulation_Index_dataArray, Sender = "simulation_index" });

            return preparedLongitudinalData;
        }

        protected List<IData> PrepareLateralData(IData additionalInformation)
        {
            lateral_simulation_index = (lateral_simulation_index + 1) % lateral_simulation_time;
            if (lateral_simulation_index == 0)
                lateral_simulation_index++;

            if (lateral_simulation_index == 1 || additionalInformation.Array[1][2] != xi || additionalInformation.Array[1][3] != zeta)
            {
                lateral_simulation_index = 1; //needed when steering 'u' has changed - simulation is interruped and recalculated
                V0 = additionalInformation.Array[0][0];
                P_e = additionalInformation.Array[0][3];
                R_e = additionalInformation.Array[0][4];
                theta_e = additionalInformation.Array[0][2];
                U_e = V0 * (float)Math.Cos(theta_e);
                W_e = V0 * (float)Math.Sin(theta_e);
                V_e = additionalInformation.Array[0][7];
                xi = additionalInformation.Array[1][2];
                zeta = additionalInformation.Array[1][3];
                phi = additionalInformation.Array[0][5];
                psi = additionalInformation.Array[0][6];

                #region A, B calculations
                const int A_size = 5;
                float m_prim = aircraftParameters.m / (0.5f * aircraftParameters.p * aircraftParameters.S);
                float I_y_prim = aircraftParameters.I_y / (0.5f * aircraftParameters.p * aircraftParameters.S * aircraftParameters.c);

                A_dataArray_lateral = new float[A_size][];
                for (int i = 0; i < A_size; i++)
                {
                    A_dataArray_lateral[i] = new float[A_size];
                }

                A_dataArray_lateral[0][0] = 0;
                A_dataArray_lateral[0][1] = 0;
                A_dataArray_lateral[0][2] = 0;
                A_dataArray_lateral[0][3] = 0;
                A_dataArray_lateral[0][4] = 0;

                A_dataArray_lateral[1][0] = 0;
                A_dataArray_lateral[1][1] = 0;
                A_dataArray_lateral[1][2] = 0;
                A_dataArray_lateral[1][3] = 0;
                A_dataArray_lateral[1][4] = 0;

                A_dataArray_lateral[2][0] = 0;
                A_dataArray_lateral[2][1] = 0;
                A_dataArray_lateral[2][2] = 0;
                A_dataArray_lateral[2][3] = 0;
                A_dataArray_lateral[2][4] = 0;

                A_dataArray_lateral[3][0] = 0;
                A_dataArray_lateral[3][1] = 1;
                A_dataArray_lateral[3][2] = 0;
                A_dataArray_lateral[3][3] = 0;
                A_dataArray_lateral[3][4] = 0;

                A_dataArray_lateral[4][0] = 0;
                A_dataArray_lateral[4][1] = 0;
                A_dataArray_lateral[4][2] = 1;
                A_dataArray_lateral[4][3] = 0;
                A_dataArray_lateral[4][4] = 0;

                const int B_size = 2;
                B_dataArray_lateral = new float[A_size][];
                for (int i = 0; i < A_size; i++)
                {
                    B_dataArray_lateral[i] = new float[B_size];
                }


                B_dataArray_lateral[0][0] = 0;
                B_dataArray_lateral[0][1] = 0;

                B_dataArray_lateral[1][0] = 0;
                B_dataArray_lateral[1][1] = 0;

                B_dataArray_lateral[2][0] = 0;
                B_dataArray_lateral[2][1] = 0;

                B_dataArray_lateral[3][0] = 0;
                B_dataArray_lateral[3][1] = 0;

                B_dataArray_lateral[4][0] = 0;
                B_dataArray_lateral[4][1] = 0;

                #endregion

            }

            List<IData> preparedLateralData = new List<IData>();

            float[][] x0_dataArray = new float[1][] { new float[5] { V_e, P_e, R_e, phi, psi } };
            float[][] u_dataArray = new float[1][] { new float[2] { xi, zeta } };
            float[][] simulation_Index_dataArray = new float[1][] { new float[1] { lateral_simulation_index } };
            preparedLateralData.Add(new Data() { InputType = DataType.Matrix, Array = A_dataArray_lateral, Sender = "A_lateral" });
            preparedLateralData.Add(new Data() { InputType = DataType.Matrix, Array = B_dataArray_lateral, Sender = "B_lateral" });
            preparedLateralData.Add(new Data() { InputType = DataType.Matrix, Array = x0_dataArray, Sender = "x0_lateral" });
            preparedLateralData.Add(new Data() { InputType = DataType.Matrix, Array = u_dataArray, Sender = "u_lateral" });
            preparedLateralData.Add(new Data() { InputType = DataType.Matrix, Array = simulation_Index_dataArray, Sender = "simulation_index" });

            return preparedLateralData;
        }
    }
}
