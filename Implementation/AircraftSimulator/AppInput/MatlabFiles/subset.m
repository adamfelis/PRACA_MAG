function [result] = subset(p, index, result)
    if index > 0
        p(index) = 1;
        [res1] = subset(p, index - 1, result);
        p(index) = 0;
        [res2] = subset(p, index - 1, result);
        result = [res1; res2];
    else
        result = [result; p];
    end
end