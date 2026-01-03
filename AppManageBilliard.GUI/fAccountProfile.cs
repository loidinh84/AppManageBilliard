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
    public partial class fAccountProfile : Form
    {
        private Account loginAccount;

        private event EventHandler updateAccount;
        public event EventHandler UpdateAccount
        {
            add { updateAccount += value; }
            remove { updateAccount -= value; }
        }
        

        public Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccount(loginAccount); }
        }

        public fAccountProfile(Account acc)
        {
            InitializeComponent();
            this.LoginAccount = acc;
            CustomizeDesign(); // Fix lỗi chữ nút + làm đẹp trắng xanh
        }

        // Hàm làm đẹp + fix lỗi chữ bị cắt trên nút
        private void CustomizeDesign()
        {
            // Nền form trắng nhẹ, dễ nhìn
            this.BackColor = Color.FromArgb(245, 250, 255);

            // Form properties
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            // Font chung hiện đại
            this.Font = new Font("Segoe UI", 11F, FontStyle.Regular);

            // TextBox: Viền nhẹ, padding rộng
            txtUserName.BackColor = Color.White;
            txtUserName.ForeColor = Color.Black;
            txtUserName.BorderStyle = BorderStyle.FixedSingle;
            txtUserName.Font = new Font("Segoe UI", 12F);
            txtUserName.Padding = new Padding(10);
            txtUserName.ReadOnly = true;

            txtDisplayName.BackColor = Color.White;
            txtDisplayName.ForeColor = Color.Black;
            txtDisplayName.BorderStyle = BorderStyle.FixedSingle;
            txtDisplayName.Font = new Font("Segoe UI", 12F);
            txtDisplayName.Padding = new Padding(10);

            txtPassWord.BackColor = Color.White;
            txtPassWord.ForeColor = Color.Black;
            txtPassWord.BorderStyle = BorderStyle.FixedSingle;
            txtPassWord.Font = new Font("Segoe UI", 12F);
            txtPassWord.Padding = new Padding(10);
            txtPassWord.UseSystemPasswordChar = true;

            txtNewPass.BackColor = Color.White;
            txtNewPass.ForeColor = Color.Black;
            txtNewPass.BorderStyle = BorderStyle.FixedSingle;
            txtNewPass.Font = new Font("Segoe UI", 12F);
            txtNewPass.Padding = new Padding(10);
            txtNewPass.UseSystemPasswordChar = true;

            txtReEnterPass.BackColor = Color.White;
            txtReEnterPass.ForeColor = Color.Black;
            txtReEnterPass.BorderStyle = BorderStyle.FixedSingle;
            txtReEnterPass.Font = new Font("Segoe UI", 12F);
            txtReEnterPass.Padding = new Padding(10);
            txtReEnterPass.UseSystemPasswordChar = true;
            // Nút Cập nhật (Màu Xanh Dương)
            btnUpdate.FlatStyle = FlatStyle.Flat;
            btnUpdate.FlatAppearance.BorderSize = 0;
            btnUpdate.BackColor = Color.FromArgb(0, 122, 204);
            btnUpdate.ForeColor = Color.White;
            btnUpdate.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnUpdate.Size = new Size(120, 45); // Đồng bộ kích thước
            btnUpdate.Cursor = Cursors.Hand;

            // Nút Thoát (Màu Cam)
            btnExit.FlatStyle = FlatStyle.Flat;
            btnExit.FlatAppearance.BorderSize = 0;
            btnExit.BackColor = Color.FromArgb(255, 140, 0);
            btnExit.ForeColor = Color.White;
            btnExit.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnExit.Size = new Size(120, 45); // Đồng bộ kích thước
            btnExit.Cursor = Cursors.Hand;

            // --- ĐỒNG BỘ NÚT ĐĂNG XUẤT (Màu Đỏ/Hồng đậm) ---
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.BackColor = Color.FromArgb(220, 53, 69); // Màu đỏ chuyên biệt cho Logout
            btnLogout.ForeColor = Color.White;
            btnLogout.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnLogout.Size = new Size(120, 45); // Đồng bộ kích thước với 2 nút kia
            btnLogout.Text = "Đăng xuất";
            btnLogout.TextAlign = ContentAlignment.MiddleCenter;
            btnLogout.Cursor = Cursors.Hand;

            // Hiệu ứng di chuột cho nút Đăng xuất
            btnLogout.MouseEnter += (s, e) => btnLogout.BackColor = Color.FromArgb(200, 35, 50);
            btnLogout.MouseLeave += (s, e) => btnLogout.BackColor = Color.FromArgb(220, 53, 69);
        }

        void ChangeAccount(Account acc)
        {
            txtUserName.Text = LoginAccount.UserName;
            txtDisplayName.Text = LoginAccount.DisplayName;
            lblHello.Text = "Xin chào: " + LoginAccount.DisplayName;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void UpdateAccountInfo()
        {
            string displayName = txtDisplayName.Text;
            string password = txtPassWord.Text;
            string newpass = txtNewPass.Text;
            string reenterPass = txtReEnterPass.Text;
            string userName = txtUserName.Text;

            if (!newpass.Equals(reenterPass))
            {
                MessageBox.Show("Vui lòng nhập lại mật khẩu đúng với mật khẩu mới!");
                return;
            }

            if (AccountDAL.Instance.UpdateAccountProfile(userName, displayName, password, newpass))
            {
                MessageBox.Show("Cập nhật thành công!");
                if (updateAccount != null)
                    updateAccount(this, new AccountEvent(AccountDAL.Instance.GetAccountByUserName(userName)));
            }
            else
            {
                MessageBox.Show("Vui lòng điền đúng mật khẩu cũ!");
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateAccountInfo();
        }

        private void fAccountProfile_Load(object sender, EventArgs e)
        {
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (updateAccount != null)
                updateAccount(this, new AccountEvent(LoginAccount));
            this.Close();
        }
    }

    public class AccountEvent : EventArgs
    {
        private Account acc;
        public Account Acc
        {
            get { return acc; }
            set { acc = value; }
        }
        public AccountEvent(Account acc)
        {
            this.Acc = acc;
        }
    }
}