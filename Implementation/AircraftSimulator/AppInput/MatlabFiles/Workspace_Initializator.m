% Could be useful for the backup tool
%if(~(exist(strcat('../../AppOutput/', num2str(client_id), '.mat')) == 2))

%% Variable initialization
simulation_time_from_AircraftStrategy_cs = 101;

simulation_step_from_fixed_update = 0.02;
total_simulation_time = simulation_step_from_fixed_update * (simulation_time_from_AircraftStrategy_cs - 1);

%% Data Interpreter

A_longitudinal = [a_longitudinal(1,1:4),;...
                    a_longitudinal(1,5:8);...
                    a_longitudinal(1,9:12);...
                    a_longitudinal(1,13:16)];
B_longitudinal = [b_longitudinal(1,1:2);...
                    b_longitudinal(1,3:4);...
                    b_longitudinal(1,5:6);...
                    b_longitudinal(1,7:8)];


A_lateral = [a_lateral(1,1:5);...
        a_lateral(1,6:10);...
        a_lateral(1,11:15);...
        a_lateral(1,16:20);...
        a_lateral(1,21:25)];
B_lateral = [b_lateral(1,1:2); b_lateral(1,3:4); b_lateral(1,5:6); b_lateral(1,7:8); b_lateral(1,9:10)];

initialized_correctly = true;

% Could be useful for the backup tool
% else
%     load(strcat('../../AppOutput/', num2str(client_id)));
% end

%% Workspace saver

save(strcat('../../AppOutput/', num2str(client_id)));
clear;