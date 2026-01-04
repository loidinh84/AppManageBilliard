namespace AppManageBilliard.GUI
{
    partial class fTimePicker
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblDisplayTime = new System.Windows.Forms.Label();
            this.pnlClock = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblMode = new System.Windows.Forms.Label();
            this.btnAM = new System.Windows.Forms.Button();
            this.btnPM = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblDisplayTime
            // 
            this.lblDisplayTime.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.lblDisplayTime.Font = new System.Drawing.Font("Segoe UI", 36F, System.Drawing.FontStyle.Bold);
            this.lblDisplayTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.lblDisplayTime.Location = new System.Drawing.Point(12, 50);
            this.lblDisplayTime.Name = "lblDisplayTime";
            this.lblDisplayTime.Size = new System.Drawing.Size(250, 80);
            this.lblDisplayTime.TabIndex = 0;
            this.lblDisplayTime.Text = "00:00";
            this.lblDisplayTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlClock
            // 
            this.pnlClock.Location = new System.Drawing.Point(280, 20);
            this.pnlClock.Name = "pnlClock";
            this.pnlClock.Size = new System.Drawing.Size(260, 260);
            this.pnlClock.TabIndex = 1;
            this.pnlClock.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlClock_Paint);
            this.pnlClock.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlClock_MouseDown);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(150, 240);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 40);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "Đồng ý";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(30, 240);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 40);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Hủy";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblMode
            // 
            this.lblMode.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblMode.Location = new System.Drawing.Point(12, 10);
            this.lblMode.Name = "lblMode";
            this.lblMode.Size = new System.Drawing.Size(250, 30);
            this.lblMode.TabIndex = 4;
            this.lblMode.Text = "Chọn Giờ";
            this.lblMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnAM
            // 
            this.btnAM.Location = new System.Drawing.Point(42, 152);
            this.btnAM.Name = "btnAM";
            this.btnAM.Size = new System.Drawing.Size(98, 35);
            this.btnAM.TabIndex = 5;
            this.btnAM.Text = "SA";
            this.btnAM.Click += new System.EventHandler(this.btnAM_Click);
            // 
            // btnPM
            // 
            this.btnPM.Location = new System.Drawing.Point(146, 152);
            this.btnPM.Name = "btnPM";
            this.btnPM.Size = new System.Drawing.Size(98, 35);
            this.btnPM.TabIndex = 6;
            this.btnPM.Text = "CH";
            this.btnPM.Click += new System.EventHandler(this.btnPM_Click);
            // 
            // fTimePicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 310);
            this.Controls.Add(this.lblMode);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.pnlClock);
            this.Controls.Add(this.lblDisplayTime);
            this.Controls.Add(this.btnAM);
            this.Controls.Add(this.btnPM);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "fTimePicker";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Chọn giờ bắt đầu";
            this.ResumeLayout(false);

        }

        #endregion

        // CÁC DÒNG QUAN TRỌNG NHẤT ĐỂ HẾT LỖI DEFINITION:
        private System.Windows.Forms.Label lblDisplayTime;
        private System.Windows.Forms.Panel pnlClock;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.Button btnAM;
        private System.Windows.Forms.Button btnPM;
    }
}