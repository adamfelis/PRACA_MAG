A = [-3, 8; 0 0];
B = [0; 4];
C = eye(2);%[1; 0];
D = [0; 0];
V = ctrb(A, B)
% k = sym('k',[1 2])
k = [8.09; 16.5]
A = A - B*k'


u.time = 1;
u.signals.values = [0.5];
u.signals.dimensions = [1];
x0 = [1.0;0];

sim('ownStateSpaceModel');