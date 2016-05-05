syms Y_v L_v N_v...
    Y_p b m_prim W_e L_p N_p...
    Y_r U_e L_r N_r...
    g theta_e...
    V_0 Y_xi L_xi N_xi...
    Y_zeta L_zeta N_zeta...
    I_x_prim I_z_prim I_xz_prim;

M = [m_prim, 0, 0, 0, 0; ...
    0, I_x_prim, -I_xz_prim, 0, 0; ...
    0, -I_xz_prim, I_z_prim, 0, 0;...
    0,0,0,1,0;
    0,0,0,0,1;];

A_prim = ...
    [Y_v, Y_p * b + m_prim*W_e, Y_r * b - m_prim*U_e, m_prim * g * cos(theta_e), m_prim * g * sin(theta_e);...
    L_v, L_p * b, L_r * b, 0, 0;...
    N_v, N_p * b, N_r * b, 0, 0;...
    0, 1, 0, 0, 0;...
    0, 0, 1, 0, 0];
 
B_prim = ...
    [V_0 * Y_xi, V_0 * Y_zeta;...
    V_0 * L_xi, V_0 * L_zeta;...
    V_0 * N_xi, V_0 * N_zeta;...
    0, 0;
    0, 0;];

A = M^(-1) * A_prim;
B = M^(-1) * B_prim;

A = simplify(A)
B = simplify(B)