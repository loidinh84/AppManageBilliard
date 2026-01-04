using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace AppManageBilliard.GUI
{
    public partial class fStatisticsbyshape : Form
    {
        private DataTable _billData;
        private DateTime _fromDate;
        private DateTime _toDate;

        // ===== Constructor mặc định (Designer bắt buộc) =====
        public fStatisticsbyshape()
        {
            InitializeComponent();
        }

        // ===== Constructor nhận dữ liệu từ fAdmin =====
        public fStatisticsbyshape(DataTable billData, DateTime fromDate, DateTime toDate)
        {
            InitializeComponent();

            _billData = billData;
            _fromDate = fromDate;
            _toDate = toDate;

            this.Load += fStatisticsbyshape_Load;
        }

        private void fStatisticsbyshape_Load(object sender, EventArgs e)
        {
            if (_billData == null || _billData.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để thống kê!", "Thông báo");
                return;
            }

            LoadRevenueChart();
        }

        // ================= VẼ BIỂU ĐỒ DOANH THU 12 THÁNG =================
        private void LoadRevenueChart()
        {
            chartRevenue.Series.Clear();
            chartRevenue.ChartAreas.Clear();
            chartRevenue.Titles.Clear();

            // ===== Chart Area =====
            ChartArea area = new ChartArea("MainArea");
            area.AxisX.Title = "Tháng";
            area.AxisY.Title = "Doanh thu (VNĐ)";
            area.AxisX.Interval = 1;
            area.AxisX.LabelStyle.Angle = 0;
            area.AxisX.MajorGrid.LineColor = Color.LightGray;
            area.AxisY.MajorGrid.LineColor = Color.LightGray;

            chartRevenue.ChartAreas.Add(area);

            // ===== Series =====
            Series series = new Series("Doanh thu");
            series.ChartType = SeriesChartType.Column;
            series.IsValueShownAsLabel = true;
            series.LabelFormat = "#,##0 VNĐ";
            series.Color = Color.FromArgb(0, 123, 255);

            int year = _fromDate.Year;

            // ===== GROUP BY THÁNG =====
            var data = _billData.AsEnumerable()
                .Where(r =>
                    r["Ngày vào"] != DBNull.Value &&
                    r["Tổng tiền"] != DBNull.Value &&
                    r.Field<DateTime>("Ngày vào").Year == year
                )
                .GroupBy(r => r.Field<DateTime>("Ngày vào").Month)
                .Select(g => new
                {
                    Month = g.Key,
                    Total = g.Sum(r => Convert.ToDecimal(r["Tổng tiền"]))
                })
                .ToList();

            // ===== VẼ ĐỦ 12 THÁNG (THÁNG KHÔNG CÓ DOANH THU = 0) =====
            for (int month = 1; month <= 12; month++)
            {
                var item = data.FirstOrDefault(x => x.Month == month);
                decimal total = item != null ? item.Total : 0;

                series.Points.AddXY($"Tháng {month}", total);
            }

            chartRevenue.Series.Add(series);

            // ===== Title =====
            chartRevenue.Titles.Add(
                $"DOANH THU 12 THÁNG NĂM {year}"
            );
        }

        // Event theo yêu cầu (để trống)
        private void chart1_Click(object sender, EventArgs e)
        {
            // Không cần xử lý
        }
    }
}
