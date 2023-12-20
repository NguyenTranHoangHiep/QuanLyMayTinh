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

    public partial class ThêmNK : Form
    {
        string ketnoi = "Server=DESKTOP-K0TD8GJ\\SQLEXPRESS;Database=QLMT;Trusted_Connection=True;";
        public ThêmNK()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Lấy thông tin từ các controls
            string maNhap = textBox1.Text;
            string maSP = textBox2.Text;
            string maNV = textBox6.Text;
            string ngayNhap = dateTimePicker1.Text;
            string soLuongNhap = textBox4.Text;
            string giaNhap = textBox5.Text;

            if (string.IsNullOrWhiteSpace(maNhap) || string.IsNullOrWhiteSpace(maSP) ||
                string.IsNullOrWhiteSpace(maNV) || string.IsNullOrWhiteSpace(ngayNhap) ||
                string.IsNullOrWhiteSpace(soLuongNhap) || string.IsNullOrWhiteSpace(giaNhap))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin nhập kho.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Không thực hiện thêm nhân viên nếu có thông tin bị thiếu
            }

            // Thực hiện câu lệnh INSERT vào cơ sở dữ liệu
            using (SqlConnection connection = new SqlConnection(ketnoi))
            {
                connection.Open();

                // Câu lệnh SQL INSERT
                string sql = "INSERT INTO NhapKho (MaPhieuNhap, MaSP, MaNV, NgayNhap, SoLuongNhap, GiaNhap) " +
                             "VALUES (@maNhap, @maSP, @maNV, @ngayNhap, @soLuongNhap, @giaNhap)";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    // Thêm tham số cho giá trị của các trường
                    command.Parameters.AddWithValue("@maNhap", maNhap);
                    command.Parameters.AddWithValue("@maSP", maSP);
                    command.Parameters.AddWithValue("@maNV", maNV);
                    command.Parameters.AddWithValue("@ngayNhap", ngayNhap);
                    command.Parameters.AddWithValue("@soLuongNhap", soLuongNhap);
                    command.Parameters.AddWithValue("@giaNhap", giaNhap);

                    // Thực hiện câu lệnh INSERT
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Thêm nhập kho thành công.");
                    }
                    else
                    {
                        MessageBox.Show("Nhập kho chưa được thêm.");
                    }
                }
            }
        }
    }
}
