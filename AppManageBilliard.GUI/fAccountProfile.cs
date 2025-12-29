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

        public event EventHandler<AccountEvent> UpdateAccount;

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

            // Nút Cập nhật: Xanh dương lớn hơn, chữ đầy đủ không cắt
            btnUpdate.FlatStyle = FlatStyle.Flat;
            btnUpdate.FlatAppearance.BorderSize = 0;
            btnUpdate.BackColor = Color.FromArgb(0, 122, 204); // Xanh dương
            btnUpdate.ForeColor = Color.White;
            btnUpdate.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            btnUpdate.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 150, 255);
            btnUpdate.Cursor = Cursors.Hand;
            btnUpdate.Height = 50;
            btnUpdate.Width = 120; // Rộng hơn để chữ "Cập nhật" đầy đủ
            btnUpdate.Text = "Cập nhật"; // Đảm bảo text đúng (nếu trước là "Cập")
            btnUpdate.TextAlign = ContentAlignment.MiddleCenter;

            // Nút Thoát: Cam lớn hơn, chữ đầy đủ không cắt
            btnExit.FlatStyle = FlatStyle.Flat;
            btnExit.FlatAppearance.BorderSize = 0;
            btnExit.BackColor = Color.FromArgb(255, 140, 0);
            btnExit.ForeColor = Color.White;
            btnExit.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            btnExit.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 170, 70);
            btnExit.Cursor = Cursors.Hand;
            btnExit.Height = 50;
            btnExit.Width = 120; // Rộng hơn để chữ "Thoát" đầy đủ
            btnExit.Text = "Thoát";
            btnExit.TextAlign = ContentAlignment.MiddleCenter;

            // Gợi ý Designer: Đặt 2 nút cạnh nhau, căn giữa form
        }

        void ChangeAccount(Account acc)
        {
            txtUserName.Text = LoginAccount.UserName;
            txtDisplayName.Text = LoginAccount.DisplayName;
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
                if (UpdateAccount != null)
                    UpdateAccount(this, new AccountEvent(AccountDAL.Instance.GetAccountByUserName(userName)));
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