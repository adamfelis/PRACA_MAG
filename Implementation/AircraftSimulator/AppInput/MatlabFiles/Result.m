classdef Result
    % Describes concrete results
    properties
        longitudinal_result
        lateral_result
    end
    
    methods
        % Constructor
        function obj = Result(longitudinal_res, lateral_res)
           obj.longitudinal_result = longitudinal_res;
           obj.lateral_result = lateral_res;
        end
    end
    
end

