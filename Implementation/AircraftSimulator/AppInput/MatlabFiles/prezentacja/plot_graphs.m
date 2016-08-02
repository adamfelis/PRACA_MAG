figure(1)
subplot(3,1,1)
plot(time(1:2501), U(1:2501),'b')
grid on
legend('U')
hold on
plot(time(1), U(1), 'ro')
subplot(3,1,2)
plot(time(1:2501), W(1:2501),'b')
grid on
legend('W')
hold on
plot(time(1), W(1), 'ro')
subplot(3,1,3)
plot(time(1:2501), theta(1:2501),'b')
grid on
legend('\theta')
hold on
plot(time(1), theta(1), 'ro')