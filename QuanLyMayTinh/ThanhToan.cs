using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyMayTinh
{
    public partial class ThanhToan : Form
    {
        string ketnoi = "Server=DESKTOP-K0TD8GJ\\SQLEXPRESS;Database=QLMT;Trusted_Connection=True;";
        public ThanhToan()
        {
            InitializeComponent();
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "Nhập vào mã sản phẩm để tìm")
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
                textBox1.Text = "Nhập vào mã sản phẩm để tìm";
                textBox1.ForeColor = Color.Silver;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(ketnoi))
            {
                connection.Open();
                string sql = "SELECT TonKho.MaTonKho, SP.MaSP, SP.TenSP, SP.ThongTin,TonKho.SoLuongTonKho,TonKho.GiaBan " +
               "FROM TonKho JOIN SP ON SP.MaSP = TonKho.MaSP " +
               "WHERE TonKho.MaSP LIKE '%' + @MaSP + '%'";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@MaSP", textBox1.Text);
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
                SuaSP suaNVForm = new SuaSP(maSP, tenSP, thongTin, nguonNhap, loaiSP, hinhAnhPath);

                // Hiển thị form SuaNV
                suaNVForm.Show();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sản phẩm để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Lấy dòng đang được chọn
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];

                // Cập nhật giá trị của các TextBox
                textBox6.Text = selectedRow.Cells["MaTonKho"].Value.ToString();
                textBox4.Text = selectedRow.Cells["MaSp"].Value.ToString();
                textBox8.Text = selectedRow.Cells["GiaBan"].Value.ToString();
            }
        }
        private void LoadDataIntoDataGridView2()
        {
            string maKH = textBox5.Text;
            using (SqlConnection connection = new SqlConnection(ketnoi))
            {
                connection.Open();

                // Assuming you have a query to select data for dataGridView2 based on MaKH
                string selectDataQuery = "SELECT * FROM DonHangChiTiet WHERE MaKH = @MaKH";

                using (SqlCommand selectDataCommand = new SqlCommand(selectDataQuery, connection))
                {
                    selectDataCommand.Parameters.AddWithValue("@MaKH", maKH);

                    SqlDataAdapter adapter = new SqlDataAdapter(selectDataCommand);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Set the DataTable as the DataSource for dataGridView2
                    dataGridView2.DataSource = dataTable;
                }

                connection.Close();
            }
        }
        private void button2_Click_1(object sender, EventArgs e)
        {
            string maDon = textBox2.Text;
            string maNV = textBox3.Text;
            string ngayLap = dateTimePicker1.Text;
            string maKH = textBox5.Text;
            string maSP = textBox4.Text;
            string maTonKho = textBox6.Text;
            string soLuong = textBox7.Text;
            string donGia = textBox8.Text;

            using (SqlConnection connection = new SqlConnection(ketnoi))
            {
                connection.Open();

                string selectQuery = "SELECT MaTonKho, GiaBan FROM TonKho WHERE MaSP = @MaSP AND SoLuongTonKho >= 1";

                using (SqlCommand selectCommand = new SqlCommand(selectQuery, connection))
                {
                    selectCommand.Parameters.AddWithValue("@MaSP", maSP);

                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();

                            string maTonKhoFromDB = reader["MaTonKho"].ToString();
                            float giaBan = Convert.ToSingle(reader["GiaBan"]);
                            reader.Close();

                            float thanhTien = Convert.ToSingle(soLuong) * Convert.ToSingle(donGia);

                            string insertQuery = "INSERT INTO DonHangChiTiet (MaDon, MaNV, MaSP, MaKH, MaTonKho, SoLuong, DonGia, ThanhTien, NgayLap) " +
                                                 "VALUES (@MaDon, @MaNV, @MaSP, @MaKH, @MaTonKho, @SoLuong, @DonGia, @ThanhTien, @NgayLap)";

                            using (SqlCommand command = new SqlCommand(insertQuery, connection))
                            {
                                command.Parameters.AddWithValue("@MaDon", maDon);
                                command.Parameters.AddWithValue("@MaNV", maNV);
                                command.Parameters.AddWithValue("@MaSP", maSP);
                                command.Parameters.AddWithValue("@MaKH", maKH);
                                command.Parameters.AddWithValue("@MaTonKho", maTonKho);
                                command.Parameters.AddWithValue("@SoLuong", Convert.ToInt32(soLuong));
                                command.Parameters.AddWithValue("@DonGia", Convert.ToSingle(donGia));
                                command.Parameters.AddWithValue("@ThanhTien", thanhTien);
                                command.Parameters.AddWithValue("@NgayLap", dateTimePicker1.Value);

                                command.ExecuteNonQuery();
                            }

                            string updateQuery = "UPDATE TonKho SET SoLuongTonKho = SoLuongTonKho - @SoLuong WHERE MaTonKho = @MaTonKho";

                            using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                            {
                                updateCommand.Parameters.AddWithValue("@MaTonKho", maTonKhoFromDB);
                                updateCommand.Parameters.AddWithValue("@SoLuong", Convert.ToInt32(soLuong));

                                updateCommand.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Không có đủ tồn kho cho sản phẩm có mã: {maSP}");
                        }
                    }
                }

                LoadDataIntoDataGridView2();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                // Get the values from the selected row
                string maSPToDelete = dataGridView2.SelectedRows[0].Cells["MaSP"].Value.ToString();
                int soLuongToDelete = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells["SoLuong"].Value);

                // Confirmation dialog
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này khỏi đơn hàng?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    using (SqlConnection connection = new SqlConnection(ketnoi))
                    {
                        try
                        {
                            connection.Open();

                            // SQL DELETE statement
                            string deleteQuery = "DELETE FROM DonHangChiTiet WHERE MaSP=@maSP";

                            using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection))
                            {
                                deleteCommand.Parameters.AddWithValue("@maSP", maSPToDelete);

                                int rowsAffected = deleteCommand.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Xóa sản phẩm khỏi đơn hàng thành công.");

                                    // Update SoLuongTonKho in TonKho table
                                    string updateTonKhoQuery = "UPDATE TonKho SET SoLuongTonKho = SoLuongTonKho + @soLuong WHERE MaSP = @maSP";

                                    using (SqlCommand updateTonKhoCommand = new SqlCommand(updateTonKhoQuery, connection))
                                    {
                                        updateTonKhoCommand.Parameters.AddWithValue("@maSP", maSPToDelete);
                                        updateTonKhoCommand.Parameters.AddWithValue("@soLuong", soLuongToDelete);
                                        updateTonKhoCommand.ExecuteNonQuery();
                                    }

                                    // Refresh the DataGridView or remove the row from the DataGridView.
                                    dataGridView2.Rows.Remove(dataGridView2.SelectedRows[0]);
                                }
                                else
                                {
                                    MessageBox.Show("Không tìm thấy sản phẩm cần xóa.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Lỗi xóa sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string maDon = textBox2.Text;
            string maNV = textBox3.Text;
            string ngayLap = dateTimePicker1.Text;
            string maKH = textBox5.Text;
            string maSP = textBox4.Text;
            string maTonKho = textBox6.Text;
            string soLuong = textBox7.Text;
            string donGia = textBox8.Text;

            using (SqlConnection connection = new SqlConnection(ketnoi))
            {
                connection.Open();

                string selectQuery = "SELECT MaTonKho, GiaBan FROM TonKho WHERE MaSP = @MaSP AND SoLuongTonKho >= 1";

                using (SqlCommand selectCommand = new SqlCommand(selectQuery, connection))
                {
                    selectCommand.Parameters.AddWithValue("@MaSP", maSP);

                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();

                            string maTonKhoFromDB = reader["MaTonKho"].ToString();
                            float giaBan = Convert.ToSingle(reader["GiaBan"]);
                            reader.Close();

                            float thanhTien = Convert.ToSingle(soLuong) * Convert.ToSingle(donGia);

                            string updateQuery = "UPDATE DonHangChiTiet " +
                                                 "SET MaNV = @MaNV, " +
                                                 "MaSP = @MaSP, " +
                                                 "MaKH = @MaKH, " +
                                                 "MaTonKho = @MaTonKho, " +
                                                 "SoLuong = @SoLuong, " +
                                                 "DonGia = @DonGia, " +
                                                 "ThanhTien = @ThanhTien, " +
                                                 "NgayLap = @NgayLap " +
                                                 "WHERE MaDon = @MaDon";

                            using (SqlCommand command = new SqlCommand(updateQuery, connection))
                            {
                                command.Parameters.AddWithValue("@MaDon", maDon);
                                command.Parameters.AddWithValue("@MaNV", maNV);
                                command.Parameters.AddWithValue("@MaSP", maSP);
                                command.Parameters.AddWithValue("@MaKH", maKH);
                                command.Parameters.AddWithValue("@MaTonKho", maTonKho);
                                command.Parameters.AddWithValue("@SoLuong", Convert.ToInt32(soLuong));
                                command.Parameters.AddWithValue("@DonGia", Convert.ToSingle(donGia));
                                command.Parameters.AddWithValue("@ThanhTien", thanhTien);
                                command.Parameters.AddWithValue("@NgayLap", ngayLap);

                                command.ExecuteNonQuery();
                            }

                            string updateTonKhoQuery = "UPDATE TonKho SET SoLuongTonKho = SoLuongTonKho - @SoLuong WHERE MaTonKho = @MaTonKho";

                            using (SqlCommand updateCommand = new SqlCommand(updateTonKhoQuery, connection))
                            {
                                updateCommand.Parameters.AddWithValue("@MaTonKho", maTonKhoFromDB);
                                updateCommand.Parameters.AddWithValue("@SoLuong", Convert.ToInt32(soLuong));
                                updateCommand.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Không có đủ tồn kho cho sản phẩm có mã: {maSP}");
                        }
                    }
                }

                LoadDataIntoDataGridView2();

                connection.Close();
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Lấy dòng đang được chọn
                DataGridViewRow selectedRow = dataGridView2.Rows[e.RowIndex];
                textBox2.Text = selectedRow.Cells["MaDon"].Value?.ToString() ?? "";
                textBox3.Text = selectedRow.Cells["MaNV"].Value?.ToString() ?? "";
                textBox5.Text = selectedRow.Cells["MaKH"].Value?.ToString() ?? "";
                textBox4.Text = selectedRow.Cells["MaSP"].Value?.ToString() ?? "";
                textBox6.Text = selectedRow.Cells["MaTonKho"].Value?.ToString() ?? "";
                textBox7.Text = selectedRow.Cells["SoLuong"].Value?.ToString() ?? "";
                textBox8.Text = selectedRow.Cells["DonGia"].Value?.ToString() ?? "";
                dateTimePicker1.Text = selectedRow.Cells["NgayLap"].Value?.ToString() ?? "";
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView2.Rows.Count > 0)
            {
                PrintDocument printDocument = new PrintDocument();
                printDocument.PrintPage += new PrintPageEventHandler(PrintInvoice_PrintPage);

                PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
                printPreviewDialog.Document = printDocument;

                // Hiển thị trước khi in
                printPreviewDialog.ShowDialog();
            }
            else
            {
                MessageBox.Show("Không có dữ liệu để in.", "Thông báo");
            }
        }
        private void PrintInvoice_PrintPage(object sender, PrintPageEventArgs e)
        {
            // Font cho nội dung hóa đơn
            Font font = new Font("Arial", 12);
            Brush brush = Brushes.Black;

            // Vị trí bắt đầu in
            float leftMargin = e.MarginBounds.Left;
            float topMargin = e.MarginBounds.Top;

            // Tạo nội dung hóa đơn
            string invoiceContent = CreateInvoiceContent();

            // In nội dung hóa đơn
            e.Graphics.DrawString(invoiceContent, font, brush, leftMargin, topMargin);
        }
        private string CreateInvoiceContent()
        {
            // Tạo nội dung hóa đơn dựa trên dữ liệu từ DataGridView
            // Bạn cần xử lý và trích xuất thông tin từ DataGridView để tạo hóa đơn
            // Ví dụ:
            string invoiceContent = "";
            string ngayLapValue = "";
            string hoadonvalue = "";
            string nguoiLapValue = "";
            decimal totalAmount = 0; // Reset totalAmount before summing up the values

            if (dataGridView2.Rows.Count > 0)
            {
                ngayLapValue = dataGridView2.Rows[0].Cells["NgayLap"].Value?.ToString() ?? "";
                hoadonvalue = dataGridView2.Rows[0].Cells["MaDon"].Value?.ToString() ?? "";
                nguoiLapValue = dataGridView2.Rows[0].Cells["MaNV"].Value?.ToString() ?? "";
            }
            string hoadon = "Hóa đơn: " + hoadonvalue;
            string ngayLap = "Ngày lập: " + ngayLapValue;
            string nguoiLap = "Người lập: " + nguoiLapValue;
            // Đầu bảng
            invoiceContent += hoadon + "\n";
            invoiceContent += ngayLap + "\n";
            string header = $"{"Mã Sản Phẩm",-23}{"Tên sản phẩm",-20}{"Số lượng",-20}{"Đơn giá",-23}{"Thành tiền",-40}\n";
            invoiceContent += header;
            invoiceContent += new string('-', header.Length) + "\n";

            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                string productName = row.Cells["MaSP"].Value?.ToString() ?? "";
                string quantity = row.Cells["SoLuong"].Value?.ToString() ?? "";
                string unitPrice = row.Cells["DonGia"].Value?.ToString() ?? "";
                string total = row.Cells["ThanhTien"].Value?.ToString() ?? "";
                string tensp = row.Cells["TenSP"].Value?.ToString() ?? "";
                // Thêm vào dòng bảng
                invoiceContent += $"{productName,-30}{tensp,-23}{quantity,-22}{unitPrice,-25}{total}\n";

                // Tính tổng ThanhTien
                if (decimal.TryParse(total, out decimal totalValue))
                {
                    totalAmount += totalValue;
                }
            }

            // Dưới bảng
            invoiceContent += new string('-', header.Length) + "\n";

            // Apply discount logic outside the loop
            decimal discountThreshold = 100000000; // 100,000,000 VND
            if (totalAmount >= discountThreshold)
            {
                decimal discount = totalAmount * 0.05m; // 5% discount
                totalAmount -= discount;
                invoiceContent += $"                                                                       Tổng cộng (Sau giảm giá 5%): {totalAmount,-40}\n\n\n\n";
            }
            else
            {
                invoiceContent += $"                                                                       Tổng cộng: {totalAmount,-40}\n";
            }
            invoiceContent += "                                             Cảm ơn quý khách đã mua hàng!\n";
            return invoiceContent;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string timHD = textBox2.Text;
            string timKH = textBox5.Text;
            using (SqlConnection connection = new SqlConnection(ketnoi))
            {
                connection.Open();
                string sql = "SELECT DHC.MaDon, DHC.MaNV,DHC.MaKH, DHC.MaTonKho, DHC.MaSP, SP.TenSP, DHC.SoLuong, DHC.DonGia, DHC.ThanhTien,DHC.NgayLap " +
                "FROM DonHangChiTiet DHC " +
                "INNER JOIN SP ON DHC.MaSP = SP.MaSP " +
                "WHERE DHC.MaDon = @timHD or DHC.MaKH=@timKH";


                // The rest of your code to execute the query...


                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@timKH", timKH);
                    command.Parameters.AddWithValue("@timHD", timHD);
                    // Sử dụng SqlDataAdapter để đổ dữ liệu từ SqlDataReader vào DataTable
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    // Hiển thị dữ liệu trên DataGridView
                    dataGridView2.DataSource = dataTable;
                }
            }
        }
    }
}
