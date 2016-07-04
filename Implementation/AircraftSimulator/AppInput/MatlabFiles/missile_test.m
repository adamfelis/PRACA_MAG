clear;
load backup
m = aircraft_collection.Instances(5).Missiles.Strategies(1).missileStates;
aircraftPosition = m(:,8:9);

dif = [aircraftPosition(1,1:2),0] - [0.5477e3   -1.0703e3 0];
dif = 100 * (dif./norm(dif));
x0 = [0.5477e3   -1.0703e3 0 dif]';

F = [zeros(3,3), eye(3);zeros(3,6)];
G = [zeros(3,3);eye(3)];
C = [eye(3), zeros(3); zeros(3,6)];
p = 1;
Q = p * (C' * C);
R = eye(3);
K = lqr(F,G,Q,R);

domain = 0:10:1500;
%aircraftPosition = [domain', 10000 * ones(length(domain),1)];%1000 * sin(domain)' + 1000];%300 * sin(0.2 *domain)'];

%x0 = [-5 -5 0 0 0 0]';
time = 0:0.2:20;
solution = [x0'];
Nbar = 1;
for i = 1:1:27
    u = [aircraftPosition(i,1) aircraftPosition(i,2) 0]';
    [T, Y] = ode45(@(t,x)StateSpace(t,x,F - G * K,G * Nbar,u), time, x0);
    
    next_solution = Y(2,:);
    
 %    maxVelocity = 1;
%     minVelocity = 0.1;
            
   %next_solution(4) = min(next_solution(4), maxVelocity);
    %next_solution(4) = max(next_solution(4), minVelocity);
    %next_solution(5) = min(next_solution(5), maxVelocity);
    %next_solution(5) = max(next_solution(5), minVelocity);
    
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
     plot(-aircraftPosition(i,1), aircraftPosition(i,2), 'bo');
     if i == 1
         ref = solution(i,:);
     else
         ref = K * solution(i,:)';
     end
     plot(-ref(1), ref(2), 'ro');
     %references = [references; ref'];
    %plot(i * 0.2, aircraftPosition(i,2), 'bo');
    %plot(i * 0.2, solution(i,2), 'ro');
    pause(0.2);
end

%%

