clear;clc;
global_simulation_time = 20;

V0 = 178;% ZMIANA sqrt(U_e * U_e + W_e * W_e);
theta_e = 9.4 * (2*pi)/360;%Dodano
U_e = V0*cos(theta_e);%DODANO
W_e = V0*sin(theta_e);%DODANO
Q_e = 0;


u = [0; 0];

simulation_step = 0.02;
global_simulation_step_amount = (1 / simulation_step) * global_simulation_time;
simulation_time = 3;
interval = 0 : simulation_step : simulation_time;

U =[];
W =[];
Q =[];
theta =[];
time =[];

counter = 0;
%[A, B] = CreateAB(U_e, W_e, theta_e);
[A, B] = CreateAB2();

ni_changed = false;
solution_index = 2;

while(global_simulation_step_amount > 0)
    global_simulation_step_amount = global_simulation_step_amount - 1;
    x0 = [U_e; W_e; Q_e; theta_e];
    if(ni_changed || solution_index > length(interval) || counter == 0)
        if(ni_changed)
            interval = interval + 2;
        end
        if(~ni_changed)
            interval = interval + simulation_time; 
        end
        [T,Y] = ode45(@(t,x)StateSpace(t,x,A,B,u), interval, x0);
        solution_index = 2;
        ni_changed = false;
    end
    U_e = Y(solution_index,1);
    W_e = Y(solution_index,2);
    Q_e = Y(solution_index,3);
    theta_e = Y(solution_index,4);
    
    U = [U; U_e];
    W = [W; W_e];
    Q = [Q; Q_e];
    theta = [theta; theta_e];
    time = [time; simulation_step * counter];
    counter = counter + 1;
    solution_index = solution_index + 1;
    if(mod(counter, 100) == 0)
       u = [u(1) + 1 * 2 * pi / 360; 1];
       ni_changed = true;
    end
end


V_x = [U; U(length(U))];
V_x_fun = @(t) (V_x(round(t/simulation_step) + 1));
S_x = [];
for i = 0 : simulation_step : global_simulation_time
    if i > 0
        S_x = [ S_x; S_x(length(S_x)) + integral(V_x_fun, i-simulation_step,  i, 'ArrayValued', true)];
    else
        S_x = [ S_x; integral(V_x_fun, 0,  i, 'ArrayValued', true)];
    end
end
% A_x = zeros(1, length(V_x) - 1);
% for i = 1:1:length(V_x)-1
%     A_x(i) = (V_x(i+1) - V_x(i)) / simulation_step;
% end
% V0t = (V_x .* time);
% S_x = V0t(2:length(time)) +  A_x' .* time(2:length(time)) .* time(2:length(time)) / 2;




V_y = [W; W(length(W))];
V_y_fun = @(t) (V_y(round(t/simulation_step) + 1));
S_y = [];
for i = 0 : simulation_step : global_simulation_time
    if i > 0
        S_y = [ S_y; S_y(length(S_y)) + integral(V_y_fun, i-simulation_step,  i, 'ArrayValued', true)];
    else
        S_y = [ S_y; integral(V_y_fun, 0,  i, 'ArrayValued', true)];
    end
end
% A_y = zeros(1, length(V_y) - 1);
% for i = 1:1:length(V_y)-1
%     A_y(i) = (V_y(i+1) - V_y(i)) / simulation_step;
% end
% S_y = A_y' .* time(2:length(time)) .* time(2:length(time)) / 2;

figure(1);


subplot(4,1,1);
plot(time , U, 'g');
hold on;
plot(time(1) , U(1), 'or');
legend('U');

subplot(4,1,2);
plot(time , W, 'g');
hold on;
plot(time(1) , W(1), 'or');
legend('W');

subplot(4,1,3);
plot(time , theta, 'b');
hold on;
plot(time(1) , theta(1), 'or');
legend('theta');

subplot(4,1,4);
plot(S_x , S_y, 'g');
hold on;
plot(S_x(1) , S_y(1), 'or');
legend('pos');
%clear;

