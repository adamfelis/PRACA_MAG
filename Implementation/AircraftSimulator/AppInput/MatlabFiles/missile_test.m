clear;
F = [zeros(3,3), eye(3);zeros(3,6)];
G = [zeros(3,3);eye(3)];
C = [eye(3), zeros(3); zeros(3,6)];
p = 10;
Q = p * (C' * C);
R = eye(3);
K = lqr(F,G,Q,R);

domain = 0:0.1:15;
aircraftPosition = [domain', sin(domain)'];%300 * sin(0.2 *domain)'];

x0 = [-50 -23 0 0 0 0]';
time = 0:0.2:20;
solution = [];
Nbar = 3.125;
for i = 1:1:length(domain)
    u = [aircraftPosition(i,1) aircraftPosition(i,2) 0]';
    [T, Y] = ode45(@(t,x)StateSpace(t,x,F - G * K,G * Nbar,u), time, x0);
    solution = [solution; Y(2,:)];
    x0 = Y(2,:)';
end
TIME = 0:0.2:(length(domain) - 1) * 0.2;
figure(1);
hold on;
for i = 1:1:length(TIME)
%     clf;
%     hold on;
%     plot(aircraftPosition(i,1), aircraftPosition(i,2), 'bo');
%     plot(solution(i,1), solution(i,2), 'ro');
    plot(i * 0.2, aircraftPosition(i,1), 'bo');
    plot(i * 0.2, solution(i,1), 'ro');
    %pause(0.2);
end