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
    public partial class SuaSP : Form
    {
        string ketnoi = "Server=DESKTOP-K0TD8GJ\\SQLEXPRESS;Database=QLMT;Trusted_Connection=True;";
        private string maSP;
        private string tenSP;
        private string thongTin;
        private string nguonNhap;
        private string loaiSP;
        private byte[] hinhAnhPath;
        public SuaSP(string maSP,string tenSP,string thongTin,string nguonNhap,string loaiSP, byte[] hinhAnhPath)
        {
            InitializeComponent();
            this.maSP = maSP;
            this.tenSP = tenSP;
            this.thongTin= thongTin;
            this.nguonNhap= nguonNhap;
            this.loaiSP= loaiSP;
            this.hinhAnhPath= hinhAnhPath;
            TruyenDuLieu();
            HienThiHinhAnh();
        }
        private void TruyenDuLieu()
        {
            // Sử dụng các giá trị của maNV, tenNV, hinhAnhPath để cập nhật giao diện
            // Ví dụ:
            textBox1.Text = maSP;
            textBox2.Text = tenSP;
            textBox3.Text = thongTin;
            textBox4.Text = nguonNhap;
            comboBox2.SelectedItem = loaiSP;
        }

        private void HienThiHinhAnh()
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(hinhAnhPath))
                {
                    Image image = Image.FromStream(ms);
                    pictureBox2.Image = image;
                    pictureBox2.SizeMode = PictureBoxSizeMode.Zoom; // Tuỳ chỉnh dựa trên sở thích của bạn
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading image: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void SuaSP_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string maSP = textBox1.Text;
            string tenSP = textBox2.Text;
            string gioiTinh = comboBox2.SelectedItem?.ToString();
            string thongTin = textBox3.Text;
            string nguonNhap = textBox4.Text;
            string loaiSP = comboBox2.SelectedItem?.ToString();

            // Lấy dữ liệu hình ảnh từ PictureBox
            Image image = pictureBox2.Image;
            byte[] imageData = null;

            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, image.RawFormat);
                imageData = ms.ToArray();
            }

            using (SqlConnection connection = new SqlConnection(ketnoi))
            {
                connection.Open();

                // UPDATE statement
                string sql = "UPDATE SP " +
                             "SET TenSP = @tenSP,ThongTin = @thongTin, NguonNhap = @nguonNhap, LoaiSP = @loaiSP, HinhAnh = @hinhAnh " +
                             "WHERE MaSP = @maSP";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@maSP", maSP);
                    command.Parameters.AddWithValue("@tenSP", tenSP);
                    command.Parameters.AddWithValue("@thongTin", thongTin);
                    command.Parameters.AddWithValue("@nguonNhap", nguonNhap);
                    command.Parameters.AddWithValue("@loaiSP", loaiSP);
                    command.Parameters.Add("@hinhAnh", SqlDbType.VarBinary).Value = imageData;

                    // Execute the UPDATE statement
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Cập nhật thông tin sản phẩm thành công.");
                    }
                    else
                    {
                        MessageBox.Show("Không có dòng nào được cập nhật.");
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
