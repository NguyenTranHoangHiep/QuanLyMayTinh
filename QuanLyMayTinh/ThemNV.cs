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

namespace QuanLyMayTinh
{
    public partial class ThemNV : Form
    {
        string ketnoi = "Server=DESKTOP-K0TD8GJ\\SQLEXPRESS;Database=QLMT;Trusted_Connection=True;";
        public ThemNV()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Lấy thông tin từ các controls
            string maNV = textBox1.Text;
            string tenNV = textBox2.Text;
            string gioiTinh = comboBox2.SelectedItem?.ToString();
            string diaChi = textBox3.Text;
            string sdt = textBox4.Text;
            string tk = textBox5.Text;
            string mk = textBox6.Text;
            string loaiNV = comboBox1.SelectedItem?.ToString();

            // Lấy dữ liệu hình ảnh từ PictureBox
            Image image = pictureBox2.Image;
            byte[] imageData = null;

            if (image != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    image.Save(ms, image.RawFormat);
                    imageData = ms.ToArray();
                }
            }
            if (string.IsNullOrWhiteSpace(maNV) || string.IsNullOrWhiteSpace(tenNV) ||
                string.IsNullOrWhiteSpace(diaChi) || string.IsNullOrWhiteSpace(sdt) || string.IsNullOrWhiteSpace(gioiTinh) ||
                string.IsNullOrWhiteSpace(tk) || string.IsNullOrWhiteSpace(mk) ||
                string.IsNullOrWhiteSpace(loaiNV) || imageData == null)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin nhân viên.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Không thực hiện thêm nhân viên nếu có thông tin bị thiếu
            }
            // Thực hiện câu lệnh INSERT vào cơ sở dữ liệu
            using (SqlConnection connection = new SqlConnection(ketnoi))
            {
                connection.Open();

                // Câu lệnh SQL INSERT
                string sql = "INSERT INTO NV (MaNV, TenNV,GioiTinh, DiaChi, Sdt, Tk, Mk, LoaiNV, HinhAnh) " +
                             "VALUES (@maNV, @tenNV,@gioiTinh, @diaChi, @sdt, @tk, @mk, @loaiNV, @imageData)";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    // Thêm tham số cho giá trị của các trường
                    command.Parameters.AddWithValue("@maNV", maNV);
                    command.Parameters.AddWithValue("@tenNV", tenNV);
                    command.Parameters.AddWithValue("@gioiTinh", gioiTinh);
                    command.Parameters.AddWithValue("@diaChi", diaChi);
                    command.Parameters.AddWithValue("@sdt", sdt);
                    command.Parameters.AddWithValue("@tk", tk);
                    command.Parameters.AddWithValue("@mk", mk);
                    command.Parameters.AddWithValue("@loaiNV", loaiNV);
                    command.Parameters.Add("@imageData", SqlDbType.VarBinary).Value = imageData;

                    // Thực hiện câu lệnh INSERT
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Thêm nhân viên thành công.");
                    }
                    else
                    {
                        MessageBox.Show("Nhân viên chưa được thêm.");
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog dopen = new OpenFileDialog();
            dopen.Filter = "Tập tin ảnh|*.jpg;*.gif;*.png;*.bmp|File tùy ý(*.*)|*.*";
            if (dopen.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    pictureBox2.Image = Image.FromFile(dopen.FileName);
                }
                catch (Exception)
                {
                    MessageBox.Show(String.Format("Không thể xem ảnh", dopen.FileName));
                }
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void ThemNV_Load(object sender, EventArgs e)
        {

        }
    }
}
