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
    public partial class NhapKho : Form
    {
        string ketnoi = "Server=DESKTOP-K0TD8GJ\\SQLEXPRESS;Database=QLMT;Trusted_Connection=True;";
        public NhapKho()
        {
            InitializeComponent();
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "Nhập vào mã nhập kho để tìm")
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
                textBox1.Text = "Nhập vào mã nhập kho để tìm";
                textBox1.ForeColor = Color.Silver;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string timNK = textBox1.Text;

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

        private void button4_Click(object sender, EventArgs e)
        {
            ThêmNK nk = new ThêmNK();
            nk.Show();
        }

        private void XuatKho_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Get the value from the selected row, assuming the MaSP is in the first column (index 0)
                string maNKToDelete = dataGridView1.SelectedRows[0].Cells["MaPhieuNhap"].Value.ToString();

                // Confirmation dialog
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa mã nhập kho này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    // Perform the delete operation
                    using (SqlConnection connection = new SqlConnection(ketnoi))
                    {
                        connection.Open();

                        // SQL DELETE statement
                        string sql = "DELETE FROM NhapKho WHERE MaPhieuNhap = @maNK";

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@maNK", maNKToDelete);

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Xóa nhập kho thành công.");
                                // Refresh the DataGridView or remove the row from the DataGridView.
                                dataGridView1.Rows.Remove(dataGridView1.SelectedRows[0]);
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy nhập kho cần xóa.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                string maPhieuNhap = dataGridView1.SelectedRows[0].Cells["MaPhieuNhap"].Value.ToString();
                string maSP = dataGridView1.SelectedRows[0].Cells["MaSP"].Value.ToString();
                string maNV = dataGridView1.SelectedRows[0].Cells["MaNV"].Value.ToString();
                string ngayNhap = dataGridView1.SelectedRows[0].Cells["NgayNhap"].Value.ToString();
                string soluongNhap = dataGridView1.SelectedRows[0].Cells["SoLuongNhap"].Value.ToString();
                string giaNhap = dataGridView1.SelectedRows[0].Cells["GiaNhap"].Value.ToString();
                // Tạo một đối tượng của form SuaNV và truyền giá trị
                SuaNK suaNVForm = new SuaNK(maPhieuNhap, maSP, maNV, ngayNhap, soluongNhap, giaNhap);

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
