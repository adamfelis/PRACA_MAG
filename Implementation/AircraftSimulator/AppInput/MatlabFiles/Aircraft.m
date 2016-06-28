classdef Aircraft < handle & Shooter
    % Describes aircraft and its properties
    
    properties
        % tbale of all available types of missiles
        Missiles
        simulation_step_from_fixed_update
        simulation_time_from_AircraftStrategy_cs
        % table of the complete solutions (for the specified
        % total_simulation_time in GetTotalSimulationTime()) - one for each
        % strategy
        current_simulation_solutions
        current_simulation_solutions_laplace = []
        % stores previous result of comput ations of lateral and
        % longitudinal versions. Used when simulation is recalculated to
        % obtain x0_lateral and x0_longitudinal
        previous_result

        % 
        DEBUG_MODE = false;
        ERROR_STATE = false;
        aircraftPosition = [0 0 0];
    end
    
    methods
        % Constructor
        function obj = Aircraft(new_A_longitudinal, new_B_longitudinal,...
                                        new_A_lateral, new_B_lateral,...
                                        simulation_step, simulation_time)
            AddStrategy(obj, new_A_longitudinal, new_B_longitudinal,...
                                        new_A_lateral, new_B_lateral, simulation_step);
            obj.simulation_step_from_fixed_update = simulation_step;
            obj.simulation_time_from_AircraftStrategy_cs = simulation_time;
            %obj.previous_result = -1;
        end
        
        
        % Add extra strategy
        function AddStrategy(obj, new_A_longitudinal, new_B_longitudinal,...
                                        new_A_lateral, new_B_lateral, simulation_step)
           obj.Strategies = [obj.Strategies, AircraftStrategy(new_A_longitudinal, new_B_longitudinal,...
                                        new_A_lateral, new_B_lateral, simulation_step)];  
        end
        %
        function ModifyStrategy(obj, strategy_id, new_A_longitudinal, new_B_longitudinal,...
                                        new_A_lateral, new_B_lateral)
                                    
           current_aircraft_velocity = obj.Strategies(strategy_id + 1).current_aircraft_velocity;
           obj.Strategies(strategy_id + 1) = AircraftStrategy(new_A_longitudinal, new_B_longitudinal,...
                                        new_A_lateral, new_B_lateral, obj.simulation_step_from_fixed_update); 
           obj.Strategies(strategy_id + 1).current_aircraft_velocity = current_aircraft_velocity;
        end
        %
        function total_simulation_time = GetTotalSimulationTime(obj)
            total_simulation_time = obj.simulation_step_from_fixed_update * (obj.simulation_time_from_AircraftStrategy_cs - 1);
        end
        
        % Add missiles
        function AddMissile(obj, missilePos, shooter_id, missile_id)
            obj.Missiles = [obj.Missiles, Missile(shooter_id, missile_id, missilePos, obj.simulation_step_from_fixed_update)];
        end
        % Returns concrete missile strategy
        function MissileStrategy = GetMissileStrategy(obj, missile_index, strategy_index)
            missile_index = max(missile_index, length(obj.Missiles));
            strategy_index = max(strategy_index, length(obj.Missiles(missile_index).Strategies));
            MissileStrategy = obj.Missiles(missile_index).Strategies(strategy_index);
        end
        % Add missile strategy
        function AddMissileStrategy(obj, missile_index)
            missile_index = max(missile_index, length(obj.Missiles));
            obj.Missiles(missile_index).AddStrategy();
        end
        % Simulates flight
        function resultsArray = Simulate(obj, x0_longitudinal, x0_lateral, u_longitudinal, u_lateral, simulation_index)
            
            results = Results();
            if simulation_index > 1
%                 interval = [(simulation_index - 1) * obj.simulation_step_from_fixed_update, simulation_index * obj.simulation_step_from_fixed_update];
                for i = 1 : 1 : length(obj.current_simulation_solutions)
