using AppManageBilliard.BUS;
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
using AppManageBilliard.DAL;

namespace AppManageBilliard.GUI
{
    public partial class fAdmin : Form
    {
        BindingSource foodList = new BindingSource();
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
        }
        void LoadCategoryIntoComboBox(ComboBox cb)
        {
            cb.DataSource = CategoryDAL.Instance.GetListCategory();
            cb.DisplayMember = "Name";
            cb.ValueMember = "ID";
        }

        void AddFoodBinding()
        {
            txtFoodName.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "Name", true, DataSourceUpdateMode.Never));

            txtFoodID.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "ID", true, DataSourceUpdateMode.Never));

            nmFoodPrice.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "Price", true, DataSourceUpdateMode.Never));
           
            cbFoodCategory.DataBindings.Add(new Binding("SelectedValue", dtgvFood.DataSource, "CategoryID", true, DataSourceUpdateMode.Never));
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
            cbFoodCategory.SelectedIndex = 0;

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

            // 3. Phân loại: THÊM hay SỬA?
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
    }
}
