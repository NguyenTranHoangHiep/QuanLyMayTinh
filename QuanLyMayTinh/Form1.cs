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

namespace QuanLyMayTinh
{
    public partial class Form1 : Form
    {
        string ketnoi = "Server=DESKTOP-K0TD8GJ\\SQLEXPRESS;Database=QLMT;Trusted_Connection=True;";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "Tài khoản")
            {
                textBox1.Text = "";
                textBox1.ForeColor= Color.Black;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "Tài khoản";
                textBox1.ForeColor= Color.Silver;
            }
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (textBox2.Text == "Mật khẩu")
            {
                textBox2.PasswordChar= '*';
                textBox2.Text = "";
                textBox2.ForeColor = Color.Black;
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                textBox2.Text = "Mật khẩu";
                textBox2.ForeColor = Color.Silver;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                textBox2.PasswordChar = '\0'; // Đặt PasswordChar thành ký tự null để không che dấu ký tự
            }
            else
            {
                textBox2.PasswordChar = '*';
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;

            using (SqlConnection connection = new SqlConnection(ketnoi))
            {
                try
                {
                    connection.Open();

                    // Kiểm tra tài khoản trong bảng QuanLy
                    using (SqlCommand quanLyCommand = new SqlCommand("SELECT 'QuanLy' AS LoaiTaiKhoan FROM QuanLy WHERE Tk = @Username AND Mk = @Password", connection))
                    {
                        quanLyCommand.Parameters.AddWithValue("@Username", username);
                        quanLyCommand.Parameters.AddWithValue("@Password", password);

                        object loaiTaiKhoanQuanLy = quanLyCommand.ExecuteScalar();

                        if (loaiTaiKhoanQuanLy != null)
                        {
                            MessageBox.Show("Đăng nhập thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Mở form QuanLy tương ứng.
                            Form3 formQuanLy = new Form3();
                            formQuanLy.Show();
                            return; // Kết thúc phương thức để không kiểm tra tiếp.
                        }
                    }

                    // Kiểm tra tài khoản trong bảng NV
                    using (SqlCommand nvCommand = new SqlCommand("SELECT LoaiNV FROM NV WHERE Tk = @Username AND Mk = @Password", connection))
                    {
                        nvCommand.Parameters.AddWithValue("@Username", username);
                        nvCommand.Parameters.AddWithValue("@Password", password);

                        object loaiNV = nvCommand.ExecuteScalar();

                        if (loaiNV != null)
                        {
                            MessageBox.Show("Đăng nhập thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Mở form tương ứng dựa trên giá trị LoaiNV.
                            string loaiNVStr = loaiNV.ToString();
                            if (loaiNVStr == "Thủ kho")
                            {
                                ThuKho formThuKho = new ThuKho();
                                formThuKho.Show();
                            }
                            else if (loaiNVStr == "Nhân viên")
                            {
                                NVBH formNhanVienBanHang = new NVBH();
                                formNhanVienBanHang.Show();
                            }
                            // Các điều kiện khác nếu cần.
                        }
                        else
                        {
                            MessageBox.Show("Đăng nhập thất bại. Vui lòng kiểm tra lại tài khoản và mật khẩu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
