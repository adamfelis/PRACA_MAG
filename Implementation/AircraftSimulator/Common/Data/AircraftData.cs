using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.AircraftData
{
    public class AircraftData
    {
        private float[][] aircraftData;

        //velocities
        #region
        public float V_0
        {
            get
            {
                return aircraftData[0][0];
            }
        }

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
        #endregion

        //steering
        #region
        /// <summary>
        /// Elevator rotation
        /// </summary>
        public float Ni
        {
            get
            {
                return aircraftData[1][0];
            }
        }
        /// <summary>
        /// Aileron rotation
        /// </summary>
        public float Xi
        {
            get
            {
                return aircraftData[1][1];
            }
        }
        /// <summary>
        /// Rudder rotatation
        /// </summary>
        public float Zeta
        {
            get
            {
                return aircraftData[1][2];
            }
        }

        /// <summary>
        /// Throttle
        /// </summary>
        public float Tau
        {
            get
            {
                return aircraftData[1][3];
            }
        }
        #endregion

        //angular velocities
        #region
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
        #endregion

        //aircraft rotations
        #region
        /// <summary>
        /// Pitch
        /// </summary>
        public float theta_e
        {
            get
            {
                return aircraftData[3][0];
            }
        }
        /// <summary>
        /// Roll
        /// </summary>
        public float phi_e
        {
            get
            {
                return aircraftData[3][1];
            }
        }
        /// <summary>
        /// Yaw
        /// </summary>
        public float psi_e
        {
            get
            {
                return aircraftData[3][2];
            }
        }
        #endregion

        //calculation constants
        #region
        public float FixedUpdateRate
        {
            get { return aircraftData[4][0]; }
        }
        public float SimulationTime
        {
            get { return aircraftData[4][1]; }
        }
        #endregion

        public AircraftData(float[][] aircraftData)
        {
            this.aircraftData = aircraftData;
        }

        public AircraftData(float V_0, float V_e, float Ni, float Xi, float Zeta, float Tau, float theta_e, float phi_e, float psi_e, float P_e, float Q_e, float R_e)
        {
            float empty = 0.0f;
            this.aircraftData = new float[][]
            {
                new float[] { V_0, V_e },
                new float[] { Ni, Xi, Zeta, Tau },
                new float[] { P_e, Q_e, R_e },
                new float[] { theta_e, phi_e, psi_e },
                new float[] {empty, empty}
            };
        }

        public AircraftData(float V_0, float theta_e, float FixedUpdateRate, float SimulationTime)
        {
            float empty = 0.0f;
            this.aircraftData = new float[][]
            {
                new float[] { V_0, empty },
                new float[] { empty, empty, empty, empty },
                new float[] { empty, empty, empty },
                new float[] { theta_e, empty, empty},
                new float[] { FixedUpdateRate, SimulationTime}
            };
        }

        public float[][] GetData()
        {
            return this.aircraftData;
        }
    }
}
