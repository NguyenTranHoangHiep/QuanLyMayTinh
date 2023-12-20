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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QuanLyMayTinh
{

    public partial class QLNV : Form
    {
        string ketnoi = "Server=DESKTOP-K0TD8GJ\\SQLEXPRESS;Database=QLMT;Trusted_Connection=True;";
        public QLNV()
        {
            InitializeComponent();
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "Nhập vào mã nhân viên hoặc tên nhân viên để tìm kiếm")
            {
                textBox1.Text ="";
                textBox1.ForeColor = Color.Black;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "Nhập vào mã nhân viên hoặc tên nhân viên để tìm kiếm";
                textBox1.ForeColor = Color.Silver;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
     
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            string timNV = textBox1.Text;

            using (SqlConnection connection = new SqlConnection(ketnoi))
            {
                connection.Open();

                // Giả sử bạn có một bảng có tên 'NhanViens' với các cột 'MaNhanVien' và 'TenNhanVien'
                string sql = "SELECT * FROM NV WHERE MaNV = @timNV OR TenNV LIKE '%' + @timNV + '%'";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@timNV", timNV);
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
            ThemNV themNV1 = new ThemNV();
            // Hiển thị Form2
           themNV1.Show();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }
        private string GetMaNV()
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                // Assuming the MaNV is in the column named "MaNV"
                DataGridViewCell maNVCell = selectedRow.Cells["MaNV"];

                // Check if the cell exists and if its value is not null
                if (maNVCell != null && maNVCell.Value != null)
                {
                    string maNV = maNVCell.Value.ToString();
                    return maNV;
                }
            }

            // Handle the case where no row is selected or the cell doesn't exist
            // For now, returning an empty string, but you might want to handle it differently.
            return string.Empty;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string maNV = GetMaNV();
            if (!string.IsNullOrEmpty(maNV))
            {
                using (SqlConnection connection = new SqlConnection(ketnoi))
                {
                    connection.Open();

                    string sql = "DELETE FROM NV WHERE MaNV = @maNV";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        // Adjust SqlDbType based on the actual data type of 'MaNV'
                        command.Parameters.Add("@maNV", SqlDbType.VarChar).Value = maNV;
                        command.ExecuteNonQuery();
                        MessageBox.Show("Xóa nhân viên thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DataTable dataTable = new DataTable();
                        dataGridView1.DataSource = dataTable;
                        
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một nhân viên để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem có hàng nào được chọn không
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Lấy giá trị từ hàng được chọn trong DataGridView
                string maNV = dataGridView1.SelectedRows[0].Cells["MaNV"].Value.ToString();
                string tenNV = dataGridView1.SelectedRows[0].Cells["TenNV"].Value.ToString();
                string gioiTinh = dataGridView1.SelectedRows[0].Cells["GioiTinh"].Value.ToString();
                string diaChi = dataGridView1.SelectedRows[0].Cells["DiaChi"].Value.ToString();
                string sdt = dataGridView1.SelectedRows[0].Cells["Sdt"].Value.ToString();
                string tk = dataGridView1.SelectedRows[0].Cells["Tk"].Value.ToString();
                string mk = dataGridView1.SelectedRows[0].Cells["MK"].Value.ToString();
                string loaiNV = dataGridView1.SelectedRows[0].Cells["LoaiNV"].Value.ToString();
                byte[] hinhAnhPath = (byte[])dataGridView1.SelectedRows[0].Cells["HinhAnh"].Value;
                // Tạo một đối tượng của form SuaNV và truyền giá trị
                SuaNV suaNVForm = new SuaNV(maNV, tenNV,gioiTinh ,diaChi, sdt, tk, mk, loaiNV, hinhAnhPath);

                // Hiển thị form SuaNV
                suaNVForm.Show();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một nhân viên để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void dataGridView1_Click(object sender, EventArgs e)
        {

        }

        private void QLNV_Load(object sender, EventArgs e)
        {

        }
    }
}
