using AppManageBida.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppManageBilliard.GUI
{
    public partial class fReport : Form
    {
        private int idTable;
        private string gioChoi;
        private double tienGio;
        private double tongTien;
        private string tenBan;
        public fReport(int id, string gio, double tien, double tong, string ten)
        {
            InitializeComponent();
            this.idTable = id;
            this.gioChoi = gio;
            this.tienGio = tien;
            this.tongTien = tong;
            this.tenBan = ten;  
        }

        private void fReport_Load(object sender, EventArgs e)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("USP_GetListBillByTableForReport @idTable", new object[] { idTable });

            BillDataSet dataSet = new BillDataSet();
            dataSet.Tables["dtBill"].Merge(data);

            cpBill report = new cpBill();
            report.SetDataSource(dataSet);

            report.SetParameterValue("pGioChoi", this.gioChoi);
            report.SetParameterValue("pTienGio", this.tienGio);
            report.SetParameterValue("pTongTien", this.tongTien);
            report.SetParameterValue("pTenBan", this.tenBan);

            crystalReportViewer1.ReportSource = report;

            crystalReportViewer1.DisplayToolbar = true;
            crystalReportViewer1.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            crystalReportViewer1.ShowGroupTreeButton = false;
            crystalReportViewer1.ShowParameterPanelButton = false;
            crystalReportViewer1.Zoom(100);
            this.Size = new System.Drawing.Size(500, 700);
        }
    }
}
