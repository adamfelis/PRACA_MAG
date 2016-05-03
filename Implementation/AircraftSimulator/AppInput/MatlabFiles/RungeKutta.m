%% Workspace loader
load(strcat('../../AppOutput/', num2str(client_id)));

%% Calculations

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

%% Workspace saver

save(strcat('../../AppOutput/', num2str(client_id)));
clear;