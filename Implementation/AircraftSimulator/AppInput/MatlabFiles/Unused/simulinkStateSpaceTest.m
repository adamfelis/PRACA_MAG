
A_B_determinant;
A = A2;
B = B2;
C = eye(4);
D = zeros(4,2);

x0 = [U_e; W_e; Q_e; theta_e];

global Ni Tau;
syms Ni Tau;

u.time = 1;
u.signals.values = [Ni; Tau];
u.signals.dimensions = [2 1];

sim('ownStateSpaceModel');