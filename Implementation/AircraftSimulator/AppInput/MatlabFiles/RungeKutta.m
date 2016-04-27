%% Variable initialization
simulation_time_from_AircraftStrategy_cs = 101;

simulation_step_from_fixed_update = 0.02;
total_simulation_time = simulation_step_from_fixed_update * (simulation_time_from_AircraftStrategy_cs - 1);

%% Data Interpreter
A_longitudinal = [a_longitudinal(1,1),a_longitudinal(1,2),a_longitudinal(1,3),a_longitudinal(1,4);...
        a_longitudinal(1,5),a_longitudinal(1,6),a_longitudinal(1,7),a_longitudinal(1,8);...
        a_longitudinal(1,9),a_longitudinal(1,10),a_longitudinal(1,11),a_longitudinal(1,12);...
        a_longitudinal(1,13),a_longitudinal(1,14),a_longitudinal(1,15),a_longitudinal(1,16)];
B_longitudinal = [b_longitudinal(1,1:2); b_longitudinal(1,3:4); b_longitudinal(1,5:6); b_longitudinal(1,7:8)];


%% Calculations
% [A_longitudinal, B_longitudinal] = CreateAB2(); 
% u_longitudinal = [0;1];
% x0_longitudinal = [178;0;0;0];


if(simulation_index > 1)
    result = Y_longitudinal(simulation_index + 1,:);
    return;
end
[T_longitudinal,Y_longitudinal] = ode45(...
    @(t,x)StateSpace(t, x, A_longitudinal, B_longitudinal, u_longitudinal'),...%function represens ode equation
    0 : simulation_step_from_fixed_update : total_simulation_time,...%time for simulation
    x0_longitudinal'...% x(t_0) = x_0
    );
result = Y_longitudinal(simulation_index + 1,:);

%% Debug Plots
%result = Y(2,:);
% figure(1);
% hold on;
% plot(T, Y(:,1));