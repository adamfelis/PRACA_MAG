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
        % stores previous result of computations of lateral and
        % longitudinal versions. Used when simulation is recalculated to
        % obtain x0_lateral and x0_longitudinal
        previous_result

        % 
        DEBUG_MODE = false;
        
        %Laplace transform elements
        delta_s
        N_u_eta
        N_w_eta
        N_q_eta
        N_theta_eta
        
        u_eta_s
        w_eta_s
        q_eta_s
        theta_eta_s
        
        u_t
        w_t
        q_t
        theta_t
        
        u_t_fun
        w_t_fun
        q_t_fun
        theta_t_fun
        
        licznik = 1;
        poprzedni
        old_u_longitudinal
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
            %obj.previous_result = -1;
            obj.PrepareTransferFunctions(new_A_longitudinal, new_B_longitudinal, new_A_lateral, new_B_lateral);
            obj.poprzedni = [178 0 0 9.4 * 2 * pi / 360];
            obj.old_u_longitudinal = [0 0];
        end
        
        function PrepareTransferFunctions(obj, new_A_longitudinal, new_B_longitudinal,...
                                        new_A_lateral, new_B_lateral)
           x_u = new_A_longitudinal(1,1);
           x_w = new_A_longitudinal(1,2);
           x_q = new_A_longitudinal(1,3);
           x_theta = new_A_longitudinal(1,4);
           
           z_u = new_A_longitudinal(2,1);
           z_w = new_A_longitudinal(2,2);
           z_q = new_A_longitudinal(2,3);
           z_theta = new_A_longitudinal(2,4);
           
           m_u = new_A_longitudinal(3,1);
           m_w = new_A_longitudinal(3,2);
           m_q = new_A_longitudinal(3,3);
           m_theta = new_A_longitudinal(3,4);
           
           x_eta = new_B_longitudinal(1,1);
           %x_tau = new_B_longitudinal(1,2);
           
           z_eta = new_B_longitudinal(2,1);
           %z_tau = new_B_longitudinal(2,2);
           
           m_eta = new_B_longitudinal(3,1);
           %m_tau = new_B_longitudinal(3,2);
           
           syms s;
           a = 1;
           b = -(m_q + x_u + z_w);
           c = (m_q * z_w - m_w * z_q) +...
               (m_q * x_u - m_u * x_q) +...
               (x_u * z_w - x_w * z_u) -...
               new_A_longitudinal(3,4);
           d = (m_theta * x_u - m_u * x_theta) +...
               (m_theta * z_w - m_w * z_theta) +...
               m_q * (x_w * z_u - x_u * z_w) +...
               x_q * (m_u * z_w - m_w * z_u) + ...
               z_q * (m_w * x_u - m_u * x_w);
           e = m_theta * (x_w * z_u - x_u * z_w) +...
               x_theta * (m_u * z_w - m_w * z_u) +...
               z_theta * (m_w * x_u - m_u * x_w);
           obj.delta_s = a * s^4 + b * s^3 + c * s^2 + d * s + e;
           
           a = x_eta;
           b = m_eta * x_q - x_eta * (m_q + z_w) + z_eta * x_w;
           c = m_eta * (x_w * z_q - x_q * z_w + x_theta) + x_eta * (m_q * z_w - m_w * z_q - m_theta) + z_eta * (m_w * x_q - m_q * x_w);
           d = m_eta * (x_w * z_theta - x_theta * z_w) + x_eta * (m_theta * z_w - m_w * z_theta) + z_eta * (m_w * x_theta - m_theta * x_w);
           %d = 1000;
           obj.N_u_eta = a * s^3 + b * s^2 + c * s + d;
            
           a = z_eta;
           b = m_eta * z_q - z_eta * (m_q + x_u) + x_eta * z_u;
           c = m_eta * (x_q * z_u - x_u * z_q + z_theta) + x_eta * (m_u * z_q - m_q * z_u) + z_eta * (m_q * x_u - m_u * x_q - m_theta);
           d = m_eta * (x_theta * z_u - x_u * z_theta) + x_eta * (m_u * z_theta - m_theta * z_u) + z_eta * (m_theta * x_u - m_u * x_theta);
           obj.N_w_eta = a * s^3 + b * s^2 + c * s + d;
            
           a = m_eta;
           b = -m_eta * (x_u + z_w) + x_eta * m_u + z_eta * m_w;
           c = m_eta * (x_u * z_w - x_w * z_u) + x_eta * (m_w * z_u - m_u * z_w) + z_eta * (m_u * x_w - m_w * x_u);
           %c =30;
           obj.N_q_eta = s * (a * s^2 + b * s + c);
           obj.N_theta_eta = a * s^2 + b * s + c;
           
           syms ni;
           precision = 4;
           obj.u_eta_s = vpa(ni/s * obj.N_u_eta / obj.delta_s, precision);
           obj.w_eta_s = vpa(ni/s * obj.N_w_eta / obj.delta_s, precision);
           obj.q_eta_s = vpa(ni/s * obj.N_q_eta / obj.delta_s, precision);
           obj.theta_eta_s = vpa(ni/s * obj.N_theta_eta / obj.delta_s, precision);
           
