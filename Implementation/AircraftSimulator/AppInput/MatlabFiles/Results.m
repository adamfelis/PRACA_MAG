classdef Results < handle
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
        function AddStrategyResult(obj, longitudinal_result, lateral_result, position)
           obj.StrategiesResults = [obj.StrategiesResults, Result(longitudinal_result, lateral_result, position)];
        end
        % Present data in the float[][] form
        function result = PresentFinalResults(obj)
            result = [];
            for i = 1:1:length(obj.StrategiesResults)
               result = [result; obj.StrategiesResults(i).lateral_result, obj.StrategiesResults(i).longitudinal_result];%, obj.StrategiesResults(i).position];
            end
        end
    end
    
end

