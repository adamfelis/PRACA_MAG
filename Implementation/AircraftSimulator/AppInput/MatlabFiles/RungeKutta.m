% To run this file, the following variables are needed: 
% x0_longitudinal, x0_lateral, u_longitudinal, u_lateral, simulation_index
clear aircraft;
%% Workspace loader
load(strcat('../../AppOutput/', num2str(client_id)));
%% Calculations
result_from_matlab = aircraft.Simulate(x0_longitudinal, x0_lateral, u_longitudinal, u_lateral, simulation_index);
%% Workspace saver

if aircraft.DEBUG_MODE == true
    if(exist('global_result', 'var')==0)
        global_result = []; 
    end
    global_result = [global_result; result_from_matlab];
end
clearvars -except aircraft client_id result_from_matlab global_result;
save(strcat('../../AppOutput/', num2str(client_id)));