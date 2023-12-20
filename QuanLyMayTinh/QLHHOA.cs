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
    public partial class QLHHOA : Form
    {
        string ketnoi = "Server=DESKTOP-K0TD8GJ\\SQLEXPRESS;Database=QLMT;Trusted_Connection=True;";
        public QLHHOA()
        {
            InitializeComponent();
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "Nhập vào mã sản phẩm hoặc tên sản phẩm để tìm kiếm")
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
                textBox1.Text = "Nhập vào mã sản phẩm hoặc tên sản phẩm để tìm kiếm";
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
                string sql = "SELECT * FROM SP WHERE MaSp = @timSP OR TenSP LIKE '%' + @timSP + '%'";

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

        private void button2_Click(object sender, EventArgs e)
        {
            ThemSP themsp = new ThemSP();
            // Hiển thị Form2
            themsp.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Get the value from the selected row, assuming the MaSP is in the first column (index 0)
                string maSPToDelete = dataGridView1.SelectedRows[0].Cells["MaSP"].Value.ToString();

                // Confirmation dialog
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    // Perform the delete operation
                    using (SqlConnection connection = new SqlConnection(ketnoi))
                    {
                        connection.Open();

                        // SQL DELETE statement
                        string sql = "DELETE FROM SP WHERE MaSP = @maSP";

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@maSP", maSPToDelete);

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Xóa sản phẩm thành công.");
                                // Refresh the DataGridView or remove the row from the DataGridView.
                                dataGridView1.Rows.Remove(dataGridView1.SelectedRows[0]);
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy sản phẩm cần xóa.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem có hàng nào được chọn không
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Lấy giá trị từ hàng được chọn trong DataGridView
                string maSP = dataGridView1.SelectedRows[0].Cells["MaSP"].Value.ToString();
                string tenSP = dataGridView1.SelectedRows[0].Cells["TenSP"].Value.ToString();
                string thongTin = dataGridView1.SelectedRows[0].Cells["ThongTin"].Value.ToString();
                string nguonNhap = dataGridView1.SelectedRows[0].Cells["NguonNhap"].Value.ToString();
                string loaiSP = dataGridView1.SelectedRows[0].Cells["LoaiSP"].Value.ToString();
                byte[] hinhAnhPath = (byte[])dataGridView1.SelectedRows[0].Cells["HinhAnh"].Value;
                // Tạo một đối tượng của form SuaNV và truyền giá trị
                SuaSP suaNVForm = new SuaSP(maSP,tenSP,thongTin,nguonNhap,loaiSP, hinhAnhPath);

                // Hiển thị form SuaNV
                suaNVForm.Show();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sản phẩm để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
 }
