classdef Results
    % Describes results of computations in Matlab, which would be presented
    % in C#
    properties
        StrategiesResults
    end
    
    methods
        % Constructor
        function obj = Results()
            obj.StrategiesResults = [];
        end
        % Add next concrete result
        function AddStrategyResult(obj, longitudinal_result, lateral_result)
           obj.StrategiesResult = [obj.StrategiesResult, Result(longitudinal_result, lateral_result)];
        end
        % Present data in the float[][] form
        function result = PresentFinalResults(obj)
            result = [];
            for i = 1:1:length(obj.StrategiesResults)
               result = [result; obj.StrategiesResults.lateral_result, obj.StrategiesResults.longitudinal_result];
            end
        end
    end
    
end

