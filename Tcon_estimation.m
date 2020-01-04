clear;
data1=xlsread('10w_data.csv');
data2=xlsread('20w_data.csv');
data3=xlsread('40w_data.csv');
data4=csvread('61w_data.csv');
time1=data1(:,1)-5;
T1=data1(:,2);

time2=data2(:,1)-5;
T2=data2(:,2);

time3=data3(:,1)-5;
T3=data3(:,2);

time4=data4(:,1)-5;
T4=data4(:,2);

L=0.45;%length in meters
W=0.35;%width in meters
H=0.2;%height in meters
Tamb1=T1(1);%Ambient temp. in Kelvin
Tin01=T1(1);%Initial temp. inside box in Kelvin

Tamb2=T2(1);%Ambient temp. in Kelvin
Tin02=T2(1);%Initial temp. inside box in Kelvin

Tamb3=T3(1);%Ambient temp. in Kelvin
Tin03=T3(1);%Initial temp. inside box in Kelvin

Tamb4=T4(1);%Ambient temp. in Kelvin
Tin04=T4(1);%Initial temp. inside box in Kelvin

dwall=7;%wall thickness in cm
%Tcon=0.075;%Spesific thermal conductivity of polystyrene in Watts/(Kelvin*meters)  
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

G1th=100*A1/dwall;%constant for face A1&A2
G2th=100*A2/dwall;%constant for face A1&A2
G3th=100*A3/dwall;%constant for face A1&A2

Ploss1=10;
Ploss2=20;
Ploss3=40;
Ploss4=61;

Tcon1= Ploss1/((T1(length(T1))-Tamb1)*(G1th+G2th+G3th));
Tcon2= Ploss2/((T2(length(T2))-Tamb2)*(G1th+G2th+G3th));
Tcon3= Ploss3/((T3(length(T3))-Tamb3)*(G1th+G2th+G3th));
Tcon4= Ploss4/((T4(length(T4))-Tamb4)*(G1th+G2th+G3th));
Tconavg=(Tcon1+Tcon2+Tcon3+Tcon4)/4
