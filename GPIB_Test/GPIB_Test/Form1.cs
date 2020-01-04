using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace GPIB_Test
{
    public partial class Form1 : Form
    {

        bool stat = false;
        UInt32 totalmes = 0,prg_mes=0;
        AbortableBackgroundWorker my_bg = new AbortableBackgroundWorker();

        Series series = new Series("T vs. Power Loss");
        public Form1()
        {
            InitializeComponent();
            //MessageBox.Show(Application.ExecutablePath);
            //    label1.Text = Convert.ToDouble("+4.8888888E+01",new CultureInfo("en-US")).ToString();
            series.ChartType = SeriesChartType.Line;
            chart1.Series.Add(series);
            chart1.Legends[0].Position = new ElementPosition(0, 0, 100, 10);
            chart1.ChartAreas[0].AxisX.Interval = 5;
            chart1.ChartAreas[0].AxisY.Interval = 2;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.DashDotDot;
            series.BorderWidth = 2;
            checkBox1.Checked = true;


        }
        StreamWriter sw;
        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = !timer1.Enabled;
            
            stat = !stat;
            //timer1.Enabled = stat;
            numericUpDown1.Enabled = !stat;
            numericUpDown2.Enabled = !stat;
            textBox1.Enabled = !stat&&!checkBox1.Checked;
            textBox2.Enabled = !stat&&!checkBox1.Checked;
            if (stat)
            {
                sw=new StreamWriter("Results"+ prg_mes+++".csv");
                totalmes = 0;
                button1.Text = "Stop";
                my_bg = new AbortableBackgroundWorker();
                my_bg.DoWork += backgroundWorker1_DoWork;
                my_bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(my_bg_completed);
                my_bg.RunWorkerAsync();
            }
            else
            {
                try
                {
                    sw.Close();
                    sw.Dispose();
                    my_bg.CancelAsync();
                    my_bg.Abort();
                    my_bg.Dispose();
                }
                catch { }
                button1.Text = "Start";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        { 
         
            //Convert.ToDouble("+4.8888888E+01", new CultureInfo("en-US")).ToString();
            // MessageBox.Show(strRead);
            //label1.Text = Convert.ToDouble(strRead, new CultureInfo("en-US")) +"C";
        }
        double calc_Ploss(double T,double time,double Tamb, double Tin0)
        {
            double L = 0.45;// length in meters
            double W = 0.35;// width in meters
            double H = 0.2; // height in meters
            //double Tamb = 0;// Ambient temp. in Kelvin
            //double Tin0 = 0;// Initial temp.inside box in Kelvin
            double dwall = 7;// wall thickness in cm
            double Tcon = 0.0837;// Spesific thermal conductivity of polystyrene in Watts / (Kelvin * meters)
            double Cp = 1000;
            double Vol = L * W * H;

            double A1 = 2 * H * L;  // front and back face area
            double A2 = 2 * H * W;  // left and right face area
            double A3 = 2 * L * W;  // top and bottom face area

            double G1th = 100 * Tcon * A1 / dwall;// constant for face A1&A2
            double G2th = 100 * Tcon * A2 / dwall;// constant for face A1&A2
            double G3th = 100 * Tcon * A3 / dwall;// constant for face A1&A2
            //MessageBox.Show(Tamb+"");
            return Cp * Vol * (1.187 * (T - Tin0) - (0.175 / 104) * (Math.Pow(T, 2) - Math.Pow(Tin0, 2))) / (time) + (T - Tamb) * (G1th + G2th + G3th);
        }

        double readTemp()
        {
            string strWrite, strRead;
            int status;
            int len;

            //Select a GPIB interface card
            int GPIBoard = 0;
            int GPIB_addr = 19;
            //Select a GPIB interface card
            IEEE.boardselect(GPIBoard);
            if (IEEE.gpib_board_present() == 1)
            {
                //Open and initialize the GPIB interface card
                IEEE.initialize(GPIBoard, 0);
            }
            else
            {
                MessageBox.Show("Invalid GPIB interface card.");
                return 0;
            }

            strWrite = "MEASURE?";
            //Write a string to the instrument with the specific address
            IEEE.send(GPIB_addr, strWrite, out status);
            if (status != 0)
            {
                MessageBox.Show("Error in writing the string command to the GPIB instrument.");
                return 0;
            }

            //Read the response string from the instrument with specific address
            IEEE.enter(out strRead, 100, out len, GPIB_addr, out status);
            if (status != 0)
            {
                MessageBox.Show("Error in reading the response string from the GPIB instrument.");
                return 0;
            }
            strRead = strRead.Substring(0, strRead.IndexOf('C') - 1);
            //MessageBox.Show(new TimeSpan(prev.Ticks- DateTime.Now.Ticks).TotalSeconds.ToString());
            return (Convert.ToDouble(strRead, new CultureInfo("en-US")));
        }

        //int temp;
        double Tin0 = 0, Tamb = 0;
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            DateTime prev;
            while (totalmes<numericUpDown1.Value)
            {
                //Declare variables
                prev = DateTime.Now;
                /* string strWrite, strRead;
                 int status;
                 int len;

                 //Select a GPIB interface card
                 int GPIBoard = 0;
                 int GPIB_addr = 19;
                 //Select a GPIB interface card
                 IEEE.boardselect(GPIBoard);
                 if (IEEE.gpib_board_present() == 1)
                 {
                     //Open and initialize the GPIB interface card
                     IEEE.initialize(GPIBoard, 0);
                 }
                 else
                 {
                     MessageBox.Show("Invalid GPIB interface card.");
                     return;
                 }

                 strWrite = "MEASURE?";
                 //Write a string to the instrument with the specific address
                 IEEE.send(GPIB_addr, strWrite, out status);
                 if (status != 0)
                 {
                     MessageBox.Show("Error in writing the string command to the GPIB instrument.");
                     return;
                 }

                 //Read the response string from the instrument with specific address
                 IEEE.enter(out strRead, 100, out len, GPIB_addr, out status);
                 if (status != 0)
                 {
                     MessageBox.Show("Error in reading the response string from the GPIB instrument.");
                     return;
                 }
                 strRead = strRead.Substring(0, strRead.IndexOf('C') - 1);
                 //MessageBox.Show(new TimeSpan(prev.Ticks- DateTime.Now.Ticks).TotalSeconds.ToString());
                 temp = (int)(Convert.ToDouble(strRead, new CultureInfo("en-US")) * 100);*/
            
                double temp_d =0;

                for (int i = 0; i < 5; i++)
                {
                    temp_d += readTemp();
                    Thread.Sleep(100);
                }
                temp_d /= 5.0;
                //temp = (int)temp_d*100;
                    
                double time = (double)(totalmes * numericUpDown2.Value * 60);
                if (checkBox1.Checked)
                {
                    if (Tamb==0)
                    {
                        Tamb = Tin0 = temp_d;
                        textBox2.Text = textBox1.Text = Tamb.ToString("N2");
                    }
                }
                else
                {
                    Tamb = Convert.ToDouble(textBox1.Text);
                    Tin0 = Convert.ToDouble(textBox2.Text);
                }
                totalmes++;
                double powerloss = calc_Ploss(temp_d + 273.15, time, Tamb + 273.15, Tin0 + 273.15);
                label8.Invoke((MethodInvoker)(() => label8.Text = powerloss.ToString("N2") +" W"));
                chart1.Invoke((MethodInvoker)(() => chart1.Series[0].Points.AddXY(time/60.0,powerloss)));

                label4.Invoke((MethodInvoker)(() => label4.Text = totalmes.ToString()));
                label1.Invoke((MethodInvoker)(() => label1.Text = (temp_d.ToString("N2")) + " C"));



                double wait =(new TimeSpan(prev.Ticks - DateTime.Now.Ticks)).TotalMinutes;
                sw.WriteLine((totalmes-1) * numericUpDown2.Value + ";" + temp_d.ToString("N2") );
                while (wait < (double)numericUpDown2.Value)
                {
                    wait = (new TimeSpan(DateTime.Now.Ticks - prev.Ticks)).TotalMinutes;
                    label12.Invoke((MethodInvoker)(() => label12.Text = (5-wait).ToString("N2")));
                    Thread.Sleep(100);
                }
                
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = !checkBox1.Checked;
            textBox2.Enabled = !checkBox1.Checked;
        }

        void my_bg_completed(object sender,RunWorkerCompletedEventArgs e)
        {
            // button1.PerformClick();

            timer1.Enabled =false;

            stat = false;
            //timer1.Enabled = stat;
            numericUpDown1.Enabled = !stat;
            numericUpDown2.Enabled = !stat;
            textBox1.Enabled = !stat && !checkBox1.Checked;
            textBox2.Enabled = !stat && !checkBox1.Checked;

            try
            {
                sw.Close();
                sw.Dispose();
                my_bg.CancelAsync();
                my_bg.Abort();
                my_bg.Dispose();
            }
            catch { }
            button1.Text = "Start";
        }
    }
}
