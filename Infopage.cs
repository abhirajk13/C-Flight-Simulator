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
    public partial class Infopage : Form
    {
        public Infopage()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Text = "Pressure (symbol: p or P) is the force applied perpendicular to the surface of an object per unit area over which that force is distributed. Gauge pressure (also spelled gage pressure) is the pressure relative to the ambient pressure. Various units are used to express pressure.";
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox2.Text = "Density is a measurement that compares the amount of matter an object has to its volume. An object with much matter in a certain volume has high density. An object with little matter in the same amount of volume has a low density. Density is found by dividing the mass of an object by its volume.";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox2.Text = "The acceleration which is gained by an object because of the gravitational force is called its acceleration due to gravity. ... The formula is 'the change in velocity= gravity x time' The acceleration due to gravity at the surface of Earth is represented as g. It has a standard value defined as 9.80665 m/s2. Formula: g = G*M/R^2";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox2.Text = "The lift equation states that lift L is equal to the lift coefficient Cl times the density r times half of the velocity V squared times the wing area A. ... The combination of terms 'density times the square of the velocity divided by two' is called the dynamic pressure and appears in Bernoulli's pressure equation.";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox2.Text = "The drag equation states that drag D is equal to the drag coefficient Cd times the density r times half of the velocity V squared times the reference area A. ... Notice that the area (A) given in the drag equation is given as a reference area. The drag depends directly on the size of the body.";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Hide();
            Homepage homepage = new Homepage();
            homepage.ShowDialog();
            this.Close();
        }
    }
}
