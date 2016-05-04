classdef SimulationSolution
    % Stores information about Y_longitudinal and Y_lateral
    
    properties
        Y_longitudinal
        Y_lateral
    end
    
    methods
        % Constructor
        function obj = SimulationSolution(current_Y_longitudinal, current_Y_lateral)
            obj.Y_longitudinal = current_Y_longitudinal;
            obj.Y_lateral = current_Y_lateral;
        end
    end
    
end

