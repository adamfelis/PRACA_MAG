using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.AircraftData
{
    public class AircraftData
    {
        private float[][] aircraftData;

        public float V_0
        {
            get
            {
                return aircraftData[0][0];
            }
        }
        //velocities
        public float U_e
        {
            get
            {
                return V_0 * (float)Math.Cos(theta_e);
            }
        }
        public float V_e
        {
            get
            {
                return aircraftData[0][1];
            }
        }
        public float W_e
        {
            get
            {
                return V_0 * (float)Math.Sin(theta_e);
            }
        }
        //rotation angles
        public float theta_e
        {
            get
            {
                return aircraftData[1][0];
            }
        }
        public float xi_e
        {
            get
            {
                return aircraftData[1][1];
            }
        }
        public float dzeta_e
        {
            get
            {
                return aircraftData[1][2];
            }
        }
        //angular velocities
        public float P_e
        {
            get
            {
                return aircraftData[2][0];
            }
        }
        public float Q_e
        {
            get
            {
                return aircraftData[2][1];
            }
        }
        public float R_e
        {
            get
            {
                return aircraftData[2][2];
            }
        }
        public float Ni
        {
            get
            {
                return aircraftData[3][0];
            }
        }
        public float Tau
        {
            get
            {
                return aircraftData[3][1];
            }
        }
        public float Phi
        {
            get
            {
                return aircraftData[3][2];
            }
        }
        public float Psi
        {
            get
            {
                return aircraftData[3][3];
            }
        }

        public AircraftData(float[][] aircraftData)
        {
            this.aircraftData = aircraftData;
        }

        public AircraftData(float V_0, float V_e, float theta_e, float xi_e, float dzeta_e, float P_e, float Q_e, float R_e)
        {
            this.aircraftData = new float[3][]
            {
                new float[2] { V_0, V_e },
                new float[3] { theta_e, xi_e, dzeta_e },
                new float[3] { P_e, Q_e, R_e }
            };
        }
        public float[][] GetData()
        {
            return this.aircraftData;
        }
    }
}
