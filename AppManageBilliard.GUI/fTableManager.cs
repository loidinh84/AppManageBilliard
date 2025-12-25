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

            lblCurrentTable.Text = "Đang chọn: " + table.Name;

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
            // Gợi ý: Bạn nên viết thêm hàm GetListFood() lấy tất cả món trong FoodBUS và FoodDAO

            foreach (AppManageBilliard.DTO.Food item in listFood)
            {
                Button btn = new Button();
                btn.Width = 100;
                btn.Height = 100;

                // Hiện tên món + giá tiền
                btn.Text = item.Name + Environment.NewLine + item.Price + " đ";
                btn.Tag = item; // Lưu món ăn vào nút
                btn.BackColor = Color.LightYellow;

                // Gắn sự kiện click chọn món
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
    }
}
