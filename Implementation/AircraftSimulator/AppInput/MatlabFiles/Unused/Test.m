

U_e = 178;
W_e = 0;
Q_e = 0;
theta_e = 0;

[A, B] = CreateAB(U_e, W_e, theta_e);

u = [0.3; 1];

simulation_step = 0.02;
simulation_time = 30;
interval = 0 : simulation_step : simulation_time;

x0 = [U_e; W_e; Q_e; theta_e];
[T,Y] = ode45(@(t,x)StateSpace(t,x,A,B,u), interval, x0);

%róznice dzielone by policzyc przyspieszenie:

V_x = Y(:,1);
A_x = zeros(1, length(V_x) - 1);
for i = 1:1:length(V_x)-1
    A_x(i) = (V_x(i+1) - V_x(i)) / simulation_step;
end
V0t = (V_x .* T);
S_x = V0t(2: length(V0t)) + A_x' .* T(2:length(T)) .* T(2:length(T)) / 2;


V_y = Y(:,2);
A_y = zeros(1, length(V_y) - 1);
for i = 1:1:length(V_y)-1
    A_y(i) = (V_y(i+1) - V_y(i)) / simulation_step;
end
S_y = A_y' .* T(2:length(T)) .* T(2:length(T)) / 2;

figure(1);
plot(-S_x , S_y);
hold on;
plot(-S_x(1) , S_y(1), 'ro');

