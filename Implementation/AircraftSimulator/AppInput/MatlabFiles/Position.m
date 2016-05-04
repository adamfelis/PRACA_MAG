classdef Position
    % Represents position
    
    properties
        value
    end
    
    methods
        function obj = Position(U, V, W)
           obj.value = [U, V, W]; 
        end
    end
    
end

