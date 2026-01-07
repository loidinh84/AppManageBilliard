using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using AppManageBilliard.DTO;
using AppManageBilliard.BUS;

namespace AppManageBilliard.GUI
{
    public partial class fSwitchTable : Form
    {
        public Table SelectedTable { get; set; }
        private int currentTableID;

        // ====== MÀU CHUẨN ======
        private readonly Color colorEmpty = Color.FromArgb(144, 238, 144);   // xanh nhạt
        private readonly Color colorBusy = Color.FromArgb(255, 182, 193);   // hồng nhạt
        private readonly Color colorSelect = Color.FromArgb(255, 215, 0);    // vàng chọn

        public fSwitchTable(int tableID)
        {
            InitializeComponent();
            currentTableID = tableID;
        }

        private void fSwitchTable_Load(object sender, EventArgs e)
        {
            this.Text = "Chuyển bàn";
            this.StartPosition = FormStartPosition.CenterParent;
            LoadTableList();
        }

        // ================= LOAD TABLE =================
        void LoadTableList()
        {
            flpTableList.Controls.Clear();
            List<Table> tableList = TableBUS.Instance.LoadTableList();

            foreach (Table item in tableList)
            {
                if (item.ID == currentTableID) continue;

                Button btn = CreateTableButton(item);
                flpTableList.Controls.Add(btn);
            }
        }

        // ================= CREATE BUTTON =================
        Button CreateTableButton(Table table)
        {
            Button btn = new Button();
            btn.Width = 95;
            btn.Height = 95;
            btn.Tag = table;
            btn.TextAlign = ContentAlignment.MiddleCenter;
            btn.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btn.Text = $"{table.Name}\n({table.Status})";
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Cursor = Cursors.Hand;

            btn.BackColor = table.Status == "Trống" ? colorEmpty : colorBusy;
            btn.Click += Btn_Click;

            return btn;
        }
        private void Btn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            Table table = btn.Tag as Table;

            if (table.Status != "Trống")
            {
                MessageBox.Show(
                    "Bàn này đang có khách, không thể chuyển!",
                    "Cảnh báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }
            foreach (Button b in flpTableList.Controls)
            {
                Table t = b.Tag as Table;
                b.BackColor = (t.Status == "Trống") ? colorEmpty : colorBusy;
            }

            btn.BackColor = colorSelect;
            SelectedTable = table;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (SelectedTable == null)
            {
                MessageBox.Show("Vui lòng chọn một bàn để chuyển!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
