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
            // Cài đặt form chính
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(40, 44, 52); // Nền tối hiện đại
            this.Size = new Size(420, 580); // Kích thước cố định đẹp
            ApplyRoundedCorners(30); // Bo tròn form

            // Panel chính (nền trắng nổi bật)
            Panel mainPanel = new Panel
            {
                Size = new Size(380, 520),
                Location = new Point((this.Width - 380) / 2, (this.Height - 520) / 2),
                BackColor = Color.White,
                Name = "mainPanel"
            };
            ApplyRoundedCornersToControl(mainPanel, 20);
            this.Controls.Add(mainPanel);
            mainPanel.BringToFront();

            // Tiêu đề
            Label lblTitle = new Label
            {
                Text = "Đăng Nhập",
                Font = new Font("Segoe UI", 28F, FontStyle.Bold),
                ForeColor = Color.FromArgb(40, 44, 52),
                AutoSize = true,
                Location = new Point((mainPanel.Width - 200) / 2, 60)
            };
            mainPanel.Controls.Add(lblTitle);

            Label lblSubtitle = new Label
            {
                Text = "Quản lý quán billiard",
                Font = new Font("Segoe UI", 11F, FontStyle.Regular),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point((mainPanel.Width - 160) / 2, 110)
            };
            mainPanel.Controls.Add(lblSubtitle);

            // Textbox Username - hiện đại hóa
            StyleModernInput(txtUserName, "Tên đăng nhập", new Point(50, 170));
            txtUserName.Size = new Size(260, 45);
            mainPanel.Controls.Add(txtUserName);

            // Password - giữ nguyên eye toggle nhưng đẹp hơn
            StyleModernInput(txtPassWord, "Mật khẩu", new Point(50, 240));
            StylePasswordSection(); // Giữ nguyên chức năng eye
            mainPanel.Controls.Add(txtPassWord);

            // Checkbox Remember
            ckbRemember.Text = "Ghi nhớ mật khẩu";
            ckbRemember.Font = new Font("Segoe UI", 10F);
            ckbRemember.ForeColor = Color.FromArgb(100, 100, 100);
            ckbRemember.Location = new Point(50, 310);
            ckbRemember.AutoSize = true;
            ckbRemember.Cursor = Cursors.Hand;
            mainPanel.Controls.Add(ckbRemember);

            // Button Login
            btnLogin.Text = "ĐĂNG NHẬP";
            btnLogin.Size = new Size(280, 55);
            btnLogin.Location = new Point(50, 360);
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.BackColor = Color.FromArgb(0, 123, 255);
            btnLogin.ForeColor = Color.White;
            btnLogin.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            btnLogin.Cursor = Cursors.Hand;
            ApplyRoundedCornersToControl(btnLogin, 12);
            mainPanel.Controls.Add(btnLogin);

            // Hover nâng lên cho Login
            btnLogin.MouseEnter += (s, e) =>
            {
                btnLogin.BackColor = Color.FromArgb(0, 105, 220);
                btnLogin.Location = new Point(btnLogin.Location.X, btnLogin.Location.Y - 3);
            };
            btnLogin.MouseLeave += (s, e) =>
            {
                btnLogin.BackColor = Color.FromArgb(0, 123, 255);
                btnLogin.Location = new Point(btnLogin.Location.X, btnLogin.Location.Y + 3);
            };

            // Button Exit
            btnExit.Text = "THOÁT";
            btnExit.Size = new Size(280, 55);
            btnExit.Location = new Point(50, 430);
            btnExit.FlatStyle = FlatStyle.Flat;
            btnExit.FlatAppearance.BorderSize = 0;
            btnExit.BackColor = Color.FromArgb(220, 53, 69);
            btnExit.ForeColor = Color.White;
            btnExit.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            btnExit.Cursor = Cursors.Hand;
            ApplyRoundedCornersToControl(btnExit, 12);
            mainPanel.Controls.Add(btnExit);

            // Hover cho Exit
            btnExit.MouseEnter += (s, e) =>
            {
                btnExit.BackColor = Color.FromArgb(200, 35, 55);
                btnExit.Location = new Point(btnExit.Location.X, btnExit.Location.Y - 3);
            };
            btnExit.MouseLeave += (s, e) =>
            {
                btnExit.BackColor = Color.FromArgb(220, 53, 69);
                btnExit.Location = new Point(btnExit.Location.X, btnExit.Location.Y + 3);
            };

            // Fade-in effect (giữ nguyên như cũ nhưng mượt hơn)
            this.Opacity = 0;
            Timer fade = new Timer { Interval = 20 };
            fade.Tick += (s, e) =>
            {
                if (this.Opacity < 1)
                    this.Opacity += 0.08;
                else
                    fade.Stop();
            };
            fade.Start();
        }
        private void ApplyRoundedCorners(int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(this.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(this.Width - radius, this.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, this.Height - radius, radius, radius, 90, 90);
            path.CloseAllFigures();
            this.Region = new Region(path);
        }

        private void ApplyRoundedCornersToControl(Control control, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            Rectangle rect = new Rectangle(0, 0, control.Width, control.Height);
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
            path.CloseAllFigures();
            control.Region = new Region(path);
        }

        private void StyleModernInput(TextBox txt, string placeholder, Point location)
        {
            txt.BorderStyle = BorderStyle.None;
            txt.BackColor = Color.FromArgb(248, 249, 251);
            txt.Font = new Font("Segoe UI", 11F);
            txt.ForeColor = Color.Gray;
            txt.Size = new Size(280, 45);
            txt.Location = location;
            txt.Padding = new Padding(15, 12, 15, 12);

            // Placeholder
            txt.Text = placeholder;
            txt.GotFocus += (s, e) =>
            {
                if (txt.Text == placeholder)
                {
                    txt.Text = "";
                    txt.ForeColor = Color.Black;
                }
            };
            txt.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txt.Text))
                {
                    txt.Text = placeholder;
                    txt.ForeColor = Color.Gray;
                }
            };

            // Viền dưới focus
            Panel bottomLine = new Panel
            {
                Height = 2,
                Dock = DockStyle.Bottom,
                BackColor = Color.FromArgb(0, 123, 255)
            };
            txt.Controls.Add(bottomLine);
            bottomLine.BringToFront();
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
            int paddingRight = 0;   // Khoảng cách icon với mép phải
            int paddingTop = 2;     // Điều chỉnh lên/xuống nếu cần

            int x = txtPassWord.ClientSize.Width - pbEye.Width - paddingRight;
            int y = (txtPassWord.ClientSize.Height - pbEye.Height) / 2 + paddingTop;

            pbEye.Location = new Point(x, y);
            pbEye.Anchor = AnchorStyles.Top | AnchorStyles.Right;
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
                ckbRemember.Checked = true;
                

                if (Login(savedUser, savedPass))
                {
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
                Properties.Settings.Default.IsRemember = ckbRemember.Checked;
                Properties.Settings.Default.UserName = userName;
                Properties.Settings.Default.PassWord = passWord;
                Properties.Settings.Default.Save();

                Account loginAccount = AccountDAL.Instance.GetAccountByUserName(userName);
                fTableManager f = new fTableManager(loginAccount);

                this.Hide();
                f.ShowDialog();
                if (Properties.Settings.Default.IsRemember == false)
                {
                    this.Show();
                }
                else
                {
                    Application.Exit();
                }
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

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {

        }
    }
}