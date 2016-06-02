function X = FindX(A, B, x0, U)

epsilon = 10e-1;
x_prev = x0;
x_new = x0;
while true
    x_new = A * x_prev + B*U;
    if sum(abs(x_new - x_prev)) < epsilon
        break;
    end
    x_prev = x_new;
end
    
X = x_new;
end