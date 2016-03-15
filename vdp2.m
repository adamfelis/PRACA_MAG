function dydt = vdp2(t,y)
kappa = 1;
tau = 0.2;
dydt = [kappa * y(4);
        kappa * y(5);
        kappa * y(6);
        -kappa * y(1) + tau * y(7);
        -kappa * y(2) + tau * y(8);
        -kappa * y(3) + tau * y(9);
        -tau * y(4);
        -tau * y(5);
        -tau * y(6)];    
end