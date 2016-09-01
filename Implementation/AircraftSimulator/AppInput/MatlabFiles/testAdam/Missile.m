classdef Missile < handle & Shooter
    % Describes missile and its properties
    
    properties
    end
    
    methods
        % Constructor
        function obj = Missile()
            AddStrategy(obj);
        end
        % Add extra strategy
        function AddStrategy(obj)
           obj.Strategies = [obj.Strategies, MissileStrategy()];  
        end
    end
    
end

