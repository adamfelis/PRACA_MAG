classdef Missile < handle & Shooter
    % Describes missile and its properties
    
    properties
        shooter_id
        missile_id
    end
    
    methods
        % Constructor
        function obj = Missile(s_id, m_id, missilePos, simulation_step, targetPosition, velocity)
            obj.shooter_id = s_id;
            obj.missile_id = m_id;
            AddStrategy(obj, missilePos, simulation_step, targetPosition, velocity);
        end
        % Add extra strategy
        function AddStrategy(obj, missilePos, simulation_step, targetPosition, velocity)
           obj.Strategies = [obj.Strategies, MissileStrategy(missilePos, simulation_step, velocity)];%, targetPosition)];  
        end
    end
    
end

