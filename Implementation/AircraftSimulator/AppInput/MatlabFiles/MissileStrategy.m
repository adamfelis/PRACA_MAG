classdef MissileStrategy < handle & Strategy
    %Describes one strategy of the missile
    
    properties
        F
        G
        K
        missileState
        timeForPrediction
        simulation_step_from_fixed_update
        
        missileStates = []
    end
    
    methods
        function obj = MissileStrategy(missilePos, simulation_step)
            
            obj.simulation_step_from_fixed_update = simulation_step;
           
            obj.missileState = [missilePos, 0 0 0];
            obj.timeForPrediction = 0:0.2:10;
            obj.F = [zeros(3,3), eye(3);zeros(3,6)];
            obj.G = [zeros(3,3);eye(3)];
            C = [eye(3), zeros(3); zeros(3,6)];
            p = 10;
            Q = p * (C' * C);
            R = eye(3);
            obj.K = lqr(obj.F,obj.G,Q,R);
            Nbar = 1;%3.125;
            
            obj.F = obj.F - obj.G * obj.K;
            obj.G = obj.G * Nbar;
        end
        
        function deltaPos = SimulateMissileFlight(obj, aircraftPosition, aircraftVelocity)%aircraftVelocity is wvu
            
            %normalisedAircraftVelocity = aircraftVelocity ./ norm(aircraftVelocity);
            
            predictedAircraftPosition = aircraftPosition + [aircraftVelocity(3), aircraftVelocity(2), aircraftVelocity(1)] * obj.simulation_step_from_fixed_update;
            
            u = [predictedAircraftPosition(1) predictedAircraftPosition(2) predictedAircraftPosition(3)]';
            x0 = obj.missileState';
            [~, Y] = ode45(@(t,x)StateSpace(t,x,obj.F ,obj.G ,u), obj.timeForPrediction, x0);
            obj.missileState = Y(2,:);
            
%             factor = 4;
%             maxVelocity = [aircraftVelocity(3), aircraftVelocity(2), aircraftVelocity(1)] * factor;
%             minVelocity = [aircraftVelocity(3), aircraftVelocity(2), aircraftVelocity(1)];
%             
%             obj.missileState(4) = min(obj.missileState(4), maxVelocity(1));
%             %obj.missileState(4) = max(obj.missileState(4), minVelocity(1));
%             obj.missileState(5) = min(obj.missileState(5), maxVelocity(2));
%             %obj.missileState(5) = max(obj.missileState(5), minVelocity(2));
%             obj.missileState(6) = min(obj.missileState(6), maxVelocity(3));
%             %obj.missileState(6) = max(obj.missileState(6), minVelocity(3));
            
%             copy_of_missile_state = obj.missileState;
            
%             obj.missileState = obj.K * obj.missileState';
            
            obj.missileState = [(obj.K * obj.missileState')', obj.missileState(4), obj.missileState(5), obj.missileState(6)];
            deltaPos = obj.missileState;
            obj.missileStates = [obj.missileStates; deltaPos, predictedAircraftPosition(1), aircraftPosition(1)];
        end
    end
    
end

