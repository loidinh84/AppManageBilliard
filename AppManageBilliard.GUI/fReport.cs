using AppManageBilliard.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace AppManageBilliard.GUI
{
    public partial class fReport : Form
    {
        public cpBill report = new cpBill();
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

        public void InitReport()
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("USP_GetListBillByTableForReport @idTable", new object[] { idTable });

            string noiDung = "Thanh toan " + tenBan;
            string bank = ConfigurationManager.AppSettings["BankName"];
            string acc = ConfigurationManager.AppSettings["AccountNumber"];
            string qrUrl = string.Format("https://img.vietqr.io/image/{0}-{1}-qr_only.jpg?amount={2}&addInfo={3}", bank, acc, tongTien, noiDung);
            byte[] qrImage = GetImageFromUrl(qrUrl);

            data.Columns.Add("qrCode", typeof(byte[]));
            foreach (DataRow row in data.Rows)
            {
                row["qrCode"] = qrImage;
            }

            BillDataSet dataSet = new BillDataSet();
            dataSet.Tables["dtBill"].Merge(data);

            report.SetDataSource(dataSet);
            report.SetParameterValue("pGioChoi", this.gioChoi);
            report.SetParameterValue("pTienGio", this.tienGio);
            report.SetParameterValue("pTongTien", this.tongTien);
            report.SetParameterValue("pTenBan", this.tenBan);

            crystalReportViewer1.ReportSource = report;
        }

        private void fReport_Load(object sender, EventArgs e)
        {
            InitReport();
        }
        private byte[] GetImageFromUrl(string url)
        {
            using (System.Net.WebClient webClient = new System.Net.WebClient())
            {
                return webClient.DownloadData(url);
            }
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}
