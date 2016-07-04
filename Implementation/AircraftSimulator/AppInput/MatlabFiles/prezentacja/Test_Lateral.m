clear;clc;
global_simulation_time = 20.2;

% V0 = 178;% ZMIANA sqrt(U_e * U_e + W_e * W_e);
% theta_e = 9.4 * (2*pi)/360;%Dodano
% U_e = V0*cos(theta_e);%DODANO
% W_e = V0*sin(theta_e);%DODANO
% Q_e = 0;

u = [-0.1;0.3];

simulation_step = 0.2;
global_simulation_step_amount = (1 / simulation_step) * global_simulation_time;
simulation_time = 2;
interval = 0 : simulation_step : simulation_time;

V =[];
P =[];
R =[];
phi =[];
psi =[];

time =[];

counter = 0;
%[A, B] = CreateAB(U_e, W_e, theta_e);
[A, B, U_e, W_e, theta_e] = CreateABLateral();

ni_changed = false;
solution_index = 2;

V_e = 0;
P_e = 0;
R_e = 0;
phi_e = 0;
psi_e = 0;

while(global_simulation_step_amount > 0)
    global_simulation_step_amount = global_simulation_step_amount - 1;
    x0 = [V_e; P_e; R_e; phi_e; psi_e];
    if(ni_changed || solution_index > length(interval) || counter == 0)
        %[A, B] = CreateAB2(sqrt(U_e * U_e + W_e * W_e), theta_e);
        [T,Y] = ode45(@(t,x)StateSpace(t,x,A,B,u), interval, x0);
        solution_index = 2;
        ni_changed = false;
    end
    V_e = Y(solution_index,1);
    P_e = Y(solution_index,2);
    R_e = Y(solution_index,3);
    phi_e = Y(solution_index,4);
    psi_e = Y(solution_index,5);
    
    V = [V; V_e];
    P = [P; P_e];
    R = [R; R_e];
    phi = [phi; phi_e];
    psi = [psi; psi_e];
    
    time = [time; simulation_step * counter];
    counter = counter + 1;
    solution_index = solution_index + 1;
     if counter == 5
% %     if(mod(counter, 5) == 0)
% % %        znak = time(length(time)) > global_simulation_time/4;
% % %        u = [u(1) - sign(znak - 0.5) * 1 * 2 * pi / 360; 0];
% %         u(2) = min(u(2) + 0.01,0.05);
         u(1) = 0;
         ni_changed = true;
     end
end

% phi = mod(phi, 2 * pi);
% psi = mod(psi, 2 * pi);


V_x = [V; V(length(V))];
V_x_fun = @(t) (V_x(round(t/simulation_step) + 1));
S_x = [];
for i = 0 : simulation_step : global_simulation_time
    if i > 0
        S_x = [ S_x; S_x(length(S_x)) + integral(V_x_fun, i-simulation_step,  i, 'ArrayValued', true)];
    else
        S_x = [ S_x; integral(V_x_fun, 0,  i, 'ArrayValued', true)];
    end
end

figure(3);


subplot(6,1,1);
plot(time , V, 'b');
hold on;
plot(time(1) , V(1), 'or');
legend('V');

subplot(6,1,2);
plot(time , P, 'b');
hold on;
plot(time(1) , P(1), 'or');
legend('P');

subplot(6,1,3);
plot(time , R, 'b');
hold on
plot(time(1) , R(1), 'or');
legend('R');

subplot(6,1,4);
plot(time , phi, 'b');
hold on;
plot(time(1) , phi(1), 'or');
%plot(time(500) , phi(500), 'or');
legend('\phi');

subplot(6,1,5);
plot(time , psi, 'b');
hold on;
plot(time(1) , psi(1), 'or');
legend('\psi');

subplot(6,1,6);
plot(time , 1/178 * V, 'b');
hold on;
plot(time(1) , 1/178 * V(1), 'or');
legend('\beta');
%clear;

