using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Common.Containers
{
    public interface IAircraftParameters
    {
        //Lateral
        float Y_v { get; set; }
        float L_v { get; set; }
        float N_v { get; set; }
        float Y_p { get; set; }
        float L_p { get; set; }
        float N_p { get; set; }
        float Y_r { get; set; }
        float L_r { get; set; }
        float N_r { get; set; }
        float Y_xi { get; set; }
        float L_xi { get; set; }
        float N_xi { get; set; }
        float Y_zeta { get; set; }
        float L_zeta { get; set; }
        float N_zeta { get; set; }
        float b { get; set; }
        float I_x { get; set; }
        float I_z { get; set; }
        float I_xz { get; set; }

        //Common

        float S { get; set; }
        /// <summary>
        /// AirDensity
        /// </summary>
        float p { get; set; }
        /// <summary>
        /// Aircraft mass
        /// </summary>
        float m { get; set; }
        /// <summary>
        /// Gravitational constant
        /// </summary>
        float g { get; set; }



        //Longitudinal

        float c { get; set; }
        /// <summary>
        /// Moment of inertia in pitch
        /// </summary>
        float I_y { get; set; }
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
