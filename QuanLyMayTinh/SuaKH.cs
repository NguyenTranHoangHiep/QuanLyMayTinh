using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QuanLyMayTinh
{
    public partial class SuaKH : Form
    {
        string ketnoi = "Server=DESKTOP-K0TD8GJ\\SQLEXPRESS;Database=QLMT;Trusted_Connection=True;";
        private string maKH;
        private string tenKH;
        private string gioiTinh;
        private string diaChi;
        private string sdt;
        private string ghiChu;
        public SuaKH(string maKH,string tenKH,string gioiTinh,string diaChi,string sdt,string ghiChu)
        {
            InitializeComponent();
            this.maKH = maKH;
            this.tenKH = tenKH;
            this.gioiTinh = gioiTinh;
            this.diaChi = diaChi;
            this.sdt = sdt;
            this.ghiChu = ghiChu;
            TruyenDuLieu();
        }
        private void TruyenDuLieu()
        {
            textBox1.Text = maKH;
            textBox2.Text = tenKH;
            textBox3.Text = diaChi;
            textBox4.Text = sdt;
            textBox5.Text = ghiChu;
            comboBox1.SelectedItem = gioiTinh;
        }
        private void SuaKH_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string maKH = textBox1.Text;
            string tenKH = textBox2.Text;
            string diaChi = textBox3.Text;
            string sdt = textBox4.Text;
            string ghiChu = textBox5.Text;
            string gioiTinh = comboBox1.SelectedItem?.ToString();
            // Thực hiện câu lệnh INSERT vào cơ sở dữ liệu
            using (SqlConnection connection = new SqlConnection(ketnoi))
            {
                connection.Open();
                string sql = "UPDATE KH SET TenKH = @tenKH, GioiTinh = @gioiTinh, DiaChi = @diaChi, Sdt = @sdt, GhiChu = @ghiChu WHERE MaKH = @maKH";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    // Thêm tham số cho giá trị của các trường
                    command.Parameters.AddWithValue("@maKH", maKH);
                    command.Parameters.AddWithValue("@tenKH", tenKH);
                    command.Parameters.AddWithValue("@gioiTinh", gioiTinh);
                    command.Parameters.AddWithValue("@diaChi", diaChi);
                    command.Parameters.AddWithValue("@sdt", sdt);
                    command.Parameters.AddWithValue("@ghiChu", ghiChu);

                    // Thực hiện câu lệnh INSERT
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Cập nhật khách hàng thành công.");
                    }
                    else
                    {
                        MessageBox.Show("Khách hàng chưa được cập nhật.");
                    }
                }
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
