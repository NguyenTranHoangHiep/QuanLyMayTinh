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
    public partial class QLHD : Form
    {
        string ketnoi = "Server=DESKTOP-K0TD8GJ\\SQLEXPRESS;Database=QLMT;Trusted_Connection=True;";
        public QLHD()
        {
            InitializeComponent();
        }
        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "Nhập vào tên khách hàng,số điện thoại,hoặc mã khách hàng để tìm")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "Nhập vào tên khách hàng,số điện thoại,hoặc mã khách hàng để tìm";
                textBox1.ForeColor = Color.Silver;
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_Enter_1(object sender, EventArgs e)
        {
            if (textBox1.Text == "Nhập vào tên khách hàng,số điện thoại,hoặc mã khách hàng để tìm")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
            }
        }

        private void textBox1_Leave_1(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "Nhập vào tên khách hàng,số điện thoại,hoặc mã khách hàng để tìm";
                textBox1.ForeColor = Color.Silver;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string timHD = textBox1.Text;

            using (SqlConnection connection = new SqlConnection(ketnoi))
            {
                connection.Open();
                string sql = "SELECT KH.TenKH, KH.Sdt, DHC.MaDon, SUM(DHC.ThanhTien) AS TongThanhTien, DHC.MaKH, DHC.NgayLap, KH.GhiChu, " +
                              "CASE WHEN KH.GhiChu = 'VIP' THEN SUM(DHC.ThanhTien) - (SUM(DHC.ThanhTien) * 0.05) " +
                              "     WHEN KH.GhiChu = 'Normal' THEN SUM(DHC.ThanhTien) " +
                              "     ELSE SUM(DHC.ThanhTien) END AS SauUuDai " +
                              "FROM KH " +
                              "INNER JOIN DonHangChiTiet DHC ON KH.MaKH = DHC.MaKH " +
                              "WHERE KH.TenKH LIKE '%' + @timHD + '%' OR DHC.MaDon LIKE '%' + @timHD + '%' OR KH.Sdt = @timHD " +
                              "GROUP BY KH.TenKH, KH.Sdt, DHC.MaDon, DHC.MaKH, DHC.NgayLap, KH.GhiChu";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@timHD", timHD);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Update GhiChu column based on TongThanhTien
                    foreach (DataRow row in dataTable.Rows)
                    {
                        // Assuming the GhiChu column is already present in the DataTable
                        decimal tongThanhTien = Convert.ToDecimal(row["TongThanhTien"]);
                        string newGhiChu = (tongThanhTien >= 100000000) ? "VIP" : "Normal";
                        row["GhiChu"] = newGhiChu;

                        // Update GhiChu in the database
                        UpdateGhiChuInDatabase(connection, row["MaKH"].ToString(), newGhiChu);
                    }

                    // Display the updated DataTable in the DataGridView
                    dataGridView1.DataSource = dataTable;
                }
            }
        }

        // Function to update GhiChu in the database
        private void UpdateGhiChuInDatabase(SqlConnection connection, string maKH, string newGhiChu)
        {
            string updateQuery = "UPDATE KH SET GhiChu = @GhiChu WHERE MaKH = @MaKH";
            using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
            {
                updateCommand.Parameters.AddWithValue("@GhiChu", newGhiChu);
                updateCommand.Parameters.AddWithValue("@MaKH", maKH);
                updateCommand.ExecuteNonQuery();
            }
        }
    }
}
