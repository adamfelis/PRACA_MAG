function [x_dot] = StateSpace (t,x, A, B, u)

x_dot = A*x + B * u;

end