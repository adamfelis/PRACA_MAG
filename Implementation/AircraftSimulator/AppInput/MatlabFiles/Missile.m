classdef Missile < handle & Shooter
    % Describes missile and its properties
    
    properties
        shooter_id
        missile_id
    end
    
    methods
        % Constructor
        function obj = Missile(s_id, m_id, missilePos)
            obj.shooter_id = s_id;
            obj.missile_id = m_id;
            AddStrategy(obj, missilePos);
        end
        % Add extra strategy
        function AddStrategy(obj, missilePos)
           obj.Strategies = [obj.Strategies, MissileStrategy(missilePos)];  
        end
    end
    
end

