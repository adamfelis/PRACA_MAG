% subplot(4,1,1);
% plot(T(2:length(T)) , Y(1,1) - (Y(2:length(T),1) - (Y(2,1) - Y(1,1)) - Y(1,1)), 'b');
% legend('U');
% grid on;
% 
% subplot(4,1,2);
% plot(T(2:length(T)) , Y(2:length(T),2) - (Y(2,2) - Y(1,2)), 'b');
% legend('W');
% grid on;
% 
% subplot(4,1,3);
% plot(T(2:length(T)) , Y(2:length(T),3) - (Y(2,3) - Y(1,3)), 'b');
% legend('q');
% grid on;
% 
% subplot(4,1,4);
% plot(T(2:length(T)) , Y(2:length(T),4) - (Y(2,4) - Y(1,4)), 'b');
% legend('theta');
% grid on;

subplot(5,1,1);
plot(time(2:length(time)) , aircraft.u_t_fun(u(1),time(2:length(time)), u(2)) - aircraft.u_t_fun(u(1),0.2, u(2)) , 'b');
legend('U');
grid on;

subplot(5,1,2);
plot(time(2:length(time)) , aircraft.w_t_fun(u(1),time(2:length(time)), u(2)) - aircraft.w_t_fun(u(1),0.2, u(2)) , 'b');
legend('W');
grid on;

subplot(5,1,3);
plot(time(2:length(time)) , aircraft.q_t_fun(u(1),time(2:length(time)), u(2)) - aircraft.q_t_fun(u(1),0.2, u(2)) , 'b');
legend('q');
grid on;

subplot(5,1,4);
plot(time(2:length(time)) , aircraft.theta_t_fun(u(1),time(2:length(time)), u(2)) - aircraft.theta_t_fun(u(1),0.2, u(2)) , 'b');
legend('theta');
grid on;

subplot(5,1,5);
plot(time(2:length(time)) , aircraft.a_z_t_fun(u(1),time(2:length(time)), u(2)) - aircraft.a_z_t_fun(u(1),0.2, u(2)) , 'b');
legend('a_z');
grid on;

%%aircraft responses

% ni = 0;
% tau = 0.1;
% 
% subplot(4,2,2);
% plot(T(2:length(T)) , aircraft.u_t_fun(ni, T(2:length(T)), tau) - aircraft.u_t_fun(ni, 0.2, tau), 'b');
% legend('U');
% grid on;
% 
% subplot(4,2,4);
% plot(T(2:length(T)) , aircraft.w_t_fun(ni, T(2:length(T)), tau) - aircraft.w_t_fun(ni, 0.2, tau), 'b');
% legend('W');
% grid on;
% 
% subplot(4,2,6);
% plot(T(2:length(T)) ,aircraft.q_t_fun(ni, T(2:length(T)), tau) - aircraft.q_t_fun(ni, 0.2, tau), 'b');
% legend('q');
% grid on;
% 
% subplot(4,2,8);
% plot(T(2:length(T)) , aircraft.theta_t_fun(ni, T(2:length(T)), tau) - aircraft.theta_t_fun(ni, 0.2, tau), 'b');
% legend('theta');
% grid on;