%            K_p = 0.00693;
%            K_i = 1.21e-05;
%            K_d = 0.989;
%            
%            pid_s = K_p + K_i / s + K_d * s;
           
           obj.u_t = vpa(simplify(ilaplace(obj.u_eta_s)), precision);
           obj.w_t = vpa(simplify(ilaplace(obj.w_eta_s)), precision);
           obj.q_t = vpa(simplify(ilaplace(obj.q_eta_s)), precision);
           obj.theta_t = vpa(simplify(ilaplace(obj.theta_eta_s)), precision);
            
           obj.u_t_fun = matlabFunction(obj.u_t);
           obj.w_t_fun = matlabFunction(obj.w_t);
           obj.q_t_fun = matlabFunction(obj.q_t);
           obj.theta_t_fun = matlabFunction(obj.theta_t);
           
           
        end
        % Add extra strategy
        function AddStrategy(obj, new_A_longitudinal, new_B_longitudinal,...
                                        new_A_lateral, new_B_lateral)
           obj.Strategies = [obj.Strategies, AircraftStrategy(new_A_longitudinal, new_B_longitudinal,...
                                        new_A_lateral, new_B_lateral)];  
        end
        %
        function ModifyStrategy(obj, strategy_id, new_A_longitudinal, new_B_longitudinal,...
                                        new_A_lateral, new_B_lateral)
           obj.Strategies(strategy_id + 1) = AircraftStrategy(new_A_longitudinal, new_B_longitudinal,...
                                        new_A_lateral, new_B_lateral);                    
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
                interval = [(simulation_index - 1) * obj.simulation_step_from_fixed_update, simulation_index * obj.simulation_step_from_fixed_update];
                for i = 1 : 1 : length(obj.current_simulation_solutions)
                    newPosition = Position(0,0,0);
                    if(obj.DEBUG_MODE)
                        newPosition = obj.UpdatePosition(interval, i);                    
                    end
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
                    if(obj.DEBUG_MODE)
                        newPosition = obj.UpdatePosition([0, obj.simulation_step_from_fixed_update], i); 
                    end
                    results.AddStrategyResult(Y_longitudinal(simulation_index + 1,:), Y_lateral(simulation_index + 1,:), newPosition.value)
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
        
        function resultsArray = SimulateLaplace(obj, x0_longitudinal, x0_lateral, u_longitudinal, u_lateral, simulation_index)
            results = Results();
%             if simulation_index > 1
%                 interval = [(simulation_index - 1) * obj.simulation_step_from_fixed_update, simulation_index * obj.simulation_step_from_fixed_update];
%                 for i = 1 : 1 : length(obj.current_simulation_solutions)
%                     results.AddStrategyResult(...
%                         obj.current_simulation_solutions(i).Y_longitudinal(simulation_index + 1,:),...
%                         obj.current_simulation_solutions(i).Y_lateral(simulation_index + 1,:),...
%                         newPosition.value)
%                 end
%             
%             else
                obj.current_simulation_solutions = [];
                %difference = u_longitudinal - obj.old_u_longitudinal;
                %obj.old_u_longitudinal = u_longitudinal;
                %u_longitudinal = difference;
                for i = 1 : 1 : length(obj.Strategies)
                    Y_longitudinal = obj.poprzedni + ...
                        [obj.u_t_fun(u_longitudinal(1),obj.simulation_step_from_fixed_update * obj.licznik),...
                        obj.w_t_fun(u_longitudinal(1),obj.simulation_step_from_fixed_update * obj.licznik),...
                        obj.q_t_fun(u_longitudinal(1),obj.simulation_step_from_fixed_update * obj.licznik),...
                        obj.theta_t_fun(u_longitudinal(1),obj.simulation_step_from_fixed_update * obj.licznik)];
                    obj.poprzedni = Y_longitudinal;
                    obj.licznik = obj.licznik + 1;
                    Y_longitudinal = real(Y_longitudinal);
                    Y_lateral = [0 0 0 0 0];
                    obj.current_simulation_solutions = [obj.current_simulation_solutions, SimulationSolution(Y_longitudinal, Y_lateral)];
                end
%             end

            results.AddStrategyResult(Y_longitudinal, Y_lateral, [0 0 0]);
            obj.previous_result = results;
            resultsArray = results.PresentFinalResults();
        end
    end
    
end

