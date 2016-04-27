using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Containers
{
    [Serializable]
    public class AircraftParameters : IAircraftParameters
    {

        public float S { get; set; }

        public float c { get; set; }

        /// <summary>
        /// AirDensity
        /// </summary>
        public float p { get; set; }
        /// <summary>
        /// Axial velocity component
        /// </summary>
        public float U_e { get; set; }
        /// <summary>
        /// Aircraft mass
        /// </summary>
        public float m { get; set; }
        /// <summary>
        /// Moment of inertia in pitch
        /// </summary>
        public float I_y { get; set; }
        /// <summary>
        /// Gravitational constant
        /// </summary>
        public float g { get; set; }
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
