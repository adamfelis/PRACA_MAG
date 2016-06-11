classdef MissileStrategy < handle & Strategy
    %Describes one strategy of the missile
    
    properties
        F
        G
        missilePosition
        timeForPrediction
        
        missilePositions = []
    end
    
    methods
        function obj = MissileStrategy(missilePos)
            obj.missilePosition = [0 0 0, missilePos];
            obj.timeForPrediction = 0:0.2:10;
            obj.F = [zeros(3,3), eye(3);zeros(3,6)];
            obj.G = [zeros(3,3);eye(3)];
            C = [eye(3), zeros(3); zeros(3,6)];
            p = 10;
            Q = p * (C' * C);
            R = eye(3);
            K = lqr(obj.F,obj.G,Q,R);
            Nbar = 3.125;
            
            obj.F = obj.F - obj.G * K;
            obj.G = obj.G * Nbar;
        end
        
        function deltaPos = SimulateMissileFlight(obj, aircraftPosition)
            u = [aircraftPosition(1) aircraftPosition(2) aircraftPosition(3)]';
            x0 = obj.missilePosition';
            [~, Y] = ode45(@(t,x)StateSpace(t,x,obj.F ,obj.G ,u), obj.timeForPrediction, x0);
            obj.missilePosition = Y(2,:);
            deltaPos = obj.missilePosition;
            obj.missilePositions = [obj.missilePositions; deltaPos];
        end
    end
    
end

