clearvars -except values_u;
global_simulation_time = 1000.2;

V0 = 178;% ZMIANA sqrt(U_e * U_e + W_e * W_e);
theta_e = 9.4 * (2*pi)/360;%Dodano
U_e = V0*cos(theta_e);%DODANO
W_e = V0*sin(theta_e);%DODANO
Q_e = 0.07;% ????

initial_U = U_e;    
initial_W = W_e;
initial_V = 0;


%u = [10 * 2 * pi / 360; 0];
u = [3 * 2 * pi  /360 0.5 ]';
simulation_step = 0.2;
global_simulation_step_amount = (1 / simulation_step) * global_simulation_time;
simulation_time = 2;
interval = 0 : simulation_step : simulation_time;

U =[U_e];
W =[W_e];
Q =[Q_e];
theta =[theta_e];
time =[0];

counter = 0;
%[A, B] = CreateAB(U_e, W_e, theta_e);
[A, B] = CreateAB2(U_e, W_e,sqrt(U_e * U_e + W_e * W_e), theta_e);
%[A_lat, B_lat] = CreateABLateral(
B(:,2) = 2 * B(:,1);
ni_changed = false;
solution_index = 2;
%figure(1);
%hold on;
%TIME = 0
C = zeros(4);
C = eye(4);
%C(2,2) = 1;
%C(4,4) = 1;
R = eye(2);
% p = 1e-6;%U
% p = 1e-10;%W
% p = 0.4;%theta
p = 1;
Q_k = p * (C' * C);
K = lqr(A,B,Q_k,R);
% Nbar = 1.5280;%U
% Nbar = 1.0001;%W
% Nbar = 789.0058;%theta

Nbar = 1.1549e+03;%3.6491e+03;
%Nbar = 1;
%  A = A - B*K;
%  B = B*Nbar;
initial_x0 = [U_e; W_e; Q_e; theta_e];
while(global_simulation_step_amount > 0)
    global_simulation_step_amount = global_simulation_step_amount - 1;
    x0 = [U_e; W_e; Q_e; theta_e];
    if(ni_changed || solution_index > length(interval) || counter == 0)
        %[A, B] = CreateAB2(U_e, W_e,sqrt(U_e * U_e + W_e * W_e), theta_e);
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
%     if( counter>500 && mod(counter,10) == 0)
%     %if counter == 250 
%        u(1) = max(u(1) - 1 * 2 * pi / 360, 5 * 2 * pi / 360);
%        %u(1) = 0;
%        ni_changed = true;
%     end
end

% current_position = [0 0 0];
% %current_velocity = [initial_W initial_V initial_U];
% positions = [current_position];
% for i = 1:1:length(U)
%     
%     current_velocity = [W(i), 0, U(i)];
%     state_vector = [U(i), W(i), Q(i), theta(i)];
%     diff_state_vector = A * state_vector' + B * u;
%     acceleration_vector = [diff_state_vector(2), 0, diff_state_vector(1)];
%     new_position = MoveAircraft(current_position, current_velocity, acceleration_vector, theta(i), 0, 0, 0.2);
%     positions = [positions; new_position];
%     
%     current_position = new_position;
% end

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


W = -W;

figure(6);

color = 'b';

subplot(6,1,1);
plot(time , U, color);
hold on;
plot(time(1) , U(1), 'or');
legend('U');
grid on;

subplot(6,1,2);
plot(time , W, color);
hold on;
plot(time(1) , W(1), 'or');
legend('W');
grid on;


subplot(6,1,3);
plot(time , Q, color);
hold on;
plot(time(1) , Q(1), 'or');
legend('q');
grid on;

subplot(6,1,4);
plot(time , theta, color);
hold on;
plot(time(1) , theta(1), 'or');
legend('\theta');
grid on;

subplot(6,1,5);
plot(time , 1/178 * W, color);
hold on;
plot(time(1) , 1/178 * W(1), 'or');
legend('\alpha');
grid on;

subplot(6,1,6);
plot(time, theta - 1 / 178 * W, color);
hold on;
plot(time(1), theta(1) - 1 / 178 * W(1), 'or');
legend('\gamma');
grid on;
%%

u_value = U(5001);
w_value = W(5001);
q_value = Q(5001);
theta_value = theta(5001);
alpha_value = 1/178 * W(5001);
gamma_value = theta(5001) - 1 / 178 * W(5001);
clearvars -except u_value w_value q_value theta_value alpha_value gamma_value;

global_simulation_time = 1000.2;

V0 = 178;% ZMIANA sqrt(U_e * U_e + W_e * W_e);
theta_e = 9.4 * (2*pi)/360;%Dodano
U_e = V0*cos(theta_e);%DODANO
W_e = V0*sin(theta_e);%DODANO
Q_e = 0;

initial_U = U_e;
initial_W = W_e;
initial_V = 0;


u = [0.1; 0];
%u = [0.1 0 ]';
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

ni_changed = false;
solution_index = 2;
%figure(1);
%hold on;
%TIME = 0
C = eye(4);
R = eye(2);
p = 10;
Q_k = p * (C' * C);
K = lqr(A,B,Q_k,R);
Nbar = 1000;


A = A - B*K;
B = B*Nbar;



while(global_simulation_step_amount > 0)
    global_simulation_step_amount = global_simulation_step_amount - 1;
    x0 = [U_e; W_e; Q_e; theta_e];
    if(ni_changed || solution_index > length(interval) || counter == 0)
        %[A, B] = CreateAB2(U_e, W_e,sqrt(U_e * U_e + W_e * W_e), theta_e);
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
W = -W;

figure(6);


subplot(6,1,1);
plot(time , u_value / U(5001) * U, 'b');
hold on;
plot(time(1) , u_value / U(5001) * U(1), 'or');
legend('U');
grid on;

subplot(6,1,2);
plot(time , w_value / W(5001) * W, 'b');
hold on;
plot(time(1) , w_value / W(5001) * W(1), 'or');
legend('W');
grid on;


subplot(6,1,3);
plot(time , q_value / Q(5001) * Q, 'b');
hold on;
plot(time(1) , q_value / Q(5001) * Q(1), 'or');
legend('q');
grid on;

subplot(6,1,4);
plot(time , theta_value / theta(5001) * theta, 'b');
hold on;
plot(time(1) , theta_value / theta(5001) * theta(1), 'or');
legend('\theta');
grid on;

subplot(6,1,5);
plot(time ,alpha_value / (1/178 * W(5001)) *  1/178 * W, 'b');
hold on;
plot(time(1) ,alpha_value / (1/178 * W(5001)) * 1/178 * W(1), 'or');
legend('\alpha');
grid on;

subplot(6,1,6);
plot(time, theta - 1 / 178 * W, 'b');
hold on;
plot(time(1), theta(1) - 1 / 178 * W(1), 'or');
legend('\gamma');
grid on;

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