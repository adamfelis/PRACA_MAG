A = [-0.00819, -25.70839, 0, -32.17095; - 0.00019, -1.27626, 1, 0; 0.00069, 1.02176, -2.40523,0;0,0,1,0];
B = [-6.80939;-0.14968;-14.06111;0];
C = [0 1 0 0];
D = [0];

p = 50;
Q = p*C'*C
R = 1;
[K] = lqr(A,B,Q,R)
Nbar = rscale(A,B,C,D,K)


sys_cl = ss(A-B*K,B*Nbar,C,D);
angle =1;
[y,t,x] = step(angle*sys_cl);
step(angle*sys_cl)
xlabel('time (sec)');
ylabel('angle of attack (rad)');
title('Closed-Loop Step Response: LQR');