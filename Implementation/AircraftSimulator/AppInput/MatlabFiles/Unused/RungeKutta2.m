function RESULT = RungeKutta2(output_file, concrete_aircraft, should_be_rounded, precision)
% Function calculate symbolic solution of equation 
%       x_dot = Ax + Bu,
% where A is matrix 4x4, B is 4x2, u is 2x1 and 
% the solution x is 4x1.
% x = [U W Q theta]

%% Prepare A, B matrices
if(concrete_aircraft)
    [A, B, U_e, W_e, theta_e] = CreateAB2();
    Q_e = 0;
    if(should_be_rounded)
        A = vpa(A,precision);
        B = vpa(B,precision);
    end
else
    A_B_determinant;
    A = A2;
    B = B2;
end

%% Symbolic variables declaration
syms Ni Tau t U(t) W(t) Q(t) S(t) ;
X = [U; W; Q; S];
u = [Ni; Tau];

%% Prepare equations

U_sys =  A(1,:) * X + B(1,:) * u;
W_sys =  A(2,:) * X + B(2,:) * u;
Q_sys =  A(3,:) * X + B(3,:) * u;
S_sys =  A(4,:) * X + B(4,:) * u;

if(concrete_aircraft && should_be_rounded)
    U_sys = vpa(U_sys, precision);
    W_sys = vpa(W_sys, precision);
    Q_sys = vpa(Q_sys, precision);
    S_sys = vpa(S_sys, precision);
end

U_sys_char = char(U_sys);
W_sys_char = char(W_sys);
Q_sys_char = char(Q_sys);
S_sys_char = char(S_sys);

%% Solving

if(concrete_aircraft)
    RESULT = dsolve(strcat('DU = ', U_sys_char), strcat('DW = ', W_sys_char), strcat('DQ = ', Q_sys_char), strcat('DS = ', S_sys_char),...
        strcat('U(0) = ', num2str(U_e)), strcat('W(0) = ', num2str(W_e)), strcat('Q(0) = ', num2str(Q_e)), strcat('S(0) = ', num2str(theta_e))); 
else
    RESULT = dsolve(strcat('DU = ', U_sys_char), strcat('DW = ', W_sys_char), strcat('DQ = ', Q_sys_char), strcat('DS = ', S_sys_char),...
        'U(0) = U_e', 'W(0) = W_e', 'Q(0) = Q_e', 'S(0) = theta_e');
end

save(output_file);

end

