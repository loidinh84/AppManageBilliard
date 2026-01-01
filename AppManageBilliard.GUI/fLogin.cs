using AppManageBilliard.BUS;
using AppManageBilliard.DAL;
using AppManageBilliard.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D; // Để vẽ gradient và bo tròn
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppManageBilliard.GUI
{
    public partial class fLogin : Form
    {
        public fLogin()
        {
            InitializeComponent();
            this.Opacity = 0; // Bắt đầu trong suốt để fade in
            CustomizeDesign(); // Chỉ thêm dòng này để làm đẹp giao diện
        }

        // === HÀM LÀM ĐẸP GIAO DIỆN - KHÔNG ĐỘNG VÀO LOGIC ===
        private void CustomizeDesign()
        {
            // 1. Nền gradient xanh nhạt rất nhẹ
            this.BackColor = Color.FromArgb(240, 248, 255); // AliceBlue
            this.Paint += (s, e) =>
            {
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    this.ClientRectangle,
                    Color.FromArgb(240, 248, 255),
                    Color.White,
                    90F))
                {
                    e.Graphics.FillRectangle(brush, this.ClientRectangle);
                }
            };

            // 2. Bo tròn form nhẹ + bóng đổ mềm
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;

            GraphicsPath path = new GraphicsPath();
            int radius = 30;
            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(this.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(this.Width - radius, this.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, this.Height - radius, radius, radius, 90, 90);
            path.CloseAllFigures();
            this.Region = new Region(path);

            // 3. Làm đẹp TextBox (viền dưới xanh)
            txtUserName.BorderStyle = BorderStyle.None;
            txtUserName.BackColor = Color.White;
            txtUserName.Font = new Font("Segoe UI", 12F);
            txtUserName.ForeColor = Color.Black;
            txtUserName.Padding = new Padding(10);
            txtUserName.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(0, 123, 255), 2))
                {
                    e.Graphics.DrawLine(pen, 0, txtUserName.Height - 1, txtUserName.Width, txtUserName.Height - 1);
                }
            };

            txtPassWord.BorderStyle = BorderStyle.None;
            txtPassWord.BackColor = Color.White;
            txtPassWord.Font = new Font("Segoe UI", 12F);
            txtPassWord.ForeColor = Color.Black;
            txtPassWord.Padding = new Padding(10);
            txtPassWord.UseSystemPasswordChar = true;
            txtPassWord.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(0, 123, 255), 2))
                {
                    e.Graphics.DrawLine(pen, 0, txtPassWord.Height - 1, txtPassWord.Width, txtPassWord.Height - 1);
                }
            };

            // 4. Button Đăng nhập đẹp, hover mượt
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.BackColor = Color.FromArgb(0, 123, 255);
            btnLogin.ForeColor = Color.White;
            btnLogin.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.MouseEnter += (s, e) => btnLogin.BackColor = Color.FromArgb(0, 105, 220);
            btnLogin.MouseLeave += (s, e) => btnLogin.BackColor = Color.FromArgb(0, 123, 255);

            // 5. Button Thoát nhẹ nhàng
            btnExit.FlatStyle = FlatStyle.Flat;
            btnExit.FlatAppearance.BorderSize = 0;
            btnExit.BackColor = Color.Transparent;
            btnExit.ForeColor = Color.FromArgb(220, 53, 69);
            btnExit.Font = new Font("Segoe UI", 11F);
            btnExit.Cursor = Cursors.Hand;
            btnExit.MouseEnter += (s, e) => btnExit.ForeColor = Color.Red;
            btnExit.MouseLeave += (s, e) => btnExit.ForeColor = Color.FromArgb(220, 53, 69);

            // 6. Fade in khi mở form
            Timer fadeInTimer = new Timer { Interval = 20 };
            fadeInTimer.Tick += (s, e) =>
            {
                if (this.Opacity < 1)
                    this.Opacity += 0.05;
                else
                    fadeInTimer.Stop();
            };
            fadeInTimer.Start();
        }
        // ================================================

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string userName = txtUserName.Text;
            string passWord = txtPassWord.Text;
            if (Login(userName, passWord))
            {
                MessageBox.Show("Đăng nhập thành công!", "Thông báo");
                Account loginAccount = AccountDAL.Instance.GetAccountByUserName(userName);
                fTableManager f = new fTableManager(loginAccount);
                this.Hide();
                f.ShowDialog();
                this.Show();
            }
            else
            {
                MessageBox.Show("Sai tên tài khoản hoặc mật khẩu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        bool Login(string userName, string passWord)
        {
            return AccountBUS.Instance.Login(userName, passWord);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void fLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn có thật sự muốn thoát chương trình?", "Thông báo", MessageBoxButtons.OKCancel) != DialogResult.OK)
            {
                e.Cancel = true;
            }
        }

        private void fLogin_Load(object sender, EventArgs e)
        {
        }

        private void txtPassWord_TextChanged(object sender, EventArgs e)
        {

        }
    }
}