function [x_dot] = StateSpace (t,x, A, B, u)

%u1 = interp1(linspace(0,0.02*1800,1800),u_eta,t);
x_dot = A*x + B *u;% [u1; u(2)];

end