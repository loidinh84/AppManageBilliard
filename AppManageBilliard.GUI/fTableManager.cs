using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using AppManageBilliard.DTO;
using AppManageBilliard.BUS;
using System.Globalization;
using MenuDTO = AppManageBilliard.DTO.Menu;

namespace AppManageBilliard.GUI
{
    public partial class fTableManager : Form
    {
        private Table currentTable;
        public fTableManager()
        {
            InitializeComponent();
            LoadTable();
            LoadFoodToTab();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        void LoadTable()
        {
            flpTable.Controls.Clear();
            List<Table> tableList = TableBUS.Instance.LoadTableList();
            foreach (Table item in tableList)
            {
                Button btn = new Button();
                btn.Width = 100;
                btn.Height = 100;
                btn.Text = item.Name + Environment.NewLine + item.Status;
                btn.Tag = item;
                switch (item.Status)
                {
                    case "Trống":
                        btn.BackColor = Color.Aqua;
                        break;
                    default:
                        btn.BackColor = Color.LightPink;
                        break;
                }

                btn.Click += btn_Click;
                btn.Tag = item;

                flpTable.Controls.Add(btn);
            }
        }

        void btn_Click(object sender, EventArgs e)
        {
            Table table = (sender as Button).Tag as Table;

            this.currentTable = table;

            lblCurrentTable.Text = table.Name;
            lsvBill.Tag = (sender as Button).Tag;

            ShowBill(table.ID);
        }

        void ShowBill(int id)
        {
            lsvBill.Items.Clear();
            List<MenuDTO> listMenu = MenuBUS.Instance.GetListMenuByTable(id);

            float totalMoney = 0;

            foreach (MenuDTO item in listMenu)
            {
                ListViewItem lsvItem = new ListViewItem(item.FoodName.ToString());
                lsvItem.SubItems.Add(item.Count.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.TotalPrice.ToString());
                totalMoney += item.TotalPrice;
                lsvBill.Items.Add(lsvItem);
            }
            CultureInfo culture = new CultureInfo("vi-VN");
            txtTongTien.Text = totalMoney.ToString("c", culture);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        void LoadFoodToTab()
        {
            flpFood.Controls.Clear();

            List<Food> listFood = FoodBUS.Instance.GetFoodByCategoryID(1);

            foreach (AppManageBilliard.DTO.Food item in listFood)
            {
                Button btn = new Button();
                btn.Width = 100;
                btn.Height = 100;

                btn.Text = item.Name + Environment.NewLine + item.Price + " đ";
                btn.Tag = item; 
                btn.BackColor = Color.LightYellow;

                btn.Click += btnFood_Click;

                flpFood.Controls.Add(btn);
            }
        }
        void btnFood_Click(object sender, EventArgs e)
        {
            if (this.currentTable == null)
            {
                MessageBox.Show("Vui lòng quay lại tab Bàn để chọn bàn trước!");
                return;
            }

            int idBill = BillBUS.Instance.GetUncheckBillID(currentTable.ID);
            int foodID = ((sender as Button).Tag as AppManageBilliard.DTO.Food).ID;
            int count = 1;

            if (idBill == -1) 
            {
                BillBUS.Instance.InsertBill(currentTable.ID);

                int newBillID = BillBUS.Instance.GetUncheckBillID(currentTable.ID);

                BillBUS.Instance.InsertBillInfo(newBillID, foodID, count);
            }
            else 
            {
                BillBUS.Instance.InsertBillInfo(idBill, foodID, count);
            }

            ShowBill(currentTable.ID);
            LoadTable();
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;
            if (table == null)
            {
                return;
            }
            int idBill = BillBUS.Instance.GetUncheckBillID(table.ID);
            double totalPrice = Convert.ToDouble(txtTongTien.Text.Split(' ')[0].Replace(".", "").Replace(",", ""));
            if (idBill != -1)
            {
                string message = string.Format("Bạn có chắc muốn thanh toán hóa đơn cho {0}?\nTổng tiền: {1} đ", table.Name, totalPrice);
                if (MessageBox.Show(message, "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    BillBUS.Instance.CheckOut(idBill);
                    ShowBill(table.ID);
                    LoadTable();
                }
            }
            else
            {
                MessageBox.Show("Bàn chưa có hóa đơn cần thanh toán!");
            }
        }
    }
}
