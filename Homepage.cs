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
    public partial class Homepage : Form
    {
        public Homepage()
        {
            InitializeComponent();
        }


        static void Main()
        {
            Application.Run(new Homepage());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Infopage info = new Infopage();
            info.ShowDialog();
            this.Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Userinputpage inputpage = new Userinputpage();
            inputpage.ShowDialog();
            this.Close();

        }

        private void Homepage_Load(object sender, EventArgs e)
        {
            PictureBox planepic = new PictureBox();
                    }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
