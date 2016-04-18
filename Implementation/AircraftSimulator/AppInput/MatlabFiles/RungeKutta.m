
% A = [-0.00819, -25.70839, 0, -32.17095; - 0.00019, -1.27626, 1, 0; 0.00069, 1.02176, -2.40523,0;0,0,1,0];
% B = [-6.90939 0;-0.14968 0;-14.06111 0 ;0 0 ];
A = [a(1,1),a(1,2),a(1,3),a(1,4);...
    a(1,5),a(1,6),a(1,7),a(1,8);...
    a(1,9),a(1,10),a(1,11),a(1,12);...
    a(1,13),a(1,14),a(1,15),a(1,16)];

%A = [a(1,1:4);a(1,5:8);a(1,9:12);a(1,13:16)];
B = [b(1,1:2); b(1,3:4); b(1,5:6); b(1,7:8)];
%A = real(A);
%u = [0.5;0.5];
u = u';
% if(simulation_index > 1)
%     result = Y(simulation_index + 1,:);
%     return;
% end

[T,Y] = ode45(@(t,x)StateSpace(t,x,A,B,u), 0:0.02:2.5, x0);
%result = Y(2,:);
% figure(1);
% hold on;
% plot(T, Y(:,1));
result = Y(simulation_index + 1,:);

% ==============================================
% A = [-0.0352010727, 0.106997319, 0, -9.81;...
%     -0.213994637, -0.44, 1000, 0; ...
%     0.000119837, -0.0153536, -0.560279, 0;...
%     0,0,1,0];
% B = [0,0;...
%     -0.0221206453, 0;...
%     -0.00465799728, 0;...
%     0,0];
% C = eye(4);%[0 1 0 0];
% D = [0 0;0 0;0 0;0 0];%0;
% 
% p = 50;
% Q = p*C'*C;
% R = 1;
% 
% [K] = lqr(A,B,Q,R);
% [K] = [0 0 0 0];
% Nbar = rscale(A,B,C,D,K);
% 
% u.time = 1;
% u.signals.values = [0.5; 0.5];
% u.signals.dimensions = [2 1];
% x0 = [1000;0;0;0];
% 
% sim('ownStateSpaceModel');
% result = y.Data(1,:);
% 
% disp('aaaa');
