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
    public partial class SuaBill : Form
    {
        string ketnoi = "Server=DESKTOP-K0TD8GJ\\SQLEXPRESS;Database=QLMT;Trusted_Connection=True;";
        private string ngayLap;
        private string maDon;
        private string maNV;
        private string maSP;
        private string maKH;
        private string maTK;
        private string soLuong;
        private string donGia;
        private string thanhTien;
        public SuaBill(string ngayLap,string maDon,string maNV,string maSP,string maKH,string maTK,string soLuong,string donGia,string thanhTien)
        {
            InitializeComponent();
            this.ngayLap = ngayLap;
            this.maDon = maDon;
            this.maNV = maNV;
            this.maSP = maSP;
            this.maKH = maKH;
            this.soLuong = soLuong;
            this.donGia = donGia;
            this.thanhTien = thanhTien;
            this.maTK = maTK;
            TruyenDuLieu();

        }
        private void TruyenDuLieu()
        {
            textBox1.Text = maDon;
            textBox2.Text = maNV;
            textBox3.Text = maSP;
            textBox4.Text = maKH;
            textBox5.Text = maTK;
            textBox6.Text = soLuong;
            textBox7.Text = donGia;
            textBox8.Text = thanhTien;
            if (DateTime.TryParse(ngayLap, out DateTime parsedDate))
            {
                dateTimePicker1.Value = parsedDate;
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            string maDon = textBox1.Text;
            string maNV = textBox2.Text;
            string maSP = textBox3.Text;
            string maKH = textBox4.Text;
            string maTK = textBox5.Text;
            string soLuong = textBox6.Text;
            string donGia = textBox7.Text;
            string thanhTien = textBox8.Text;
            string ngayLap = dateTimePicker1.Text;
            using (SqlConnection connection = new SqlConnection(ketnoi))
            {
                connection.Open();

                // UPDATE statement
                string sql = "UPDATE DonHangChiTiet " +
                             "SET MaDon = @maDon, MaNV = @maNV,MaSP = @maSP, MaKH = @maKH, MaTonKho = @maTK, SoLuong = @soLuong, DonGia = @donGia, ThanhTien = @thanhTien " +
                             "WHERE MaSP = @maSP";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@maNV", maNV);
                    command.Parameters.AddWithValue("@maDon", maDon);
                    command.Parameters.AddWithValue("@maSP", maSP);
                    command.Parameters.AddWithValue("@maKH", maKH);
                    command.Parameters.AddWithValue("@maTK", maTK);
                    command.Parameters.AddWithValue("@soLuong", soLuong);
                    command.Parameters.AddWithValue("@donGia", donGia);
                    command.Parameters.AddWithValue("@thanhTien", thanhTien);
                    command.Parameters.AddWithValue("@ngayLap", ngayLap);

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

        private void SuaBill_Load(object sender, EventArgs e)
        {

        }
    }
}
