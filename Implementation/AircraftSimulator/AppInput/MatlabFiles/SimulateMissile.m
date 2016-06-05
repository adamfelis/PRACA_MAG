% shooter_id
% target_id
% missile_id
target_aircraft = aircraft_collection.GetAircraft(target_id + 1);

result_from_matlab = target_aircraft.SimulateMissile(shooter_id + 1, missile_id + 1);

clearvars -except aircraft_collection result_from_matlab global_result; 