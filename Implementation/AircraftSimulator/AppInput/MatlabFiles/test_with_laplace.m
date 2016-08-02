
figure(11);
offset = 0;

U = [];
W = [];
Q = [];
theta = [];

for i = 1:1:10
    if i == 10
       time = offset+0.2:0.2:400.2;
    else
        time = offset + 0.2:0.2:offset + 20;
        eta = i * 2 * pi / 360;
    end
    
    U = [U , aircraft.Strategies(1).u_t_fun(eta,time)];
    W = [W , aircraft.Strategies(1).w_t_fun(eta,time)];
    Q = [Q , aircraft.Strategies(1).q_t_fun(eta,time)];
    theta = [theta , aircraft.Strategies(1).theta_t_fun(eta,time)];
    offset = i * 20;
end

time = 0:0.2:400;

hold on;
subplot(4,1,1);
plot(time, U, 'b')
hold on;
plot(time, aircraft.Strategies(1).u_t_fun(eta, time), 'r--')
grid on;
grid minor;
legend('U');

hold on;
subplot(4,1,2);
plot(time,W, 'b')
hold on;
plot(time, aircraft.Strategies(1).w_t_fun(eta, time), 'r--')
grid on;
grid minor;
legend('W');

hold on;
subplot(4,1,3);
plot(time, Q, 'b')
hold on;
plot(time, aircraft.Strategies(1).q_t_fun(eta, time), 'r--')
grid on;
grid minor;
legend('Q');

hold on;
subplot(4,1,4);
plot(time, theta, 'b')
hold on;
plot(time, aircraft.Strategies(1).theta_t_fun(eta, time), 'r--')
grid on;
grid minor;
legend('theta');