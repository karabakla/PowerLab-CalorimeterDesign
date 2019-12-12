data=xlsread('20wData.xlsx');
%data=xlsread('50wData.xlsx');
%data=xlsread('40w_data.csv');
time=data(:,1);
T=data(:,2)+273.13;

L=0.45;%length in meters
W=0.35;%width in meters
H=0.2;%height in meters
Tamb=T(1);%Ambient temp. in Kelvin
Tin0=T(1);%Initial temp. inside box in Kelvin
dwall=7;%wall thickness in cm
Tcon=0.078733;%Spesific thermal conductivity of polystyrene in Watts/(Kelvin*meters)  
%Cp=210000;%Spesific constant volume heat capacity j/(kg*K)
Cp=1000;
%time=1:1:101;%working time in minutes

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


for i=2:1:length(T)
Ploss(i)=Cp*Vol*(1.187*(T(i)-Tin0)-(0.175/104)*(T(i)^2-Tin0^2))/(time(i))+ (T(i)-Tamb)*(G1th+G2th+G3th);
end
Ploss(1)=0;
plot(time,Ploss);
