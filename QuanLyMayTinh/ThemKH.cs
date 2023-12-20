using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QuanLyMayTinh
{
    public partial class ThemKH : Form
    {
        string ketnoi = "Server=DESKTOP-K0TD8GJ\\SQLEXPRESS;Database=QLMT;Trusted_Connection=True;";
        public ThemKH()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void button2_Click(object sender, EventArgs e)
        {
            // Lấy thông tin từ các controls
            string maKH = textBox1.Text;
            string tenKH = textBox2.Text;
            string diaChi = textBox3.Text;
            string sdt = textBox4.Text;
            string ghiChu = textBox5.Text;
            string gioiTinh = comboBox1.SelectedItem?.ToString();

            if (string.IsNullOrWhiteSpace(maKH) || string.IsNullOrWhiteSpace(tenKH) ||
                string.IsNullOrWhiteSpace(diaChi) || string.IsNullOrWhiteSpace(sdt) ||
                string.IsNullOrWhiteSpace(ghiChu) || string.IsNullOrWhiteSpace(gioiTinh))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin khách hàng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Không thực hiện thêm nhân viên nếu có thông tin bị thiếu
            }

            // Thực hiện câu lệnh INSERT vào cơ sở dữ liệu
            using (SqlConnection connection = new SqlConnection(ketnoi))
            {
                connection.Open();

                // Câu lệnh SQL INSERT
                string sql = "INSERT INTO KH (MaKH, TenKH, GioiTinh, DiaChi, Sdt, GhiChu) " +
                             "VALUES (@maKH, @tenKH, @gioiTinh, @diaChi, @sdt, @ghiChu)";

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
                        MessageBox.Show("Thêm khách hàng thành công.");
                    }
                    else
                    {
                        MessageBox.Show("Khách hàng chưa được thêm.");
                    }
                }
            }
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

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
