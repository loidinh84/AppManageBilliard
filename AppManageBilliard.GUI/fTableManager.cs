using AppManageBida.DAL;
using AppManageBilliard.BUS;
using AppManageBilliard.DAL;
using AppManageBilliard.DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
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
                tongTien = Math.Round(tongTien / 1000) * 1000;
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
                        printDocument1.DefaultPageSettings.PaperSize = new PaperSize("Bill", 300, 600);
                        printDocument1.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);

                        printPreviewDialog1.WindowState = FormWindowState.Normal;

                        printPreviewDialog1.Size = new Size(450, 600);

                        printPreviewDialog1.StartPosition = FormStartPosition.CenterScreen;

                        printPreviewDialog1.PrintPreviewControl.AutoZoom = false;

                        printPreviewDialog1.PrintPreviewControl.Zoom = 1.0;

                        printPreviewDialog1.Document = printDocument1;
                        printPreviewDialog1.ShowDialog();
                        printDocument1.Print();
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
            LoadTable();
            LoadAllFood();
            LoadCategory();
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
                tongCong = Math.Round(tongCong / 1000) * 1000;

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

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Table table = lsvBill.Tag as Table;
            int idBill = BillDAL.Instance.GetUncheckBillIDByTableID(table.ID);

            Graphics g = e.Graphics;
            Font fontTitle = new Font("Arial", 14, FontStyle.Bold); 
            Font fontHeader = new Font("Arial", 9, FontStyle.Bold); 
            Font fontBody = new Font("Arial", 9, FontStyle.Regular);
            Font fontBold = new Font("Arial", 9, FontStyle.Bold);
            Brush brush = Brushes.Black;

            StringFormat centerFormat = new StringFormat() { Alignment = StringAlignment.Center };
            StringFormat rightFormat = new StringFormat() { Alignment = StringAlignment.Far };

            int width = 300;
            int y = 10;

            g.DrawString("HÓA ĐƠN THANH TOÁN", fontTitle, brush, new Rectangle(0, y, width, 25), centerFormat);
            y += 25;
            g.DrawString("Billiards Club Vip Pro", fontHeader, brush, new Rectangle(0, y, width, 20), centerFormat);
            y += 20;
            g.DrawString("Ngày: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"), fontBody, brush, new Rectangle(0, y, width, 20), centerFormat);
            y += 20;
            g.DrawString("------------------------------------------------------------", fontBody, brush, new Point(5, y));
            y += 15;

            g.DrawString("Tên món", fontBold, brush, new Point(5, y));
            g.DrawString("SL", fontBold, brush, new Point(130, y));
            g.DrawString("Đ.Giá", fontBold, brush, new Point(160, y));
            g.DrawString("T.Tiền", fontBold, brush, new Rectangle(215, y, 80, 20), rightFormat);
            y += 15;
            g.DrawString("------------------------------------------------------------", fontBody, brush, new Point(5, y));
            y += 15;

            List<MenuDTO> listMenu = MenuBUS.Instance.GetListMenuByTable(table.ID);
            double tongTienNuoc = 0;
            double giaGioHienTai = TableDAL.Instance.GetPriceByTableID(table.ID);

            foreach (MenuDTO item in listMenu)
            {
                if (item.IsService == true)
                {
                    giaGioHienTai = item.Price;
                    continue;
                }

                string tenMon = item.FoodName;
                if (tenMon.Length > 18) tenMon = tenMon.Substring(0, 15) + "...";

                g.DrawString(tenMon, fontBody, brush, new Point(5, y));
                g.DrawString(item.Count.ToString(), fontBody, brush, new Point(135, y)); 
                g.DrawString(item.Price.ToString("##,###"), fontBody, brush, new Point(160, y));
                g.DrawString(item.TotalPrice.ToString("##,###"), fontBody, brush, new Rectangle(215, y, 80, 20), rightFormat);

                tongTienNuoc += item.TotalPrice;
                y += 20;
            }

            g.DrawString("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - -", fontBody, brush, new Point(5, y));
            y += 15;

            DateTime dateCheckIn = BillDAL.Instance.GetDateCheckIn(idBill);
            TimeSpan timeSpan = DateTime.Now - dateCheckIn;
            double tienGio = timeSpan.TotalHours * giaGioHienTai;

            g.DrawString(string.Format("Tiền giờ ({0}h {1}p) {2}s", (int)timeSpan.TotalHours, timeSpan.Minutes, timeSpan.Seconds), fontBody, brush, new Point(5, y));
            g.DrawString(tienGio.ToString("N0"), fontBody, brush, new Rectangle(215, y, 80, 20), rightFormat);
            y += 25;

            double tongCong = tongTienNuoc + tienGio;
            tongCong = Math.Round(tongCong / 1000) * 1000;

            string strDiscount = cbDiscount.Text.Replace("Giảm", "").Replace("%", "").Trim();
            int discount = 0;
            int.TryParse(strDiscount, out discount);
            double tienGiam = (tongCong / 100) * discount;
            double phaiTra = tongCong - tienGiam;

            g.DrawString("Tổng cộng:", fontBold, brush, new Point(120, y));
            g.DrawString(tongCong.ToString("N0"), fontBold, brush, new Rectangle(215, y, 80, 20), rightFormat);
            y += 20;

            if (discount > 0)
            {
                g.DrawString("Giảm giá " + discount + "%:", fontBody, brush, new Point(120, y));
                g.DrawString("-" + tienGiam.ToString("N0"), fontBody, brush, new Rectangle(215, y, 80, 20), rightFormat);
                y += 20;
            }

            Font fontTotalBig = new Font("Arial", 12, FontStyle.Bold);
            g.DrawString("THANH TOÁN:", fontBold, brush, new Point(80, y + 5));
            g.DrawString(phaiTra.ToString("N0") + " đ", fontTotalBig, brush, new Rectangle(180, y, 115, 30), rightFormat);
            y += 40;

            g.DrawString("Cảm ơn Quý Khách!", fontBody, brush, new Rectangle(0, y, width, 20), centerFormat);
            g.DrawString("Hẹn Gặp Lại", fontBody, brush, new Rectangle(0, y + 15, width, 20), centerFormat);
        }

        Button currentCategoryButton = null;

        void LoadFoodListByCategoryID(int id)
        {
            flpFood.Controls.Clear();

            List<Food> listFood = FoodDAL.Instance.GetFoodByCategoryID(id);

            foreach (Food item in listFood)
            {
                Button btn = new Button() { Width = 90, Height = 90 };
                btn.Text = item.Name + "\n" + item.Price.ToString("N0");
                btn.Click += btn_Click;
                btn.Tag = item;

                btn.BackColor = Color.White;

                flpFood.Controls.Add(btn);
            }
        }

        void LoadCategory()
        {
            flpCategory.Controls.Clear();
            flpCategory.WrapContents = false;
            flpCategory.AutoScroll = true;
            List<Category> listCategory = CategoryDAL.Instance.GetListCategory();

            Button btnAll = new Button();
            int textWidth = TextRenderer.MeasureText(btnAll.Text, btnAll.Font).Width;

            btnAll.Text = "Tất cả";
            btnAll.Tag = null;
            btnAll.Margin = new Padding(3);
            btnAll.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnAll.Width = textWidth + 80;
            btnAll.Height = 45;

            btnAll.Click += CategoryBtn_Click;

            currentCategoryButton = btnAll;

            TaoNutTronMin(btnAll);
            flpCategory.Controls.Add(btnAll);

            foreach (Category item in listCategory)
            {
                Button btn = new Button();
                int width = TextRenderer.MeasureText(btn.Text, btn.Font).Width;
                btn.Text = item.Name;
                btn.Tag = item;
                btn.Width = width + 80;
                btn.Height = 45;
                btn.Margin = new Padding(5);
                btn.Font = new Font("Segoe UI", 10, FontStyle.Bold);

                btn.Click += CategoryBtn_Click;

                TaoNutTronMin(btn); 
                flpCategory.Controls.Add(btn);
            }
        }

        void TaoNutTronMin(Button btn)
        {
            btn.Region = null;          
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btn.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btn.UseVisualStyleBackColor = false; 

            btn.BackColor = Color.White; 
            btn.ForeColor = Color.DimGray;
            btn.Cursor = Cursors.Hand;
            btn.TabStop = false;           

            btn.Paint -= Btn_Paint;
            btn.Paint += Btn_Paint;
        }

        private void Btn_Paint(object sender, PaintEventArgs e)
        {
            Button btn = sender as Button;
            Graphics g = e.Graphics;

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

            g.Clear(Color.White);

            Color activeColor = Color.DodgerBlue;
            Color normalColor = Color.FromArgb(225, 225, 225); 

            Color backColor = (btn == currentCategoryButton) ? activeColor : normalColor;
            Color textColor = (btn == currentCategoryButton) ? Color.White : Color.Black;

            Rectangle rect = new Rectangle(0, 0, btn.Width - 1, btn.Height - 1);
            int radius = 18; 

            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
            path.CloseFigure();

            using (SolidBrush brush = new SolidBrush(backColor))
            {
                g.FillPath(brush, path);
            }

            TextRenderer.DrawText(g, btn.Text, btn.Font, new Rectangle(0, 0, btn.Width, btn.Height), textColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        void CategoryBtn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            Button oldBtn = currentCategoryButton;

            if (currentCategoryButton != null)
            {
                currentCategoryButton.BackColor = Color.FromArgb(240, 240, 240);
                currentCategoryButton.ForeColor = Color.DimGray;
            }

            currentCategoryButton = btn;
            currentCategoryButton.BackColor = Color.DodgerBlue;
            currentCategoryButton.ForeColor = Color.Black;

            if (oldBtn != null)
            {
                oldBtn.Invalidate();
            }
            btn.Invalidate();

            Category category = btn.Tag as Category;

            if (category == null)
            {
                LoadAllFood();
            }
            else
            {
                LoadFoodListByCategoryID(category.ID);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text;

            if (string.IsNullOrEmpty(keyword))
            {
                if (currentCategoryButton == null || currentCategoryButton.Text == "Tất cả")
                {
                    LoadAllFood();
                }
                else
                {
                    Category cat = currentCategoryButton.Tag as Category;
                    if (cat != null) LoadFoodListByCategoryID(cat.ID);
                }
                return;
            }

            flpFood.Controls.Clear();
            List<Food> listSearch = FoodDAL.Instance.SearchFoodByName(keyword);

            foreach (Food item in listSearch)
            {
                Button btn = new Button() { Width = 90, Height = 90 };
                btn.Text = item.Name + "\n" + item.Price.ToString("N0");
                btn.Click += btn_Click;
                btn.Tag = item;
                btn.BackColor = Color.White;

                flpFood.Controls.Add(btn);
            }
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            txtSearch.Visible = false;
        }

        private void fTableManager_Click(object sender, EventArgs e)
        {
            txtSearch.Visible = false;
        }

        private void flpFood_Click(object sender, EventArgs e)
        {
            if (txtSearch.Visible)
            {
                txtSearch.Visible = false;
            }
            this.Focus();
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            if (txtSearch.Visible)
            {
                txtSearch.Visible = false;
            }
            this.Focus();
        }

        private void btnSearch_Click_1(object sender, EventArgs e)
        {
            txtSearch.Visible = !txtSearch.Visible;

            if (txtSearch.Visible)
            {
                txtSearch.Focus();
                txtSearch.SelectAll();
            }
            else
            {
                LoadAllFood();
            }
        }
    }
}