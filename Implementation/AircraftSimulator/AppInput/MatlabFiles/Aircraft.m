classdef Aircraft < handle & Shooter
    % Describes aircraft and its properties
    
    properties
        Missiles
        simulation_step_from_fixed_update
        simulation_time_from_AircraftStrategy_cs
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
        function resultsArray = Simulate(obj, x0_longitudinal, x0_lateral, u_longitudinal, u_lateral)
            results = Results();
            for i = 1 : 1 : length(obj.Strategies)
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
                results.AddStrategyResult(Y_longitudinal(simulation_index + 1,:), Y_lateral(simulation_index + 1,:))
            end
            resultsArray = results.PresentFinalResults();
        end
    end
    
end

