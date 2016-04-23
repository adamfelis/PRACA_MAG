function [ A, B ] = CreateAB( U_e, W_e, theta_e )
%% A and B setter

rho = 0.3809;
m = 17642;
I_y = 165669;
g = 9.81;
X_u = 0.0076;
Z_u = -0.7273;
M_u = 0.0340;
X_w = 0.0483;
Z_w = -3.1245;
M_w = -0.2169;
X_w_dot = 0;
Z_w_dot = -0.3997;
M_w_dot = -0.5910;
X_q = 0;
Z_q = -1.2109;
M_q = -1.2732;
X_ni = -0.0618;
Z_ni = -0.3741;
M_ni = -0.5581;
X_tau = 0;
Z_tau = 0;
M_tau = 0;
V0 = sqrt(U_e * U_e + W_e * W_e);

S = 49.239;
c = 4.889;
m_prim = m / (0.5 * rho * V0 * S);
I_y_prim = I_y / (0.5 * rho * V0 * S * c);

%%

%%
M = [m_prim, -X_w_dot * c / V0, 0, 0;...
    0, m_prim-Z_w_dot * c / V0, 0, 0;...
    0, -M_w_dot * c / V0, I_y_prim, 0;...
    0,0,0,1];
A_prim = ...
    [X_u, X_w, X_q*c - m_prim * W_e, -m_prim * g * cos(theta_e);...
    Z_u, Z_w, Z_q * c + m_prim * U_e, -m_prim * g * sin(theta_e);...
    M_u, M_w, M_q * c, 0;...
    0,0,1,0];
B_prim = ...
    [V0 * X_ni, V0 * X_tau;...
    V0 * Z_ni, V0 * Z_tau;...
    V0 * M_ni, V0 * M_tau;...
    0,0];

A = M^(-1) * A_prim;
B = M^(-1) * B_prim;

%%

end

