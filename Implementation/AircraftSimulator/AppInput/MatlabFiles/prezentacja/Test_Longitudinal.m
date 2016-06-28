clear;clc;
global_simulation_time = 500;

V0 = 178;% ZMIANA sqrt(U_e * U_e + W_e * W_e);
theta_e = 9.4 * (2*pi)/360;%Dodano
U_e = V0*cos(theta_e);%DODANO
W_e = V0*sin(theta_e);%DODANO
Q_e = 0;

initial_U = U_e;
initial_W = W_e;
initial_V = 0;


u = [0.1; 0];
%u = [0 0 ]';
simulation_step = 0.2;
global_simulation_step_amount = (1 / simulation_step) * global_simulation_time;
simulation_time = 2;
interval = 0 : simulation_step : simulation_time;

U =[];
W =[];
Q =[];
theta =[];
time =[];

counter = 0;
%[A, B] = CreateAB(U_e, W_e, theta_e);
[A, B] = CreateAB2(U_e, W_e,sqrt(U_e * U_e + W_e * W_e), theta_e);
%[A_lat, B_lat] = CreateABLateral(
C = eye(4);
R = eye(2);
ni_changed = false;
solution_index = 2;
%figure(1);
%hold on;
%TIME = 0;
initial_x0 = [U_e; W_e; Q_e; theta_e];
while(global_simulation_step_amount > 0)
    global_simulation_step_amount = global_simulation_step_amount - 1;
    x0 = [U_e; W_e; Q_e; theta_e];
    if(ni_changed || solution_index > length(interval) || counter == 0)
        %[A, B] = CreateAB2(U_e, W_e,sqrt(U_e * U_e + W_e * W_e), theta_e);
        p = 50;
        Q_k = p * C'*C;
        K = lqr(A, B, Q_k, R);
        K = zeros(2,4);
        [T,Y] = ode45(@(t,x)StateSpace(t,x,A,B,u), interval, x0);
        solution_index = 2;
        ni_changed = false;
 %       plot(T + TIME, Y(:,2));
 %       TIME = TIME + 2;
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
    %if(mod(counter, ) == 0)
%     if counter == 250 
%        %u(1) = min(u(1) + 1 * 2 * pi / 360, 0.3);
%        u(1) = 0;
%        ni_changed = true;
%     end
end

current_position = [0 0 0];
%current_velocity = [initial_W initial_V initial_U];
positions = [current_position];
for i = 1:1:length(U)
    
    current_velocity = [W(i), 0, U(i)];
    state_vector = [U(i), W(i), Q(i), theta(i)];
    diff_state_vector = A * state_vector' + B * u;
    acceleration_vector = [diff_state_vector(2), 0, diff_state_vector(1)];
    new_position = MoveAircraft(current_position, current_velocity, acceleration_vector, theta(i), 0, 0, 0.2);
    positions = [positions; new_position];
    
    current_position = new_position;
end

% V_x = [U; U(length(U))];
% X_x = V_x * simulation_step;
% W = W;
% V_y = [W; W(length(W))];
% X_y = V_y * simulation_step;
% theta = [theta; theta(length(theta))];

% S_x = [X_x(1)];
% S_y = [X_y(1)];
% for i = 2:1:length(X_x)
%     S_x = [S_x; S_x(i-1) + X_x(i) * cos(theta(i)) + X_y(i) * sin(theta(i))];
%     S_y = [S_y; S_y(i-1) + X_x(i) * sin(theta(i)) - X_y(i) * cos(theta(i))];
% end
figure(5);


subplot(4,1,1);
plot(time , U, 'r');
hold on;
plot(time(1) , U(1), 'or');
legend('U');
grid on;

subplot(4,1,2);
plot(time , theta - 0.0056179775280899 * W, 'r');
hold on;
plot(time(1) , theta(1) - 0.0056179775280899 * W(1), 'or');
legend('W');
grid on;

subplot(4,1,3);
plot(time , theta, 'r');
hold on;
plot(time(1) , theta(1), 'or');
legend('theta');
grid on;

subplot(4,1,4);
plot(time, W, 'r');
hold on;
plot(time(1), W(1), 'or');
legend('pos');
grid on;

% subplot(4,1,4);
% plot(positions(:,1), positions(:,3), 'g');
% hold on;
% plot(positions(1,1), positions(1,3), 'or');
% legend('pos');
% grid on;
%clear;
%%
figure(2)
[T,Y] = ode45(@(t,x)StateSpace(t,x,A,B,[0.18;0]), 0:0.2:30, initial_x0);
[T1,Y1] = ode45(@(t,x)StateSpace(t,x,A,B,[0.18;0]), 0:0.2:70, Y(151,:)');

subplot(4,1,1);
plot(T , Y(:,1), 'g');
hold on;
plot(T(1) , Y(1,1), 'or');
plot(T1 + 30 , Y1(:,1), 'g');
hold on;
plot(T1(1) + 30 , Y1(1,1), 'or');
legend('U');
grid on;

subplot(4,1,2);
plot(T , 0.0056179775280899 * Y(:,2), 'g');
hold on;
plot(T(1) , 0.0056179775280899 * Y(1,2), 'or');
plot(T1 + 30 ,  0.0056179775280899 * Y1(:,2), 'g');
hold on;
plot(T1(1) + 30 ,  0.0056179775280899 * Y1(1,2), 'or');
legend('W');
grid on;

subplot(4,1,3);
plot(T , Y(:,3), 'b');
hold on;
plot(T(1) , Y(1,3), 'or');
plot(T1 + 30 , Y1(:,3), 'g');
hold on;
plot(T1(1) + 30 , Y1(1,3), 'or');
legend('q');
grid on;

subplot(4,1,4);
plot(T, Y(:,4), 'g');
hold on;
plot(T(1), Y(1,4), 'or');
plot(T1 + 30 , Y1(:,4), 'g');
hold on;
plot(T1(1) + 30 , Y1(1,4), 'or');
legend('theta');
grid on;