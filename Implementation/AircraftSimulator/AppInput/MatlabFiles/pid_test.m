%%

clear all;
clc;

s = tf('s');
P_pitch = (-4.888 * s^2 - 1.373 * s + 0.0006846) / (s^4 + 0.741 * s^3 + 2.008 * s^2 + 0.03272 * s + 0.01192 );

H = [1];

M = feedback(P_pitch, H);
step(P_pitch);
grid on;
hold on;


%%

Kp = 0;
Ki = 0.0010069;
Kd = 0;

controller = pid(Kp, Ki, Kd);

Mc = feedback(controller * P_pitch, H);
step(Mc);
grid on;