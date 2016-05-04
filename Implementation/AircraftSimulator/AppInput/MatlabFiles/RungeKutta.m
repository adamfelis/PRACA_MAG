%% Workspace loader
load(strcat('../../AppOutput/', num2str(client_id)));
%% Calculations
result_from_matlab = aircraft.Simulate(x0_longitudinal, x0_lateral, u_longitudinal, u_lateral);
%% Workspace saver
clearvars -except aircraft client_id;
save(strcat('../../AppOutput/', num2str(client_id)));
clear;