%                     newPosition = Position(0,0,0);
%                     if(obj.DEBUG_MODE)
%                         newPosition = obj.UpdatePosition(interval, i);                    
%                     end
                    Y_longitudinal = obj.current_simulation_solutions(i).Y_longitudinal(simulation_index + 1,:);
                    Y_lateral = obj.current_simulation_solutions(i).Y_lateral(simulation_index + 1,:);
                    movement = obj.Strategies(i).MoveAircraft(Y_longitudinal', Y_lateral', u_longitudinal', u_lateral');
                    if i == 1
                        obj.aircraftPosition = obj.aircraftPosition + movement;
                    end
                    results.AddStrategyResult(...
                        Y_longitudinal,...
                        Y_lateral,...
                        movement);
                        %newPosition.value)
                end
            else
                obj.current_simulation_solutions = [];
                for i = 1 : 1 : length(obj.Strategies)

                    if obj.previous_result ~= -1
                         x0_longitudinal = obj.previous_result.StrategiesResults(i).longitudinal_result;
                         x0_lateral = obj.previous_result.StrategiesResults(i).lateral_result;
                    end
                    [~,Y_longitudinal] = ode45(...
                            @(t,x)StateSpace(t, x, obj.Strategies(i).A_longitudinal, obj.Strategies(i).B_longitudinal, u_longitudinal'),...
                            0 : obj.simulation_step_from_fixed_update : obj.GetTotalSimulationTime(),...
                            x0_longitudinal'...
                            );
                        
                    [~,Y_lateral] = ode45(...
                            @(t,x)StateSpace(t, x, obj.Strategies(i).A_lateral, obj.Strategies(i).B_lateral, u_lateral'),...
                            0 : obj.simulation_step_from_fixed_update : obj.GetTotalSimulationTime(),...
                            x0_lateral'...
                            );
                    obj.current_simulation_solutions = [obj.current_simulation_solutions, SimulationSolution(Y_longitudinal, Y_lateral)];
                   
                    movement = obj.Strategies(i).MoveAircraft(Y_longitudinal(simulation_index + 1,:)', Y_lateral(simulation_index + 1,:)', u_longitudinal', u_lateral');
                    if i == 1
                        obj.aircraftPosition = obj.aircraftPosition + movement;
                    end
                
%                     newPosition = Position(0,0,0);
%                     if(obj.DEBUG_MODE)
%                         newPosition = obj.UpdatePosition([0, obj.simulation_step_from_fixed_update], i); 
%                     end
                    results.AddStrategyResult(Y_longitudinal(simulation_index + 1,:), Y_lateral(simulation_index + 1,:), movement);%newPosition.value)
                end
            end
            obj.previous_result = results;
            resultsArray = results.PresentFinalResults();
        end
        % Calculates the velocity value of time t along direction U for
        % strategy specified by the strategy_index
        function velocityU = VelocityUInTime(obj, t, strategy_index)
            strategy_index = max(strategy_index, length(obj.current_simulation_solutions));
            U = obj.current_simulation_solutions(strategy_index).Y_longitudinal(:,1);
            U = [U; U(length(U))];
            velocityU = U(round(t/obj.simulation_step_from_fixed_update) + 1);
        end
        % Calculates the velocity value of time t along direction V for
        % strategy specified by the strategy_index
        function velocityV = VelocityVInTime(obj, t, strategy_index)
            strategy_index = max(strategy_index, length(obj.current_simulation_solutions));
            V = obj.current_simulation_solutions(strategy_index).Y_lateral(:,1);
            V = [V; V(length(V))];
            velocityV = V(round(t/obj.simulation_step_from_fixed_update) + 1);
        end
        % Calculates the velocity value of time t along direction W for
        % strategy specified by the strategy_index
        function velocityW = VelocityWInTime(obj, t, strategy_index)
            strategy_index = max(strategy_index, length(obj.current_simulation_solutions));
            W = obj.current_simulation_solutions(strategy_index).Y_longitudinal(:,2);
            W = [W; W(length(W))];
            velocityW = W(round(t/obj.simulation_step_from_fixed_update) + 1);
        end

        function newPosition = UpdatePosition(obj, interval, strategy_index)
            newPosition = Position(0,0,0);
            newPosition.value(1) = newPosition.value(1) + ...
                    integral(@(t) obj.VelocityUInTime(t,strategy_index), interval(1), interval(2), 'ArrayValued', true);

            newPosition.value(2) = newPosition.value(2) + ...
                    integral(@(t) obj.VelocityVInTime(t,strategy_index), interval(1), interval(2), 'ArrayValued', true);

            newPosition.value(3) = newPosition.value(3) + ...
                    integral(@(t) obj.VelocityWInTime(t,strategy_index), interval(1), interval(2), 'ArrayValued', true);
        end
        
        function resultsArray = SimulateLaplace(obj, u_longitudinal, u_lateral)
            results = Results();
            %obj.current_simulation_solutions = [];
            for i = 1 : 1 : length(obj.Strategies)
                
                Y_longitudinal = obj.Strategies(i).SimulateLaplaceLongitudinal(u_longitudinal);
                Y_longitudinal = real(Y_longitudinal);
                
                Y_lateral = obj.Strategies(i).SimulateLaplaceLateral(u_lateral);
                Y_lateral = real(Y_lateral);
                
                if sum(isnan(Y_lateral))
                    obj.ERROR_STATE = true;
                end
                
                movement = obj.Strategies(i).MoveAircraft(Y_longitudinal', Y_lateral', u_longitudinal, u_lateral);
                if i == 1
                    obj.aircraftPosition = obj.aircraftPosition + movement;
                end
%               obj.current_simulation_solutions = [obj.current_simulation_solutions, SimulationSolution(Y_longitudinal, Y_lateral)];
                obj.current_simulation_solutions_laplace = [obj.current_simulation_solutions_laplace; obj.aircraftPosition];
                    
                results.AddStrategyResult(Y_longitudinal, Y_lateral, movement);
            end                
            
            obj.previous_result = results;
            resultsArray = results.PresentFinalResults();
        end
        
        function missileDeltaPositions = SimulateMissile(obj, shooter_id, missile_id)            
            
            missileDeltaPositions = [];
            for i = 1:1:length(obj.Missiles)
                if ~(obj.Missiles(i).shooter_id == shooter_id && obj.Missiles(i).missile_id == missile_id)
                    continue;
                end
               for j = 1:1:length(obj.Missiles(i).Strategies)
                   deltaPos = obj.Missiles(i).Strategies(j).SimulateMissileFlight(obj.aircraftPosition, obj.Strategies(1).current_aircraft_velocity);
                   missileDeltaPositions = [missileDeltaPositions; deltaPos];
               end
            end
            
        end
    end
    
end

