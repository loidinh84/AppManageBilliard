using AppManageBilliard.DAL;
using AppManageBilliard.BUS;
using AppManageBilliard.DAL;
using AppManageBilliard.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace AppManageBilliard.GUI
{
    public partial class fAdmin : Form
    {
        BindingSource foodList = new BindingSource();
        BindingSource CategoryList = new BindingSource();
        BindingSource tableList = new BindingSource();
        BindingSource accountList = new BindingSource();
        BindingSource discountList = new BindingSource();
        BindingSource logList = new BindingSource();

        private Panel cardBan, cardHangHoa, cardNhanSu, cardHoaDon, cardThongKe;
        private Label lblBanCount, lblHangHoaCount, lblNhanSuCount, lblHoaDonCount, lblThongKeCount;

        public fAdmin()
        {
            InitializeComponent();

            tcAdmin.Appearance = TabAppearance.FlatButtons;
            tcAdmin.ItemSize = new Size(0, 1);
            tcAdmin.SizeMode = TabSizeMode.Fixed;

            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
            LoadDateTimePickerBill();
            StylizeGrid(dtgvBill);
            SetActiveButton(btnRevenue);

            dtgvFood.DataSource = foodList;
            LoadListFood();
            LoadCategoryIntoComboBox(cbFoodCategory);
            AddFoodBinding();
            StylizeGrid(dtgvFood);

            dtgvCategory.DataSource = CategoryList;
            LoadListCategory();
            AddCategoryBinding();
            StylizeGrid(dtgvCategory);

            dtgvTable.DataSource = tableList;
            LoadlistTable();
            AddTableBinding();
            StylizeGrid(dtgvTable);

            dtgvAccount.DataSource = accountList;
            LoadAccountType();
            LoadAccount();
            AddAccountBinding();
            StylizeGrid(dtgvAccount);


            dtgvDiscount.DataSource = discountList;
            LoadListDiscount();
            AddDiscountBinding();

            dtgvHistory.DataSource = logList;
            LoadListLogByDate(dtpStartDay.Value, dtpEndDay.Value);
            dtgvHistory.DataBindingComplete += dtgvHistory_DataBindingComplete;
        }

        private Panel CreateColorfulCard(string title, string value, Color backColor, string linkText, Point location)
        {
            Panel card = new Panel
            {
                Size = new Size(300, 150),
                Location = location,
                BackColor = backColor
            };

            Label lblValue = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 36F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 40),
                AutoSize = true
            };

            Label lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 12F),
                ForeColor = Color.White,
                Location = new Point(20, 100),
                AutoSize = true
            };

            Label lblLink = new Label
            {
                Text = linkText,
                Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                ForeColor = Color.White,
                Location = new Point(20, 125),
                AutoSize = true,
                Cursor = Cursors.Hand
            };
            lblLink.Click += (s, e) => MessageBox.Show("Chuyển đến danh sách " + title);

            Label lblIcon = new Label
            {
                Text = "●●●", // Giả lập icon
                Font = new Font("Segoe UI", 30F),
                ForeColor = Color.White,
                Location = new Point(200, 50),
                AutoSize = true
            };

            card.Controls.Add(lblValue);
            card.Controls.Add(lblTitle);
            card.Controls.Add(lblLink);
            card.Controls.Add(lblIcon);

            return card;
        }

        private Button GetActiveButton()
        {
            switch (tcAdmin.SelectedIndex)
            {
                case 0: return btnRevenue;
                case 1: return btnFood;
                case 2: return btnCategory;
                case 3: return btnTable;
                case 4: return btnAccount;
                case 5: return btnDiscount;
                case 6: return btnHistory;
                default: return null;
            }
        }

        void SetActiveButton(Button btnActive)
        {
            foreach (Control c in panelMenu.Controls)
            {
                if (c is Button btn)
                {
                    btn.BackColor = Color.Transparent;
                }
            }
            if (btnActive != null)
            {
                btnActive.BackColor = Color.FromArgb(0, 130, 180);
            }
        }

        private void btnRevenue_Click(object sender, EventArgs e)
        {
            tcAdmin.SelectedIndex = 0;
            SetActiveButton(btnRevenue);
        }

        private void btnFood_Click(object sender, EventArgs e)
        {
            tcAdmin.SelectedIndex = 1;
            SetActiveButton(btnFood);
        }

        private void btnCategory_Click(object sender, EventArgs e)
        {
            tcAdmin.SelectedIndex = 2;
            SetActiveButton(btnCategory);
        }

        private void btnTable_Click(object sender, EventArgs e)
        {
            tcAdmin.SelectedIndex = 3;
            SetActiveButton(btnTable);
        }

        private void btnAccount_Click(object sender, EventArgs e)
        {
            tcAdmin.SelectedIndex = 4;
            SetActiveButton(btnAccount);
        }

        void LoadListBillByDate(DateTime checkIn, DateTime checkOut)
        {
            dtgvBill.DataSource = BillBUS.Instance.GetBillListByDate(checkIn, checkOut);
            if (dtgvBill.Columns["ID"] != null)
            {
                dtgvBill.Columns["ID"].Visible = false;
            }
        }

        private void btnViewBill_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
        }

        void LoadDateTimePickerBill()
        {
            DateTime today = DateTime.Now;
            dtpkFromDate.Value = new DateTime(today.Year, today.Month, 1);
            dtpkToDate.Value = dtpkFromDate.Value.AddMonths(1).AddDays(-1);
        }

        void LoadListFood()
        {
            foodList.DataSource = FoodDAL.Instance.GetListFood();
            dtgvFood.Columns["ID"].HeaderText = "Mã số";
            dtgvFood.Columns["Name"].HeaderText = "Tên món";
            dtgvFood.Columns["CategoryID"].HeaderText = "Danh mục";
            dtgvFood.Columns["Price"].HeaderText = "Giá tiền";
        }

        void LoadCategoryIntoComboBox(ComboBox cb)
        {
            List<Category> list = CategoryDAL.Instance.GetListCategory();
            if (list == null || list.Count == 0) return;
            cb.DataSource = list;
            cb.DisplayMember = "Name";
            cb.ValueMember = "ID";
            cb.SelectedIndex = 0;
        }

        void AddFoodBinding()
        {
            txtFoodName.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "Name", true, DataSourceUpdateMode.Never));
            txtFoodID.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "ID", true, DataSourceUpdateMode.Never));
            nmFoodPrice.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "Price", true, DataSourceUpdateMode.Never));
            cbFoodCategory.DataBindings.Add(new Binding("SelectedValue", dtgvFood.DataSource, "CategoryID", true, DataSourceUpdateMode.Never));
        }


        void LoadAccount()
        {
            accountList.DataSource = DataProvider.Instance.ExecuteQuery("EXEC USP_GetAccountList");
            dtgvAccount.Columns["UserName"].HeaderText = "Tên đăng nhập";
            dtgvAccount.Columns["DisplayName"].HeaderText = "Tên hiển thị";
            dtgvAccount.Columns["Type"].HeaderText = "Loại tài khoản";
        }

        void AddAccountBinding()
        {
            txtUserName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "UserName", true, DataSourceUpdateMode.Never));
            txtDisplayName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "DisplayName", true, DataSourceUpdateMode.Never));
            cbbType.DataBindings.Add(new Binding("SelectedValue", dtgvAccount.DataSource, "Type", true, DataSourceUpdateMode.Never));
        }

        void LoadAccountType()
        {
            cbbType.DataSource = new List<object>()
            {
                new { Name = "Admin", Value = 1 },
                new { Name = "Nhân viên", Value = 0 }
            };
            cbbType.DisplayMember = "Name";
            cbbType.ValueMember = "Value";
        }
        
        void StylizeGrid(DataGridView grid)
        {
            // 1. Cấu hình tổng quan
            grid.BackgroundColor = Color.White;
            grid.BorderStyle = BorderStyle.None;
            grid.RowHeadersVisible = false;
            grid.AllowUserToResizeRows = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.GridColor = Color.FromArgb(235, 237, 239); // Đường kẻ mờ hiện đại

            // 2. Định dạng Header (Tiêu đề cột)
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grid.ColumnHeadersHeight = 40; // Tăng độ cao cho thoáng
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(44, 62, 80); // Màu xanh Midnight
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            // 3. Định dạng Dòng dữ liệu
            grid.DefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            grid.DefaultCellStyle.ForeColor = Color.FromArgb(71, 89, 126);
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 152, 219); // Màu xanh dương sáng
            grid.DefaultCellStyle.SelectionForeColor = Color.White;

            // Đổ màu xen kẽ giữa các dòng (Zebra stripes)
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
            grid.RowTemplate.Height = 35; // Dòng cao hơn giúp dễ đọc dữ liệu

            // 4. Định dạng ngày tháng (Giữ nguyên logic cũ của bạn)
            if (grid.Columns["Ngày vào"] != null)
            {
                grid.Columns["Ngày vào"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";
            }
            if (grid.Columns["Ngày ra"] != null)
            {
                grid.Columns["Ngày ra"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";
            }

            // Tự động giãn cách các cột để lấp đầy bảng
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void btnShowFood_Click(object sender, EventArgs e)
        {
            txtFoodID.Text = "";
            txtFoodName.Text = "";
            nmFoodPrice.Value = 0;
            if (cbFoodCategory.Items.Count > 0)
            {
                cbFoodCategory.SelectedIndex = 0;
            }
            txtFoodName.Focus();
        }

        private void btnAddEditFood_Click(object sender, EventArgs e)
        {
            string name = txtFoodName.Text;
            int categoryID = (int)cbFoodCategory.SelectedValue;
            float price = (float)nmFoodPrice.Value;
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Vui lòng nhập tên món!");
                return;
            }
            if (string.IsNullOrEmpty(txtFoodID.Text))
            {
                if (FoodDAL.Instance.InsertFood(name, categoryID, price))
                {
                    MessageBox.Show("Thêm món thành công!");
                    LoadListFood();
                }
                else
                {
                    MessageBox.Show("Có lỗi khi thêm món!");
                }
            }
            else
            {
                int id = Convert.ToInt32(txtFoodID.Text);
                if (FoodDAL.Instance.UpdateFood(id, name, categoryID, price))
                {
                    MessageBox.Show("Sửa món thành công!");
                    LoadListFood();
                }
                else
                {
                    MessageBox.Show("Có lỗi khi sửa món!");
                }
            }
        }

        private void btnDeleteFood_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFoodID.Text))
            {
                MessageBox.Show("Hãy chọn món cần xóa trước!");
                return;
            }
            int id = Convert.ToInt32(txtFoodID.Text);
            if (MessageBox.Show("Bạn có chắc muốn xóa món này không?", "Cảnh báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (FoodDAL.Instance.DeleteFood(id))
                {
                    MessageBox.Show("Xóa món thành công!");
                    LoadListFood();
                    btnShowFood_Click(null, null);
                }
                else
                {
                    MessageBox.Show("Có lỗi khi xóa món!");
                }
            }
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearch.Text))
            {
                LoadListFood();
            }
            else
            {
                foodList.DataSource = FoodDAL.Instance.SearchFoodByName(txtSearch.Text);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            foodList.DataSource = FoodDAL.Instance.SearchFoodByName(txtSearch.Text);
        }

        void LoadListCategory()
        {
            CategoryList.DataSource = CategoryDAL.Instance.GetListCategory();
            dtgvCategory.Columns["ID"].HeaderText = "Mã loại";
            dtgvCategory.Columns["Name"].HeaderText = "Tên danh mục";
        }

        void AddCategoryBinding()
        {
            txtCategoryID.DataBindings.Add(new Binding("Text", dtgvCategory.DataSource, "ID", true, DataSourceUpdateMode.Never));
            txtCategoryName.DataBindings.Add(new Binding("Text", dtgvCategory.DataSource, "Name", true, DataSourceUpdateMode.Never));
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtCategoryID.Text = "";
            txtCategoryName.Text = "";
            txtCategoryName.Focus();
        }

        private void btnAddEdit_Click(object sender, EventArgs e)
        {
            string name = txtCategoryName.Text;
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Tên danh mục không được để trống!");
                return;
            }
            if (string.IsNullOrEmpty(txtCategoryID.Text))
            {
                if (CategoryDAL.Instance.InsertCategory(name))
                {
                    MessageBox.Show("Thêm danh mục thành công!");
                    LoadListCategory();
                    LoadCategoryIntoComboBox(cbFoodCategory);
                }
                else MessageBox.Show("Có lỗi khi thêm!");
            }
            else
            {
                int id = Convert.ToInt32(txtCategoryID.Text);
                if (CategoryDAL.Instance.UpdateCategory(id, name))
                {
                    MessageBox.Show("Cập nhật danh mục thành công!");
                    LoadListCategory();
                    LoadCategoryIntoComboBox(cbFoodCategory);
                }
                else MessageBox.Show("Có lỗi khi cập nhật!");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCategoryID.Text)) return;
            int id = Convert.ToInt32(txtCategoryID.Text);
            if (MessageBox.Show("Bạn có thật sự muốn xóa danh mục này?", "Cảnh báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (CategoryDAL.Instance.DeleteCategory(id))
                {
                    MessageBox.Show("Xóa thành công!");
                    LoadListCategory();
                    LoadCategoryIntoComboBox(cbFoodCategory);
                    btnReset_Click(null, null);
                }
                else
                {
                    MessageBox.Show("Không thể xóa danh mục này (Có thể do đang chứa món ăn)!");
                }
            }
        }

        void LoadlistTable()
        {
            tableList.DataSource = TableDAL.Instance.LoadTableList();
            dtgvTable.Columns["ID"].HeaderText = "Mã bàn";
            dtgvTable.Columns["Name"].HeaderText = "Tên bàn";
            dtgvTable.Columns["Status"].HeaderText = "Trạng thái";
        }

        void AddTableBinding()
        {
            txtTableID.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "ID", true, DataSourceUpdateMode.Never));
            txtTableName.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "Name", true, DataSourceUpdateMode.Never));
            txtTableStatus.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "Status", true, DataSourceUpdateMode.Never));
        }

        private void btnResetTable_Click(object sender, EventArgs e)
        {
            txtTableID.Text = "";
            txtTableName.Text = "";
            txtTableStatus.Text = "";
            txtTableName.Focus();
        }

        private void btnAddEditTable_Click(object sender, EventArgs e)
        {
            string name = txtTableName.Text;
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Tên bàn không được để trống!");
                return;
            }
            if (string.IsNullOrEmpty(txtTableID.Text))
            {
                if (TableDAL.Instance.InsertTable(name))
                {
                    MessageBox.Show("Thêm bàn mới thành công!");
                    LoadlistTable();
                }
                else MessageBox.Show("Có lỗi khi thêm bàn!");
            }
            else
            {
                int id = Convert.ToInt32(txtTableID.Text);
                if (TableDAL.Instance.UpdateTable(id, name))
                {
                    MessageBox.Show("Cập nhật tên bàn thành công!");
                    LoadlistTable();
                }
                else MessageBox.Show("Có lỗi khi cập nhật!");
            }
        }

        private void btnDeleteTable_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTableID.Text)) return;
            int id = Convert.ToInt32(txtTableID.Text);
            string status = txtTableStatus.Text;
            if (status != "Trống")
            {
                MessageBox.Show("Bàn đang có người chơi, không thể xóa!");
                return;
            }
            if (MessageBox.Show("Bạn có thật sự muốn xóa bàn này?", "Cảnh báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (TableDAL.Instance.DeleteTable(id))
                {
                    MessageBox.Show("Xóa bàn thành công!");
                    LoadlistTable();
                    txtTableID.Text = "";
                    txtTableName.Text = "";
                    txtTableStatus.Text = "";
                }
                else
                {
                    MessageBox.Show("Có lỗi khi xóa bàn!");
                }
            }
        }

        private void btnWatchAccount_Click(object sender, EventArgs e)
        {
            LoadAccount();
            txtUserName.Text = "";
            txtDisplayName.Text = "";
            txtUserName.ReadOnly = false;
            txtUserName.Focus();
        }

        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            string userName = txtUserName.Text;
            string displayName = txtDisplayName.Text;
            int type = (int)cbbType.SelectedValue;
            if (AccountDAL.Instance.InsertAccount(userName, displayName, type))
            {
                MessageBox.Show("Thêm tài khoản thành công!");
                LoadAccount();
            }
            else MessageBox.Show("Thêm thất bại (Trùng tên tài khoản)!");
        }

        private void btnEditAccount_Click(object sender, EventArgs e)
        {
            string userName = txtUserName.Text;
            string displayName = txtDisplayName.Text;
            int type = (int)cbbType.SelectedValue;
            if (AccountDAL.Instance.UpdateAccount(userName, displayName, type))
            {
                MessageBox.Show("Cập nhật tài khoản thành công!");
                LoadAccount();
            }
            else MessageBox.Show("Cập nhật thất bại!");
        }

        void LoadListDiscount()
        {
            discountList.DataSource = DataProvider.Instance.ExecuteQuery("SELECT eventName AS [Tên sự kiện], discountPercent AS [Giảm giá (%)], fromDate AS [Từ ngày], toDate AS [Đến ngày], status AS [Kích hoạt] FROM EventsDiscount");
        }

        void AddDiscountBinding()
        {
            txtEvent.DataBindings.Clear();
            nudDiscount.DataBindings.Clear();
            dtpStartDay.DataBindings.Clear();
            dtpEndDay.DataBindings.Clear();
            cbStatus.DataBindings.Clear();

            txtEvent.DataBindings.Add(new Binding("Text", dtgvDiscount.DataSource, "Tên sự kiện", true, DataSourceUpdateMode.Never));
            nudDiscount.DataBindings.Add(new Binding("Value", dtgvDiscount.DataSource, "Giảm giá (%)", true, DataSourceUpdateMode.Never));
            dtpStartDay.DataBindings.Add(new Binding("Value", dtgvDiscount.DataSource, "Từ ngày", true, DataSourceUpdateMode.Never));
            dtpEndDay.DataBindings.Add(new Binding("Value", dtgvDiscount.DataSource, "Đến ngày", true, DataSourceUpdateMode.Never));
            cbStatus.DataBindings.Add(new Binding("Checked", dtgvDiscount.DataSource, "Kích hoạt", true, DataSourceUpdateMode.OnPropertyChanged));
        }
        private void dtgvHistory_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dtgvHistory.Columns.Count >= 4)
            {
                dtgvHistory.Columns[0].Width = 75; 
                dtgvHistory.Columns[1].Width = 90;
                dtgvHistory.Columns[2].Width = 350;
                dtgvHistory.Columns[3].Width = 110;

                dtgvHistory.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

                dtgvHistory.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dtgvHistory.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                dtgvHistory.ScrollBars = ScrollBars.Both;
            }
            foreach (DataGridViewRow row in dtgvHistory.Rows)
            {
                if (row.Cells[1].Value != null)
                {
                    string action = row.Cells[1].Value.ToString();

                    if (action == "HỦY MÓN")
                    {
                        row.DefaultCellStyle.ForeColor = Color.IndianRed;
                    }
                    else if (action == "ĐỔI GIỜ")
                    {
                        row.DefaultCellStyle.ForeColor = Color.DarkOrange;
                    }
                    else if (action == "THANH TOÁN")
                    {
                        row.DefaultCellStyle.ForeColor = Color.DarkGreen;
                    }
                }
            }
        }

        void LoadListLogByDate(DateTime fromDate, DateTime toDate)
        {
            string query = "EXEC USP_GetFullHistoryByDate @fromDate , @toDate";
            logList.DataSource = DataProvider.Instance.ExecuteQuery(query, new object[] { fromDate, toDate });

        }

        private void btnLoc_Click(object sender, EventArgs e)
        {
            DateTime fromDate = dtpStartDay.Value.Date;

            DateTime toDate = dtpEndDay.Value.Date.AddDays(1).AddTicks(-1);

            LoadListLogByDate(fromDate, toDate);
        }

        private void btnDeleteDiscount_Click(object sender, EventArgs e)
        {
            string name = txtEvent.Text;

            if (MessageBox.Show("Bạn có chắc muốn xóa khuyến mãi: " + name + "?", "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                string query = "EXEC USP_DeleteDiscount @name";
                if (DataProvider.Instance.ExecuteNonQuery(query, new object[] { name }) > 0)
                {
                    MessageBox.Show("Xóa thành công!");
                    LoadListDiscount();
                }
            }
        }

        private void btnAddDiscount_Click(object sender, EventArgs e)
        {
            string name = txtEvent.Text;
            int percent = (int)nudDiscount.Value;
            DateTime from = dtpStartDay.Value;
            DateTime to = dtpEndDay.Value;
            int status = cbStatus.Checked ? 1 : 0;

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Vui lòng nhập tên sự kiện!");
                return;
            }

            string checkQuery = "EXEC USP_CheckDiscountExists @name";
            int exists = (int)DataProvider.Instance.ExecuteScalar(checkQuery, new object[] { name });

            if (exists > 0) 
            {
                string query = "EXEC USP_UpdateDiscount @name , @percent , @from , @to , @status";
                if (DataProvider.Instance.ExecuteNonQuery(query, new object[] { name, percent, from, to, status }) > 0)
                {
                    MessageBox.Show("Cập nhật khuyến mãi thành công!");
                }
            }
            else 
            {
                string query = "EXEC USP_InsertDiscount @name , @percent , @from , @to , @status";
                if (DataProvider.Instance.ExecuteNonQuery(query, new object[] { name, percent, from, to, status }) > 0)
                {
                    MessageBox.Show("Thêm mới khuyến mãi thành công!");
                }
            }
            LoadListDiscount();
        }

        private void btnResetDiscount_Click(object sender, EventArgs e)
        {
            txtEvent.Text = "";
            nudDiscount.Value = 0;
            dtpStartDay.Value = DateTime.Now;
            dtpEndDay.Value = DateTime.Now;
            cbStatus.Checked = false;
            txtEvent.Focus();
        }

        private void btnDiscount_Click(object sender, EventArgs e)
        {
            tcAdmin.SelectedIndex = 5;
            SetActiveButton(btnDiscount);
        }


        private void button2_Click(object sender, EventArgs e)
        {
            DataGridView grid = dtgvBill;

            int dataRowCount = grid.Rows.Cast<DataGridViewRow>().Count(r => !r.IsNewRow);

            if (dataRowCount == 0)
            {
                MessageBox.Show("Không có dữ liệu doanh thu để xuất Excel!\n\n" +
                                "Hãy thử thay đổi khoảng thời gian và nhấn 'Thống kê'",
                                "Không có dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel Files (*.xlsx)|*.xlsx";
            sfd.Title = "Xuất doanh thu ra file Excel";
            sfd.FileName = "DoanhThu_" + DateTime.Now.ToString("ddMMyyyy_HHmm");

            if (sfd.ShowDialog() != DialogResult.OK) return;

            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;

            try
            {
                excelApp = new Excel.Application();
                excelApp.Visible = false;
                workbook = excelApp.Workbooks.Add();
                worksheet = workbook.ActiveSheet;

                // Header
                for (int i = 0; i < grid.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1] = grid.Columns[i].HeaderText;
                    worksheet.Cells[1, i + 1].Font.Bold = true;
                    worksheet.Cells[1, i + 1].Interior.Color = Color.FromArgb(44, 62, 80);
                    worksheet.Cells[1, i + 1].Font.Color = Color.White;
                }

                // Data
                int excelRow = 2;
                foreach (DataGridViewRow row in grid.Rows)
                {
                    if (row.IsNewRow) continue;
                    for (int j = 0; j < grid.Columns.Count; j++)
                    {
                        object cellValue = row.Cells[j].Value;
                        worksheet.Cells[excelRow, j + 1] = cellValue?.ToString() ?? "";
                    }
                    excelRow++;
                }

                worksheet.Columns.AutoFit();
                workbook.SaveAs(sfd.FileName, Excel.XlFileFormat.xlWorkbookDefault);

                MessageBox.Show("Xuất doanh thu thành công!\nFile lưu tại:\n" + sfd.FileName,
                                "Thành công!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (workbook != null) { workbook.Close(false); System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook); }
                if (excelApp != null) { excelApp.Quit(); System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp); }
                if (worksheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int dataRowCount = dtgvHistory.Rows.Cast<DataGridViewRow>().Count(r => !r.IsNewRow);

            if (dataRowCount == 0)
            {
                MessageBox.Show("Không có dữ liệu lịch sử để xuất Excel!\n\n" +
                                "Gợi ý:\n" +
                                "• Chọn khoảng thời gian Từ ngày - Đến ngày phù hợp\n" +
                                "• Nhấn nút 'Lọc' để tải dữ liệu lịch sử\n" +
                                "• Thực hiện thanh toán, hủy món, đổi giờ bàn để tạo log lịch sử",
                                "Không có dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel Files (*.xlsx)|*.xlsx";
            sfd.Title = "Xuất lịch sử hoạt động ra Excel";
            sfd.FileName = "LichSuHoatDong_" + DateTime.Now.ToString("ddMMyyyy_HHmm");

            if (sfd.ShowDialog() != DialogResult.OK)
                return;

            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;

            try
            {
                excelApp = new Excel.Application();
                excelApp.Visible = false;
                workbook = excelApp.Workbooks.Add();
                worksheet = workbook.ActiveSheet;

                for (int i = 0; i < dtgvHistory.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1] = dtgvHistory.Columns[i].HeaderText;
                    worksheet.Cells[1, i + 1].Font.Bold = true;
                    worksheet.Cells[1, i + 1].Interior.Color = Color.FromArgb(44, 62, 80); 
                    worksheet.Cells[1, i + 1].Font.Color = Color.White; 
                }

                int excelRow = 2;
                foreach (DataGridViewRow row in dtgvHistory.Rows)
                {
                    if (row.IsNewRow) continue;

                    for (int j = 0; j < dtgvHistory.Columns.Count; j++)
                    {
                        object cellValue = row.Cells[j].Value;
                        worksheet.Cells[excelRow, j + 1] = cellValue?.ToString() ?? "";
                    }
                    excelRow++;
                }

                worksheet.Columns.AutoFit();

                workbook.SaveAs(sfd.FileName, Excel.XlFileFormat.xlWorkbookDefault);

                MessageBox.Show("Xuất lịch sử thành công!\n\nFile đã lưu tại:\n" + sfd.FileName,
                                "Thành công!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất Excel:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (workbook != null)
                {
                    workbook.Close(false);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                }
                if (excelApp != null)
                {
                    excelApp.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                }
                if (worksheet != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }



        private void btnHistory_Click(object sender, EventArgs e)
        {
            tcAdmin.SelectedIndex = 6;
            SetActiveButton(btnHistory);
        }

        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            string userName = txtUserName.Text;
            if (AccountDAL.Instance.DeleteAccount(userName))
            {
                MessageBox.Show("Xóa tài khoản thành công!");
                LoadAccount();
            }
            else MessageBox.Show("Xóa thất bại!");
        }

        private void btnResetPass_Click(object sender, EventArgs e)
        {
            string userName = txtUserName.Text;
            if (AccountDAL.Instance.ResetPassword(userName))
            {
                MessageBox.Show("Đặt lại mật khẩu thành công! Mật khẩu mới là: 0");
            }
            else MessageBox.Show("Có lỗi xảy ra!"); 
        }

        private void btnDeleteBill_Click(object sender, EventArgs e)
        {
            if (dtgvBill.SelectedCells.Count > 0)
            {
                if (MessageBox.Show("Bạn có chắc chắn muốn xóa hóa đơn này không?", "Cảnh báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    foreach (DataGridViewRow row in dtgvBill.SelectedRows)
                    {
                        int id = (int)row.Cells["ID"].Value;
                        BillDAL.Instance.DeleteBill(id);
                    }
                    LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
                    MessageBox.Show("Xóa thành công!");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn hóa đơn cần xóa!");
            }
        }
    }

}