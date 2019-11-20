Ploss=10;%Watt
L=0.5;%length in meters
W=0.5;%width in meters
H=0.5;%height in meters
Tamb=298;%Ambient temp. in Kelvin
Tin0=298;%Initial temp. inside box in Kelvin
dwall=5;%wall thickness in cm
Tcon=0.035;%Spesific thermal conductivity in Watts/(Kelvin*meters)  
Cv=720;%Spesific constant volume heat capacity j/(kg*K)

time=0:0.001:1;%working time in minutes

%Ploss=Vol*(1.187-(0.175/52)*T)dT/dt+sum((T-Tamb)Tcon*A/dwall)

%Ploss*t=Vol(1.187(T-Tin0)-(0.175/104)*T^2)+sum((T-Tamb)*Tcon*A/dwall*t)

%Calculations
Vol=L*W*H;

A1=2*H*L;  %front and back face area
A2=2*H*W;  %left and right face area
A3=2*L*W;  %top and bottom face area

G1th=100*Tcon*A1/dwall;%constant for face A1&A2
G2th=100*Tcon*A2/dwall;%constant for face A1&A2
G3th=100*Tcon*A3/dwall;%constant for face A1&A2

%AT^2+BT+C     quadratic equation

A=-Cv*Vol*0.175/104;
B=1.187*Vol*Cv+time*60*(G1th+G2th+G3th);
C=Cv*Vol*0.175/104*Tin0^2-Tamb*60*time*(G1th+G2th+G3th)-1.187*Cv*Vol*Tin0-Ploss*time*60;

delta=B.^2-4*A*C;

T1=(-B+sqrt(abs(delta)))/(2*A);
T2=(-B-sqrt(abs(delta)))/(2*A);

plot(time*60,T1-273);
ylabel("Temp(C) in the Box");
xlabel("Time(s)");
