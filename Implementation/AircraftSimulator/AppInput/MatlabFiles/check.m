function [] = check(A)
B = zeros(6);
B(1, :) = A(1 : 6);
B(2, :) = A(7 : 12);
B(3, :) = A(13 : 18);
B(4, :) = A(19 : 24);
B(5, :) = A(25 : 30);
B(6, :) = A(31 : 36);

A = B;
%     A = [0 15 21 12 16 24;...
%          25 8 7 4 8 2;...
%          18 6 5 3 6 4;...
%          17 2 1 8 5 9;...
%          12 1 2 1 8 3;...
%          16 5 7 9 7 9]

     result = subset(zeros(1, 5), 5, []);

     M = {};
    for i = 2 : 6
         N = {};
         for j = 1 : 32
             kolumna = A(2:6, i);
             wynik = sum(kolumna .* result(j, :)');
             if wynik == A(1, i)
                 if i == 2
                     N = [N result(j, :)'];
                 else
                     for k = 1 : length(M)
                         m = [M{k} result(j, :)'];
                         N = [N m];
                     end
                 end
             end
         end
         M = N;
    end

    for i = 1 : length(M)
        sentinel = true;
        for j = 2 : 6
            wiersz = A(j, 2:6);
            wiersz = sum(wiersz .* M{i}(j-1, :));
            if wiersz ~= A(j, 1)
                sentinel = false;
                break;
            end
        end
        if sentinel
            M{i}
        end
    end
end