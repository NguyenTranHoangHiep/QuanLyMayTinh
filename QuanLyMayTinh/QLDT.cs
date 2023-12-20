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
using System.Windows.Forms.DataVisualization.Charting;

namespace QuanLyMayTinh
{
    public partial class QLDT : Form
    {
        string ketnoi = "Server=DESKTOP-K0TD8GJ\\SQLEXPRESS;Database=QLMT;Trusted_Connection=True;";
        public QLDT()
        {
            InitializeComponent();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            string Nam = dateTimePicker1.Value.Year.ToString();
            using (SqlConnection connection = new SqlConnection(ketnoi))
            {
                connection.Open();

                // First Chart - Total Revenue by Month
                string query1 = $"SELECT YEAR(NgayLap) AS Năm, MONTH(NgayLap) AS Tháng, SUM(DonHangChiTiet.ThanhTien) AS [Tổng doanh thu] FROM DonHangChiTiet WHERE YEAR(NgayLap) = {Nam} GROUP BY YEAR(NgayLap), MONTH(NgayLap)";
                using (SqlCommand command1 = new SqlCommand(query1, connection))
                {
                    SqlDataReader reader1 = command1.ExecuteReader();

                    // Xóa dữ liệu cũ trong biểu đồ
                    chart1.Series.Clear();

                    // Tạo một loạt dữ liệu mới
                    Series series1 = new Series("DoanhThu");

                    while (reader1.Read())
                    {
                        int thang = reader1.GetInt32(reader1.GetOrdinal("Tháng"));
                        decimal tongDoanhThu = Convert.ToDecimal(reader1["Tổng doanh thu"]);

                        // Thêm điểm dữ liệu cho từng tháng
                        series1.Points.AddXY(thang, tongDoanhThu);
                    }

                    Title chartTitle1 = new Title("Tổng doanh thu theo tháng trong năm", Docking.Top, new Font("Arial", 10, FontStyle.Bold), Color.DeepSkyBlue);
                    chart1.Titles.Clear();
                    chart1.Titles.Add(chartTitle1);

                    // Đặt kiểu biểu đồ (bạn có thể thay đổi tùy thuộc vào sở thích của bạn)
                    series1.ChartType = SeriesChartType.Column;

                    // Add the series to the chart
                    chart1.Series.Add(series1);

                    // Đặt tiêu đề cho trục X và trục Y
                    chart1.ChartAreas["ChartArea1"].AxisX.Title = "Tháng";
                    chart1.ChartAreas["ChartArea1"].AxisY.Title = "Tổng doanh thu";

                    // Hiển thị biểu đồ
                    chart1.Visible = true;

                    reader1.Close();
                }

                // Second Chart - Product Types
                string query2 = $"SELECT YEAR(DonHangChiTiet.NgayLap) AS Năm, SP.LoaiSP AS [Loại sản phẩm], " +
                $"SUM(DonHangChiTiet.SoLuong) AS [Số lượng] " +
                $"FROM DonHangChiTiet " +
                $"INNER JOIN SP ON DonHangChiTiet.MaSP = SP.MaSP " +
                $"WHERE YEAR(DonHangChiTiet.NgayLap) = {Nam} " +
                $"GROUP BY YEAR(DonHangChiTiet.NgayLap), SP.LoaiSP " +
                $"ORDER BY Năm, [Số lượng] DESC";

                using (SqlCommand command2 = new SqlCommand(query2, connection))
                {
                    SqlDataReader reader2 = command2.ExecuteReader();

                    // Xóa dữ liệu cũ trong biểu đồ
                    chart2.Series.Clear();

                    // Tạo một loạt dữ liệu mới
                    Series series2 = new Series("Loại sản phẩm");

                    while (reader2.Read())
                    {
                        string nam = reader2.GetInt32(reader2.GetOrdinal("Năm")).ToString();
                        string loaiSP = reader2.GetString(reader2.GetOrdinal("Loại sản phẩm"));
                        int soLuong = reader2.GetInt32(reader2.GetOrdinal("Số Lượng"));

                        // Thêm điểm dữ liệu cho từng loại sản phẩm
                        series2.Points.AddXY($"{loaiSP} ({soLuong})", nam);
                    }
                    series2.ChartType = SeriesChartType.Pie;

                    // Add the series to the chart
                    chart2.Series.Add(series2);
                    chart2.Titles.Clear();  // Xóa tất cả các tiêu đề cũ

                    Title chartTitle2 = new Title("Sản phẩm được mua nhiều trong năm", Docking.Top, new Font("Arial", 10, FontStyle.Bold), Color.DeepSkyBlue);
                    chartTitle2.Alignment = ContentAlignment.MiddleLeft;  // Đặt căn lề trái
                    chart2.Titles.Add(chartTitle2);

                    // Hiển thị biểu đồ
                    chart2.Visible = true;
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void chart1_Click_1(object sender, EventArgs e)
        {

        }
    }
}
