using AppManageBida.DAL;
using AppManageBilliard.BUS;
using AppManageBilliard.DAL;
using AppManageBilliard.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppManageBilliard.GUI
{
    public partial class fAdmin : Form
    {
        BindingSource foodList = new BindingSource();
        BindingSource CategoryList = new BindingSource();
        BindingSource tableList = new BindingSource();
        BindingSource accountList = new BindingSource();
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
            dtgvTable.DataSource = tableList;
            LoadlistTable();
            AddTableBinding();
            dtgvAccount.DataSource = accountList;
            LoadAccountType();
            LoadAccount();
            AddAccountBinding();
            LoadDashboardStats();
        }

        private void fAdmin_Load(object sender, EventArgs e)
        {

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
        void SetActiveButton(Button btnActive)
        {
            foreach (Control c in panelMenu.Controls)
            {
                if (c is Button) c.BackColor = Color.DimGray;
            }
            btnActive.BackColor = Color.Teal;
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
            grid.RowHeadersVisible = false;
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(238, 239, 249);
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 25, 72);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.DefaultCellStyle.SelectionBackColor = Color.DarkTurquoise;
            grid.DefaultCellStyle.SelectionForeColor = Color.WhiteSmoke;
            grid.BackgroundColor = Color.White;

            grid.ColumnHeadersHeight = 30;
            if (grid.Columns["Ngày vào"] != null)
            {
                grid.Columns["Ngày vào"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";
            }
            if (grid.Columns["Ngày ra"] != null)
            {
                grid.Columns["Ngày ra"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";
            }
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
                    // Reset ô nhập liệu
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

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void dtgvAccount_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        void LoadDashboardStats()
        {
            int soBan = TableDAL.Instance.GetTotalTable();
            int soTK = AccountDAL.Instance.GetTotalAccount();


            pnlTableBox.Value = soBan.ToString();
            pnlTableBox.Invalidate();
            pnlAccountBox.Value = soTK.ToString(); 
            pnlAccountBox.Invalidate();
        }

        private void btnReset_Click_1(object sender, EventArgs e)
        {
            txtCategoryID.Text = "";

            txtCategoryName.Text = "";

            txtCategoryName.Focus();
        }
    }
}
