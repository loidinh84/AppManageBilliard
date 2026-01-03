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
        private CheckBox ckbRemember = new CheckBox();

        public fLogin()
        {
            InitializeComponent();
            this.Opacity = 0;
            CustomizeDesign();

            this.Load += fLogin_Load;
            this.Resize += fLogin_Resize;
        }

        private void CustomizeDesign()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(245, 248, 250);

            StyleTextBox(txtUserName);
            StylePasswordSection();

            ckbRemember.Text = "Ghi nhớ mật khẩu";
            ckbRemember.Font = new Font("Segoe UI", 9F);
            ckbRemember.ForeColor = Color.Gray;
            ckbRemember.Location = new Point(txtPassWord.Left, txtPassWord.Bottom + 10);
            ckbRemember.AutoSize = true;
            ckbRemember.Cursor = Cursors.Hand;
            this.Controls.Add(ckbRemember);

            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.BackColor = Color.FromArgb(0, 123, 255);
            btnLogin.ForeColor = Color.White;
            btnLogin.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.MouseEnter += (s, e) => btnLogin.BackColor = Color.FromArgb(0, 105, 220);
            btnLogin.MouseLeave += (s, e) => btnLogin.BackColor = Color.FromArgb(0, 123, 255);

            btnExit.FlatStyle = FlatStyle.Flat;
            btnExit.FlatAppearance.BorderSize = 0;
            btnExit.BackColor = Color.FromArgb(220, 53, 69);
            btnExit.ForeColor = Color.White;
            btnExit.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnExit.Cursor = Cursors.Hand;
            btnExit.MouseEnter += (s, e) => btnExit.BackColor = Color.FromArgb(200, 35, 55);
            btnExit.MouseLeave += (s, e) => btnExit.BackColor = Color.FromArgb(220, 53, 69);

            Timer fade = new Timer { Interval = 15 };
            fade.Tick += (s, e) =>
            {
                if (this.Opacity < 1) this.Opacity += 0.05;
                else fade.Stop();
            };
            fade.Start();
        }

        private void StylePasswordSection()
        {
            txtPassWord.BorderStyle = BorderStyle.None;
            txtPassWord.Font = new Font("Segoe UI", 11F);
            txtPassWord.UseSystemPasswordChar = true;
            txtPassWord.BackColor = Color.White;

            pbEye.Size = new Size(22, 22);
            pbEye.Cursor = Cursors.Hand;
            pbEye.BackColor = Color.White;
            pbEye.SizeMode = PictureBoxSizeMode.Zoom;
            pbEye.Image = Properties.Resources.eye_hide;

            txtPassWord.Controls.Add(pbEye);
            UpdateEyeLocation();

            pbEye.Click += (s, e) =>
            {
                isPasswordVisible = !isPasswordVisible;
                txtPassWord.UseSystemPasswordChar = !isPasswordVisible;
                pbEye.Image = isPasswordVisible ? Properties.Resources.eye_show : Properties.Resources.eye_hide;
                txtPassWord.Focus();
                txtPassWord.SelectionStart = txtPassWord.Text.Length;
            };

            pbEye.MouseEnter += (s, e) => pbEye.BackColor = Color.FromArgb(240, 240, 240);
            pbEye.MouseLeave += (s, e) => pbEye.BackColor = Color.White;
        }

        private void UpdateEyeLocation()
        {
            int x = txtPassWord.Width - pbEye.Width - 2;
            int y = (txtPassWord.Height - pbEye.Height) / 2 - 2;
            pbEye.Location = new Point(x, y);
            pbEye.BringToFront();
        }

        private void StyleTextBox(TextBox txt)
        {
            txt.BorderStyle = BorderStyle.None;
            txt.BackColor = Color.White;
            txt.Font = new Font("Segoe UI", 11F);

            Panel line = new Panel();
            line.Height = 2;
            line.Dock = DockStyle.Bottom;
            line.BackColor = Color.LightGray;
            txt.Controls.Add(line);

            txt.Enter += (s, e) => line.BackColor = Color.FromArgb(0, 123, 255);
            txt.Leave += (s, e) => line.BackColor = Color.LightGray;
        }

        private void ApplyRoundedCorners()
        {
            int radius = 25;
            GraphicsPath path = new GraphicsPath();
            Rectangle rect = this.ClientRectangle;
            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(rect.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(rect.Width - radius, rect.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, rect.Height - radius, radius, radius, 90, 90);
            path.CloseAllFigures();
            this.Region = new Region(path);
        }

        // ===== PHẦN THAY ĐỔI CHÍNH =====
        private void fLogin_Load(object sender, EventArgs e)
        {
            ApplyRoundedCorners();

            if (Properties.Settings.Default.IsRemember)
            {
                string savedUser = Properties.Settings.Default.UserName;
                string savedPass = Properties.Settings.Default.PassWord;

                // Kiểm tra xem dữ liệu cũ có hợp lệ không
                if (Login(savedUser, savedPass))
                {
                    // Tự động điền để người dùng thấy (tùy chọn)
                    txtUserName.Text = savedUser;
                    txtPassWord.Text = savedPass;
                    ckbRemember.Checked = true;

                    // Chuyển form ngay lập tức
                    Account loginAccount = AccountDAL.Instance.GetAccountByUserName(savedUser);
                    fTableManager f = new fTableManager(loginAccount);

                    this.Hide();
                    f.ShowDialog();
                    this.Close(); // Đóng luôn form login sau khi thoát TableManager
                }
                else
                {
                    // Nếu thông tin cũ không còn đúng, xóa đi để nhập lại
                    txtUserName.Text = savedUser;
                    ckbRemember.Checked = true;
                }
            }
        }

        private void fLogin_Resize(object sender, EventArgs e)
        {
            ApplyRoundedCorners();
            UpdateEyeLocation();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string userName = txtUserName.Text;
            string passWord = txtPassWord.Text;

            if (Login(userName, passWord))
            {
                if (ckbRemember.Checked)
                {
                    Properties.Settings.Default.UserName = userName;
                    Properties.Settings.Default.PassWord = passWord;
                    Properties.Settings.Default.IsRemember = true;
                }
                else
                {
                    Properties.Settings.Default.UserName = "";
                    Properties.Settings.Default.PassWord = "";
                    Properties.Settings.Default.IsRemember = false;
                }
                Properties.Settings.Default.Save();

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
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(passWord)) return false;
            return AccountBUS.Instance.Login(userName, passWord);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}