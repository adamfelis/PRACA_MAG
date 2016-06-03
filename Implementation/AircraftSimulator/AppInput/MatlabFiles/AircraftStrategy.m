classdef AircraftStrategy < handle & Strategy
    %Describes one strategy of the aircraft
    
    properties
        A_longitudinal;
        B_longitudinal;
        A_lateral;
        B_lateral
        
        simulation_step_from_fixed_update
        simulation_counter = 1;
        
        stable_conditions = false;
        longitudinal_stability_condition = 0.05;
        previous_longitudinal_result = false;
        previous_u_longitudinal = false;
        previous_lateral_result = false;
        previous_u_lateral = false;
        difference_longitudinal = false;
        last_stable_longitudinal_value = false;
        longitudinal_result_monotonicity = -1; % true means that function grows
        
        delta_s
        N_u_eta
        N_w_eta
        N_q_eta
        N_theta_eta
        N_a_z_eta
        
        N_u_tau
        N_w_tau
        N_q_tau
        N_theta_tau
        N_a_z_tau
        
        u_eta_s
        w_eta_s
        q_eta_s
        theta_eta_s
        a_z_eta_s
        
        u_tau_s
        w_tau_s
        q_tau_s
        theta_tau_s
        a_z_tau_s
        
        u_t
        w_t
        q_t
        theta_t
        a_z_t
        
        u_t_fun
        w_t_fun
        q_t_fun
        theta_t_fun
        a_z_t_fun
        
        
        u_fun
        w_fun
        q_fun
        theta_fun
        a_z_fun
    end
    
    methods
        % Constructor
        function obj = AircraftStrategy(new_A_longitudinal, new_B_longitudinal,...
                                        new_A_lateral, new_B_lateral, simulation_step)
           obj.A_longitudinal = new_A_longitudinal;
           obj.B_longitudinal = new_B_longitudinal;
           obj.A_lateral = new_A_lateral;
           obj.B_lateral = new_B_lateral;
           
           obj.simulation_step_from_fixed_update = simulation_step;
           
           C = eye(4);
           R = eye(2);
           p = 10;
           Q = p * (C' * C);
           K = lqr(new_A_longitudinal,new_B_longitudinal,Q,R);
           Nbar = 1000;
           
           obj.A_longitudinal = obj.A_longitudinal - obj.B_longitudinal * K;
           obj.B_longitudinal = Nbar * obj.B_longitudinal;
           
           obj.PrepareTransferFunctions();
        end
        %
        function PrepareTransferFunctions(obj)
           x_u = obj.A_longitudinal(1,1);
           x_w = obj.A_longitudinal(1,2);
           x_q = obj.A_longitudinal(1,3);
           x_theta = obj.A_longitudinal(1,4);
           
           z_u = obj.A_longitudinal(2,1);
           z_w = obj.A_longitudinal(2,2);
           z_q = obj.A_longitudinal(2,3);
           z_theta = obj.A_longitudinal(2,4);
           
           m_u = obj.A_longitudinal(3,1);
           m_w = obj.A_longitudinal(3,2);
           m_q = obj.A_longitudinal(3,3);
           m_theta = obj.A_longitudinal(3,4);
           
           x_eta = obj.B_longitudinal(1,1);
           x_tau = obj.B_longitudinal(1,2);
           
           z_eta = obj.B_longitudinal(2,1);
           z_tau = obj.B_longitudinal(2,2);
           
           m_eta = obj.B_longitudinal(3,1);
           m_tau = obj.B_longitudinal(3,2);
           
           syms s;
           a = 1;
           b = -(m_q + x_u + z_w);
           c = (m_q * z_w - m_w * z_q) +...
               (m_q * x_u - m_u * x_q) +...
               (x_u * z_w - x_w * z_u) -...
               m_theta;
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
           
           U_e = 178;
           obj.N_a_z_eta = -s * (obj.N_w_eta - U_e * obj.N_theta_eta);
           
           a = x_tau;
           b = m_tau * x_q - x_tau * (m_q + z_w) + z_tau * x_w;
           c = m_tau * (x_w * z_q - x_q * z_w + x_theta) + x_tau * (m_q * z_w - m_w * z_q - m_theta) + z_tau * (m_w * x_q - m_q * x_w);
           d = m_tau * (x_w * z_theta - x_theta * z_w) + x_tau * (m_theta * z_w - m_w * z_theta) + z_tau * (m_w * x_theta - m_theta * x_w);
           %d = 1000;
           obj.N_u_tau = a * s^3 + b * s^2 + c * s + d;
            
           a = z_tau;
           b = m_tau * z_q - z_tau * (m_q + x_u) + x_tau * z_u;
           c = m_tau * (x_q * z_u - x_u * z_q + z_theta) + x_tau * (m_u * z_q - m_q * z_u) + z_tau * (m_q * x_u - m_u * x_q - m_theta);
           d = m_tau * (x_theta * z_u - x_u * z_theta) + x_tau * (m_u * z_theta - m_theta * z_u) + z_tau * (m_theta * x_u - m_u * x_theta);
           obj.N_w_tau = a * s^3 + b * s^2 + c * s + d;
            
           a = m_tau;
           b = -m_tau * (x_u + z_w) + x_tau * m_u + z_tau * m_w;
           c = m_tau * (x_u * z_w - x_w * z_u) + x_tau * (m_w * z_u - m_u * z_w) + z_tau * (m_u * x_w - m_w * x_u);
           %c =30;
           obj.N_q_tau = s * (a * s^2 + b * s + c);
           obj.N_theta_tau = a * s^2 + b * s + c;
           
           obj.N_a_z_tau = -s * (obj.N_w_tau - U_e * obj.N_theta_tau);
           
           
           syms ni tau;
           precision = 4;
           obj.u_eta_s = vpa(ni/s * obj.N_u_eta / obj.delta_s, precision);
           obj.w_eta_s = vpa(ni/s * obj.N_w_eta / obj.delta_s, precision);
           obj.q_eta_s = vpa(ni/s * obj.N_q_eta / obj.delta_s, precision);
           obj.theta_eta_s = vpa(ni/s * obj.N_theta_eta / obj.delta_s, precision);
           obj.a_z_eta_s = vpa(ni/s * obj.N_a_z_eta / obj.delta_s, precision);
           
           obj.u_tau_s = vpa(tau/s * obj.N_u_tau / obj.delta_s, precision);
           obj.w_tau_s = vpa(tau/s * obj.N_w_tau / obj.delta_s, precision);
           obj.q_tau_s = vpa(tau/s * obj.N_q_tau / obj.delta_s, precision);
           obj.theta_tau_s = vpa(tau/s * obj.N_theta_tau / obj.delta_s, precision);
           obj.a_z_tau_s = vpa(tau/s * obj.N_a_z_tau / obj.delta_s, precision);
           
%            K_p = 0.00693;
%            K_i = 1.21e-05;
%            K_d = 0.989;
%            
%            pid_s = K_p + K_i / s + K_d * s;
           
           obj.u_t = vpa(simplify(ilaplace(obj.u_eta_s + obj.u_tau_s)), precision);
           obj.w_t = vpa(simplify(ilaplace(obj.w_eta_s + obj.w_tau_s)), precision);
           obj.q_t = vpa(simplify(ilaplace(obj.q_eta_s + obj.q_tau_s)), precision);
           obj.theta_t = vpa(simplify(ilaplace(obj.theta_eta_s) + ilaplace(obj.theta_tau_s)), precision);
           obj.a_z_t = vpa(simplify(ilaplace(obj.a_z_eta_s + obj.a_z_tau_s)), precision);
            
           obj.u_t_fun = matlabFunction(obj.u_t);
           obj.w_t_fun = matlabFunction(obj.w_t);
           obj.q_t_fun = matlabFunction(obj.q_t);
           obj.theta_t_fun = matlabFunction(obj.theta_t);
           obj.a_z_t_fun = matlabFunction(obj.a_z_t);
           
           obj.u_fun = @(ni,t, tau) (obj.u_t_fun(ni, t) - obj.u_t_fun(ni, obj.simulation_step_from_fixed_update));
           obj.w_fun = @(ni,t, tau) (obj.w_t_fun(ni, t) - obj.w_t_fun(ni, obj.simulation_step_from_fixed_update));
           obj.q_fun = @(ni,t, tau) (obj.q_t_fun(ni, t) - obj.q_t_fun(ni, obj.simulation_step_from_fixed_update));
           obj.theta_fun = @(ni,t, tau) (obj.theta_t_fun(ni, t) - obj.theta_t_fun(ni, obj.simulation_step_from_fixed_update));
           
        end
        %
        function longitudinal_result = SimulateLaplaceLongitudinal(obj, u_longitudinal)
            
            if length(obj.difference_longitudinal) == 1
                obj.difference_longitudinal = [0;0];
            end
            if length(obj.last_stable_longitudinal_value) == 1
                obj.last_stable_longitudinal_value = [0 0 0 0];
            end
%             if length(obj.previous_u_longitudinal) == 1
%                obj.previous_u_longitudinal = u_longitudinal; 
%             end
            u_changed = false;
            u_longitudinal_rised = true; % new u_longitudnal is greater or equals than previous one
            if length(obj.previous_u_longitudinal) > 1 &&  norm(obj.previous_u_longitudinal - u_longitudinal) > 0
                u_changed = true;
                if length(obj.previous_u_longitudinal) > 1 &&  u_longitudinal(1) - obj.previous_u_longitudinal(1) < 0
                    u_longitudinal_rised = false;
                end
            end
            
                
            if length(obj.previous_u_longitudinal) > 1 &&  u_longitudinal(1) - obj.previous_u_longitudinal(1) == 0
                u_longitudinal_rised = -1;
            end
            
            monotonicity_changed = false;
            if obj.longitudinal_result_monotonicity ~= -1
                if (obj.longitudinal_result_monotonicity == true && u_longitudinal_rised == false) ||...
                        (obj.longitudinal_result_monotonicity == false && u_longitudinal_rised == true)
                    monotonicity_changed = true;
                end
            end
            if u_longitudinal_rised ~= -1
                obj.longitudinal_result_monotonicity = u_longitudinal_rised;
            end
            
            if ~u_changed || (u_changed && obj.stable_conditions == false && monotonicity_changed == false)
                obj.difference_longitudinal = obj.difference_longitudinal + (u_longitudinal - obj.previous_u_longitudinal);
                longitudinal_difference_result = ...
                    [obj.u_fun(obj.difference_longitudinal(1),obj.simulation_step_from_fixed_update * obj.simulation_counter, obj.difference_longitudinal(2)),...
                    obj.w_fun(obj.difference_longitudinal(1),obj.simulation_step_from_fixed_update * obj.simulation_counter, obj.difference_longitudinal(2)),...
                    obj.q_fun(obj.difference_longitudinal(1),obj.simulation_step_from_fixed_update * obj.simulation_counter, obj.difference_longitudinal(2)),...
                    obj.theta_fun(obj.difference_longitudinal(1),obj.simulation_step_from_fixed_update * obj.simulation_counter, obj.difference_longitudinal(2))];
                longitudinal_result = obj.last_stable_longitudinal_value + longitudinal_difference_result;
                % Is flight stable?
                %syms t;
                if obj.stable_conditions == false && length(obj.previous_longitudinal_result) > 1 && norm(obj.previous_longitudinal_result - longitudinal_result) < obj.longitudinal_stability_condition
                    obj.stable_conditions = true;
                    disp('stable');
                    disp(obj.simulation_step_from_fixed_update * obj.simulation_counter);
                end 
            else % u_changed && (obj.stable_conditions == true || monotonicity_changed == true)
                %if monotonicity_changed
                    obj.difference_longitudinal = u_longitudinal - obj.previous_u_longitudinal;
%                 else
%                     obj.difference_longitudinal = obj.difference_longitudinal + u_longitudinal - obj.previous_u_longitudinal;
%                 end
                
                syms t;
                
                obj.last_stable_longitudinal_value = [double(vpa(limit(obj.u_fun(obj.previous_u_longitudinal(1),t,obj.previous_u_longitudinal(2)),t,Inf),3)),...
                    double(vpa(limit(obj.w_fun(obj.previous_u_longitudinal(1),t,obj.previous_u_longitudinal(2)),t,Inf),3)),...
                    double(vpa(limit(obj.q_fun(obj.previous_u_longitudinal(1),t,obj.previous_u_longitudinal(2)),t,Inf),3)),...
                    double(vpa(limit(obj.theta_fun(obj.previous_u_longitudinal(1),t,obj.previous_u_longitudinal(2)),t,Inf),3))];
                %obj.last_stable_longitudinal_value = obj.previous_longitudinal_result;
                obj.simulation_counter = 1;
                longitudinal_difference_result = ...
                    [obj.u_fun(obj.difference_longitudinal(1),obj.simulation_step_from_fixed_update * obj.simulation_counter, obj.difference_longitudinal(2)),...
                    obj.w_fun(obj.difference_longitudinal(1),obj.simulation_step_from_fixed_update * obj.simulation_counter, obj.difference_longitudinal(2)),...
                    obj.q_fun(obj.difference_longitudinal(1),obj.simulation_step_from_fixed_update * obj.simulation_counter, obj.difference_longitudinal(2)),...
                    obj.theta_fun(obj.difference_longitudinal(1),obj.simulation_step_from_fixed_update * obj.simulation_counter, obj.difference_longitudinal(2))];
                longitudinal_result = obj.last_stable_longitudinal_value + longitudinal_difference_result;
                obj.stable_conditions = false;
            end
            
%             obj.previous_u_longitudinal_rised = false;
%             if length(obj.previous_longitudinal_result) > 1 && longitudinal_result(4) - obj.previous_longitudinal_result(4) > 0
%                 obj.previous_u_longitudinal_rised = true;
%             end
            
            obj.previous_u_longitudinal = u_longitudinal;
            obj.previous_longitudinal_result = longitudinal_result;
            obj.simulation_counter = obj.simulation_counter + 1;
        end
        %
        function lateral_result = SimulateLaplaceLateral(obj, u_lateral)
            lateral_result = ...
                    [0 0 0 0 0];
        end
    end
    
end

