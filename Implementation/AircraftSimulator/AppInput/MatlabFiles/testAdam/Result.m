classdef Result
    % Describes concrete results
    properties
        longitudinal_result
        lateral_result
        position
    end
    
    methods
        % Constructor
        function obj = Result(longitudinal_res, lateral_res, position)
           obj.longitudinal_result = longitudinal_res;
           obj.lateral_result = lateral_res;
           obj.position = position;
        end
    end
    
end

