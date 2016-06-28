clear;
F = [zeros(3,3), eye(3);zeros(3,6)];
G = [zeros(3,3);eye(3)];
C = [eye(3), zeros(3); zeros(3,6)];
p = 10;
Q = p * (C' * C);
R = eye(3);
K = lqr(F,G,Q,R);

domain = 0:0.1:15;
aircraftPosition = [domain', 1000 * sin(domain)' + 1000];%300 * sin(0.2 *domain)'];

x0 = [-5 -5 0 0 0 0]';
time = 0:0.2:20;
solution = [];
Nbar = 1;
for i = 1:1:length(domain)
    u = [aircraftPosition(i,1) aircraftPosition(i,2) 0]';
    [T, Y] = ode45(@(t,x)StateSpace(t,x,F - G * K,G * Nbar,u), time, x0);
    
    next_solution = Y(2,:);
    
    maxVelocity = 1;
    minVelocity = 0.1;
            
    next_solution(4) = min(next_solution(4), maxVelocity);
    %next_solution(4) = max(next_solution(4), minVelocity);
    next_solution(5) = min(next_solution(5), maxVelocity);
    %next_solution(5) = max(next_solution(5), minVelocity);
    
    solution = [solution; next_solution]; 
    
    x0 = next_solution';
end
TIME = 0:0.2:(length(domain) - 1) * 0.2;
figure(1);
hold on;
for i = 1:1:length(TIME)
%     clf;
%     hold on;
     plot(aircraftPosition(i,1), aircraftPosition(i,2), 'bo');
     ref = K * solution(i,:)';
     plot(ref(1), ref(2), 'ro');
    %plot(i * 0.2, aircraftPosition(i,2), 'bo');
    %plot(i * 0.2, solution(i,2), 'ro');
    pause(0.2);
end