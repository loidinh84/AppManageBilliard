using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using AppManageBilliard.DTO;
using AppManageBilliard.BUS;
using System.Globalization;
using MenuDTO = AppManageBilliard.DTO.Menu;
using AppManageBilliard.DAL;

namespace AppManageBilliard.GUI
{
    public partial class fTableManager : Form
    {
        private Account loginAccount;
        public Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccount(loginAccount.Type); }
        }
        private Table currentTable;

        public fTableManager(Account acc)
        {
            InitializeComponent();
            this.LoginAccount = acc;
            CustomizeDesign();      // Áp dụng giao diện trắng xanh mới
            LoadTable();
            LoadFoodToTab();
            LoadDiscount();
        }

        public fTableManager()
        {
            InitializeComponent();
        }

        // Hàm tùy chỉnh giao diện trắng - xanh sạch sẽ
        private void CustomizeDesign()
        {
            // Nền form trắng nhẹ, sạch
            this.BackColor = Color.FromArgb(240, 248, 255); // AliceBlue rất nhẹ

            // Font chung hiện đại, dễ đọc
            this.Font = new Font("Segoe UI", 10F, FontStyle.Regular);

            // MenuStrip (nếu có)
            if (menuStrip1 != null)
            {
                menuStrip1.BackColor = Color.White;
                menuStrip1.ForeColor = Color.FromArgb(30, 144, 255); // Xanh dương
            }

            // Tiêu đề bàn hiện tại
            lblCurrentTable.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblCurrentTable.ForeColor = Color.FromArgb(0, 128, 255); // Xanh dương đậm
            lblCurrentTable.TextAlign = ContentAlignment.MiddleCenter;

            // ListView hóa đơn
            lsvBill.BackColor = Color.White;
            lsvBill.ForeColor = Color.Black;
            lsvBill.GridLines = true;
            lsvBill.FullRowSelect = true;
            lsvBill.Font = new Font("Segoe UI", 11F);

            // Thiết lập cột nếu chưa có (trong Designer bạn có thể thêm sẵn)
            if (lsvBill.Columns.Count == 0)
            {
                lsvBill.Columns.Add("Tên món", 220);
                lsvBill.Columns.Add("S.lượng", 90, HorizontalAlignment.Center);
                lsvBill.Columns.Add("Đơn giá", 120, HorizontalAlignment.Right);
                lsvBill.Columns.Add("Thành tiền", 140, HorizontalAlignment.Right);
            }

            // ComboBox giảm giá
            cbDiscount.FlatStyle = FlatStyle.Flat;
            cbDiscount.BackColor = Color.White;
            cbDiscount.ForeColor = Color.Black;

            // Tổng tiền nổi bật
            txtTongTien.ReadOnly = true;
            txtTongTien.BackColor = Color.FromArgb(0, 150, 136); // Teal xanh lá
            txtTongTien.ForeColor = Color.White;
            txtTongTien.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            txtTongTien.TextAlign = HorizontalAlignment.Right;
            txtTongTien.BorderStyle = BorderStyle.None;
            txtTongTien.Text = "0 đ";

            // Nút chức năng đẹp hơn
            btnChuyenBan.FlatStyle = FlatStyle.Flat;
            btnChuyenBan.BackColor = Color.FromArgb(255, 140, 0); // Cam nổi bật
            btnChuyenBan.ForeColor = Color.White;
            btnChuyenBan.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnChuyenBan.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 170, 70);

            btnThanhToan.FlatStyle = FlatStyle.Flat;
            btnThanhToan.BackColor = Color.FromArgb(40, 167, 69); // Xanh lá thành công
            btnThanhToan.ForeColor = Color.White;
            btnThanhToan.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnThanhToan.FlatAppearance.MouseOverBackColor = Color.FromArgb(70, 200, 100);
        }

        void LoadTable()
        {
            flpTable.Controls.Clear();
            List<Table> tableList = TableBUS.Instance.LoadTableList();

            foreach (Table item in tableList)
            {
                Button btn = new Button
                { 
                    // chiều cao và rộng để tạo ra viên thuốc trị Khùng
                    Width = 170,   
                    Height = 80,  
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Margin = new Padding(15) 
                };

                btn.FlatAppearance.BorderSize = 0;
                if (item.Status == "Trống")
                {
                    //màu của nền viên thuốc
                    btn.BackColor = Color.FromArgb(0, 119, 190);
                    // màu của chữ trong viên thuốc
                    btn.ForeColor = Color.FromArgb(255, 255, 255);                
                    btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(240, 248, 255); 
                }
                else
                {
                    btn.BackColor = Color.FromArgb(30, 144, 255);                 
                    btn.ForeColor = Color.White;                              
                    btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 120, 255);   
                }

                btn.Text = item.Name + Environment.NewLine + item.Status;
                btn.Tag = item;
                btn.Click += btn_Click;
                int diameter = btn.Height; 

                System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
                path.AddArc(0, 0, diameter, diameter, 180, 90);                   
                path.AddArc(btn.Width - diameter, 0, diameter, diameter, 270, 90); 
                path.AddArc(btn.Width - diameter, btn.Height - diameter, diameter, diameter, 0, 90);   
                path.AddArc(0, btn.Height - diameter, diameter, diameter, 90, 90); 
                path.CloseAllFigures();

                btn.Region = new Region(path);
             

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
                ListViewItem lsvItem = new ListViewItem("  " + item.FoodName);
                lsvItem.SubItems.Add(item.Count.ToString());
                lsvItem.SubItems.Add(item.Price.ToString("N0") + " đ");
                lsvItem.SubItems.Add(item.TotalPrice.ToString("N0") + " đ");

                // Thành tiền nổi bật đỏ
                lsvItem.SubItems[3].ForeColor = Color.FromArgb(220, 53, 69);
                lsvItem.SubItems[3].Font = new Font("Segoe UI", 11F, FontStyle.Bold);

                totalMoney += item.TotalPrice;
                lsvBill.Items.Add(lsvItem);
            }

            CultureInfo culture = new CultureInfo("vi-VN");
            txtTongTien.Text = totalMoney > 0 ? totalMoney.ToString("c", culture) : "0 đ";
            txtTongTien.Tag = totalMoney;
        }

        void LoadFoodToTab()
        {
            flpFood.Controls.Clear();
            List<Food> listFood = FoodBUS.Instance.GetFoodByCategoryID(1);

            foreach (Food item in listFood)
            {
                Button btn = new Button
                {
                    Width = 120,
                    Height = 120,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = Color.White,
                    BackColor = Color.FromArgb(255, 193, 7), // Vàng ấm cho món ăn/đồ uống
                    TextAlign = ContentAlignment.MiddleCenter,
                    Margin = new Padding(15)
                };

                btn.FlatAppearance.BorderSize = 0;
                btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 213, 100);

                btn.Text = item.Name + Environment.NewLine + item.Price.ToString("N0") + " đ";
                btn.Tag = item;
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
            if (table == null) return;

            int idBill = BillDAL.Instance.GetUncheckBillIDByTableID(table.ID);
            DiscountItem selectedDiscount = cbDiscount.SelectedItem as DiscountItem;
            int discount = selectedDiscount.Value;
            double totalPrice = Convert.ToDouble(txtTongTien.Tag);
            double finalTotalPrice = totalPrice - (totalPrice / 100) * discount;

            if (idBill != -1)
            {
                string msg = string.Format("Thanh toán cho {0}\n{1}\nTổng tiền: {2:N0} đ\n\nCẦN TRẢ: {3:N0} đ",
                                    table.Name, selectedDiscount.Name, totalPrice, finalTotalPrice);

                if (MessageBox.Show(msg, "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    BillDAL.Instance.CheckOut(idBill, discount, (float)finalTotalPrice);
                    ShowBill(table.ID);
                    LoadTable();
                    MessageBox.Show("Thanh toán thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAdmin f = new fAdmin();
            f.ShowDialog();
        }

        private void thôngTinTàiKhoảnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAccountProfile f = new fAccountProfile(loginAccount);
            f.UpdateAccount += f_UpdateAccount;
            f.ShowDialog();
        }

        void ChangeAccount(int type)
        {
            adminToolStripMenuItem.Enabled = type == 1;
            thôngTinTàiKhoảnToolStripMenuItem.Text = "Thông tin tài khoản (" + loginAccount.DisplayName + ")";
        }

        void f_UpdateAccount(object sender, AccountEvent e)
        {
            this.thôngTinTàiKhoảnToolStripMenuItem.Text = "Thông tin tài khoản (" + e.Acc.DisplayName + ")";
        }

        // Các event khác giữ nguyên
        private void listView1_SelectedIndexChanged(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void fTableManager_Load(object sender, EventArgs e) { }
        private void flpTable_Paint(object sender, PaintEventArgs e) { }
    }
}