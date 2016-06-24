classdef AircraftStrategy < handle & Strategy
    %Describes one strategy of the aircraft
    
    properties
        A_longitudinal_without_regulation;
        B_longitudinal_without_regulation;
        A_lateral_without_regulation;
        B_lateral_without_regulation;
        A_longitudinal;
        B_longitudinal;
        A_lateral;
        B_lateral
        
        initial_aircraft_velocity = [0 0 50]; %w v u
        current_aircraft_velocity
        simulation_step_from_fixed_update
        simulation_counter_eta = 1;
        
        stable_conditions_eta = false;
        longitudinal_stability_condition = 0.05;
        previous_longitudinal_eta_result = false;
        previous_u_longitudinal = false;
        previous_xi_lateral_result = false;
        previous_zeta_lateral_result = false;
        previous_u_lateral = false;
        eta_difference_longitudinal = false;
        last_stable_longitudinal_eta_value = false;
        longitudinal_eta_result_monotonicity = -1; % true means that function grows
        
        factor_of_reference_longitudinal
        factor_of_reference_lateral
        
        
        
        xi_difference_lateral = false
        zeta_difference_lateral = false
        last_stable_lateral_xi_value = false
        last_stable_lateral_zeta_value = false
        lateral_xi_result_monotonicity = -1
        lateral_zeta_result_monotonicity = -1
        stable_conditions_lateral_xi = false
        stable_conditions_lateral_zeta = false
        simulation_counter_lateral_xi = 1
        simulation_counter_lateral_zeta = 1
        lateral_stability_condition = 0.001;
        
        
        tau_difference_longitudinal = false;
        last_stable_longitudinal_tau_value = false;
        longitudinal_tau_result_monotonicity = -1;
        stable_conditions_tau = false;
        simulation_counter_tau = 1
        previous_longitudinal_tau_result = false;
            
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
        
        delta_s_lateral
        N_v_xi
        N_p_xi
        N_r_xi
        N_phi_xi
        N_psi_xi
        
        N_v_zeta
        N_p_zeta
        N_r_zeta
        N_phi_zeta
        N_psi_zeta
        
        v_xi_s
        p_xi_s
        r_xi_s
        phi_xi_s
        psi_xi_s
        
        v_zeta_s
        p_zeta_s
        r_zeta_s
        phi_zeta_s
        psi_zeta_s 
        
        v_t
        p_t
        r_t
        phi_t
        psi_t
        
        v_t_fun
        p_t_fun
        r_t_fun
        phi_t_fun
        psi_t_fun
        
        
        v_fun
        p_fun
        r_fun
        phi_fun
        psi_fun
    end
    
    methods
        % Constructor
        function obj = AircraftStrategy(new_A_longitudinal, new_B_longitudinal,...
                                        new_A_lateral, new_B_lateral, simulation_step)
                                    
           obj.A_longitudinal_without_regulation = new_A_longitudinal;
           obj.B_longitudinal_without_regulation = new_B_longitudinal;
           
           obj.A_lateral_without_regulation = new_A_lateral;
           obj.B_lateral_without_regulation = new_B_lateral;
           
           obj.A_longitudinal = new_A_longitudinal;
           obj.B_longitudinal = new_B_longitudinal;
           obj.A_lateral = new_A_lateral;
           obj.B_lateral = new_B_lateral;
           
           obj.current_aircraft_velocity = obj.initial_aircraft_velocity;
           
           obj.simulation_step_from_fixed_update = simulation_step;
           
           C = eye(4);
           R = eye(2);
           p = 10;
           Q = p * (C' * C);
           K = lqr(new_A_longitudinal,new_B_longitudinal,Q,R);
           Nbar = 1000;
           %Nbar = 1;
           %K = zeros(2,4);
           %obj.A_longitudinal = obj.A_longitudinal - obj.B_longitudinal * K;
           %obj.B_longitudinal = Nbar * obj.B_longitudinal;
           
           C = eye(5);
           R = eye(2);
           p = 10000;
           Q = p * (C' * C);
           K = lqr(new_A_lateral,new_B_lateral,Q,R);
           %K = zeros(2,5);
           Nbar = 200;
           %Nbar = 1;
           %obj.A_lateral = obj.A_lateral - obj.B_lateral * K;
           %obj.B_lateral = Nbar * obj.B_lateral;
           %tic
           obj.PrepareTransferFunctions();
           %toc
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
           
           %U_e = 178;
           %obj.N_a_z_eta = -s * (obj.N_w_eta - U_e * obj.N_theta_eta);
           
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
           
           %obj.N_a_z_tau = -s * (obj.N_w_tau - U_e * obj.N_theta_tau);
           
           
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
           %obj.a_z_tau_s = vpa(tau/s * obj.N_a_z_tau / obj.delta_s, precision);
           
%            K_p = 0.00693;
%            K_i = 1.21e-05;
%            K_d = 0.989;
%            
%            pid_s = K_p + K_i / s + K_d * s;
           
%            obj.u_t = vpa(simplify(ilaplace(4.4622e+03 * obj.u_eta_s + obj.u_tau_s)), precision);
%            obj.w_t = vpa(simplify(ilaplace(-1.1778e+03 * obj.w_eta_s + obj.w_tau_s)), precision);
%            obj.q_t = vpa(simplify(ilaplace(4145.619 * obj.q_eta_s + obj.q_tau_s)), precision);
%            obj.theta_t = vpa(simplify(ilaplace(83.239 * obj.theta_eta_s) + ilaplace(obj.theta_tau_s)), precision);
%            obj.a_z_t = vpa(simplify(ilaplace(obj.a_z_eta_s + obj.a_z_tau_s)), precision);
%            
           obj.u_t = ilaplace(obj.u_eta_s + obj.u_tau_s);
           obj.w_t = ilaplace(obj.w_eta_s + obj.w_tau_s);
           obj.q_t = ilaplace(obj.q_eta_s + obj.q_tau_s);
           obj.theta_t = ilaplace(obj.theta_eta_s) + ilaplace(obj.theta_tau_s);
           %obj.a_z_t = vpa(simplify(ilaplace(obj.a_z_eta_s + obj.a_z_tau_s)), precision);
            
           obj.u_t_fun = matlabFunction(obj.u_t);
           obj.w_t_fun = matlabFunction(obj.w_t);
           obj.q_t_fun = matlabFunction(obj.q_t);
           obj.theta_t_fun = matlabFunction(obj.theta_t);
           %obj.a_z_t_fun = matlabFunction(obj.a_z_t);
           
           obj.u_fun = @(ni,t, tau) (obj.u_t_fun(ni, t) - obj.u_t_fun(ni, obj.simulation_step_from_fixed_update));
           obj.w_fun = @(ni,t, tau) (obj.w_t_fun(ni, t) - obj.w_t_fun(ni, obj.simulation_step_from_fixed_update));
           obj.q_fun = @(ni,t, tau) (obj.q_t_fun(ni, t) - obj.q_t_fun(ni, obj.simulation_step_from_fixed_update));
           obj.theta_fun = @(ni,t, tau) (obj.theta_t_fun(ni, t) - obj.theta_t_fun(ni, obj.simulation_step_from_fixed_update));
           
            %syms t;
%                 
           precision = 5;
            obj.factor_of_reference_longitudinal = [round(double(limit(obj.u_fun(0.1,s,0),s,Inf)),precision) / 0.1,...
                    round(double(limit(obj.w_fun(0.1,s,0),s,Inf)),precision)/0.1,...
                    round(double(limit(obj.q_fun(0.1,s,0),s,Inf)),precision)/0.1,...
                    round(double(limit(obj.theta_fun(0.1,s,0),s,Inf)),precision)/0.1;...
                    ...
                    round(double(limit(obj.u_fun(0,s,0.1),s,Inf)),precision) / 0.1,...
                    round(double(limit(obj.w_fun(0,s,0.1),s,Inf)),precision)/0.1,...
                    round(double(limit(obj.q_fun(0,s,0.1),s,Inf)),precision)/0.1,...
                    round(double(limit(obj.theta_fun(0,s,0.1),s,Inf)),precision)/0.1];
            %lateral
            
           y_v = obj.A_lateral(1,1);
           y_p = obj.A_lateral(1,2);
           y_r = obj.A_lateral(1,3);
           y_phi = obj.A_lateral(1,4);
           y_psi = obj.A_lateral(1,5);
            
           l_v = obj.A_lateral(2,1);
           l_p = obj.A_lateral(2,2);
           l_r = obj.A_lateral(2,3);
           l_phi = obj.A_lateral(2,4);
           l_psi = obj.A_lateral(2,5);
            
           n_v = obj.A_lateral(3,1);
           n_p = obj.A_lateral(3,2);
           n_r = obj.A_lateral(3,3);
           n_phi = obj.A_lateral(3,4);
           n_psi = obj.A_lateral(3,5);
           
           y_xi = obj.B_lateral(1,1);
           y_zeta = obj.B_lateral(1,2);
           
           l_xi = obj.B_lateral(2,1);
           l_zeta = obj.B_lateral(2,2);
           
           n_xi = obj.B_lateral(3,1);
           n_zeta = obj.B_lateral(3,2);
           
           a = 1;
           b = -(l_p + n_r + y_v);
           c = (l_p * n_r - l_r * n_p) + (n_r * y_v - n_v * y_r) + (l_p*y_v - l_v * y_p) - (l_phi + n_psi);
           d = (l_p * n_psi - l_psi * n_p) + (l_phi * n_r - l_r *n_phi) + l_v * (n_r * y_p - n_p * y_r - y_phi)+...
               n_v * (l_p * y_r - l_r * y_p - y_psi) + y_v * (l_r * n_p - l_p * n_r + l_phi + n_psi);
           e = (l_phi * n_psi - l_psi * n_phi) + l_v * ( (n_r * y_phi - n_phi * y_r) + (n_psi * y_p - n_p * y_psi) ) +...
               n_v * ( (l_phi * y_r - l_r * y_phi) + (l_p * y_psi - l_psi * y_p) ) +...
               y_v * ( (l_r * n_phi - l_phi * n_r) + (l_psi * n_p - l_p * n_psi));
           f = l_v * (n_psi * y_phi - n_phi * y_psi) + n_v * (l_phi * y_psi - l_psi * y_phi) + y_v * (l_psi * n_phi - l_phi * n_psi);
           
           obj.delta_s_lateral = a * s^5 + b * s^4 + c * s^3 + d * s^2 + e * s + f;
           
           a = y_xi;
           b = l_xi * y_p + n_xi * y_r - y_xi * (l_p + n_r);
           c = l_xi * (n_p * y_r - n_r * y_p + y_phi) + n_xi * (l_r * y_p - l_p * y_r + y_psi) + y_xi * (l_p * n_r - l_r * n_p - l_phi - n_psi);
           d = l_xi * (n_phi * y_r - n_r * y_phi +n_p * y_psi - n_psi * y_p) + ...
               n_xi * (l_r * y_phi - l_phi * y_r + l_psi * y_p - l_p * y_psi) + ...
               y_xi * (l_phi * n_r - l_r * n_phi + l_p * n_psi - l_psi * n_p);
           e = l_xi * (n_phi * y_psi - n_psi * y_phi) + n_xi * (l_psi * y_phi - l_phi * y_psi) + y_xi * (l_phi * n_psi - l_psi * n_phi);
           
           obj.N_v_xi = a * s^4 + b * s^3 + c * s^2 + d * s + e;
           
           a = l_xi;
           b = -l_xi * (n_r + y_v) + n_xi * l_r + y_xi * l_v;
           c = l_xi * (n_r * y_v - n_v * y_r - n_psi) + n_xi * (l_v * y_r - l_r * y_v + l_psi) + y_xi * (l_r * n_v - l_v * n_r);
           d = l_xi * (n_psi * y_v - n_v * y_psi) + n_xi * (l_v * y_psi - l_psi * y_r) + y_xi * (l_psi * n_v - l_v * n_psi);
           
           obj.N_p_xi = s * (a * s^3 + b * s^2 + c * s + d);
           obj.N_phi_xi = a * s^3 + b * s^2 + c * s + d;
           
           a = n_xi;
           b = l_xi * n_p - n_xi * (l_p + y_v) + y_xi * n_v;
           c = l_xi * (n_v * y_p - n_p * y_v + n_phi) + n_xi * (l_p * y_v - l_v * y_p - l_phi) + y_xi * (l_v * n_p - l_p * n_v);
           d = l_xi * (n_v * y_phi - n_phi * y_v) + n_xi * (l_phi * y_v - l_v * y_phi) + y_xi * (l_v * n_phi - l_phi * n_v);
           
           obj.N_r_xi = s * (a * s^3 + b * s^2 + c * s + d);
           obj.N_psi_xi = a * s^3 + b * s^2 + c * s + d;
           
           
           a = y_zeta;
           b = l_zeta * y_p + n_zeta * y_r - y_zeta * (l_p + n_r);
           c = l_zeta * (n_p * y_r - n_r * y_p + y_phi) + n_zeta * (l_r * y_p - l_p * y_r + y_psi) + y_zeta * (l_p * n_r - l_r * n_p - l_phi - n_psi);
           d = l_zeta * (n_phi * y_r - n_r * y_phi +n_p * y_psi - n_psi * y_p) + ...
               n_zeta * (l_r * y_phi - l_phi * y_r + l_psi * y_p - l_p * y_psi) + ...
               y_zeta * (l_phi * n_r - l_r * n_phi + l_p * n_psi - l_psi * n_p);
           e = l_zeta * (n_phi * y_psi - n_psi * y_phi) + n_zeta * (l_psi * y_phi - l_phi * y_psi) + y_zeta * (l_phi * n_psi - l_psi * n_phi);
           
           obj.N_v_zeta = a * s^4 + b * s^3 + c * s^2 + d * s + e;
           
           a = l_zeta;
           b = -l_zeta * (n_r + y_v) + n_zeta * l_r + y_zeta * l_v;
           c = l_zeta * (n_r * y_v - n_v * y_r - n_psi) + n_zeta * (l_v * y_r - l_r * y_v + l_psi) + y_zeta * (l_r * n_v - l_v * n_r);
           d = l_zeta * (n_psi * y_v - n_v * y_psi) + n_zeta * (l_v * y_psi - l_psi * y_r) + y_zeta * (l_psi * n_v - l_v * n_psi);
           
           obj.N_p_zeta = s * (a * s^3 + b * s^2 + c * s + d);
           obj.N_phi_zeta = a * s^3 + b * s^2 + c * s + d;
           
           a = n_zeta;
           b = l_zeta * n_p - n_zeta * (l_p + y_v) + y_zeta * n_v;
           c = l_zeta * (n_v * y_p - n_p * y_v + n_phi) + n_zeta * (l_p * y_v - l_v * y_p - l_phi) + y_zeta * (l_v * n_p - l_p * n_v);
           d = l_zeta * (n_v * y_phi - n_phi * y_v) + n_zeta * (l_phi * y_v - l_v * y_phi) + y_zeta * (l_v * n_phi - l_phi * n_v);
           
           obj.N_r_zeta = s * (a * s^3 + b * s^2 + c * s + d);
           obj.N_psi_zeta = a * s^3 + b * s^2 + c * s + d;
        
           %syms xi zeta; using ni tau
           
           precision = 4;
           obj.v_xi_s = vpa(ni/s * obj.N_v_xi / obj.delta_s_lateral, precision);
           obj.p_xi_s = vpa(ni/s * obj.N_p_xi / obj.delta_s_lateral, precision);
           obj.r_xi_s = vpa(ni/s * obj.N_r_xi / obj.delta_s_lateral, precision);
           obj.phi_xi_s = vpa(ni/s * obj.N_phi_xi / obj.delta_s_lateral, precision);
           obj.psi_xi_s = vpa(ni/s * obj.N_psi_xi / obj.delta_s_lateral, precision);
           
           obj.v_zeta_s = vpa(tau/s * obj.N_v_zeta / obj.delta_s_lateral, precision);
           obj.p_zeta_s = vpa(tau/s * obj.N_p_zeta / obj.delta_s_lateral, precision);
           obj.r_zeta_s = vpa(tau/s * obj.N_r_zeta / obj.delta_s_lateral, precision);
           obj.phi_zeta_s = vpa(tau/s * obj.N_phi_zeta / obj.delta_s_lateral, precision);
           obj.psi_zeta_s = vpa(tau/s * obj.N_psi_zeta / obj.delta_s_lateral, precision);

%            obj.v_t = vpa(simplify(ilaplace(obj.v_xi_s + obj.v_zeta_s)), precision);
%            obj.p_t = vpa(simplify(ilaplace(obj.p_xi_s + obj.p_zeta_s)), precision);
%            obj.r_t = vpa(simplify(ilaplace(obj.r_xi_s + obj.r_zeta_s)), precision);
%            obj.phi_t = vpa(simplify(ilaplace(obj.phi_xi_s) + ilaplace(obj.phi_zeta_s)), precision);
%            obj.psi_t = vpa(simplify(ilaplace(obj.psi_xi_s + obj.psi_zeta_s)), precision);

           obj.v_t = ilaplace(obj.v_xi_s + obj.v_zeta_s);
           obj.p_t = ilaplace(obj.p_xi_s + obj.p_zeta_s);
           obj.r_t = ilaplace(obj.r_xi_s + obj.r_zeta_s);
           obj.phi_t = ilaplace(obj.phi_xi_s) + ilaplace(obj.phi_zeta_s);
           obj.psi_t = ilaplace(obj.psi_xi_s + obj.psi_zeta_s);
            
           obj.v_t_fun = matlabFunction(obj.v_t);
           obj.p_t_fun = matlabFunction(obj.p_t);
           obj.r_t_fun = matlabFunction(obj.r_t);
           obj.phi_t_fun = matlabFunction(obj.phi_t);
           obj.psi_t_fun = matlabFunction(obj.psi_t);
           
           obj.v_fun = @(ni,t, tau) (obj.v_t_fun(ni, t, tau) - obj.v_t_fun(ni, obj.simulation_step_from_fixed_update, tau));
           obj.p_fun = @(ni,t, tau) (obj.p_t_fun(ni, t, tau) - obj.p_t_fun(ni, obj.simulation_step_from_fixed_update, tau));
           obj.r_fun = @(ni,t, tau) (obj.r_t_fun(ni, t, tau) - obj.r_t_fun(ni, obj.simulation_step_from_fixed_update, tau));
           obj.phi_fun = @(ni,t, tau) (obj.phi_t_fun(ni, t, tau) - obj.phi_t_fun(ni, obj.simulation_step_from_fixed_update, tau));
           obj.psi_fun = @(ni,t, tau) (obj.psi_t_fun(ni, t, tau) - obj.psi_t_fun(ni, obj.simulation_step_from_fixed_update, tau));
           
           precision = 5;
           obj.factor_of_reference_lateral = [round(double(limit(obj.v_fun(0.1,s,0),s,Inf)), precision) / 0.1,...
                    round(double(limit(obj.p_fun(0.1,s,0),s,Inf)), precision)/0.1,...
                    round(double(limit(obj.r_fun(0.1,s,0),s,Inf)), precision)/0.1,...
                    round(double(limit(obj.phi_fun(0.1,s,0),s,Inf)), precision)/0.1,...
                    round(double(limit(obj.psi_fun(0.1,s,0),s,Inf)), precision)/0.1;...
                    ...
                    round(double(limit(obj.v_fun(0,s,0.1),s,Inf)), precision) / 0.1,...
                    round(double(limit(obj.p_fun(0,s,0.1),s,Inf)), precision)/0.1,...
                    round(double(limit(obj.r_fun(0,s,0.1),s,Inf)), precision)/0.1,...
                    round(double(limit(obj.phi_fun(0,s,0.1),s,Inf)), precision)/0.1,...
                    round(double(limit(obj.psi_fun(0,s,0.1),s,Inf)), precision)/0.1];
                
%            obj.phi_fun = @(xi,t, zeta) (mod(obj.phi_t_fun(t,xi, zeta) - obj.phi_t_fun(obj.simulation_step_from_fixed_update,xi, zeta), 2 * pi));
%            obj.psi_fun = @(xi,t, zeta) (mod(obj.psi_t_fun(t,xi, zeta) - obj.psi_t_fun(obj.simulation_step_from_fixed_update,xi, zeta), 2 * pi));
            
        end
        %        
        function longitudinal_result = SimulateLaplaceLongitudinal(obj, u_longitudinal)
            %% Initial Conditions
            if length(obj.eta_difference_longitudinal) == 1
                obj.eta_difference_longitudinal = [0 0 0 0 ; 0 0 0 0 ];
            end            
            if length(obj.tau_difference_longitudinal) == 1
                obj.tau_difference_longitudinal = [0 0 0 0 ; 0 0 0 0 ];
            end
            
            if length(obj.last_stable_longitudinal_eta_value) == 1
                obj.last_stable_longitudinal_eta_value = [0 0 0 0];
            end
            if length(obj.last_stable_longitudinal_tau_value) == 1
                obj.last_stable_longitudinal_tau_value = [0 0 0 0];
            end
            
            eta_changed = false;
            eta_longitudinal_rised = true; % new u_longitudnal is greater or equals than previous one
            if length(obj.previous_u_longitudinal) > 1 &&  norm(obj.previous_u_longitudinal(1) - u_longitudinal(1)) > 0
                eta_changed = true;
                if length(obj.previous_u_longitudinal) > 1 &&  u_longitudinal(1) - obj.previous_u_longitudinal(1) < 0
                    eta_longitudinal_rised = false;
                end
            end
            tau_changed = false;
            tau_longitudinal_rised = true; % new u_longitudnal is greater or equals than previous one
            if length(obj.previous_u_longitudinal) > 1 &&  norm(obj.previous_u_longitudinal(2) - u_longitudinal(2)) > 0
                tau_changed = true;
                if length(obj.previous_u_longitudinal) > 1 &&  u_longitudinal(2) - obj.previous_u_longitudinal(2) < 0
                    tau_longitudinal_rised = false;
                end
            end
            
                
            if length(obj.previous_u_longitudinal) > 1 &&  u_longitudinal(1) - obj.previous_u_longitudinal(1) == 0
                eta_longitudinal_rised = -1;
            end
            if length(obj.previous_u_longitudinal) > 1 &&  u_longitudinal(2) - obj.previous_u_longitudinal(2) == 0
                tau_longitudinal_rised = -1;
            end
            
            eta_monotonicity_changed = false;
            if obj.longitudinal_eta_result_monotonicity ~= -1
                if (obj.longitudinal_eta_result_monotonicity == true && eta_longitudinal_rised == false) ||...
                        (obj.longitudinal_eta_result_monotonicity == false && eta_longitudinal_rised == true)
                    eta_monotonicity_changed = true;
                end
            end            
            tau_monotonicity_changed = false;
            if obj.longitudinal_tau_result_monotonicity ~= -1
                if (obj.longitudinal_tau_result_monotonicity == true && tau_longitudinal_rised == false) ||...
                        (obj.longitudinal_tau_result_monotonicity == false && tau_longitudinal_rised == true)
                    tau_monotonicity_changed = true;
                end
            end
            
            if eta_longitudinal_rised ~= -1
                obj.longitudinal_eta_result_monotonicity = eta_longitudinal_rised;
            end
            if tau_longitudinal_rised ~= -1
                obj.longitudinal_tau_result_monotonicity = tau_longitudinal_rised;
            end
            %% Main Algorithm
            if ~eta_changed || (eta_changed && obj.stable_conditions_eta == false && eta_monotonicity_changed == false)
                dif = (u_longitudinal - obj.previous_u_longitudinal);
                obj.eta_difference_longitudinal = ([dif(1).* ones(1,4); dif(2).* ones(1,4) ]) + obj.eta_difference_longitudinal;
                longitudinal_difference_result = obj.CalculateLongitudinalUpdate(obj.eta_difference_longitudinal(1,:), zeros(1,4), obj.simulation_step_from_fixed_update * obj.simulation_counter_eta);
                eta_longitudinal_result = obj.last_stable_longitudinal_eta_value + longitudinal_difference_result;
                if obj.stable_conditions_eta == false && length(obj.previous_longitudinal_eta_result) > 1 && norm(obj.previous_longitudinal_eta_result - eta_longitudinal_result) < obj.longitudinal_stability_condition
                    obj.stable_conditions_eta = true;
                    %disp('stable');
                    %disp(obj.simulation_step_from_fixed_update * obj.simulation_counter_eta);
                end 
            else % u_changed && (obj.stable_conditions_eta == true || monotonicity_changed == true)
                temporary_eta_which_replace_current_state = 1./obj.factor_of_reference_longitudinal(1,:) .* obj.previous_longitudinal_eta_result;
                obj.eta_difference_longitudinal(1,:) = u_longitudinal(1) * ones(1,4) - temporary_eta_which_replace_current_state;
                obj.last_stable_longitudinal_eta_value = obj.previous_longitudinal_eta_result;
                obj.simulation_counter_eta = 2;
                longitudinal_difference_result = obj.CalculateLongitudinalUpdate(obj.eta_difference_longitudinal(1,:), zeros(1,4), obj.simulation_step_from_fixed_update * obj.simulation_counter_eta);                
                eta_longitudinal_result = obj.last_stable_longitudinal_eta_value + longitudinal_difference_result;
                obj.stable_conditions_eta = false;
            end 
            if ~tau_changed || (tau_changed && obj.stable_conditions_tau == false && tau_monotonicity_changed == false)
                dif = (u_longitudinal - obj.previous_u_longitudinal);
                obj.tau_difference_longitudinal = ([dif(1).* ones(1,4); dif(2).* ones(1,4) ]) + obj.tau_difference_longitudinal;
                longitudinal_difference_result = obj.CalculateLongitudinalUpdate(zeros(1,4), obj.tau_difference_longitudinal(2,:), obj.simulation_step_from_fixed_update * obj.simulation_counter_tau);
                tau_longitudinal_result = obj.last_stable_longitudinal_tau_value + longitudinal_difference_result;
                if obj.stable_conditions_tau == false && length(obj.previous_longitudinal_tau_result) > 1 && norm(obj.previous_longitudinal_tau_result - tau_longitudinal_result) < obj.longitudinal_stability_condition
                    obj.stable_conditions_tau = true;
                    %disp('stable');
                    %disp(obj.simulation_step_from_fixed_update * obj.simulation_counter_tau);
                end 
            else % u_changed && (obj.stable_conditions_tau == true || monotonicity_changed == true)
                temporary_tau_which_replace_current_state = 1./obj.factor_of_reference_longitudinal(2,:) .* obj.previous_longitudinal_tau_result;
                obj.tau_difference_longitudinal(2,:) = u_longitudinal(2) * ones(1,4) - temporary_tau_which_replace_current_state;
                obj.last_stable_longitudinal_tau_value = obj.previous_longitudinal_tau_result;
                obj.simulation_counter_tau = 2;
                longitudinal_difference_result = obj.CalculateLongitudinalUpdate(zeros(1,4), obj.tau_difference_longitudinal(2,:), obj.simulation_step_from_fixed_update * obj.simulation_counter_tau);                
                tau_longitudinal_result = obj.last_stable_longitudinal_tau_value + longitudinal_difference_result;
                obj.stable_conditions_tau = false;
            end 
            %% Variables Update      
            longitudinal_result = eta_longitudinal_result + tau_longitudinal_result;
            obj.previous_u_longitudinal = u_longitudinal;
            obj.previous_longitudinal_eta_result = eta_longitudinal_result;
            obj.previous_longitudinal_tau_result = tau_longitudinal_result;
            obj.simulation_counter_eta = obj.simulation_counter_eta + 1;
            obj.simulation_counter_tau = obj.simulation_counter_tau + 1;
        end
        %
        
%         function longitudinalUpdate = CalculateLongitudinalUpdate(obj, eta, tau, t)
%             longitudinalUpdate = [4462.2 * obj.u_fun(double(eta(1)), t, double(tau(1))),...
%                     1177.8 * obj.w_fun(double(eta(2)), t, double(tau(2))),...
%                     4139 * obj.q_fun(double(eta(3)), t, double(tau(3))),...
%                     83.0601 * obj.theta_fun(double(eta(4)), t, double(tau(4)))];
%         end
        
        function longitudinalUpdate = CalculateLongitudinalUpdate(obj, eta, tau, t)
            longitudinalUpdate = [obj.u_fun(double(eta(1)), t, double(tau(1))),...
                    obj.w_fun(double(eta(2)), t, double(tau(2))),...
                    obj.q_fun(double(eta(3)), t, double(tau(3))),...
                    obj.theta_fun(double(eta(4)), t, double(tau(4)))];
        end

        function lateral_result = SimulateLaplaceLateral(obj, u_lateral)
            %% Initial Conditions
            if length(obj.xi_difference_lateral) == 1
                obj.xi_difference_lateral = [0 0 0 0 0;0 0 0 0 0];
            end
            if length(obj.zeta_difference_lateral) == 1
                obj.zeta_difference_lateral = [0 0 0 0 0;0 0 0 0 0];
            end
            if length(obj.last_stable_lateral_xi_value) == 1
                obj.last_stable_lateral_xi_value = [0 0 0 0 0];
            end
            if length(obj.last_stable_lateral_zeta_value) == 1
                obj.last_stable_lateral_zeta_value = [0 0 0 0 0];
            end

            xi_changed = false;
            xi_lateral_rised = true; % new u_longitudnal is greater or equals than previous one
            if length(obj.previous_u_lateral) > 1 &&  norm(obj.previous_u_lateral(1) - u_lateral(1)) > 0
                xi_changed = true;
                if length(obj.previous_u_lateral) > 1 &&  u_lateral(1) - obj.previous_u_lateral(1) < 0
                    xi_lateral_rised = false;
                end
            end
            zeta_changed = false;
            zeta_lateral_rised = true; % new u_longitudnal is greater or equals than previous one
            if length(obj.previous_u_lateral) > 1 &&  norm(obj.previous_u_lateral(2) - u_lateral(2)) > 0
                zeta_changed = true;
                if length(obj.previous_u_lateral) > 1 &&  u_lateral(2) - obj.previous_u_lateral(2) < 0
                    zeta_lateral_rised = false;
                end
            end
                
            if length(obj.previous_u_lateral) > 1 &&  u_lateral(1) - obj.previous_u_lateral(1) == 0
                xi_lateral_rised = -1;
            end
            if length(obj.previous_u_lateral) > 1 &&  u_lateral(2) - obj.previous_u_lateral(2) == 0
                zeta_lateral_rised = -1;
            end
            
            xi_monotonicity_changed = false;
            if obj.lateral_xi_result_monotonicity ~= -1
                if (obj.lateral_xi_result_monotonicity == true && xi_lateral_rised == false) ||...
                        (obj.lateral_xi_result_monotonicity == false && xi_lateral_rised == true)
                    xi_monotonicity_changed = true;
                end
            end
            if xi_lateral_rised ~= -1
                obj.lateral_xi_result_monotonicity = xi_lateral_rised;
            end
            
            
            zeta_monotonicity_changed = false;
            if obj.lateral_zeta_result_monotonicity ~= -1
                if (obj.lateral_zeta_result_monotonicity == true && zeta_lateral_rised == false) ||...
                        (obj.lateral_zeta_result_monotonicity == false && zeta_lateral_rised == true)
                    zeta_monotonicity_changed = true;
                end
            end
            if zeta_lateral_rised ~= -1
                obj.lateral_zeta_result_monotonicity = zeta_lateral_rised;
            end
            %% Main Algorithm
            if ~xi_changed || (xi_changed && obj.stable_conditions_lateral_xi == false && xi_monotonicity_changed == false)
                dif = (u_lateral - obj.previous_u_lateral);
                obj.xi_difference_lateral = ([dif(1).* ones(1,5); dif(2).* ones(1,5) ]) + obj.xi_difference_lateral;
                lateral_difference_result = obj.CalculateLateralUpdate(obj.xi_difference_lateral(1,:),zeros(1,5), obj.simulation_step_from_fixed_update * obj.simulation_counter_lateral_xi);
                xi_lateral_result = obj.last_stable_lateral_xi_value + lateral_difference_result;
                if obj.stable_conditions_lateral_xi == false && length(obj.previous_xi_lateral_result) > 1 && norm(obj.previous_xi_lateral_result - xi_lateral_result) < obj.lateral_stability_condition
                    obj.stable_conditions_lateral_xi = true;
                    %disp('stable');
                    %disp(obj.simulation_step_from_fixed_update * obj.simulation_counter_lateral);
                end 
            else % u_changed && (obj.stable_conditions == true || monotonicity_changed == true)
                temporary_xi_which_replace_current_state = 1./obj.factor_of_reference_lateral(1,:) .* obj.previous_xi_lateral_result;
                obj.xi_difference_lateral(1,:) = u_lateral(1) * ones(1,5) - temporary_xi_which_replace_current_state;
                obj.last_stable_lateral_xi_value = obj.previous_xi_lateral_result;
                obj.simulation_counter_lateral_xi = 2;                
                lateral_difference_result = obj.CalculateLateralUpdate(obj.xi_difference_lateral(1,:),zeros(1,5), obj.simulation_step_from_fixed_update * obj.simulation_counter_lateral_xi);
                xi_lateral_result = obj.last_stable_lateral_xi_value + lateral_difference_result;
                obj.stable_conditions_lateral_xi = false;
            end
            if ~zeta_changed || (zeta_changed && obj.stable_conditions_lateral_zeta == false && zeta_monotonicity_changed == false)
                dif = (u_lateral - obj.previous_u_lateral);
                obj.zeta_difference_lateral = ([dif(1).* ones(1,5); dif(2).* ones(1,5) ]) + obj.zeta_difference_lateral;
                lateral_difference_result = obj.CalculateLateralUpdate(zeros(1,5), obj.zeta_difference_lateral(2,:), obj.simulation_step_from_fixed_update * obj.simulation_counter_lateral_zeta);
                zeta_lateral_result = obj.last_stable_lateral_zeta_value + lateral_difference_result;
                if obj.stable_conditions_lateral_zeta == false && length(obj.previous_zeta_lateral_result) > 1 && norm(obj.previous_zeta_lateral_result - zeta_lateral_result) < obj.lateral_stability_condition
                    obj.stable_conditions_lateral_zeta = true;
                    %disp('stable');
                    %disp(obj.simulation_step_from_fixed_update * obj.simulation_counter_lateral);
                end 
            else % u_changed && (obj.stable_conditions == true || monotonicity_changed == true)
                temporary_zeta_which_replace_current_state = 1./obj.factor_of_reference_lateral(2,:) .* obj.previous_zeta_lateral_result;
                obj.zeta_difference_lateral(2,:) = u_lateral(2) * ones(1,5) - temporary_zeta_which_replace_current_state;
                obj.last_stable_lateral_zeta_value = obj.previous_zeta_lateral_result;
                obj.simulation_counter_lateral_zeta = 2;                
                lateral_difference_result = obj.CalculateLateralUpdate(zeros(1,5), obj.zeta_difference_lateral(2,:), obj.simulation_step_from_fixed_update * obj.simulation_counter_lateral_zeta);
                zeta_lateral_result = obj.last_stable_lateral_zeta_value + lateral_difference_result;
                obj.stable_conditions_lateral_zeta = false;
            end
            %% Variables Update
            lateral_result = xi_lateral_result + zeta_lateral_result;
            obj.previous_u_lateral = u_lateral;
            obj.previous_xi_lateral_result = xi_lateral_result;
            obj.previous_zeta_lateral_result = zeta_lateral_result;
            obj.simulation_counter_lateral_xi = obj.simulation_counter_lateral_xi + 1;
            obj.simulation_counter_lateral_zeta = obj.simulation_counter_lateral_zeta + 1;
            
%             lateral_result(4) = mod(lateral_result(4), 2 * pi);
%             lateral_result(5) = mod(lateral_result(5), 2 * pi);
        end
        
%         function lateralUpdate = CalculateLateralUpdate(obj, xi, zeta, t)
%             lateralUpdate = [7962.09 * obj.v_fun(double(xi(1)), t, double(zeta(1))),...
%                     1.7914 * obj.p_fun(double(xi(2)), t, double(zeta(2))),...
%                     -44.91 * obj.r_fun(double(xi(3)), t, double(zeta(3))),...
%                     200 * obj.phi_fun(double(xi(4)), t, double(zeta(4))),...
%                     200 * obj.psi_fun(double(xi(5)), t, double(zeta(5)))];
%         end

        function lateralUpdate = CalculateLateralUpdate(obj, xi, zeta, t)
            lateralUpdate = [obj.v_fun(double(xi(1)), t, double(zeta(1))),...
                   obj.p_fun(double(xi(2)), t, double(zeta(2))),...
                    obj.r_fun(double(xi(3)), t, double(zeta(3))),...
                  obj.phi_fun(double(xi(4)), t, double(zeta(4))),...
                    obj.psi_fun(double(xi(5)), t, double(zeta(5)))];
        end
        
        function movementVector = MoveAircraft(obj, longitudinal_solution, lateral_solution, u_longitudinal, u_lateral)
                        
            temp_velocity = obj.initial_aircraft_velocity + [longitudinal_solution(2), lateral_solution(1), longitudinal_solution(1)];
            obj.current_aircraft_velocity = temp_velocity;
            
            longitudinal_derivatives = obj.A_longitudinal_without_regulation * longitudinal_solution + obj.B_longitudinal_without_regulation * u_longitudinal;
            lateral_derivatives = obj.A_lateral_without_regulation * lateral_solution + obj.B_lateral_without_regulation * u_lateral;
%             longitudinal_derivatives = obj.A_longitudinal * longitudinal_solution + obj.B_longitudinal * u_longitudinal;
%             lateral_derivatives = obj.A_lateral * lateral_solution + obj.B_lateral * u_lateral;
            
            acceleration_vector = [longitudinal_derivatives(2); lateral_derivatives(1) ; longitudinal_derivatives(1)];%w v u
            
            pitch_angle = longitudinal_solution(4);
            roll_angle = lateral_solution(4);
            yaw_angle = lateral_solution(5); 
            
            R_x = [1 0 0;...
                0 cos(roll_angle) -sin(roll_angle);...
                0 sin(roll_angle) cos(roll_angle)];
            
            R_y = [cos(pitch_angle) 0 sin(pitch_angle);...
                0 1 0;...
                -sin(pitch_angle) 0 cos(pitch_angle)];
            
            R_z = [cos(yaw_angle) -sin(yaw_angle) 0;...
                sin(yaw_angle) cos(yaw_angle) 0;...
                0 0 1];
            
            rotation_matrix = R_z * R_y * R_x;
            
            acceleration_vector_after_rotation = rotation_matrix * acceleration_vector;
            gravity_vector = [-9.81; 0; 0];
            acceleration_vector_after_rotation_including_gravity = acceleration_vector_after_rotation + gravity_vector;
            
            tmpMovementVector = ...
                temp_velocity' * obj.simulation_step_from_fixed_update + ... %vt
                acceleration_vector_after_rotation_including_gravity * ...
                obj.simulation_step_from_fixed_update * obj.simulation_step_from_fixed_update / 2;%at^2/2
            movementVector = [tmpMovementVector(3), tmpMovementVector(2), tmpMovementVector(1)];
        end
    end
    
end

