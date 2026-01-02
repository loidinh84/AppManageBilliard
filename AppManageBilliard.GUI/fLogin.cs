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

namespace AppManageBilliard.GUI
{
    public partial class fLogin : Form
    {
        private bool isPasswordVisible = false;

        public fLogin()
        {
            InitializeComponent();
            this.Opacity = 0;
            CustomizeDesign();

            // Thêm 2 event này để bo góc đúng (fix lỗi chính)
            this.Load += fLogin_Load;
            this.Resize += fLogin_Resize;
        }

        private void CustomizeDesign()
        {
            // ===== FORM =====
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(245, 248, 250);

            // ===== TEXTBOX USER =====
            StyleTextBox(txtUserName);

            // ===== TEXTBOX PASSWORD =====
            StyleTextBox(txtPassWord);
            txtPassWord.UseSystemPasswordChar = true;

            // ===== PICTUREBOX EYE =====
            pbEye.SizeMode = PictureBoxSizeMode.Zoom;
            pbEye.Cursor = Cursors.Hand;
            pbEye.BackColor = Color.Transparent;

            // Dùng icon từ Resources bạn đã thêm (eye_show và eye_hide)
            pbEye.Image = Properties.Resources.eye_hide; // Ban đầu ẩn mật khẩu

            pbEye.Click += (s, e) =>
            {
                isPasswordVisible = !isPasswordVisible;
                txtPassWord.UseSystemPasswordChar = !isPasswordVisible;
                pbEye.Image = isPasswordVisible ? Properties.Resources.eye_show : Properties.Resources.eye_hide;
            };

            pbEye.MouseEnter += (s, e) => pbEye.BackColor = Color.FromArgb(230, 240, 255);
            pbEye.MouseLeave += (s, e) => pbEye.BackColor = Color.Transparent;

            // ===== BUTTON LOGIN =====
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.BackColor = Color.FromArgb(0, 123, 255);
            btnLogin.ForeColor = Color.White;
            btnLogin.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnLogin.Height = 50;
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.Text = "Đăng nhập";
            btnLogin.TextAlign = ContentAlignment.MiddleCenter;

            btnLogin.MouseEnter += (s, e) => btnLogin.BackColor = Color.FromArgb(0, 105, 220);
            btnLogin.MouseLeave += (s, e) => btnLogin.BackColor = Color.FromArgb(0, 123, 255);

            // ===== BUTTON THOÁT =====
            btnExit.FlatStyle = FlatStyle.Flat;
            btnExit.FlatAppearance.BorderSize = 0;
            btnExit.BackColor = Color.FromArgb(220, 53, 69);
            btnExit.ForeColor = Color.White;
            btnExit.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnExit.Height = 50;
            btnExit.Width = 120;
            btnExit.Text = "Thoát";
            btnExit.TextAlign = ContentAlignment.MiddleCenter;
            btnExit.Cursor = Cursors.Hand;

            btnExit.MouseEnter += (s, e) => btnExit.BackColor = Color.FromArgb(200, 35, 55);
            btnExit.MouseLeave += (s, e) => btnExit.BackColor = Color.FromArgb(220, 53, 69);

            // ===== FADE IN =====
            Timer fade = new Timer { Interval = 15 };
            fade.Tick += (s, e) =>
            {
                if (this.Opacity < 1) this.Opacity += 0.05;
                else fade.Stop();
            };
            fade.Start();
        }

        // Hàm bo góc đúng - gọi khi form load và resize
        private void ApplyRoundedCorners()
        {
            int radius = 20;
            GraphicsPath path = new GraphicsPath();
            Rectangle rect = this.ClientRectangle;
            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(rect.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(rect.Width - radius, rect.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, rect.Height - radius, radius, radius, 90, 90);
            path.CloseAllFigures();
            this.Region = new Region(path);
        }

        private void fLogin_Load(object sender, EventArgs e)
        {
            ApplyRoundedCorners(); // Bo góc đúng sau khi form có kích thước
        }

        private void fLogin_Resize(object sender, EventArgs e)
        {
            ApplyRoundedCorners(); // An toàn nếu form bị resize
        }

        private void StyleTextBox(TextBox txt)
        {
            txt.BorderStyle = BorderStyle.None;
            txt.BackColor = Color.White;
            txt.Font = new Font("Segoe UI", 11F);
            txt.ForeColor = Color.Black;
            txt.Height = 40;

            Panel line = new Panel();
            line.Height = 2;
            line.Dock = DockStyle.Bottom;
            line.BackColor = Color.LightGray;
            txt.Controls.Add(line);

            txt.Enter += (s, e) => line.BackColor = Color.FromArgb(0, 123, 255);
            txt.Leave += (s, e) => line.BackColor = Color.LightGray;
        }

        // LOGIC GIỮ NGUYÊN
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
    }
}