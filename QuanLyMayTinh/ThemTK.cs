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
    public partial class ThemTK : Form
    {
        string ketnoi = "Server=DESKTOP-K0TD8GJ\\SQLEXPRESS;Database=QLMT;Trusted_Connection=True;";
        public ThemTK()
        {
            InitializeComponent();
        }

        private void ThemTK_Load(object sender, EventArgs e)
        {

        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            if (textBox3.Text == "Nhập vào mã nhập kho để tìm")
            {
                textBox3.Text = "";
                textBox3.ForeColor = Color.Black;
            }
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            if (textBox3.Text == "")
            {
                textBox3.Text = "Nhập vào mã nhập kho để tìm";
                textBox3.ForeColor = Color.Silver;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string timNK = textBox3.Text;

            using (SqlConnection connection = new SqlConnection(ketnoi))
            {
                connection.Open();
                string sql = "SELECT MaPhieuNhap, MaSP, SoLuongNhap, GiaNhap, NgayNhap, MaNV, SoLuongNhap * GiaNhap AS TongTien FROM NhapKho WHERE MaPhieuNhap LIKE '%' + @timNK + '%'";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@timNK", timNK);
                    // Sử dụng SqlDataAdapter để đổ dữ liệu từ SqlDataReader vào DataTable
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    // Hiển thị dữ liệu trên DataGridView
                    dataGridView1.DataSource = dataTable;

                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Lấy thông tin từ các controls
            string maTonKho = textBox1.Text;
            string maSP = textBox2.Text;
            string soLuongTonKho = textBox6.Text;
            string giaBan = textBox5.Text;

            if (string.IsNullOrWhiteSpace(maTonKho) || string.IsNullOrWhiteSpace(maSP) || string.IsNullOrWhiteSpace(soLuongTonKho) ||
                string.IsNullOrWhiteSpace(giaBan))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin tồn kho.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Không thực hiện thêm nhân viên nếu có thông tin bị thiếu
            }

            // Thực hiện câu lệnh INSERT vào cơ sở dữ liệu
            using (SqlConnection connection = new SqlConnection(ketnoi))
            {
                connection.Open();

                // Câu lệnh SQL INSERT
                string sql = "INSERT INTO TonKho (MaTonKho, MaSp, SoLuongTonKho, GiaBan) " +
                             "VALUES (@maTonKho, @maSP, @soLuongTonKho, @giaBan)";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    // Thêm tham số cho giá trị của các trường
                    command.Parameters.AddWithValue("@maTonKho", maTonKho);
                    command.Parameters.AddWithValue("@maSP", maSP);
                    command.Parameters.AddWithValue("@soLuongTonKho", soLuongTonKho);
                    command.Parameters.AddWithValue("@giaBan", giaBan);
                    // Thực hiện câu lệnh INSERT
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Thêm tồn kho thành công.");
                    }
                    else
                    {
                        MessageBox.Show("Tồn kho chưa được thêm.");
                    }
                }
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ensure that the clicked area is not a header
            if (e.RowIndex >= 0)
            {
                // Get the selected row
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];

                // Populate TextBoxes with values from the selected row
                textBox2.Text = selectedRow.Cells["MaSP"].Value?.ToString() ?? "";
                textBox6.Text = selectedRow.Cells["SoLuongNhap"].Value?.ToString() ?? "";
                textBox5.Text = selectedRow.Cells["GiaNhap"].Value?.ToString() ?? "";
            }
        }

    }
}
