function [ t, y ] = ODE(tspan, y0)
% tspan = [0 0.001];
options = odeset('Refine', 1,'AbsTol',1e-3,'Stats','on');
[t,y] = ode45(@vdp2, tspan, y0', options);
end