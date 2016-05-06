result_W = zeros(length(W),1);
result_W(1,1) = W(1);

for i = 2 : 1 : length(W)
    result_W(i,1) = result_W(i-1,1) + W(i);     
end