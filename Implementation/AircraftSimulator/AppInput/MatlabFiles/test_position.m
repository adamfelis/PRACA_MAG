missilePositions = aircraft_collection.Instances(1).Missiles(1).Strategies(1).missilePositions;
shooterPositions = aircraft_collection.Instances(2).current_simulation_solutions;
targetPositions = aircraft_collection.Instances(1).current_simulation_solutions;

figure(1)
hold on;
grid on;
axis equal;

time = 0:0.2: 0.2 * length(targetPositions);

for i = 1 : 1 : length(time)
    plot(targetPositions(i, 1), targetPositions(i, 3),'ro');
    %plot(shooterPositions(i, 1), shooterPositions(i, 3), 'o');
    
    if i >= 66 && i < 66 + length(missilePositions)
        plot(missilePositions(i - 65, 1), missilePositions(i-65, 3), 'bo');
    end
    pause(0.2);
end

legend on;