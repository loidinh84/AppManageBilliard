using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AppManageBilliard.DTO;
using AppManageBilliard.BUS;

namespace AppManageBilliard.GUI
{
    public partial class fSwitchTable : Form
    {
        public Table SelectedTable { get; set; }
        private int currentTableID;
        public fSwitchTable(int tableID)
        {
            InitializeComponent();
            currentTableID = tableID;
            LoadTableList();
        }
        private void fSwitchTable_Load(object sender, EventArgs e)
        {

        }
        void LoadTableList()
        {
            flpTableList.Controls.Clear();
            List<Table> tableList = TableBUS.Instance.LoadTableList();

            foreach (Table item in tableList)
            {
                if (item.ID == currentTableID) continue;

                Button btn = new Button() { Width = 90, Height = 90 };
                btn.Text = item.Name + Environment.NewLine + item.Status;
                btn.Tag = item;

                if (item.Status == "Trống")
                    btn.BackColor = Color.Aqua;
                else
                    btn.BackColor = Color.LightPink;

                btn.Click += Btn_Click;
                flpTableList.Controls.Add(btn);
            }
        }
        private void Btn_Click(object sender, EventArgs e)
        {
            foreach (Button b in flpTableList.Controls)
            {
                Table t = b.Tag as Table;
                if (t.Status == "Trống") b.BackColor = Color.Aqua;
                else b.BackColor = Color.LightPink;
            }

            Button btn = sender as Button;
            btn.BackColor = Color.Yellow;
            this.SelectedTable = btn.Tag as Table;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (this.SelectedTable == null)
            {
                MessageBox.Show("Vui lòng chọn một bàn để chuyển!", "Thông báo");
                return;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
