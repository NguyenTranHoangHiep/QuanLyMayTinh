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

    public partial class SuaNV : Form
    {
        string ketnoi = "Server=DESKTOP-K0TD8GJ\\SQLEXPRESS;Database=QLMT;Trusted_Connection=True;";
        private string maNV;
        private string tenNV;
        private string gioiTinh;
        private string diaChi;
        private string sdt;
        private string tk;
        private string mk;
        private string loaiNV;
        private byte[] hinhAnhPath;

        public SuaNV(string maNV,string tenNV,string gioiTinh,string diaChi,string sdt,string tk,string mk,string loaiNV,byte[] hinhAnhPath)
        {
            InitializeComponent();
            this.maNV = maNV;
            this.tenNV = tenNV;
            this.gioiTinh = gioiTinh;
            this.diaChi = diaChi;
            this.sdt= sdt;
            this.tk = tk;
            this.mk= mk;
            this.loaiNV = loaiNV;
            this.hinhAnhPath= hinhAnhPath;
            // Gọi hàm để cập nhật giao diện của form dựa trên các giá trị đã nhận
            TruyenDuLieu();
            HienThiHinhAnh();
        }
        private void TruyenDuLieu()
        {
            // Sử dụng các giá trị của maNV, tenNV, hinhAnhPath để cập nhật giao diện
            // Ví dụ:
            textBox1.Text = maNV;
            textBox2.Text = tenNV;
            textBox3.Text = diaChi;
            textBox4.Text = sdt;
            textBox5.Text = tk;
            textBox6.Text = mk;
            comboBox1.SelectedItem = loaiNV;
            comboBox2.SelectedItem = gioiTinh;
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
        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
        private void button2_Click(object sender, EventArgs e)
        {
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

            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, image.RawFormat);
                imageData = ms.ToArray();
            }

            using (SqlConnection connection = new SqlConnection(ketnoi))
            {
                connection.Open();

                // UPDATE statement
                string sql = "UPDATE NV " +
                             "SET TenNV = @tenNV, GioiTinh = @gioiTinh,DiaChi = @diaChi, Sdt = @sdt, Tk = @tk, MK = @mk, LoaiNV = @loaiNV, HinhAnh = @hinhAnh " +
                             "WHERE MaNV = @maNV";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@maNV", maNV);
                    command.Parameters.AddWithValue("@tenNV", tenNV);
                    command.Parameters.AddWithValue("@gioiTinh", gioiTinh);
                    command.Parameters.AddWithValue("@diaChi", diaChi);
                    command.Parameters.AddWithValue("@sdt", sdt);
                    command.Parameters.AddWithValue("@tk", tk);
                    command.Parameters.AddWithValue("@mk", mk);
                    command.Parameters.AddWithValue("@loaiNV", loaiNV);
                    command.Parameters.Add("@hinhAnh", SqlDbType.VarBinary).Value = imageData;

                    // Execute the UPDATE statement
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Cập nhật nhân viên thành công.");
                    }
                    else
                    {
                        MessageBox.Show("Không có dòng nào được cập nhật.");
                    }
                }
            }
        }


        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void SuaNV_Load(object sender, EventArgs e)
        {

        }
    }
}
