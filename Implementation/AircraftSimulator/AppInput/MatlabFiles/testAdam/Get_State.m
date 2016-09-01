result_from_matlab = [];
if(exist(strcat('../../AppOutput/', num2str(client_id), '.mat'), 'file') == 2) % if workspace exists
    load(strcat('../../AppOutput/', num2str(client_id)));
    result_from_matlab = aircraft.previous_result(0); 
end