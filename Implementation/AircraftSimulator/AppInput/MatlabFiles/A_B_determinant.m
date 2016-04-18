syms m I_y g ...
    X_dot_u Z_dot_u M_dot_u ...
    X_dot_w Z_dot_w M_dot_w ...
    X_dot_w_dot Z_dot_w_dot M_dot_w_dot ...
    X_dot_q Z_dot_q M_dot_q ...
    X_dot_ni Z_dot_ni M_dot_ni ...
    X_dot_tau Z_dot_tau M_dot_tau ...
    W_e U_e theta_e...
    V0 rho c S... 
    X_u Z_u M_u ...
    X_w Z_w M_w ...
    X_w_dot Z_w_dot M_w_dot ...
    X_q Z_q M_q ...
    X_ni Z_ni M_ni ...
    X_tau Z_tau M_tau ...
;

M = [m, -X_dot_w_dot, 0, 0;...
    0, m-Z_dot_w_dot, 0, 0;...
    0, -M_dot_w_dot, I_y, 0;...
    0,0,0,1];

A_prim = ...
    [X_dot_u, X_dot_w, X_dot_q - m * W_e, -m * g * cos(theta_e);...
    Z_dot_u, Z_dot_w, Z_dot_q + m * U_e, -m * g * sin(theta_e);...
    M_dot_u, M_dot_w, M_dot_q, 0;...
    0,0,1,0];
A = M^(-1) * A_prim

%m_prim = m / (0.5 * rho * V0 * S);
%I_y_prim = I_y / (0.5 * rho * V0 * S * c);
syms m_prim I_y_prim;

M2 = [m_prim, -X_w_dot * c / V0, 0, 0;...
    0, m_prim-Z_w_dot * c / V0, 0, 0;...
    0, -M_w_dot * c / V0, I_y_prim, 0;...
    0,0,0,1];
A2_prim = ...
    [X_u, X_w, X_q*c - m_prim * W_e, -m_prim * g * cos(theta_e);...
    Z_u, Z_w, Z_q * c + m_prim * U_e, -m_prim * g * sin(theta_e);...
    M_u, M_w, M_q * c, 0;...
    0,0,1,0];
B2_prim = ...
    [V0 * X_ni, V0 * X_tau;...
    V0 * Z_ni, V0 * Z_tau;...
    V0 * M_ni, V0 * M_tau;...
    0,0];

A2 = M2^(-1) * A2_prim
B2 = M2^(-1) * B2_prim
