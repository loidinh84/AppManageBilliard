using System;
using System.Drawing;
using System.Windows.Forms;

namespace AppManageBilliard.GUI
{
    public partial class fTimePicker : Form
    {
        public TimeSpan SelectedTime { get; set; }

        private int currentHour = DateTime.Now.Hour;
        private int currentMinute = DateTime.Now.Minute;
        private bool isSelectingHour = true;
        private bool isAM = true;

        public fTimePicker()
        {
            InitializeComponent();
            UpdateDisplay();
        }
        private void btnAM_Click(object sender, EventArgs e)
        {
            isAM = true;
            btnAM.BackColor = Color.LightBlue;
            btnPM.BackColor = Color.WhiteSmoke;
            UpdateDisplay();
        }
        private void btnPM_Click(object sender, EventArgs e)
        {
            isAM = false;
            btnPM.BackColor = Color.LightBlue;
            btnAM.BackColor = Color.WhiteSmoke;
            UpdateDisplay();
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            int hour24 = currentHour;

            if (isAM)
            {
                if (hour24 == 12) hour24 = 0;
            }
            else
            {
                if (hour24 != 12) hour24 += 12;
            }

            SelectedTime = new TimeSpan(hour24, currentMinute, 0);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void pnlClock_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            int radius = pnlClock.Width / 2 - 30;
            Point center = new Point(pnlClock.Width / 2, pnlClock.Height / 2);

            g.FillEllipse(Brushes.WhiteSmoke, center.X - radius - 20, center.Y - radius - 20, (radius + 20) * 2, (radius + 20) * 2);

            double hourAngle = (currentHour % 12 * 30 + currentMinute * 0.5 - 90) * Math.PI / 180;
            double minuteAngle = (currentMinute * 6 - 90) * Math.PI / 180;

            using (Pen penMinute = new Pen(Color.LightBlue, 3))
            {
                int minHandLength = radius - 10;
                g.DrawLine(penMinute, center, new Point(
                    (int)(center.X + minHandLength * Math.Cos(minuteAngle)),
                    (int)(center.Y + minHandLength * Math.Sin(minuteAngle))));
            }

            using (Pen penHour = new Pen(Color.FromArgb(0, 120, 215), 5))
            {
                int hourHandLength = radius / 2;
                g.DrawLine(penHour, center, new Point(
                    (int)(center.X + hourHandLength * Math.Cos(hourAngle)),
                    (int)(center.Y + hourHandLength * Math.Sin(hourAngle))));
            }

            g.FillEllipse(Brushes.DarkBlue, center.X - 5, center.Y - 5, 10, 10);

            for (int i = 1; i <= 12; i++)
            {
                double angle = (i * 30 - 90) * Math.PI / 180;
                int x = (int)(center.X + radius * Math.Cos(angle)) - 10;
                int y = (int)(center.Y + radius * Math.Sin(angle)) - 10;

                string text = isSelectingHour ? i.ToString() : (i * 5 % 60).ToString("00");
                bool isSelected = (isSelectingHour && i == currentHour % 12) || (!isSelectingHour && i * 5 % 60 == currentMinute);

                Brush textBrush = isSelected ? Brushes.Blue : Brushes.Black;
                Font textFont = new Font("Segoe UI", isSelected ? 11 : 10, isSelected ? FontStyle.Bold : FontStyle.Regular);

                g.DrawString(text, textFont, textBrush, x, y);
            }
        }

        private void pnlClock_MouseDown(object sender, MouseEventArgs e)
        {
            Point center = new Point(pnlClock.Width / 2, pnlClock.Height / 2);
            double angle = Math.Atan2(e.Y - center.Y, e.X - center.X) * 180 / Math.PI + 90;
            if (angle < 0) angle += 360;

            if (isSelectingHour)
            {
                currentHour = (int)Math.Round(angle / 30);
                if (currentHour == 0) currentHour = 12;
                isSelectingHour = false;
                lblMode.Text = "Chọn Phút";
            }
            else
            {
                currentMinute = (int)Math.Round(angle / 6);
                if (currentMinute == 60) currentMinute = 0;
            }

            UpdateDisplay();
            pnlClock.Invalidate();
        }

        

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void UpdateDisplay()
        {
            lblDisplayTime.Text = string.Format("{0:00}:{1:00}", currentHour, currentMinute);
        }
        
    }
}