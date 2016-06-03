% To run this file, the following variables are needed: 
% x0_longitudinal, x0_lateral, u_longitudinal, u_lateral, simulation_index
%clear aircraft;
%% Workspace loader
%load(strcat('../../AppOutput/', num2str(client_id)));
aircraft = aircraft_collection.GetAircraft(client_id + 1); %matlab table counter starts with 1, not 0
%% Calculations
result_from_matlab = aircraft.SimulateLaplace( u_longitudinal', u_lateral');
for i = 1 : 1: length(result_from_matlab)
    result_from_matlab(i) = double(round(result_from_matlab(i), 4));
end
result_from_matlab = double(result_from_matlab);
%% Workspace saver

if aircraft.DEBUG_MODE == true
    if(exist('global_result', 'var')==0)
        global_result = []; 
    end
    global_result = [global_result; result_from_matlab];
end
%clearvars -except aircraft client_id result_from_matlab global_result;
%save(strcat('../../AppOutput/', num2str(client_id)));
clearvars -except aircraft_collection result_from_matlab global_result; 