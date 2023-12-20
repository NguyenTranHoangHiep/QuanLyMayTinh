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
    public partial class TonKho : Form
    {
        string ketnoi = "Server=DESKTOP-K0TD8GJ\\SQLEXPRESS;Database=QLMT;Trusted_Connection=True;";
        public TonKho()
        {
            InitializeComponent();
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "Nhập vào mã tồn kho để tìm")
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
                textBox1.Text = "Nhập vào mã tồn kho để tìm";
                textBox1.ForeColor = Color.Silver;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(ketnoi))
            {
                connection.Open();
                string sql = "SELECT *, SoLuongTonKho * GiaBan AS TongTien FROM TonKho WHERE MaTonKho LIKE '%' + @tonkho + '%'";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@tonkho", textBox1.Text);
                    // Sử dụng SqlDataAdapter để đổ dữ liệu từ SqlDataReader vào DataTable
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    // Hiển thị dữ liệu trên DataGridView
                    dataGridView1.DataSource = dataTable;

                }
            }
        }

        private void TonKho_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            ThemTK tonKho = new ThemTK();
            tonKho.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Get the value from the selected row, assuming the MaSP is in the first column (index 0)
                string maTKToDelete = dataGridView1.SelectedRows[0].Cells["MaTonKho"].Value.ToString();

                // Confirmation dialog
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa tồn kho này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    // Perform the delete operation
                    using (SqlConnection connection = new SqlConnection(ketnoi))
                    {
                        connection.Open();

                        // SQL DELETE statement
                        string sql = "DELETE FROM TonKho WHERE MaTonKho = @maTonKho";

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@maTonKho", maTKToDelete);

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Xóa tồn kho thành công.");
                                dataGridView1.Rows.Remove(dataGridView1.SelectedRows[0]);
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy tồn kho cần xóa.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem có hàng nào được chọn không
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Lấy giá trị từ hàng được chọn trong DataGridView
                string maTonKho = dataGridView1.SelectedRows[0].Cells["MaTonKho"].Value.ToString();
                string maSP = dataGridView1.SelectedRows[0].Cells["MaSP"].Value.ToString();
                string soLuongTon = dataGridView1.SelectedRows[0].Cells["SoLuongTonKho"].Value.ToString();
                string giaBan = dataGridView1.SelectedRows[0].Cells["GiaBan"].Value.ToString();
                // Tạo một đối tượng của form SuaTK và truyền giá trị
                SuaTK suaTKForm = new SuaTK(maTonKho, maSP, soLuongTon, giaBan);

                // Hiển thị form SuaNV
                suaTKForm.Show();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sản phẩm để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }
    }
}
