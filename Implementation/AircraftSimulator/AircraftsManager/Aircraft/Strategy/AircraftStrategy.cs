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
        private const int longitudinal_simulation_time = 11;
        private int longitudinal_simulation_index = 0;
        //lateral
        private float V_e;
        private float xi;
        private float zeta;
        private float phi;
        private float psi;
        private float P_e;
        private float R_e;

        private IData additionalInformation;
        public IData AdditionalInformation
        {
            get
            {
                longitudinal_simulation_index = 0;
                return additionalInformation;
            }
        }

        protected IAircraftParameters aircraftParameters;
        public IAircraftParameters AircraftParameters
        {
            get
            {
                return aircraftParameters;
            }
            set
            {
                aircraftParameters = value;
            }
        }

        internal abstract List<IData> GetLongitudinalData(IData additionalInformation = null);
        internal abstract List<IData> GetLateralData(IData additionalInformation = null);
        internal abstract List<IData> GetLongitudinalInitialData(IData additionalInformation = null);
        internal abstract List<IData> GetLateralInitialData(IData additionalInformation = null);

        private List<IData> matrixes;

        public List<IData> Matrixes
        {
            get
            {
                return matrixes;
            }
        }

        public void RefreshMatrixes()
        {
            this.matrixes.Clear();
            GetLateralInitialData();
            GetLongitudinalInitialData();
        }

        protected override void Initialize()
        {
            matrixes = new List<IData>();
            aircraftParameters = GetData(aircraftDataFileName);
            lateralData = null;
            longitudinalData = null;
        }

        private AircraftParameters GetData(string dataFileName)
        {
            AircraftParameters parameters = null;
            try
            {
                string path = "";
#if !DEBUG
                path += @"..\";
#endif
                path += @"..\..\PRACA_MAG\Implementation\AircraftSimulator\AppInput\XMLFiles\";
                using (TextReader reader = new StreamReader(path + dataFileName))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(AircraftParameters));
                    parameters = (AircraftParameters)serializer.Deserialize(reader);
                }
            }
            catch (Exception)
            {
                throw new global::Common.Exceptions.XMLFileNotFoundException();
            }
            return parameters;
        }

        protected List<IData> PrepareLongitudinalMatrixes(IData additionalInformation)
        {
            if (additionalInformation == null)
                additionalInformation = this.additionalInformation;
            else
                this.additionalInformation = additionalInformation;
            #region A, B calculations
            const int A_size = 4;
            float m_prim = aircraftParameters.m / (0.5f * aircraftParameters.p * aircraftParameters.S);
            float I_y_prim = aircraftParameters.I_y / (0.5f * aircraftParameters.p * aircraftParameters.S * aircraftParameters.c);

            global::Common.AircraftData.AircraftData aircraftData = new global::Common.AircraftData.AircraftData(additionalInformation.Array);
            float initial_V0 = aircraftData.V_0;
            float initial_theta_e = aircraftData.theta_e;
            float initial_U_e = aircraftData.U_e;
            float initial_W_e = aircraftData.W_e;

            m_prim /= initial_V0;
            I_y_prim /= initial_V0;

            float[][] A_dataArray_longitudinal;
            float[][] B_dataArray_longitudinal;

            A_dataArray_longitudinal = new float[A_size][];
            for (int i = 0; i < A_size; i++)
            {
                A_dataArray_longitudinal[i] = new float[A_size];
            }

            A_dataArray_longitudinal[0][0] = (1.0f / m_prim) *
                (aircraftParameters.X_dot_u +
                (aircraftParameters.X_dot_w_dot * aircraftParameters.Z_dot_u * aircraftParameters.c) / (initial_V0 * m_prim - aircraftParameters.Z_dot_w_dot * aircraftParameters.c)
                );
            A_dataArray_longitudinal[0][1] = (1.0f / m_prim) *
                (aircraftParameters.X_dot_w +
                (aircraftParameters.X_dot_w_dot * aircraftParameters.Z_dot_w * aircraftParameters.c) / (initial_V0 * m_prim - aircraftParameters.Z_dot_w_dot * aircraftParameters.c)
                );
            A_dataArray_longitudinal[0][2] = (1.0f / m_prim) *
                (aircraftParameters.X_dot_q * aircraftParameters.c - m_prim * initial_W_e +
                (aircraftParameters.Z_dot_q * aircraftParameters.c + m_prim * initial_U_e) * aircraftParameters.X_dot_w_dot * aircraftParameters.c / (initial_V0 * m_prim - aircraftParameters.Z_dot_w_dot * aircraftParameters.c)
                );
            A_dataArray_longitudinal[0][3] =
                (-aircraftParameters.g * (float)Math.Cos(initial_theta_e) -
                (aircraftParameters.X_dot_w_dot * aircraftParameters.c * aircraftParameters.g * (float)Math.Sin(initial_theta_e) / (initial_V0 * m_prim - aircraftParameters.Z_dot_w_dot * aircraftParameters.c))
                );

            A_dataArray_longitudinal[1][0] = aircraftParameters.Z_dot_u * initial_V0 / (initial_V0 * m_prim - aircraftParameters.Z_dot_w_dot * aircraftParameters.c);
            A_dataArray_longitudinal[1][1] = aircraftParameters.Z_dot_w * initial_V0 / (initial_V0 * m_prim - aircraftParameters.Z_dot_w_dot * aircraftParameters.c);
            A_dataArray_longitudinal[1][2] = (aircraftParameters.Z_dot_q * aircraftParameters.c + m_prim * initial_U_e) * initial_V0 / (initial_V0 * m_prim - aircraftParameters.Z_dot_w_dot * aircraftParameters.c);
            A_dataArray_longitudinal[1][3] = (-initial_V0 * aircraftParameters.g * m_prim * (float)Math.Sin(initial_theta_e)) / (initial_V0 * m_prim - aircraftParameters.Z_dot_w_dot * aircraftParameters.c);

            A_dataArray_longitudinal[2][0] =
                (aircraftParameters.M_dot_w_dot * aircraftParameters.Z_dot_u * aircraftParameters.c) / (I_y_prim * (initial_V0 * m_prim - aircraftParameters.Z_dot_w_dot * aircraftParameters.c)) +
                (aircraftParameters.M_dot_u) / (I_y_prim);
            A_dataArray_longitudinal[2][1] =
                (aircraftParameters.M_dot_w_dot * aircraftParameters.Z_dot_w * aircraftParameters.c) / (I_y_prim * (initial_V0 * m_prim - aircraftParameters.Z_dot_w_dot * aircraftParameters.c)) +
                (aircraftParameters.M_dot_w) / (I_y_prim);
            A_dataArray_longitudinal[2][2] =
                (aircraftParameters.M_dot_w_dot * aircraftParameters.c * (aircraftParameters.Z_dot_q * aircraftParameters.c + m_prim * initial_U_e)) / (I_y_prim * (initial_V0 * m_prim - aircraftParameters.Z_dot_w_dot * aircraftParameters.c)) +
                (aircraftParameters.M_dot_q * aircraftParameters.c) / (I_y_prim);
            A_dataArray_longitudinal[2][3] =
                (-aircraftParameters.M_dot_w_dot * aircraftParameters.c * aircraftParameters.g * m_prim * (float)Math.Sin(initial_theta_e)) / (I_y_prim * (initial_V0 * m_prim - aircraftParameters.Z_dot_w_dot * aircraftParameters.c));

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
                (initial_V0 * aircraftParameters.X_dot_ni + (initial_V0 * aircraftParameters.X_dot_w_dot * aircraftParameters.Z_dot_ni * aircraftParameters.c) / (initial_V0 * m_prim - aircraftParameters.Z_dot_w_dot * aircraftParameters.c));
            B_dataArray_longitudinal[0][1] = (1 / m_prim) *
                (initial_V0 * aircraftParameters.X_dot_tau + (initial_V0 * aircraftParameters.X_dot_w_dot * aircraftParameters.Z_dot_tau * aircraftParameters.c) / (initial_V0 * m_prim - aircraftParameters.Z_dot_w_dot * aircraftParameters.c));

            B_dataArray_longitudinal[1][0] = initial_V0 * initial_V0 * aircraftParameters.Z_dot_ni / (initial_V0 * m_prim - aircraftParameters.Z_dot_w_dot * aircraftParameters.c);
            B_dataArray_longitudinal[1][1] = initial_V0 * initial_V0 * aircraftParameters.Z_dot_tau / (initial_V0 * m_prim - aircraftParameters.Z_dot_w_dot * aircraftParameters.c);

            B_dataArray_longitudinal[2][0] = (1 / I_y_prim) *
                ((aircraftParameters.M_dot_w_dot * initial_V0 * aircraftParameters.Z_dot_ni * aircraftParameters.c) / (initial_V0 * m_prim - aircraftParameters.Z_dot_w_dot * aircraftParameters.c) + aircraftParameters.M_dot_ni * initial_V0);
            B_dataArray_longitudinal[2][1] = (1 / I_y_prim) *
                ((aircraftParameters.M_dot_w_dot * initial_V0 * aircraftParameters.Z_dot_tau * aircraftParameters.c) / (initial_V0 * m_prim - aircraftParameters.Z_dot_w_dot * aircraftParameters.c) + aircraftParameters.M_dot_tau * initial_V0);

            B_dataArray_longitudinal[3][0] = 0.0f;
            B_dataArray_longitudinal[3][1] = 0.0f;

            #endregion

            List<IData> preparedLongitudinalData = new List<IData>();
            preparedLongitudinalData.Add(new Data() { InputType = DataType.Matrix, Array = A_dataArray_longitudinal, Sender = "A_longitudinal" });
            preparedLongitudinalData.Add(new Data() { InputType = DataType.Matrix, Array = B_dataArray_longitudinal, Sender = "B_longitudinal" });
            
            matrixes.AddRange(preparedLongitudinalData);

            return preparedLongitudinalData;
        }

        protected List<IData> PrepareLateralMatrixes(IData additionalInformation)
        {
            #region A, B calculations
            const int A_size = 5;
            float m_prim = aircraftParameters.m / (0.5f * aircraftParameters.p * aircraftParameters.S);
            float I_x_prim = aircraftParameters.I_x / (0.5f * aircraftParameters.p * aircraftParameters.S * aircraftParameters.b);
            float I_z_prim = aircraftParameters.I_z / (0.5f * aircraftParameters.p * aircraftParameters.S * aircraftParameters.b);
            float I_xz_prim = aircraftParameters.I_xz / (0.5f * aircraftParameters.p * aircraftParameters.S * aircraftParameters.b);
            if (additionalInformation == null)
                additionalInformation = this.additionalInformation;
            else
                this.additionalInformation = additionalInformation;

            global::Common.AircraftData.AircraftData aircraftData = new global::Common.AircraftData.AircraftData(additionalInformation.Array);

            float initial_V0 = aircraftData.V_0;
            float initial_theta_e = aircraftData.theta_e;
            float initial_U_e = aircraftData.U_e;
            float initial_W_e = aircraftData.W_e;

            m_prim /= initial_V0;
            I_x_prim /= initial_V0;
            I_z_prim /= initial_V0;
            I_xz_prim /= initial_V0;

            float[][] A_dataArray_lateral;
            float[][] B_dataArray_lateral;

            A_dataArray_lateral = new float[A_size][];
            for (int i = 0; i < A_size; i++)
            {
                A_dataArray_lateral[i] = new float[A_size];
            }

            A_dataArray_lateral[0][0] = aircraftParameters.Y_v / m_prim;
            A_dataArray_lateral[0][1] = (aircraftParameters.Y_p * aircraftParameters.b + initial_W_e * m_prim)/m_prim;
            A_dataArray_lateral[0][2] = (aircraftParameters.Y_r * aircraftParameters.b - initial_U_e * m_prim) / m_prim;
            A_dataArray_lateral[0][3] = aircraftParameters.g * (float)Math.Cos(initial_theta_e);
            A_dataArray_lateral[0][4] = aircraftParameters.g * (float)Math.Sin(initial_theta_e);

            A_dataArray_lateral[1][0] = -(I_z_prim * aircraftParameters.L_v + I_xz_prim * aircraftParameters.N_v) / (I_xz_prim * I_xz_prim - I_x_prim * I_z_prim);
            A_dataArray_lateral[1][1] = -(aircraftParameters.b * (I_z_prim * aircraftParameters.L_p + I_xz_prim * aircraftParameters.N_p))/ (I_xz_prim * I_xz_prim - I_x_prim * I_z_prim);
            A_dataArray_lateral[1][2] = -(aircraftParameters.b * (I_z_prim * aircraftParameters.L_r + I_xz_prim * aircraftParameters.N_r)) / (I_xz_prim * I_xz_prim - I_x_prim * I_z_prim);
            A_dataArray_lateral[1][3] = 0;
            A_dataArray_lateral[1][4] = 0;

            A_dataArray_lateral[2][0] = -(I_xz_prim * aircraftParameters.L_v + I_x_prim * aircraftParameters.N_v) / (I_xz_prim * I_xz_prim - I_x_prim * I_z_prim);
            A_dataArray_lateral[2][1] = -(aircraftParameters.b * (I_xz_prim * aircraftParameters.L_p + I_x_prim * aircraftParameters.N_p)) / (I_xz_prim * I_xz_prim - I_x_prim * I_z_prim);
            A_dataArray_lateral[2][2] = -(aircraftParameters.b * (I_xz_prim * aircraftParameters.L_r + I_x_prim * aircraftParameters.N_r)) / (I_xz_prim * I_xz_prim - I_x_prim * I_z_prim);
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


            B_dataArray_lateral[0][0] = initial_V0 * aircraftParameters.Y_xi / m_prim;
            B_dataArray_lateral[0][1] = initial_V0 * aircraftParameters.Y_zeta / m_prim;

            B_dataArray_lateral[1][0] = -(initial_V0 * (I_z_prim * aircraftParameters.L_xi + I_xz_prim * aircraftParameters.N_xi)) / (I_xz_prim * I_xz_prim - I_x_prim * I_z_prim);
            B_dataArray_lateral[1][1] = -(initial_V0 * (I_z_prim * aircraftParameters.L_zeta + I_xz_prim * aircraftParameters.N_zeta)) / (I_xz_prim * I_xz_prim - I_x_prim * I_z_prim);

            B_dataArray_lateral[2][0] = -(initial_V0 * (I_xz_prim * aircraftParameters.L_xi + I_x_prim * aircraftParameters.N_xi)) / (I_xz_prim * I_xz_prim - I_x_prim * I_z_prim);
            B_dataArray_lateral[2][1] = -(initial_V0 * (I_xz_prim * aircraftParameters.L_zeta + I_x_prim * aircraftParameters.N_zeta)) / (I_xz_prim * I_xz_prim - I_x_prim * I_z_prim);

            B_dataArray_lateral[3][0] = 0;
            B_dataArray_lateral[3][1] = 0;

            B_dataArray_lateral[4][0] = 0;
            B_dataArray_lateral[4][1] = 0;

            #endregion

            List<IData> preparedLateralData = new List<IData>();
            preparedLateralData.Add(new Data() { InputType = DataType.Matrix, Array = A_dataArray_lateral, Sender = "A_lateral" });
            preparedLateralData.Add(new Data() { InputType = DataType.Matrix, Array = B_dataArray_lateral, Sender = "B_lateral" });

            matrixes.AddRange(preparedLateralData);

            return preparedLateralData;
        }

        protected List<IData> PrepareLongitudinalData(IData additionalInformation)
        {
            global::Common.AircraftData.AircraftData aircraftData = new global::Common.AircraftData.AircraftData(additionalInformation.Array);


            List<IData> preparedLongitudinalData = new List<IData>();

            float[][] x0_dataArray = new float[1][] { new float[4] { U_e, W_e, Q_e, theta_e } };
            float[][] u_dataArray = new float[1][] { new float[2] { ni, tau } };
            float[][] simulation_Index_dataArray = new float[1][] { new float[1] { longitudinal_simulation_index } };
            preparedLongitudinalData.Add(new Data() { InputType = DataType.Matrix, Array = x0_dataArray, Sender = "x0_longitudinal" });
            preparedLongitudinalData.Add(new Data() { InputType = DataType.Matrix, Array = u_dataArray, Sender = "u_longitudinal" });
            preparedLongitudinalData.Add(new Data() { InputType = DataType.Matrix, Array = simulation_Index_dataArray, Sender = "simulation_index" });

            return preparedLongitudinalData;
        }

        protected List<IData> PrepareLateralData(IData additionalInformation)
        {
            global::Common.AircraftData.AircraftData aircraftData = new global::Common.AircraftData.AircraftData(additionalInformation.Array);


            longitudinal_simulation_index = (longitudinal_simulation_index + 1) % longitudinal_simulation_time;
            if (longitudinal_simulation_index == 0)
                longitudinal_simulation_index++;

            if (longitudinal_simulation_index == 1 || aircraftData.Ni != ni || aircraftData.Tau != tau || aircraftData.Xi != xi || aircraftData.Zeta != zeta)
            {
                longitudinal_simulation_index = 1; //needed when steering 'u' has changed - simulation is interruped and recalculated
                V0 = aircraftData.V_0;
                Q_e = aircraftData.Q_e;
                theta_e = aircraftData.theta_e;
                U_e = aircraftData.U_e;
                W_e = aircraftData.W_e;
                ni = aircraftData.Ni;
                tau = aircraftData.Tau;
                P_e = aircraftData.P_e;
                R_e = aircraftData.R_e;
                V_e = aircraftData.V_e;
                xi = aircraftData.Xi;
                zeta = aircraftData.Zeta;
                phi = aircraftData.phi_e;
                psi = aircraftData.psi_e;
            }
            List<IData> preparedLateralData = new List<IData>();

            float[][] x0_dataArray = new float[1][] { new float[5] { V_e, P_e, R_e, phi, psi } };
            float[][] u_dataArray = new float[1][] { new float[2] { xi, zeta } };
            //float[][] simulation_Index_dataArray = new float[1][] { new float[1] { lateral_simulation_index } };
            preparedLateralData.Add(new Data() { InputType = DataType.Matrix, Array = x0_dataArray, Sender = "x0_lateral" });
            preparedLateralData.Add(new Data() { InputType = DataType.Matrix, Array = u_dataArray, Sender = "u_lateral" });

            return preparedLateralData;
        }
    }
}
