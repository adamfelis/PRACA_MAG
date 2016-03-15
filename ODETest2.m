clc
clear
format
y0 = zeros(9, 1);
y0(1) = 1;
y0(4) = 1;
y0(9) = 1;
tspan = [0 0.001];
options = odeset('Refine', 1,'AbsTol',1e-3,'Stats','on');
[t,y] = ode45(@vdp2, tspan, y0, options);

% [n,m] = size(y)
% time = linspace(tspan(1), tspan(2), n);
% x1(1, 1) = 0;
% y1(1, 1) = 0;
% z1(1, 1) = 0;
% ti = 0.2;
% for i = 2 : n
%     x1(i, 1) = x1(i - 1, 1) + y(i,1) * ti;
%     y1(i, 1) = y1(i - 1, 1) + y(i,2) * ti;
%     z1(i, 1) = z1(i - 1, 1) + y(i,3) * ti;
% end
% figure
% quiver3(x1, y1, z1,   y(:,1),y(:,2),y(:,3))
% view(-35,45)

% plot(t,y(:,1),'-o',t,y(:,2),'-o',t,y(:,3),'-o')
% xlabel('Time t');
% ylabel('Solution y');
% legend('y_1','y_2','y_3')