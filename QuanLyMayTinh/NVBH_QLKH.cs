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
    public partial class NVBH_QLKH : Form
    {
        string ketnoi = "Server=DESKTOP-K0TD8GJ\\SQLEXPRESS;Database=QLMT;Trusted_Connection=True;";
        public NVBH_QLKH()
        {
            InitializeComponent();
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "Nhập vào tên khách hàng hoặc số điện thoại khách hàng để tìm")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
                if (textBox1.Text == "")
                {
                    textBox1.Text = "Nhập vào tên khách hàng hoặc số điện thoại khách hàng để tìm";
                    textBox1.ForeColor = Color.Silver;
                }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ThemKH themKH = new ThemKH();
            // Hiển thị Form2
            themKH.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string timKH = textBox1.Text;
            using (SqlConnection connection = new SqlConnection(ketnoi))
            {
                connection.Open();
                string sql = "SELECT KH.MaKH,KH.TenKH,KH.GioiTinh,KH.DiaChi,KH.Sdt,KH.GhiChu,COUNT(DonHangChiTiet.MaDon) AS SoLuongDon " +
                             "FROM KH LEFT JOIN DonHangChiTiet ON KH.MaKH = DonHangChiTiet.MaKH " +
                             "WHERE KH.TenKH LIKE '%' + @timKH + '%' OR KH.Sdt LIKE '%' + @timKH + '%' " +
                             "GROUP BY KH.MaKH,KH.TenKH,KH.GioiTinh,KH.DiaChi,KH.Sdt,KH.GhiChu";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@timKH", timKH);
                    // Sử dụng SqlDataAdapter để đổ dữ liệu từ SqlDataReader vào DataTable
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    // Hiển thị dữ liệu trên DataGridView
                    dataGridView1.DataSource = dataTable;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Lấy giá trị từ hàng được chọn trong DataGridView
                string maKH = dataGridView1.SelectedRows[0].Cells["MaKH"].Value.ToString();
                string tenKH = dataGridView1.SelectedRows[0].Cells["TenKH"].Value.ToString();
                string gioiTinh = dataGridView1.SelectedRows[0].Cells["GioiTinh"].Value.ToString();
                string diaChi = dataGridView1.SelectedRows[0].Cells["DiaChi"].Value.ToString();
                string sdt = dataGridView1.SelectedRows[0].Cells["Sdt"].Value.ToString();
                string ghiChu = dataGridView1.SelectedRows[0].Cells["GhiChu"].Value.ToString();
                // Tạo một đối tượng của form SuaNV và truyền giá trị
                SuaKH suaKHForm = new SuaKH(maKH, tenKH, gioiTinh, diaChi, sdt, ghiChu);

                // Hiển thị form SuaNV
                suaKHForm.Show();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một khách hàng để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Get the value from the selected row, assuming the MaSP is in the first column (index 0)
                string maKHToDelete = dataGridView1.SelectedRows[0].Cells["MaKH"].Value.ToString();

                // Confirmation dialog
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa khách hàng này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    // Perform the delete operation
                    using (SqlConnection connection = new SqlConnection(ketnoi))
                    {
                        connection.Open();

                        // SQL DELETE statement
                        string sql = "DELETE FROM KH WHERE MaKH = @maKH";

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@maKH", maKHToDelete);

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Xóa khách hàng thành công.");
                                // Refresh the DataGridView or remove the row from the DataGridView.
                                dataGridView1.Rows.Remove(dataGridView1.SelectedRows[0]);
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy khách hàng cần xóa.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
        }
    }
}
