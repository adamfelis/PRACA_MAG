
simulation_step = 0.2;
global_time = 100;

%%

load('../../../AppOutput/0.mat');
A = aircraft.Strategies.A_longitudinal;
B = aircraft.Strategies.B_longitudinal;
%B = B(:,1);
% B(:,2) = B(:,1);
% B(3,2) = 0;
D = 0;
C = eye(4);
R = eye(2);
p = 2;
Q = p * (C' * C);
K = lqr(A,B,Q,R);
%K = zeros(2,4);
%Nbar = 500;%should be calculated in respect to K
%Nbar = rscale(A,B,[1 1 1 1],D,K);
Nbar = 30;
%Nbar=1;
u = [0.1;0];
%K = zeros(2,4);
% 
% B(1,2) = -2*B(1,1);
% B(2,2) = -2*B(2,1);
% B(3,2) = -2*B(3,1);

time = 0 : simulation_step : global_time;
%x0 = [178;0;0;9.4 * 2 * pi / 360];
x0 = [0; 0; 0; 0];
[T, Y] = ode45(@(t,x)StateSpace(t,x,A - B * K,B * Nbar,u), time, x0);

% 
% x0 = real(Y(250,:));
% 
% [T2, Y2] = ode45(@(t,x)StateSpace(t,x,A - B * K,B * Nbar,u), time, x0);

aircraft.PrepareTransferFunctions(A - B * K, B * Nbar);
figure(1);
angle = 0;
modification_u = aircraft.u_t_fun(angle * 2 * pi / 360, time) - aircraft.u_t_fun(angle * 2 * pi / 360,0.2);
modification_w = aircraft.w_t_fun(angle * 2 * pi / 360, time) - aircraft.w_t_fun(angle * 2 * pi / 360,0.2);
modification_q = aircraft.q_t_fun(angle * 2 * pi / 360, time) - aircraft.q_t_fun(angle * 2 * pi / 360,0.2);
modification_theta = aircraft.theta_t_fun(angle * 2 * pi / 360, time) - aircraft.theta_t_fun(angle * 2 * pi / 360,0.2);
moment = 0.1;
Y(round(moment * length(time)): length(time),1) = Y(round(moment * length(time)): length(time),1) + modification_u(2: length(modification_u') - round(moment * length(time)) + 2)';
Y(round(moment * length(time)): length(time),2) = Y(round(moment * length(time)): length(time),2) + modification_w(2: length(modification_w') - round(moment * length(time)) + 2)';
Y(round(moment * length(time)): length(time),3) = Y(round(moment * length(time)): length(time),3) + modification_q(2: length(modification_q') - round(moment * length(time)) + 2)';
Y(round(moment * length(time)): length(time),4) = Y(round(moment * length(time)): length(time),4) + modification_theta(2: length(modification_theta') - round(moment * length(time)) + 2)';
% Draw_longitudinal;

% u = [0;0];
% [T2, Y2] = ode45(@(t,x)StateSpace(t,x,A - B * K,B * Nbar,u), time, x0);
% Y(:,1) = Y(:,1) + Y2(:,1);
% Y(:,2) = Y(:,2) + Y2(:,2);
% Y(:,4) = Y(:,4) - Y2(:,4);
% figure(2)
% Draw_longitudinal;
% Hurwitz criterion
% [43 1 0 0; 155 888 43 1; 0 14 155 888; 0 0 0 14]
% hur = ans
% det(hur(1:1,1:1))
% det(hur(1:2,1:2))
% det(hur(1:3,1:3))
% det(hur(1:4,1:4))

%%
U = Y(1,1) - (Y(2:length(T),1) - (Y(2,1) - Y(1,1)) - Y(1,1));
W = Y(2:length(T),2) - (Y(2,2) - Y(1,2));
theta = Y(2:length(T),4) - (Y(2,4) - Y(1,4));

V_x = [U; U(length(U))];
X_x = V_x * simulation_step;
%W = -W;
V_y = [W; W(length(W))];
X_y = V_y * simulation_step;
theta = [theta; theta(length(theta))];

figure(2);
hold on;
grid on;
S_x = X_x(1);
S_y = X_y(1);
plot(S_x,S_y);
for i = 2:1:length(X_x)
    %pause(0.2);
    S_x = S_x + X_x(i) * cos(theta(i)) + X_y(i) * sin(theta(i));
    S_y = S_y + X_x(i) * sin(theta(i)) + X_y(i) * cos(theta(i));
    hold on;
    plot(S_x,S_y, 'bo');
    if mod(0.2 * i,10) == 0
       plot(S_x,S_y, 'ro');
    end
    title(0.2 * i);
end

%%
changes_of_theta = [linspace(30,30,10000)];%, linspace(10,0,3000),linspace(0,-10,4000)];
Y_temp = Y(:,4);
for i = 1:1:100/0.01
    val = aircraft.theta_t_fun(changes_of_theta(i) * 2 * pi / 360, i* 0.01);
    Y_temp(i) = Y_temp(i) + val;
end

plot(T, Y_temp);