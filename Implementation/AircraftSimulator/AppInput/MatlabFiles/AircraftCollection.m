classdef AircraftCollection < handle
    %UNTITLED Summary of this class goes here
    %   Detailed explanation goes here
    
    properties
        Instances
        ERROR = false;
    end
    
    methods
        function obj = AircraftCollection()
            obj.Instances = Aircraft.empty;
        end
        
        function AddAircraft(obj, newAircraft)
           obj.Instances = [obj.Instances, newAircraft];
        end
        
        function aircraft = GetAircraft(obj, client_id)
            if client_id > length(obj.Instances) || client_id < 1
                obj.ERROR = true;
                aircraft = -1;
                return;
            end
            aircraft = obj.Instances(client_id);
        end
    end
    
end

