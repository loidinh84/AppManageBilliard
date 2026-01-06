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

        public fStatisticsbyshape()
        {
            InitializeComponent();
        }

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

        private void LoadRevenueChart()
        {
            chartRevenue.Series.Clear();
            chartRevenue.ChartAreas.Clear();
            chartRevenue.Titles.Clear();

            // ===== CHART AREA =====
            ChartArea area = new ChartArea("MainArea");
           
            area.AxisY.Title = "Doanh thu (VNĐ)";

            area.AxisX.Interval = 1;
            area.AxisX.IsLabelAutoFit = false;
            area.AxisX.IsMarginVisible = false;   
            area.AxisX.Minimum = 0.5;             
            area.AxisX.Maximum = 12.5;            
            area.AxisX.MajorGrid.LineColor = Color.LightGray;

            area.AxisY.Minimum = 0;
            area.AxisY.Maximum = 100000000;
            area.AxisY.MajorGrid.LineColor = Color.LightGray;

            area.AxisX.CustomLabels.Clear();

            chartRevenue.ChartAreas.Add(area);

            // ===== SERIES =====
            Series series = new Series("Doanh thu");
            series.ChartType = SeriesChartType.Column;
            series.IsValueShownAsLabel = true;
            series.LabelFormat = "#,##0 VNĐ";
            series.Color = Color.FromArgb(0, 123, 255);
            series.XValueType = ChartValueType.Int32;

            int year = _fromDate.Year;

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

            for (int month = 1; month <= 12; month++)
            {
                var item = data.FirstOrDefault(x => x.Month == month);
                decimal total = item != null ? item.Total : 0;

                int index = series.Points.AddXY(month, total);
                series.Points[index].AxisLabel = $"Tháng {month}";
            }

            chartRevenue.Series.Add(series);

            chartRevenue.Titles.Add($"DOANH THU 12 THÁNG NĂM {year}");
        }

        private void chart1_Click(object sender, EventArgs e)
        {
        }
    }
}
