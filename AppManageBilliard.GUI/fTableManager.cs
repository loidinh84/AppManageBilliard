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
            LoadDiscount();
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
            cbDiscount.SelectedIndex = 0;
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
            txtTongTien.Tag = totalMoney;
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
            DiscountItem selectedDiscount = cbDiscount.SelectedItem as DiscountItem;
            int discount = selectedDiscount.Value;
            double totalPrice = Convert.ToDouble(txtTongTien.Tag);
            double finalTotalPrice = totalPrice - (totalPrice /100) * discount;

            if (idBill != -1)
            {
                string msg = string.Format("Thanh toán cho {0}\n{1}\nTổng tiền: {2}\n\nCẦN TRẢ: {3}",
                                    table.Name,
                                    selectedDiscount.Name,
                                    totalPrice,
                                    finalTotalPrice);

                if (MessageBox.Show(msg, "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    BillBUS.Instance.CheckOut(idBill, discount);
                    ShowBill(table.ID);
                    LoadTable();
                    cbDiscount.SelectedIndex = 0;
                }
            }
        }

        private void btnChuyenBan_Click(object sender, EventArgs e)
        {
            Table tableOld = lsvBill.Tag as Table;
            if (tableOld == null)
            {
                MessageBox.Show("Hãy chọn bàn cần chuyển trước!");
                return;
            }
            fSwitchTable f = new fSwitchTable(tableOld.ID);
            if (f.ShowDialog() == DialogResult.OK)
            {
                Table tableNew = f.SelectedTable;

                string msg = string.Format("Xác nhận chuyển {0} sang {1}?", tableOld.Name, tableNew.Name);
                if (MessageBox.Show(msg, "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    TableBUS.Instance.SwitchTable(tableOld.ID, tableNew.ID);
                    LoadTable();
                    ShowBill(tableOld.ID);

                    MessageBox.Show("Chuyển bàn thành công!", "Thông báo");
                }
            }
        }
        public class DiscountItem
        {
            public string Name { get; set; }
            public int Value { get; set; } 

            public DiscountItem(string name, int value)
            {
                this.Name = name;
                this.Value = value;
            }

            public override string ToString()
            {
                return Name;
            }
        }
        void LoadDiscount()
        {
            List<DiscountItem> listDiscount = new List<DiscountItem>();
            listDiscount.Add(new DiscountItem("Giảm 0%", 0));
            listDiscount.Add(new DiscountItem("Giảm 5%", 5));
            listDiscount.Add(new DiscountItem("Giảm 10%", 10));
            listDiscount.Add(new DiscountItem("Giảm 20%", 20));
            listDiscount.Add(new DiscountItem("Giảm 50%", 50));
            listDiscount.Add(new DiscountItem("Giảm 100%", 100));

            cbDiscount.DataSource = listDiscount;
             cbDiscount.DisplayMember = "Name";
        }
    }
}
