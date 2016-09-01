% 747 longitudinal axis example for 263
% 40000 feet steady, level flight, 774 ft/sec
% from bryson p151
% x = u, w, q, theta
A = [ -0.003 0.039 0 -.322;...
    -0.065 -0.319 7.74 0;...
    0.020 -.101 -.429 0;...
    0 0 1 0]
%input = u_w, w_w, delta_e, delta_t
Bw= -A(:,[1,2]); Bc= [0.01 1; -.18 -.04; -1.16 .598; 0 0]; 
B = [Bw, Bc]
% output: u, climb rate = -w + 7.74 theta
C = [ 1 0 0 0; 0 -1 0 7.74]; H0 = -C*inv(A)*B;
H01 = H0(:,[3,4]); % DC gain matrix from delta_e delta_t to
% speed, climb rate
% modal analysis
[V,Gam]=eig(A); xshort = real(V(:,1)); xphug = real(V(:,3)); 
xshort = xshort/norm(xshort); xphug = xphug/norm(xphug);
Nsamp = 100; %number of time samples
yshort=zeros(2,Nsamp); t=linspace(0,20,Nsamp); 
for i=1:Nsamp,
yshort(:,i)=C*expm(t(i)*A)*xshort;
end
figure(3)
subplot(2,1,1)
plot(t,yshort(1,:)');
axis([0 20 -1 1])
ylabel('u')
subplot(2,1,2)
plot(t,yshort(2,:)');
axis([0 20 -1 1])
ylabel('hdot')

print -deps aircraft_short
Nsamp = 400; %number of time samples
yshort=zeros(2,Nsamp); t=linspace(0,2000,Nsamp); 
for i=1:Nsamp,
yphug(:,i)=C*expm(t(i)*A)*xphug; 
end
figure(4)
subplot(2,1,1) 
plot(t,yphug(1,:)'); 
axis([0 2000 -2 2])
ylabel('u') 
subplot(2,1,2)
plot(t,yphug(2,:)'); 
axis([0 2000 -2 2])
ylabel('hdot')
print -deps aircraft_phug
% now do responses to various impulses
figure(1)
Nsamp = 200; %number of time samples
h1=zeros(2,Nsamp); h2=zeros(2,Nsamp); t=linspace(0,20,Nsamp);
for i=1:Nsamp,
h1(:,i)=C*expm(t(i)*A)*B(:,1); % impulse response from u_w
h2(:,i)=C*expm(t(i)*A)*B(:,2); % imp resp from v_w
end
subplot(2,2,1) 
plot(t,h1(1,:)');
axis([0 20 -.1 0.1]) 
ylabel('h11')
subplot(2,2,2) 
plot(t,h2(1,:)'); 
axis([0 20 -.1 0.1])
ylabel('h12')
subplot(2,2,3) 
plot(t,h1(2,:)'); 
axis([0 20 -.5 0.5])
ylabel('h21')
subplot(2,2,4) 
plot(t,h2(2,:)'); 
axis([0 20 -.5 0.5])
ylabel('h22')
print -deps aircraft_gust1
% now do same plots over longer time scale
figure(2); t=linspace(0,600,Nsamp); for i=1:Nsamp,
h1(:,i)=C*expm(t(i)*A)*B(:,1); % impulse response from u_w
h2(:,i)=C*expm(t(i)*A)*B(:,2); % imp resp from v_w
end
subplot(2,2,1)
plot(t,h1(1,:)'); 
axis([0 600 -.1 0.1]) 
ylabel('h11')
subplot(2,2,2) 
plot(t,h2(1,:)'); 
axis([0 600 -.1 0.1])
ylabel('h12')
subplot(2,2,3)
plot(t,h1(2,:)');
axis([0 600 -.5 0.5])
ylabel('h21')
subplot(2,2,4) 
plot(t,h2(2,:)'); 
axis([0 600 -.5 0.5]) 
ylabel('h22')
print -deps aircraft_gust2
% now do same things, but for actuator inputs
figure(1)

Nsamp = 200; %number of time samples
h1=zeros(2,Nsamp); h2=zeros(2,Nsamp); t=linspace(0,20,Nsamp);
for i=1:Nsamp,
h1(:,i)=C*expm(t(i)*A)*B(:,3); % impulse response from delta_e
h2(:,i)=C*expm(t(i)*A)*B(:,4); % imp resp from delta_t
end
subplot(2,2,1)
plot(t,h1(1,:)');
axis([0 20 -2 2]) 
ylabel('h11')
subplot(2,2,2) 
plot(t,h2(1,:)');
axis([0 20 -2 2]) 
ylabel('h12')
subplot(2,2,3)
plot(t,h1(2,:)'); 
axis([0 20 -5 5])
ylabel('h21')
subplot(2,2,4)
plot(t,h2(2,:)');
axis([0 20 0 3]) 
ylabel('h22')
print -deps aircraft_act1
% now do same plots over longer time scale
figure(2); t=linspace(0,600,Nsamp); 
for i=1:Nsamp,
h1(:,i)=C*expm(t(i)*A)*B(:,3); % impulse response from delta_e
h2(:,i)=C*expm(t(i)*A)*B(:,4); % imp resp from delta_t
end
subplot(2,2,1) 
plot(t,h1(1,:)'); 
axis([0 600 -2 2])
ylabel('h11')
subplot(2,2,2) 
plot(t,h2(1,:)');
axis([0 600 -2 2])
ylabel('h12')
subplot(2,2,3) 
plot(t,h1(2,:)'); 
axis([0 600 -3 3]) 
ylabel('h21')
subplot(2,2,4) 
plot(t,h2(2,:)');
axis([0 600 -3 3]) 
ylabel('h22')
print -deps aircraft_act2