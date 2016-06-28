function [ A, B, U_e, W_e, theta_e ] = CreateABLateral()%( U_e, W_e, theta_e )
%WSZYTKIE OBLICZENIA ZGODNE Z MACIERZAMI ZE STRONY 90 chapter 4
%% A and B setter
theta_e = 9.4 * (2*pi)/360;%Dodano
m = 17642;
I_x = 33898;
I_z = 189496;
I_xz = 2952;
g = 9.81;
ro = 0.3809;
V_0 = 178;
S = 49.239;
b = 11.787;

Y_v = -0.5974;
L_v = -0.1048;
N_v = 0.0987;
Y_p = 0;
L_p = -0.1164;
N_p = -0.0045;
Y_r = 0;
L_r = 0.0455;
N_r = -0.1132;
Y_ksi = -0.0159;
L_ksi = 0.0454;
N_ksi = 0.00084;
Y_dzeta = 0.1193;
L_dzeta = 0.0086;
N_dzeta = -0.0741;

m_prim =  m / (1/2 * ro * V_0 * S);
I_x_prim =  I_x / (1/2 * ro * V_0 * S * b);
I_z_prim =  I_z / (1/2 * ro * V_0 * S * b);
I_xz_prim =  I_xz / (1/2 * ro * V_0 * S * b);

U_e = V_0 * cos(theta_e);
W_e = V_0 * sin(theta_e);



%%

%%

M = [m_prim, 0, 0, 0, 0; ...
    0, I_x_prim, -I_xz_prim, 0, 0; ...
    0, -I_xz_prim, I_z_prim, 0, 0;...
    0,0,0,1,0;
    0,0,0,0,1;];
vpa(M);
A_prim = ...
    [Y_v, Y_p * b + m_prim*W_e, Y_r * b - m_prim*U_e, m_prim * g * cos(theta_e), m_prim * g * sin(theta_e);...
    L_v, L_p * b, L_r * b, 0, 0;...
    N_v, N_p * b, N_r * b, 0, 0;...
    0, 1, 0, 0, 0;...
    0, 0, 1, 0, 0];
vpa(A_prim); 
B_prim = ...
    [V_0 * Y_ksi, V_0 * Y_dzeta;...
    V_0 * L_ksi, V_0 * L_dzeta;...
    V_0 * N_ksi, V_0 * N_dzeta;...
    0, 0;
    0, 0;];
vpa(B_prim);

A = M^(-1) * A_prim
vpa(A);
B = M^(-1) * B_prim
vpa(B);

A(2,3) = - A(2,3);
end

