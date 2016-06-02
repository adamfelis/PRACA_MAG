%%
a = Aircraft(aircraft.Strategies.A_longitudinal, aircraft.Strategies.B_longitudinal, aircraft.Strategies.A_lateral, aircraft.Strategies.B_lateral, 0.2,10);
time = 0.2:0.2:75;
u_longitudinal = [0.2;0];
u_lateral = [0;0];
values = zeros(1, length(time));
index = -1;
max_elevator = 30 * 2 * pi / 360;
for i = 1:1:length(time)
   result = a.SimulateLaplace(u_longitudinal, u_lateral);
   values(i) = result(9);
   if(u_longitudinal(1) == -max_elevator && index == -1)
       index = i;
   end
   if( i > 150 && i < 350 && mod(i,5) == 0)
       u_longitudinal = u_longitudinal + [2 * 2 * pi /360;0]; 
   end
%    if( i > 350 && i < 500 && mod(i,5) == 0)
%        u_longitudinal = u_longitudinal - [2 * 2 * pi /360;0]; 
%    end
%    if( i > 500 && mod(i,5) == 0)
%        u_longitudinal = u_longitudinal + [2 * 2 * pi /360;0]; 
%    end
%    if(i > 500 && i < 600 && mod(i,5) == 0)
%        u_longitudinal = u_longitudinal - [2 * 2 * pi /360;0]; 
%    end
%    if(i > 600 && i < 670 && mod(i,5) == 0)
%        u_longitudinal = u_longitudinal + [2 * 2 * pi /360;0]; 
%    end
%    if(i > 670 && mod(i,5) == 0)
%        u_longitudinal = u_longitudinal - [2 * 2 * pi /360;0]; 
%    end
   
   u_longitudinal(1) = min(u_longitudinal(1), max_elevator);
   u_longitudinal(1) = max(u_longitudinal(1), -max_elevator);
end
figure(1);
plot(time, values);
hold on;
if(index ~= -1)
    plot(time(index), values(index), 'go');
end
grid on;