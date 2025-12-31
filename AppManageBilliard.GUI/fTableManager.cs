using AppManageBida.DAL;
using AppManageBilliard.BUS;
using AppManageBilliard.DAL;
using AppManageBilliard.DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;
using MenuDTO = AppManageBilliard.DTO.Menu;

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
            CustomizeDesign();      
            LoadTable();
            LoadFoodToTab();
            LoadDiscount();
            BoTronButton(btnChuyenBan, 25, Color.FromArgb(255, 140, 0), Color.FromArgb(255, 170, 70));
            BoTronButton(btnThanhToan, 25, Color.FromArgb(40, 167, 69), Color.FromArgb(70, 200, 100));
            TaoHinhVienThuoc(btnChuyenBan, btnChuyenBan.Width, btnChuyenBan.Height,
                          Color.FromArgb(255, 140, 0),
                          Color.FromArgb(255, 170, 70));

            TaoHinhVienThuoc(btnThanhToan, btnThanhToan.Width, btnThanhToan.Height,
                          Color.FromArgb(40, 167, 69),
                          Color.FromArgb(70, 200, 100));
        }

        public fTableManager()
        {
            InitializeComponent();
        }

        private void CustomizeDesign()
        {
            this.BackColor = Color.FromArgb(240, 248, 255);

            this.Font = new Font("Segoe UI", 10F, FontStyle.Regular);

            if (menuStrip1 != null)
            {
                menuStrip1.BackColor = Color.White;
                menuStrip1.ForeColor = Color.FromArgb(30, 144, 255); // Xanh dương
            }

            lblCurrentTable.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblCurrentTable.ForeColor = Color.FromArgb(0, 128, 255); // Xanh dương đậm
            lblCurrentTable.TextAlign = ContentAlignment.MiddleCenter;

            lsvBill.BackColor = Color.White;
            lsvBill.ForeColor = Color.Black;
            lsvBill.GridLines = true;
            lsvBill.FullRowSelect = true;
            lsvBill.Font = new Font("Segoe UI", 11F);

            if (lsvBill.Columns.Count == 0)
            {
                lsvBill.Columns.Add("Tên món", 220);
                lsvBill.Columns.Add("S.lượng", 90, HorizontalAlignment.Center);
                lsvBill.Columns.Add("Đơn giá", 120, HorizontalAlignment.Right);
                lsvBill.Columns.Add("Thành tiền", 140, HorizontalAlignment.Right);
            }

            cbDiscount.FlatStyle = FlatStyle.Flat;
            cbDiscount.BackColor = Color.White;
            cbDiscount.ForeColor = Color.Black;

            txtTongTien.BackColor = Color.Transparent;
            txtTongTien.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            txtTongTien.TextAlign = ContentAlignment.MiddleRight;
            txtTongTien.Text = "0 đ";

            btnChuyenBan.FlatStyle = FlatStyle.Flat;
            btnChuyenBan.BackColor = Color.FromArgb(255, 140, 0);
            btnChuyenBan.ForeColor = Color.White;
            btnChuyenBan.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnChuyenBan.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 170, 70);

            btnThanhToan.FlatStyle = FlatStyle.Flat;
            btnThanhToan.BackColor = Color.FromArgb(40, 167, 69);
            btnThanhToan.ForeColor = Color.White;
            btnThanhToan.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnThanhToan.FlatAppearance.MouseOverBackColor = Color.FromArgb(70, 200, 100);
        }

        void BoTronButton(Button btn, int radius, Color backColor, Color hoverColor)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = Color.Transparent;
            btn.Cursor = Cursors.Hand;
            btn.ForeColor = Color.White;

            btn.TabStop = false;

            Color parentColor = btn.Parent != null ? btn.Parent.BackColor : Color.White;
            btn.FlatAppearance.BorderColor = parentColor;

            btn.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btn.FlatAppearance.MouseOverBackColor = Color.Transparent;

            Color originalColor = backColor;
            Color curColor = backColor;

            btn.MouseEnter += (s, e) => { curColor = hoverColor; btn.Invalidate(); };
            btn.MouseLeave += (s, e) => { curColor = originalColor; btn.Invalidate(); };

            btn.Paint += (sender, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                Color currentParentColor = btn.Parent != null ? btn.Parent.BackColor : Color.White;

                RectangleF rect = new RectangleF(0, 0, btn.Width, btn.Height);
                using (SolidBrush brushParent = new SolidBrush(currentParentColor))
                {
                    e.Graphics.FillRectangle(brushParent, rect);
                }

                GraphicsPath path = new GraphicsPath();
                path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                path.AddArc(rect.Width - radius, rect.Y, radius, radius, 270, 90);
                path.AddArc(rect.Width - radius, rect.Height - radius, radius, radius, 0, 90);
                path.AddArc(rect.X, rect.Height - radius, radius, radius, 90, 90);
                path.CloseFigure();

                using (SolidBrush brushBtn = new SolidBrush(curColor))
                {
                    e.Graphics.FillPath(brushBtn, path);
                }

                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;

                using (SolidBrush brushText = new SolidBrush(btn.ForeColor))
                {
                    e.Graphics.DrawString(btn.Text, btn.Font, brushText, rect, sf);
                }
            };
        }

        void TaoHinhVienThuoc(Button btn, int width, int height, Color backColor, Color hoverColor)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
            btn.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btn.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btn.BackColor = Color.Transparent;

            btn.TabStop = false;
            btn.Cursor = Cursors.Hand;
            btn.ForeColor = Color.White;

            btn.Width = width;
            btn.Height = height;

            Color originalColor = backColor;
            Color curColor = backColor;

            btn.MouseEnter += (s, e) => { curColor = hoverColor; btn.Invalidate(); };
            btn.MouseLeave += (s, e) => { curColor = originalColor; btn.Invalidate(); };

            btn.Paint += (sender, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                Color parentColor = btn.Parent != null ? btn.Parent.BackColor : Color.White;
                using (SolidBrush brushParent = new SolidBrush(parentColor))
                {
                    e.Graphics.FillRectangle(brushParent, new RectangleF(-1, -1, btn.Width + 2, btn.Height + 2));
                }

                GraphicsPath path = new GraphicsPath();
                float diameter = btn.Height;

                path.AddArc(0, 0, diameter, diameter, 180, 90);
                path.AddArc(btn.Width - diameter, 0, diameter, diameter, 270, 90);
                path.AddArc(btn.Width - diameter, btn.Height - diameter, diameter, diameter, 0, 90);
                path.AddArc(0, btn.Height - diameter, diameter, diameter, 90, 90);
                path.CloseAllFigures();

                using (SolidBrush brushBtn = new SolidBrush(curColor))
                {
                    e.Graphics.FillPath(brushBtn, path);
                }

                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;

                using (SolidBrush brushText = new SolidBrush(Color.White))
                {
                    e.Graphics.DrawString(btn.Text, btn.Font, brushText, new RectangleF(0, 0, btn.Width, btn.Height), sf);
                }
            };
        }


        void LoadTable()
        {
            flpTable.Controls.Clear();
            List<Table> tableList = TableBUS.Instance.LoadTableList();

            foreach (Table item in tableList)
            {
                Button btn = new Button();
                btn.Width = TableDAL.TableWidth;
                btn.Height = TableDAL.TableHeight;

                btn.Text = item.Name + Environment.NewLine + item.Status;
                btn.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
                btn.Tag = item;

                btn.Click += btn_Click;

                if (item.Status == "Trống")
                {
                    TaoHinhVienThuoc(btn, 140, 70,
                                  Color.FromArgb(0, 191, 255),
                                  Color.FromArgb(0, 170, 230));
                }
                else
                {
                    TaoHinhVienThuoc(btn, 140, 70,
                                  Color.FromArgb(255, 182, 193),
                                  Color.FromArgb(250, 80, 130));
                }

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
            if (table.Status == "Trống")
            {
                btnThanhToan.BackColor = Color.FromArgb(40, 167, 69);     
                btnThanhToan.FlatAppearance.MouseOverBackColor = Color.FromArgb(70, 200, 100);
                btnThanhToan.Enabled = false; 
            }
            else
            {
        
                btnThanhToan.BackColor = Color.FromArgb(220, 53, 69);   
                btnThanhToan.FlatAppearance.MouseOverBackColor = Color.FromArgb(245, 90, 110); 
                btnThanhToan.Enabled = true;
            }
        }


        private double tongTienNuoc = 0;
        private double giaGioHienTai = 0;

        void ShowBill(int id)
        {
            lsvBill.Items.Clear();
            List<MenuDTO> listMenu = MenuBUS.Instance.GetListMenuByTable(id);

            tongTienNuoc = 0;
            int tongSoMon = 0;

            giaGioHienTai = TableDAL.Instance.GetPriceByTableID(id);

            foreach (MenuDTO item in listMenu)
            {
                ListViewItem lsvItem = new ListViewItem(" " + item.FoodName);
                lsvItem.SubItems.Add(item.Count.ToString());
                lsvItem.SubItems.Add(item.Price.ToString("N0") + " đ");
                lsvItem.SubItems.Add(item.TotalPrice.ToString("N0") + " đ");

                if (item.IsService == true)
                {
                    giaGioHienTai = item.Price;

                    lsvItem.ForeColor = Color.Blue;
                    lsvItem.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

                }
                else
                {
                    tongTienNuoc += item.TotalPrice;

                    lsvItem.SubItems[3].ForeColor = Color.FromArgb(220, 53, 69);
                    
                    tongSoMon += item.Count;
                }

                lsvBill.Items.Add(lsvItem);
            }

            CultureInfo culture = new CultureInfo("vi-VN");

            txtTongTien.Text = tongTienNuoc > 0 ? tongTienNuoc.ToString("c", culture) : "0 đ";

            if (tongTienNuoc > 0)
            {
                txtTongTien.ForeColor = Color.Red;
                txtTongTien.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
                txtTongMon.Text = tongSoMon.ToString();
            }
            else
            {
                txtTongTien.ForeColor = Color.Gray;
                txtTongTien.Font = new Font("Segoe UI", 15F, FontStyle.Regular);
            }
            int idBill = BillDAL.Instance.GetUncheckBillIDByTableID(id);
            if (idBill != -1)
            {
                DateTime dateCheckIn = BillDAL.Instance.GetDateCheckIn(idBill);
                txtGioVao.Text = dateCheckIn.ToString("HH:mm:ss");

                TimeSpan timeSpan = DateTime.Now - dateCheckIn;
                txtTongGio.Text = string.Format("{0}h {1}p", (int)timeSpan.TotalHours, timeSpan.Minutes);
            }
            else
            {
                txtGioVao.Text = "";
                txtTongGio.Text = "";
                txtTongMon.Text = "0";
            }
        }

        void LoadFoodToTab()
        {
            flpFood.Controls.Clear();
            List<Food> listFood = FoodBUS.Instance.GetFoodByCategoryID(1);

            foreach (Food item in listFood)
            {
                Button btn = new Button
                {
                    Width = 170,   // Chiều rộng lớn hơn để tạo dạng viên thuốc ngang đẹp
                    Height = 80,   // Chiều cao vừa đủ cho 2 dòng chữ
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                    ForeColor = Color.White,
                    BackColor = Color.FromArgb(0, 191, 255), // Xanh dương nhạt giống bàn trống
                    TextAlign = ContentAlignment.MiddleCenter,
                    Margin = new Padding(15)
                };

                // Tắt viền và fix lỗi trắng khi click
                btn.FlatAppearance.BorderSize = 0;
                btn.UseVisualStyleBackColor = false; // Quan trọng: tránh màu trắng hệ thống

                // Hover: xanh sáng hơn (đẹp, mượt)
                btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(100, 210, 255);

                // Khi ấn: xanh đậm hơn (có cảm giác nhấn nút)
                btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(0, 170, 230);

                btn.Text = item.Name + Environment.NewLine + item.Price.ToString("N0") + " đ";
                btn.Tag = item;
                btn.Click += btnFood_Click;

                // === BO TRÒN VIÊN THUỐC GIỐNG HỆT BÀN ===
                int diameter = btn.Height;
                System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
                path.AddArc(0, 0, diameter, diameter, 180, 90);
                path.AddArc(btn.Width - diameter, 0, diameter, diameter, 270, 90);
                path.AddArc(btn.Width - diameter, btn.Height - diameter, diameter, diameter, 0, 90);
                path.AddArc(0, btn.Height - diameter, diameter, diameter, 90, 90);
                path.CloseAllFigures();
                btn.Region = new Region(path);
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
            int idBill = BillDAL.Instance.GetUncheckBillIDByTableID(table.ID);
            string strDiscount = cbDiscount.Text;
            string strNumber = strDiscount.Replace("Giảm", "").Replace("%", "").Trim();
            int discount = 0;
            int.TryParse(strNumber, out discount);

            if (idBill != -1)
            {
                

                DateTime dateCheckIn = BillDAL.Instance.GetDateCheckIn(idBill);
                TimeSpan timeSpan = DateTime.Now - dateCheckIn;
                double pricePerHour = TableDAL.Instance.GetPriceByTableID(table.ID);
                double tienGio = timeSpan.TotalHours * giaGioHienTai;

                double tongTien = tongTienNuoc + tienGio;
                tongTien = Math.Ceiling(tongTien / 1000) * 1000;
                double finalTotalPrice = tongTien - (tongTien / 100) * discount;

                string thongBao = string.Format("Tổng tiền {0} là: {1}\nBạn có muốn in hóa đơn không?",
                                                table.Name,
                                                finalTotalPrice.ToString("c", new CultureInfo("vi-VN")));

                DialogResult result = MessageBox.Show(thongBao, "Xác nhận thanh toán", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (result == DialogResult.Cancel)
                {
                    return;
                }

                if (idBill != -1)
                {
                    if (result == DialogResult.Yes)
                    {
                        MessageBox.Show("Đang in hóa đơn...", "Thông báo");
                    }

                    BillDAL.Instance.CheckOut(idBill, discount, (float)finalTotalPrice);

                    ShowBill(table.ID);
                    LoadTable();

                    this.ActiveControl = null;
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

        private void listView1_SelectedIndexChanged(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void fTableManager_Load(object sender, EventArgs e) 
        {
            LoadAllFood();
        }
        private void flpTable_Paint(object sender, PaintEventArgs e) { }

        private void btnCancelTable_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;
            if (table == null) return;

            int idBill = BillDAL.Instance.GetUncheckBillIDByTableID(table.ID);

            if (idBill == -1)
            {
                string query = "UPDATE dbo.TableFood SET status = N'Trống' WHERE id = " + table.ID;
                DataProvider.Instance.ExecuteNonQuery(query);

                LoadTable();
                ShowBill(table.ID);
                return;
            }

            int countFood = BillDAL.Instance.GetCountBillInfo(idBill);
            if (countFood > 0)
            {
                MessageBox.Show("Bàn đang có món, không thể hủy!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (MessageBox.Show("Hủy bàn " + table.Name + "?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                BillDAL.Instance.DeleteBill(idBill);
                LoadTable();
                ShowBill(table.ID);
            }
        }

        private void contextMenuStrip2_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }
        private void xóaHẳnMónNàyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lsvBill.SelectedItems.Count > 0)
            {
                ListViewItem item = lsvBill.SelectedItems[0];
                int soLuongHienTai = int.Parse(item.SubItems[1].Text);

                GiamMonAn(-soLuongHienTai);
            }
        }
        void GiamMonAn(int soLuongTru)
        {
            if (lsvBill.SelectedItems.Count == 0) return;

            Table table = lsvBill.Tag as Table;
            int idBill = BillDAL.Instance.GetUncheckBillIDByTableID(table.ID);

            string foodName = lsvBill.SelectedItems[0].Text.Trim();

            string query = "SELECT id FROM Food WHERE name = N'" + foodName + "'";
            object result = DataProvider.Instance.ExecuteScalar(query);

            if (result == null)
            {
                MessageBox.Show("Không tìm thấy món '" + foodName + "' trong dữ liệu gốc!", "Lỗi lệch dữ liệu");
                return;
            }
            int idFood = (int)result;

            BillDAL.Instance.InsertBillInfo(idBill, idFood, soLuongTru);

            ShowBill(table.ID);
            LoadTable();
        }

        private void giảm1MónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GiamMonAn(-1);
        }
        private void xóaHẳnMónNàyToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (lsvBill.SelectedItems.Count > 0)
            {
                int soLuongHienTai = 0;

                try
                {
                    soLuongHienTai = int.Parse(lsvBill.SelectedItems[0].SubItems[1].Text);
                }
                catch
                {
                    return;
                }
                GiamMonAn(-soLuongHienTai);
            }
        }

        private void lsvBill_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            GiamMonAn(-1);
        }
        void LoadAllFood()
        {
            List<Food> listFood = FoodDAL.Instance.GetListFood();

            flpFood.Controls.Clear();

            foreach (Food item in listFood)
            {
                Button btn = new Button() { Width = FoodDAL.ButtonWidth, Height = FoodDAL.ButtonHeight };

                btn.Text = item.Name + Environment.NewLine + item.Price.ToString("#,##0") + " đ";

                btn.Click += btnAddFood_Click;

                btn.Tag = item;

                flpFood.Controls.Add(btn);
            }
        }
        private void btnAddFood_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            Food food = btn.Tag as Food;

            Table table = lsvBill.Tag as Table;
            if (table == null)
            {
                MessageBox.Show("Hãy chọn bàn trước!");
                return;
            }

            int idBill = BillDAL.Instance.GetUncheckBillIDByTableID(table.ID);
            int idFood = food.ID;
            int count = 1;

            if (idBill == -1)
            {
                BillDAL.Instance.InsertBill(table.ID);

                idBill = BillDAL.Instance.GetUncheckBillIDByTableID(table.ID);
            }

            BillDAL.Instance.InsertBillInfo(idBill, idFood, count);

            ShowBill(table.ID);
            LoadTable();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;
            if (table == null) return;

            int idBill = BillDAL.Instance.GetUncheckBillIDByTableID(table.ID);

            if (idBill != -1) 
            {
                DateTime dateCheckIn = BillDAL.Instance.GetDateCheckIn(idBill);
                TimeSpan timeSpan = DateTime.Now - dateCheckIn;

                txtTongGio.Text = string.Format("{0}h {1}p {2}s",
                                                (int)timeSpan.TotalHours,
                                                timeSpan.Minutes,
                                                timeSpan.Seconds);

                double tienGio = timeSpan.TotalHours * giaGioHienTai;

                double tongCong = tongTienNuoc + tienGio;
                tongCong = Math.Ceiling(tongCong / 1000) * 1000;

                CultureInfo culture = new CultureInfo("vi-VN");
                txtTongTien.Text = tongCong.ToString("c", culture);

                txtTongTien.ForeColor = Color.Red;
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtTongGio_Click(object sender, EventArgs e)
        {

        }
    }
}