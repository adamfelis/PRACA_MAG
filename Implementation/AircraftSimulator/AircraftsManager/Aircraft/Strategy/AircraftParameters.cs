using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Containers
{
    [Serializable]
    public class AircraftParameters : IAircraftParameters
    {
        //Lateral
        public float Y_v { get; set; }
        public float L_v { get; set; }
        public float N_v { get; set; }
        public float Y_p { get; set; }
        public float L_p { get; set; }
        public float N_p { get; set; }
        public float Y_r { get; set; }
        public float L_r { get; set; }
        public float N_r { get; set; }
        public float Y_xi { get; set; }
        public float L_xi { get; set; }
        public float N_xi { get; set; }
        public float Y_zeta { get; set; }
        public float L_zeta { get; set; }
        public float N_zeta { get; set; }
        public float b { get; set; }
        public float I_x { get; set; }
        public float I_z { get; set; }
        public float I_xz { get; set; }

        //Common

        public float S { get; set; }
        /// <summary>
        /// AirDensity
        /// </summary>
        public float p { get; set; }
        /// <summary>
        /// Aircraft mass
        /// </summary>
        public float m { get; set; }
        /// <summary>
        /// Gravitational constant
        /// </summary>
        public float g { get; set; }

        public float c { get; set; }


        //Longitudinal

        /// <summary>
        /// Moment of inertia in pitch
        /// </summary>
        public float I_y { get; set; }
        public float X_dot_u { get; set; }
        public float Z_dot_u { get; set; }
        public float M_dot_u { get; set; }
        public float X_dot_w { get; set; }
        public float Z_dot_w { get; set; }
        public float M_dot_w { get; set; }
        public float X_dot_w_dot { get; set; }
        public float Z_dot_w_dot { get; set; }
        public float M_dot_w_dot { get; set; }
        public float X_dot_q { get; set; }
        public float Z_dot_q { get; set; }
        public float M_dot_q { get; set; }
        public float X_dot_ni { get; set; }
        public float Z_dot_ni { get; set; }
        public float M_dot_ni { get; set; }
        public float X_dot_tau { get; set; }
        public float Z_dot_tau { get; set; }
        public float M_dot_tau { get; set; }
    }
}
