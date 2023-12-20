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
    public partial class XuatKho : Form
    {
        string ketnoi = "Server=DESKTOP-K0TD8GJ\\SQLEXPRESS;Database=QLMT;Trusted_Connection=True;";
        public XuatKho()
        {
            InitializeComponent();
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "Nhập vào mã sản phẩm hoặc tên sản phẩm")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "Nhập vào mã sản phẩm hoặc tên sản phẩm";
                textBox1.ForeColor = Color.Silver;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string timSP = textBox1.Text;

            using (SqlConnection connection = new SqlConnection(ketnoi))
            {
                connection.Open();

                // Giả sử bạn có một bảng có tên 'NhanViens' với các cột 'MaNhanVien' và 'TenNhanVien'
                string sql = "SELECT SP.TenSP,SP.LoaiSP ,DHC.SoLuong, DHC.DonGia,DHC.NgayLap " +
              "FROM SP " +
              "JOIN DonHangChiTiet DHC ON SP.MaSP = DHC.MaSP " +
              "WHERE SP.MaSp = @timSP OR SP.TenSP LIKE '%' + @timSP + '%'";


                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@timSP", timSP);
                    // Sử dụng SqlDataAdapter để đổ dữ liệu từ SqlDataReader vào DataTable
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    // Hiển thị dữ liệu trên DataGridView
                    dataGridView1.DataSource = dataTable;

                }
            }
        }
    }
}
