classdef Shooter < handle
    
    properties
        Strategies
    end
    
    methods
        % Returns concrete strategy
        function Strategy = GetStrategy(obj, index)
            index = max(index, length(obj.Strategies));
            Strategy = obj.Strategies(index);
        end
        % Return amount of available strategies
        function amount = GetStrategiesAmount(obj)
            amount = length(obj.Strategies);
        end
    end
    
end

