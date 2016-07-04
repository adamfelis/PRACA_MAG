time = 0:0.2:1000;
figure(10);

eta = 1 * 2 * pi / 360;


subplot(4,1,1);
plot(time, aircraft.Strategies(1).u_t_fun(eta,time))
grid on;
legend('U');

subplot(4,1,2);
plot(time, aircraft.Strategies(1).w_t_fun(eta,time))
grid on;
legend('W');

subplot(4,1,3);
plot(time, aircraft.Strategies(1).q_t_fun(eta,time))
grid on;
legend('Q');

subplot(4,1,4);
plot(time, aircraft.Strategies(1).theta_t_fun(eta,time))
grid on;
legend('\theta');