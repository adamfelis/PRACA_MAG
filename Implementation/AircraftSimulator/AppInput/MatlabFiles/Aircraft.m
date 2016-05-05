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

        previous_result
    end
    
    methods
        % Constructor
        function obj = Aircraft(new_A_longitudinal, new_B_longitudinal,...
                                        new_A_lateral, new_B_lateral,...
                                        simulation_step, simulation_time)
            AddStrategy(obj, new_A_longitudinal, new_B_longitudinal,...
                                        new_A_lateral, new_B_lateral);
            obj.simulation_step_from_fixed_update = simulation_step;
            obj.simulation_time_from_AircraftStrategy_cs = simulation_time;
            obj.previous_result = -1;
        end
        % Add extra strategy
        function AddStrategy(obj, new_A_longitudinal, new_B_longitudinal,...
                                        new_A_lateral, new_B_lateral)
           obj.Strategies = [obj.Strategies, AircraftStrategy(new_A_longitudinal, new_B_longitudinal,...
                                        new_A_lateral, new_B_lateral)];  
        end
        %
        function total_simulation_time = GetTotalSimulationTime(obj)
            total_simulation_time = obj.simulation_step_from_fixed_update * (obj.simulation_time_from_AircraftStrategy_cs - 1);
        end
        
        % Add missiles
        function AddMissile(obj)
            obj.Missiles = [obj.Missiles, Missile()];
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
                for i = 1 : 1 : length(obj.current_simulation_solutions)
                    newPosition = Position(0,0,0);
                    results.AddStrategyResult(...
                        obj.current_simulation_solutions(i).Y_longitudinal(simulation_index + 1,:),...
                        obj.current_simulation_solutions(i).Y_lateral(simulation_index + 1,:),...
                        newPosition.value)
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
                    newPosition = Position(0,0,0);

                    results.AddStrategyResult(Y_longitudinal(simulation_index + 1,:), Y_lateral(simulation_index + 1,:), newPosition.value)
                end
            end
            obj.previous_result = results;
            resultsArray = results.PresentFinalResults();
        end
        % Calculates the velocity value of time t along all directions for
        % strategy specified by the strategy_index
        function velocity = VelocityInTime(obj, t, strategy_index)
            strategy_index = max(strategy_index, length(obj.current_simulation_solutions));
            U = obj.current_simulation_solutions(strategy_index).Y_longitudinal(:,1);
            V = obj.current_simulation_solutions(strategy_index).Y_lateral(:,1);
            W = obj.current_simulation_solutions(strategy_index).Y_longitudinal(:,2);
            U = [U; U(length(U))];
            V = [V; V(length(V))];
            W = [W; W(length(W))];
            velocity = [U(round(t/obj.simulation_step_from_fixed_update) + 1),...
                V(round(t/obj.simulation_step_from_fixed_update) + 1),...
                W(round(t/obj.simulation_step_from_fixed_update) + 1)];
        end
    end
    
end

