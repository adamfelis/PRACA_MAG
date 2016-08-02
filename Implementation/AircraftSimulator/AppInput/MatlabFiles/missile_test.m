clear;
% load backup
% m = aircraft_collection.Instances(5).Missiles.Strategies(1).missileStates;
% aircraftPosition = m(:,8:9);

% dif = [aircraftPosition(1,1:2),0] - [0.5477e3   -1.0703e3 0];
% dif = 100 * (dif./norm(dif));
% x0 = [0.5477e3   -1.0703e3 0 dif]';

F = [zeros(3,3), eye(3);zeros(3,6)];
G = [zeros(3,3);eye(3)];
C = [eye(3), zeros(3); zeros(3,6)];
p = 1;
Q = p * (C' * C);
R = eye(3);
K = lqr(F,G,Q,R);

domain = 0:10:1500;
aircraftPosition = [0*domain', 100 * sin(domain)' + 1000];%300 * sin(0.2 *domain)'];%10000 * ones(length(domain),1)];

x0 = [-5 -5 0 0 0 0]';
time = 0:0.2:25;
solution = [x0'];
Nbar = 1;
for i = 1:1:length(time);
    u = [aircraftPosition(i,1) aircraftPosition(i,2) 0]';
    [T, Y] = ode45(@(t,x)StateSpace(t,x,F - G * K,G * Nbar,u), time, x0);
    
    next_solution = Y(2,:);
    
    maxVelocity = 3;
    minVelocity = -2;
            
   next_solution(4) = min(next_solution(4), maxVelocity);
    next_solution(4) = max(next_solution(4), minVelocity);
    next_solution(5) = min(next_solution(5), maxVelocity);
    next_solution(5) = max(next_solution(5), minVelocity);
    
    solution = [solution; next_solution]; 
    
    x0 = next_solution';
end
TIME = 0:0.2:(27 - 1) * 0.2;
figure(1);
hold on;
references =[];
for i = 1:1:length(solution)
%     clf;
%     hold on;
%      plot(-aircraftPosition(i,1), aircraftPosition(i,2), 'bo');
     if i == 1
         ref = solution(i,:);
     else
         ref = K * solution(i,:)';
     end
     solution(i,1) = ref(1);
     solution(i,2) = ref(2);
     solution(i,3) = ref(3);
%      plot(-ref(1), ref(2), 'ro');
    % references = [references; ref'];
%     plot(i * 0.2, aircraftPosition(i,2), 'bo');
%     plot(i * 0.2, solution(i,2), 'ro');
% 
%     plot(aircraftPosition(i,1), aircraftPosition(i,2), 'bo');
%     plot(solution(i,1), solution(i,2), 'ro');
%     pause(0.2);
end
%subplot(2,1,1);
plot3(0:0.2:0.2 * length(solution) - 0.2,  aircraftPosition(1:length(solution),1), aircraftPosition(1:length(solution),2));
hold on;
plot3(0:0.2:0.2 * length(solution) - 0.2, solution(1:length(solution),1), solution(1:length(solution),2));
legend('targets position', 'missiles position')
grid on;
% subplot(2,1,2);
% plot3(0:0.2:0.2 * length(solution) - 0.2,  aircraftPosition(1:length(solution),1), aircraftPosition(1:length(solution),2));
% hold on;
% plot3(0:0.2:0.2 * length(solution) - 0.2, solution(1:length(solution),1), solution(1:length(solution),2));
% grid on;
xlabel('time');
ylabel('x');
zlabel('y');
%%

[T, Y] = ode45(@(t,x)StateSpace(t,x,F,G,[1 2 0.2]'), 0:0.2:20, [0 0 0 0 0 0]');
plot3(Y(:,1), Y(:,2), Y(:,3))
grid on
xlabel('x')
ylabel('y')
zlabel('z')

%%
clear;
F = [zeros(3,3), eye(3);zeros(3,6)];
G = [zeros(3,3);eye(3)];
initial_u = [0 0 0]';
x0 = [0 0 0 0 0 0]';
time = 0:0.2:50;
solution = [x0'];

u = initial_u;
indexes = [];
for i = 1:1:length(time);
    [T, Y] = ode45(@(t,x)StateSpace(t,x,F,G,u), 0:0.2:4, x0);
    
    next_solution = Y(2,:);
    solution = [solution; next_solution]; 
    
    x0 = next_solution';
    
    if mod(i,10) == 0
       indexes = [indexes i];
       u(1) = (rand - 0.5) * 2;
       u(2) = (rand - 0.5) * 2;
       u(3) = (rand - 0.5) * 2; 
    end
end
plot3(solution(1:length(solution),1), solution(1:length(solution),2), solution(1:length(solution),3));
grid on;
xlabel('x');
ylabel('y');
zlabel('z');
hold on

plot3(solution(indexes,1), solution(indexes,2), solution(indexes,3), 'ro');