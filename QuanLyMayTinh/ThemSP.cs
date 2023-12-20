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
    public partial class ThemSP : Form
    {
        string ketnoi = "Server=DESKTOP-K0TD8GJ\\SQLEXPRESS;Database=QLMT;Trusted_Connection=True;";
        public ThemSP()
        {
            InitializeComponent();
        }

        private void ThemSP_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Lấy thông tin từ các controls
            string maSP = textBox1.Text;
            string tenSP = textBox2.Text;
            string loaiSP = comboBox2.SelectedItem?.ToString();
            string thongTin = textBox3.Text;
            string nguonNhap = textBox4.Text;
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
            if (string.IsNullOrWhiteSpace(maSP) || string.IsNullOrWhiteSpace(tenSP) ||
                string.IsNullOrWhiteSpace(loaiSP) || string.IsNullOrWhiteSpace(thongTin) ||
                string.IsNullOrWhiteSpace(nguonNhap) || imageData == null)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin sản phẩm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Thực hiện câu lệnh INSERT vào cơ sở dữ liệu
            using (SqlConnection connection = new SqlConnection(ketnoi))
            {
                connection.Open();

                // Câu lệnh SQL INSERT
                string sql = "INSERT INTO SP (MaSP, TenSP, ThongTin, HinhAnh, NguonNhap, LoaiSP) " +
                             "VALUES (@maSP, @tenSP, @thongTin, @imageData, @nguonNhap, @loaiSP)";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    // Thêm tham số cho giá trị của các trường
                    command.Parameters.AddWithValue("@maSP", maSP);
                    command.Parameters.AddWithValue("@tenSP", tenSP);
                    command.Parameters.AddWithValue("@thongTin", thongTin);
                    command.Parameters.Add("@imageData", SqlDbType.VarBinary).Value = imageData;
                    command.Parameters.AddWithValue("@nguonNhap", nguonNhap);
                    command.Parameters.AddWithValue("@loaiSP", loaiSP);

                    // Thực hiện câu lệnh INSERT
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Thêm sản phẩm thành công.");
                    }
                    else
                    {
                        MessageBox.Show("Sản phẩm chưa được thêm.");
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
    }
}
