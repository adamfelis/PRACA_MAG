function new_position = MoveAircraft( current_position, current_velocity, acceleration_vector,...
    pitch_angle, roll_angle, yaw_angle, time_span)
% current_velocity, acceleration_vector - those have to be prepared in order w,v,u  
            
            R_x = [1 0 0;...
                0 cos(roll_angle) -sin(roll_angle);...
                0 sin(roll_angle) cos(roll_angle)];
            
            R_y = [cos(pitch_angle) 0 sin(pitch_angle);...
                0 1 0;...
                -sin(pitch_angle) 0 cos(pitch_angle)];
            
            R_z = [cos(yaw_angle) -sin(yaw_angle) 0;...
                sin(yaw_angle) cos(yaw_angle) 0;...
                0 0 1];
            
            rotation_matrix = R_z * R_y * R_x;
            
            acceleration_vector_after_rotation = rotation_matrix * acceleration_vector';
            %gravity_vector = [-9.81; 0; 0];
            gravity_vector = [0; 0; 0];
            acceleration_vector_after_rotation_including_gravity = acceleration_vector_after_rotation + gravity_vector;
            
            tmpMovementVector = ...
                current_velocity' * time_span + ... %vt
                acceleration_vector_after_rotation_including_gravity * ...
                time_span * time_span / 2;%at^2/2
            new_position = current_position + [tmpMovementVector(3), tmpMovementVector(2), tmpMovementVector(1)];
end

