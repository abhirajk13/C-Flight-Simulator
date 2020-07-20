using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Glider_Simulation_V1
{
    public partial class Userinputpage : Form
    {
        private Userinput userinputData;
        private bool testingmode;

        public Userinputpage()
        {
            userinputData = new Userinput();
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (testingmode == true)
            {

                testingmode = false;
                MessageBox.Show("Testing Mode Disactivated");
            }
            else
            {
                testingmode = true;
                MessageBox.Show("Testing Mode Active");
            }
            
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (testingmode == false)
            {
                if (string.IsNullOrWhiteSpace(textBox8.Text) || string.IsNullOrWhiteSpace(textBox14.Text) || string.IsNullOrWhiteSpace(textBox13.Text)
             || string.IsNullOrWhiteSpace(textBox12.Text) || string.IsNullOrWhiteSpace(textBox10.Text) || string.IsNullOrWhiteSpace(textBox9.Text)
             || string.IsNullOrWhiteSpace(textBox16.Text) || string.IsNullOrWhiteSpace(textBox18.Text))
                {
                    MessageBox.Show("Please Enter All Values or Choose Pre-Set Parameters!");
                    
                }

                else if ((float)Convert.ToDouble(textBox8.Text) < 30 || (float)Convert.ToDouble(textBox8.Text) > 400)
                {
                    MessageBox.Show("Please Enter Value For Mass Between 30 and 400");
                }
                else if((float)Convert.ToDouble(textBox14.Text) < 0.5 || (float)Convert.ToDouble(textBox14.Text) > 2)
                {
                    MessageBox.Show("Please Enter Value For Drag Coefficient between 0.5 and 2");
                }
                else if((float)Convert.ToDouble(textBox13.Text) < 10 || (float)Convert.ToDouble(textBox13.Text) > 150)
                {
                    MessageBox.Show("Please Enter Value For Launch Height Between 10 and 150");
                }
                else if((float)Convert.ToDouble(textBox12.Text) < 100 || (float)Convert.ToDouble(textBox12.Text) > 400)
                {
                    MessageBox.Show("Please Enter Value For Temperature Between 100.00K and 400.00K");
                }
                else if((float)Convert.ToDouble(textBox10.Text) < 10 || (float)Convert.ToDouble(textBox10.Text) > 80)
                {
                    MessageBox.Show("Please Enter Value For Initial Velocity Between 10 and 80");
                }
                else if((float)Convert.ToDouble(textBox16.Text) < 0 || (float)Convert.ToDouble(textBox16.Text) > 80)
                {
                    MessageBox.Show("Please Enter Value For Thrust Force Between 0 (no thrust) and 80");
                }
                else if((float)Convert.ToDouble(textBox9.Text) < 3500 || (float)Convert.ToDouble(textBox9.Text) > 8000)
                {
                    MessageBox.Show("Please Enter Value For Cross-Sectional Area Between 3500 and 8000");
                }
                else if((float)Convert.ToDouble(textBox18.Text) < 0 || (float)Convert.ToDouble(textBox18.Text) > 50)
                {
                    MessageBox.Show("Please Enter Value For Fuel Mass (fuel amount) Between 0 (no fuel) and 50");
                }
                else
                {

                    userinputData.glidermass = (float)Convert.ToDouble(textBox8.Text);
                    userinputData.dragcoef = (float)Convert.ToDouble(textBox14.Text);
                    userinputData.launchheight = (float)Convert.ToDouble(textBox13.Text);
                    userinputData.temperature = (float)Convert.ToDouble(textBox12.Text);
                    userinputData.initialvelocity = (float)Convert.ToDouble(textBox10.Text);
                    userinputData.crosssectionalarea = (float)Convert.ToDouble(textBox9.Text);
                    userinputData.thrust = (float)Convert.ToDouble(textBox16.Text);
                    userinputData.fuel = (float)Convert.ToDouble(textBox18.Text);

                    MessageBox.Show("All Correct Values Loaded - Simulation Started");

                    System.Threading.Thread thStartGame = new System.Threading.Thread(StartGame);
                    thStartGame.Start();
                }
                
            }
            else
            {
                userinputData.glidermass = (float)Convert.ToDouble(textBox8.Text);
                userinputData.dragcoef = (float)Convert.ToDouble(textBox14.Text);
                userinputData.launchheight = (float)Convert.ToDouble(textBox13.Text);
                userinputData.temperature = (float)Convert.ToDouble(textBox12.Text);
                userinputData.initialvelocity = (float)Convert.ToDouble(textBox10.Text);
                userinputData.crosssectionalarea = (float)Convert.ToDouble(textBox9.Text);
                userinputData.thrust = (float)Convert.ToDouble(textBox16.Text);
                userinputData.fuel = (float)Convert.ToDouble(textBox18.Text);



                MessageBox.Show("All Correct Values Loaded - Simulation Started");

                System.Threading.Thread thStartGame = new System.Threading.Thread(StartGame);
                thStartGame.Start();


            }

           


        }
        public void StartGame()
        {
            // create object of user input class
            // pass user input class of data to Game1
         

            Game1 game = new Game1(userinputData);
            game.Run();
            

        }
        private void textBox8_TextChanged(object sender, EventArgs e)
        {
        }
        private void textBox14_TextChanged(object sender, EventArgs e)
        {
        }
        private void textBox13_TextChanged(object sender, EventArgs e)
        {
        }
        private void textBox12_TextChanged(object sender, EventArgs e)
        {
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text.Contains("Glaser"))
            {
                textBox8.Text = Convert.ToString(200);
                textBox14.Text = Convert.ToString(0.75);
                textBox13.Text = Convert.ToString(100);
                textBox12.Text = Convert.ToString(288.15);
                textBox10.Text = Convert.ToString(50);
                textBox9.Text = Convert.ToString(6400);
                textBox16.Text = Convert.ToString(50);
            }
            else if (comboBox1.Text.Contains("Otto"))
            {
                textBox8.Text = Convert.ToString(260);
                textBox14.Text = Convert.ToString(0.90);
                textBox13.Text = Convert.ToString(100);
                textBox12.Text = Convert.ToString(288.15);
                textBox10.Text = Convert.ToString(50);
                textBox9.Text = Convert.ToString(6800);
                textBox16.Text = Convert.ToString(50);
            }
            else if (comboBox1.Text.Contains("Ultralight"))
            {
                textBox8.Text = Convert.ToString(100);
                textBox14.Text = Convert.ToString(0.6);
                textBox13.Text = Convert.ToString(100);
                textBox12.Text = Convert.ToString(288.15);
                textBox10.Text = Convert.ToString(50);
                textBox9.Text = Convert.ToString(5000);
                textBox16.Text = Convert.ToString(50);
            }
            else if (comboBox1.Text.Contains("Waco"))
            {
                textBox8.Text = Convert.ToString(250);
                textBox14.Text = Convert.ToString(0.8);
                textBox13.Text = Convert.ToString(100);
                textBox12.Text = Convert.ToString(288.15);
                textBox10.Text = Convert.ToString(50);
                textBox9.Text = Convert.ToString(6700);
                textBox16.Text = Convert.ToString(50);
            }
            else 
            {
                textBox8.Text = Convert.ToString(300);
                textBox14.Text = Convert.ToString(0.83);
                textBox13.Text = Convert.ToString(100);
                textBox12.Text = Convert.ToString(288.15);
                textBox10.Text = Convert.ToString(50);
                textBox9.Text = Convert.ToString(7000);
                textBox16.Text = Convert.ToString(50);
            }




        }
        private void textBox10_TextChanged(object sender, EventArgs e)
        {
        }
        private void textBox9_TextChanged(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {

            textBox8.Text = Convert.ToString(200);
            textBox14.Text = Convert.ToString(0.75);
            textBox13.Text = Convert.ToString(100);
            textBox12.Text = Convert.ToString(288.15);
            textBox10.Text = Convert.ToString(50);
            textBox9.Text = Convert.ToString(6400);
            textBox16.Text = Convert.ToString(50);
            textBox18.Text = Convert.ToString(20);

          



        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Homepage homepage = new Homepage();
            homepage.ShowDialog();
            this.Close();
        }

        
        


    }

        
    }


