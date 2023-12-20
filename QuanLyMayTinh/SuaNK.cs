using System;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QuanLyMayTinh
{
    public partial class SuaNK : Form
    {
        string ketnoi = "Server=DESKTOP-K0TD8GJ\\SQLEXPRESS;Database=QLMT;Trusted_Connection=True;";
        private string maPhieuNhap;
        private string maSP;
        private string maNV;
        private string ngayNhap;
        private string soluongNhap;
        private string giaNhap;

        public SuaNK()
        {
            InitializeComponent();
        }

        public SuaNK(string maPhieuNhap, string maSP, string maNV, string ngayNhap, string soluongNhap, string giaNhap)
        {
            InitializeComponent();
            this.maPhieuNhap = maPhieuNhap;
            this.maSP = maSP;
            this.maNV = maNV;
            this.ngayNhap = ngayNhap;
            this.soluongNhap = soluongNhap;
            this.giaNhap = giaNhap;
            TruyenDuLieu();
        }

        private void TruyenDuLieu()
        {
            textBox1.Text = maPhieuNhap;
            textBox2.Text = maSP;
            textBox4.Text = soluongNhap;
            textBox5.Text = giaNhap;
            textBox6.Text = maNV;
            if (DateTime.TryParse(ngayNhap, out DateTime parsedDate))
            {
                dateTimePicker1.Value = parsedDate;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            string maPhieuNhap = textBox1.Text;
            string maSP = textBox2.Text;
            string maNV = textBox6.Text;
            string ngayNhap = dateTimePicker1.Text;
            string soluongNhap = textBox4.Text;
            string giaNhap = textBox5.Text;
            using (SqlConnection connection = new SqlConnection(ketnoi))
            {
                connection.Open();

                // UPDATE statement
                string sql = "UPDATE NhapKho " +
                             "SET MaPhieuNhap = @maPhieuNhap,MaSP = @maSP, MaNV=@maNV,SoLuongNhap = @soluongNhap, NgayNhap = @ngayNhap, GiaNhap = @giaNhap " +
                             "WHERE MaPhieuNhap = @maPhieuNhap";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@maSP", maSP);
                    command.Parameters.AddWithValue("@maPhieuNhap", maPhieuNhap);
                    command.Parameters.AddWithValue("@maNV", maNV);
                    command.Parameters.AddWithValue("@ngayNhap", ngayNhap);
                    command.Parameters.AddWithValue("@soluongNhap", soluongNhap);
                    command.Parameters.AddWithValue("@giaNhap", giaNhap);
                    // Execute the UPDATE statement
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Cập nhật thông tin nhập kho thành công.");
                    }
                    else
                    {
                        MessageBox.Show("Không có dòng nào được cập nhật.");
                    }
                }
            }
        }

        private void SuaNK_Load(object sender, EventArgs e)
        {

        }
    }
}
