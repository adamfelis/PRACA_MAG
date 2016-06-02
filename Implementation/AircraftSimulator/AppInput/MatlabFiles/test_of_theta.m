clear;clc;
simulation_step = 0.2;
global_time = 100;

%%

load('../../AppOutput/0.mat');
A = aircraft.Strategies.A_longitudinal;
B = aircraft.Strategies.B_longitudinal;
%B = B(:,1);
% B(:,2) = B(:,1);
% B(3,2) = 0;
B(1,2) = 2*B(1,1);
B(2,2) = 2*B(2,1);
B(3,2) = 2*B(3,1);
D = 0;
C = eye(4);
R = eye(2);
p = 10;
Q = p * (C' * C);
K = lqr(A,B,Q,R);
%K = zeros(2,4);
%Nbar = 500;%should be calculated in respect to K
%Nbar = rscale(A,B,[1 1 1 1],D,K);
Nbar = 2000;
%Nbar=1;
u = [0.2;0];
%K = zeros(2,4);
% 
% B(1,2) = -2*B(1,1);
% B(2,2) = -2*B(2,1);
% B(3,2) = -2*B(3,1);

time = 0 : simulation_step : global_time;
%x0 = [178;0;0;9.4 * 2 * pi / 360];
% x0 = [0; 0; 0; 0];
% [T, Y] = ode45(@(t,x)StateSpace(t,x,A - B * K,B * Nbar,u), time, x0);

% 
% x0 = real(Y(250,:));
% 
% [T2, Y2] = ode45(@(t,x)StateSpace(t,x,A - B * K,B * Nbar,u), time, x0);

aircraft.PrepareTransferFunctions(A - B * K, B * Nbar);
figure(1);
% angle = 0;
% modification_u = aircraft.u_t_fun(angle * 2 * pi / 360, time) - aircraft.u_t_fun(angle * 2 * pi / 360,0.2);
% modification_w = aircraft.w_t_fun(angle * 2 * pi / 360, time) - aircraft.w_t_fun(angle * 2 * pi / 360,0.2);
% modification_q = aircraft.q_t_fun(angle * 2 * pi / 360, time) - aircraft.q_t_fun(angle * 2 * pi / 360,0.2);
% modification_theta = aircraft.theta_t_fun(angle * 2 * pi / 360, time) - aircraft.theta_t_fun(angle * 2 * pi / 360,0.2);
% moment = 0.1;
% Y(round(moment * length(time)): length(time),1) = Y(round(moment * length(time)): length(time),1) + modification_u(2: length(modification_u') - round(moment * length(time)) + 2)';
% Y(round(moment * length(time)): length(time),2) = Y(round(moment * length(time)): length(time),2) + modification_w(2: length(modification_w') - round(moment * length(time)) + 2)';
% Y(round(moment * length(time)): length(time),3) = Y(round(moment * length(time)): length(time),3) + modification_q(2: length(modification_q') - round(moment * length(time)) + 2)';
% Y(round(moment * length(time)): length(time),4) = Y(round(moment * length(time)): length(time),4) + modification_theta(2: length(modification_theta') - round(moment * length(time)) + 2)';
Draw_longitudinal;

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
U = aircraft.u_t_fun(u(1),time(2:length(time)), u(2)) - aircraft.u_t_fun(u(1),0.2, u(2));
W = aircraft.w_t_fun(u(1),time(2:length(time)), u(2)) - aircraft.w_t_fun(u(1),0.2, u(2));
theta = aircraft.theta_t_fun(u(1),time(2:length(time)), u(2)) - aircraft.theta_t_fun(u(1),0.2, u(2));
U = U';
W = W';
theta = theta';
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
time = 0.2:0.2:100;

values = [];
for i = 1:1:length(time)/5
    val = aircraft.theta_t_fun(0.1, 0.2 * i, 0) - aircraft.theta_t_fun(0.1, 0.2, 0);
    values = [values; val];
end
count = length(values);
for i = 1:1:length(time)/5
    val = aircraft.theta_t_fun(0.105, 0.2 * (count + i), 0) - aircraft.theta_t_fun(0.105, 0.2, 0);
    values = [values; val];
end
count = length(values);
for i = 1:1:length(time)
    val = aircraft.theta_t_fun(0.11, 0.2 * (count + i), 0) - aircraft.theta_t_fun(0.11, 0.2, 0);
    values = [values; val];
end
plot(0:0.2:(length(values)-1) * 0.2, values);

%%
figure(4);
hold on;
time = 0.2:0.2:300;
values=[];
ni = 0;
last_ni = 0;
ni_changed = false;
counter = 1;
another_counter = 1;
count = 0;
additional_factor = 0;
factor = 1;
while true
    another_counter = another_counter + 1;
    if(ni_changed)
        count = length(values);
        additional_factor = values(count);
        %counter = 1;
        ni_changed = false;
    end
    additional_factor = 0;
    values = [values, additional_factor + aircraft.theta_fun(ni, time(counter),0)];
    counter = counter + 1;
    if counter == length(time)
        break;
    end  
    if(mod(counter, 5) == 0)
        last_ni = ni;
        ni = ni + (factor * rand)/100;
        ni = min(ni, 0.4);
        ni = max(ni, 0);
        if ni == 0.4
           factor = -1; 
        end
        if ni == 0
           factor = 0; 
        end
        disp(ni);
        ni_changed = true;
        plot(time(counter - 1), values(length(values)),'ro')
    end
end
plot(0.2:0.2: (length(values) ) * 0.2, values);
%%
a = Aircraft(aircraft.Strategies.A_longitudinal, aircraft.Strategies.B_longitudinal, aircraft.Strategies.A_lateral, aircraft.Strategies.B_lateral, 0.2,10);
time = 0:0.2:100;
u_longitudinal = [0.1;0];
u_lateral = [0;0];
values = zeros(1, length(time));
for i = 1:1:length(time)
   result = a.SimulateLaplace (u_longitudinal, u_lateral);
   values(i) = result(9);
   if( i * 0.2 == 10)
       u_longitudinal = u_longitudinal + [0.1;0];
   end
end
plot(time, values);