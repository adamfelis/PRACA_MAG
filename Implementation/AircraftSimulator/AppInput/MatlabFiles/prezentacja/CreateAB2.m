function [ A, B ] = CreateAB2( V0, theta_e )
%WSZYTKIE OBLICZENIA ZGODNE Z MACIERZAMI ZE STRONY 89 chapter 5
%% A and B setter
%theta_e = 9.4 * (2*pi)/360;%Dodano
rho = 0.3809;%OK
m = 17642;%OK
I_y = 165669;%OK
g = 9.81;%OK
X_u = 0.0076;%OK
Z_u = -0.7273;%OK
M_u = 0.0340;%OK
X_w = 0.0483;%OK
Z_w = -3.1245;%OK
M_w = -0.2169;%OK
X_w_dot = 0;%OK
Z_w_dot = -0.3997;%OK
M_w_dot = -0.5910;%OK
X_q = 0;%OK
Z_q = -1.2109;%OK
M_q = -1.2732;%OK
X_ni = 0.0618;%ZMIANA
Z_ni = -0.3741;%OK
M_ni = -0.5581;%OK
X_tau = 0;
Z_tau = 0;
M_tau = 0;

%V0 = 178;% ZMIANA sqrt(U_e * U_e + W_e * W_e);
U_e = V0*cos(theta_e);%DODANO
W_e = V0*sin(theta_e);%DODANO

S = 49.239;%OK
c = 4.889;%OK
m_prim = m / (0.5 * rho * V0 * S);%OK
I_y_prim = I_y / (0.5 * rho * V0 * S * c);%OK

%%

%%
M = [m_prim, -X_w_dot * c / V0, 0, 0;...%OK
    0, m_prim-Z_w_dot * c / V0, 0, 0;...%OK
    0, -M_w_dot * c / V0, I_y_prim, 0;...%OK
    0,0,0,1];%OK
vpa(M); %OK
A_prim = ...
    [X_u, X_w, X_q*c - m_prim * W_e, -m_prim * g * cos(theta_e);...%OK
    Z_u, Z_w, Z_q * c + m_prim * U_e, -m_prim * g * sin(theta_e);...%OK
    M_u, M_w, M_q * c, 0;...%OK
    0,0,1,0];%OK
vpa(A_prim); %OK
B_prim = ...
    [V0 * X_ni, V0 * X_tau;...%OK
    V0 * Z_ni, V0 * Z_tau;...%OK
    V0 * M_ni, V0 * M_tau;...%OK
    0,0];
vpa(B_prim);%OK

A = M^(-1) * A_prim;%OK
vpa(A); %OK
B = M^(-1) * B_prim;%OK
vpa(B); %OK

%%

end

