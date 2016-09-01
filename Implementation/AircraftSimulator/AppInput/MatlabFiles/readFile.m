filename = 'input.txt';
fileID = fopen('filename','r');
fprintf(fileID,'%f %f\n',x,y)
x
y
fclose(fileID);