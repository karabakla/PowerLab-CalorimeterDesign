clear;
data1=csvread('10w_data.csv');
data2=csvread('20w_data.csv');
data3=csvread('40w_data.csv');
data4=csvread('61w_data.csv');
data5=csvread('10w_1.1w_data_3.csv');

L=0.45;%length in meters
W=0.35;%width in meters
H=0.2;%height in meters


dwall=7;%wall thickness in cm
Tcon=0.0837;%Spesific thermal conductivity of polystyrene in Watts/(Kelvin*meters)  
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

time1=data1(:,1)-5;
T1=data1(:,2)+273.15;
Tamb1=T1(1);%Ambient temp. in Kelvin
Tin01=T1(1);%Initial temp. inside box in Kelvin

time2=data2(:,1)-5;
T2=data2(:,2)+273.15;
Tamb2=T2(1);%Ambient temp. in Kelvin
Tin02=T2(1);%Initial temp. inside box in Kelvin


time3=data3(:,1)-5;
T3=data3(:,2)+273.15;
Tamb3=T3(1);%Ambient temp. in Kelvin
Tin03=T3(1);%Initial temp. inside box in Kelvin

time4=data4(:,1)-5;
T4=data4(:,2)+273.15;
Tamb4=T4(1);%Ambient temp. in Kelvin
Tin04=T4(1);%Initial temp. inside box in Kelvin

time5=data5(:,1);
T5=data5(:,2)+273.15;
Tamb5=T5(1);%Ambient temp. in Kelvin
Tin05=T5(1);%Initial temp. inside box in Kelvin

for i=2:1:length(T1)
    diff1(i)=(T1(i)-T1(i-1))/5;
    Ploss1(i)=Cp*Vol*(1.187*(T1(i)-T1(i-1))-(0.175/104)*(T1(i)^2-T1(i-1)^2))/(time1(i)-time1(i-1))+ (T1(i)-Tamb1)*(G1th+G2th+G3th);
end
Ploss1(1)=0;

for i=2:1:length(T2)
    diff2(i)=(T2(i)-T2(i-1))/5;
    Ploss2(i)=Cp*Vol*(1.187*(T2(i)-T2(i-1))-(0.175/104)*(T2(i)^2-T2(i-1)^2))/(60*(time2(i)-time2(i-1)))+ (T2(i)-Tamb2)*(G1th+G2th+G3th);
end
Ploss2(1)=0;

for i=2:1:length(T3)
    diff3(i)=(T3(i)-T3(i-1))/5;
    Ploss3(i)=Cp*Vol*(1.187*(T3(i)-T3(i-1))-(0.175/104)*(T3(i)^2-T3(i-1)^2))/(60*(time3(i)-time3(i-1)))+ (T3(i)-Tamb3)*(G1th+G2th+G3th);
end
Ploss3(1)=0;

for i=2:1:length(T4)
    diff4(i)=(T4(i)-T4(i-1))/5;
    Ploss4(i)=Cp*Vol*(1.187*(T4(i)-T4(i-1))-(0.175/104)*(T4(i)^2-T4(i-1)^2))/(60*(time4(i)-time4(i-1)))+ (T4(i)-Tamb4)*(G1th+G2th+G3th);
end

for i=2:1:length(T5)
    diff5(i)=(T5(i)-T5(i-1))/5;
    Ploss5(i)=Cp*Vol*(1.187*(T5(i)-T5(i-1))-(0.175/104)*(T5(i)^2-T5(i-1)^2))/(60*(time5(i)-time5(i-1)))+ (T5(i)-Tamb5)*(G1th+G2th+G3th);
end

maxTime = max(max(time1(length(time1)),time2((length(time2)))),max(time3((length(time3))),time4((length(time4)))));
Ploss1(1)=0;

diff1(1)=diff1(2);
diff5(1)=diff5(2);
diff2(1)=diff2(2);
diff4(1)=diff4(2);
diff3(1)=diff3(2);
subplot(2,1,1);
hold all;
plot(time1,Ploss1,'r-','LineWidth',1.5);
plot(time2,Ploss2,'b-','LineWidth',1.5);
plot(time3,Ploss3,'g-','LineWidth',1.5);
plot(time4,Ploss4,'m-','LineWidth',1.5);
%plot(time5,Ploss5,'c-','LineWidth',1.5);

x = [1 maxTime];
y = [61 61];
line(x,y,'Color','black','LineStyle','--');
y = [40 40];
line(x,y,'Color','black','LineStyle','--');
y = [20 20];
line(x,y,'Color','black','LineStyle','--');
%y = [11.1 11.1];
%line(x,y,'Color','black','LineStyle','--');
y = [10 10];
line(x,y,'Color','black','LineStyle','--');

legend('Ploss 10w','Ploss 20w','Ploss 40w','Ploss 61w');
%legend('Ploss 10w','Ploss 11.5w');
ylabel('Estimated Power Loss(W)','Fontweight','Bold');
xlabel('Time(m)','Fontweight','Bold');
hold off;
%grid on;

subplot(2,1,2);
hold all;
plot(time1,(abs(10-Ploss1)/10)*100,'r-','LineWidth',1);
plot(time2,abs(20-Ploss2)/20*100,'b-','LineWidth',1);
plot(time3,abs(40-Ploss3)/40*100,'g-','LineWidth',1);
plot(time4,abs(61-Ploss4)/61*100,'m-','LineWidth',1);
%plot(time5,abs(11.1-Ploss5)/11.1*100,'m-','LineWidth',1);
x = [1 maxTime];
y = [5 5];
line(x,y,'Color','black','LineStyle','--','LineWidth',2);
legend('Ploss 10w','Ploss 20w','Ploss 40w','Ploss 61w',' 5% Line');
%legend('Ploss 10w','Ploss 11.1w',' 5% Line');
ylabel('Error(%)','Fontweight','Bold');
xlabel('Time(m)','Fontweight','Bold');
hold off;
%grid on;