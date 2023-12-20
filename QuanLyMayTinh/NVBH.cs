using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyMayTinh
{
    public partial class NVBH : Form
    {
        public NVBH()
        {
            InitializeComponent();
        }
        private Form chill;
        private void openchill(Form childen)
        {
            if (chill != null)
            {
                chill.Close();
            }
            chill = childen;
            childen.TopLevel = false;
            childen.FormBorderStyle = FormBorderStyle.None;
            childen.Dock = DockStyle.Fill;
            panel2.Controls.Add(childen);
            panel2.Tag = childen;
            childen.BringToFront();
            childen.Show();
        }
        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            openchill(new NVBH_QLKH());
            label2.Text = button3.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openchill(new QLHD());
            label2.Text = button1.Text;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            label2.Text = "Home";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openchill(new ThanhToan());
            label2.Text = button2.Text;
        }

        private void NVBH_Load(object sender, EventArgs e)
        {

        }
    }
}
