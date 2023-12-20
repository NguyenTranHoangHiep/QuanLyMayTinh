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
    public partial class SuaTK : Form
    {
        string ketnoi = "Server=DESKTOP-K0TD8GJ\\SQLEXPRESS;Database=QLMT;Trusted_Connection=True;";
        private string maTonKho;
        private string maSP;
        private string soLuongTon;
        private string giaBan;

        public SuaTK(string maTonKho, string maSP, string soLuongTon, string giaBan)
        {
            InitializeComponent();
            this.maTonKho = maTonKho;
            this.maSP = maSP;
            this.soLuongTon = soLuongTon;
            this.giaBan = giaBan;
            TruyenDuLieu();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string maTonKho = textBox1.Text;
            string maSP = textBox2.Text;
            string soluongTonKho = textBox6.Text;
            string giaBan = textBox5.Text;
            using (SqlConnection connection = new SqlConnection(ketnoi))
            {
                connection.Open();

                // UPDATE statement
                string sql = "UPDATE TonKho " +
                             "SET MaTonKho = @maTonKho,MaSP = @maSP,SoLuongTonKho = @soluongTonKho, GiaBan = @giaBan " +
                             "WHERE MaTonKho = @maTonKho";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@maTonKho", maTonKho);
                    command.Parameters.AddWithValue("@maSP", maSP);
                    command.Parameters.AddWithValue("@soluongTonKho", soluongTonKho);
                    command.Parameters.AddWithValue("@giaBan", giaBan);
                    // Execute the UPDATE statement
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Cập nhật thông tin tồn kho thành công.");
                    }
                    else
                    {
                        MessageBox.Show("Không có dòng nào được cập nhật.");
                    }
                }
            }
        }

        private void TruyenDuLieu()
        {
            textBox1.Text = maTonKho;
            textBox2.Text = maSP;
            textBox6.Text = soLuongTon;
            textBox5.Text = giaBan;
        }
    }
}

