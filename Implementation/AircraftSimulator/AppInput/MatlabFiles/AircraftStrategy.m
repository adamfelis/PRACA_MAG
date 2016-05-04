classdef AircraftStrategy < Strategy
    %Describes one strategy of the aircraft
    
    properties
        A_longitudinal;
        B_longitudinal;
        A_lateral;
        B_lateral
    end
    
    methods
        % Constructor
        function obj = AircraftStrategy(new_A_longitudinal, new_B_longitudinal,...
                                        new_A_lateral, new_B_lateral)
           obj.A_longitudinal = new_A_longitudinal;
           obj.B_longitudinal = new_B_longitudinal;
           obj.A_lateral = new_A_lateral;
           obj.B_lateral = new_B_lateral;
        end
    end
    
end

