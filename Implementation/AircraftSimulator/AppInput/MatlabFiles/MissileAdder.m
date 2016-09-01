% shooter_id
% target_id
% missile_id

shooter_aircraft = aircraft_collection.GetAircraft(shooter_id + 1);
target_aircraft = aircraft_collection.GetAircraft(target_id + 1);
shooter_aircraft.aircraftPosition = shooter_position;
target_aircraft.AddMissile( shooter_aircraft.aircraftPosition, shooter_id + 1, missile_id + 1, target_position, velocity);

clearvars -except aircraft_collection result_from_matlab global_result; 