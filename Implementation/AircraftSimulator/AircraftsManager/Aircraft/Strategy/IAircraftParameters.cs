using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Common.Containers
{
    public interface IAircraftParameters
    {
        /// <summary>
        /// AirDensity
        /// </summary>
        float p{ get; set; }
        /// <summary>
        /// Axial velocity component
        /// </summary>
        float U_e { get; set; }
        /// <summary>
        /// Aircraft mass
        /// </summary>
        float m { get; set; }
        /// <summary>
        /// Moment of inertia in pitch
        /// </summary>
        float I_y { get; set; }
        /// <summary>
        /// Gravitational constant
        /// </summary>
        float g { get; set; }
        float X_dot_u { get; set; }
        float Z_dot_u { get; set; }
        float M_dot_u { get; set; }
        float X_dot_w { get; set; }
        float Z_dot_w { get; set; }
        float M_dot_w { get; set; }
        float X_dot_w_dot { get; set; }
        float Z_dot_w_dot { get; set; }
        float M_dot_w_dot { get; set; }
        float X_dot_q { get; set; }
        float Z_dot_q { get; set; }
        float M_dot_q { get; set; }
        float X_dot_ni { get; set; }
        float Z_dot_ni { get; set; }
        float M_dot_ni { get; set; }
        float X_dot_tau { get; set; }
        float Z_dot_tau { get; set; }
        float M_dot_tau { get; set; }
    }
}
