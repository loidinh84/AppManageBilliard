 namespace AppManageBilliard.GUI
{
    partial class fTableManager
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fTableManager));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.adminToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.thôngTinTàiKhoảnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCancelTable = new System.Windows.Forms.Button();
            this.cbDiscount = new System.Windows.Forms.ComboBox();
            this.lblCurrentTable = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnChuyenBan = new System.Windows.Forms.Button();
            this.lsvBill = new System.Windows.Forms.ListView();
            this.colTenMon = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSoLuong = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDonGia = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colThanhTien = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.giảm1MónToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xóaHẳnMónNàyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.btnThanhToan = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.flpTable = new System.Windows.Forms.FlowLayoutPanel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.flpFood = new System.Windows.Forms.FlowLayoutPanel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.txtGioVao = new System.Windows.Forms.Label();
            this.txtTongGio = new System.Windows.Forms.Label();
            this.txtTongMon = new System.Windows.Forms.Label();
            this.txtTongTien = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.adminToolStripMenuItem,
            this.thôngTinTàiKhoảnToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1191, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // adminToolStripMenuItem
            // 
            this.adminToolStripMenuItem.Name = "adminToolStripMenuItem";
            this.adminToolStripMenuItem.Size = new System.Drawing.Size(67, 24);
            this.adminToolStripMenuItem.Text = "Admin";
            this.adminToolStripMenuItem.Click += new System.EventHandler(this.adminToolStripMenuItem_Click);
            // 
            // thôngTinTàiKhoảnToolStripMenuItem
            // 
            this.thôngTinTàiKhoảnToolStripMenuItem.Name = "thôngTinTàiKhoảnToolStripMenuItem";
            this.thôngTinTàiKhoảnToolStripMenuItem.Size = new System.Drawing.Size(151, 24);
            this.thôngTinTàiKhoảnToolStripMenuItem.Text = "Thông tin tài khoản";
            this.thôngTinTàiKhoảnToolStripMenuItem.Click += new System.EventHandler(this.thôngTinTàiKhoảnToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtTongTien);
            this.panel1.Controls.Add(this.txtTongMon);
            this.panel1.Controls.Add(this.txtTongGio);
            this.panel1.Controls.Add(this.txtGioVao);
            this.panel1.Controls.Add(this.btnCancelTable);
            this.panel1.Controls.Add(this.cbDiscount);
            this.panel1.Controls.Add(this.lblCurrentTable);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.btnChuyenBan);
            this.panel1.Controls.Add(this.lsvBill);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btnThanhToan);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(796, 28);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(395, 660);
            this.panel1.TabIndex = 2;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // btnCancelTable
            // 
            this.btnCancelTable.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnCancelTable.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelTable.Location = new System.Drawing.Point(290, 10);
            this.btnCancelTable.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCancelTable.Name = "btnCancelTable";
            this.btnCancelTable.Size = new System.Drawing.Size(91, 34);
            this.btnCancelTable.TabIndex = 17;
            this.btnCancelTable.Text = "Hủy bàn";
            this.btnCancelTable.UseVisualStyleBackColor = false;
            this.btnCancelTable.Click += new System.EventHandler(this.btnCancelTable_Click);
            // 
            // cbDiscount
            // 
            this.cbDiscount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDiscount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbDiscount.FormattingEnabled = true;
            this.cbDiscount.Location = new System.Drawing.Point(252, 75);
            this.cbDiscount.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbDiscount.Name = "cbDiscount";
            this.cbDiscount.Size = new System.Drawing.Size(129, 28);
            this.cbDiscount.TabIndex = 16;
            // 
            // lblCurrentTable
            // 
            this.lblCurrentTable.AutoSize = true;
            this.lblCurrentTable.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentTable.Location = new System.Drawing.Point(5, 10);
            this.lblCurrentTable.Name = "lblCurrentTable";
            this.lblCurrentTable.Size = new System.Drawing.Size(153, 28);
            this.lblCurrentTable.TabIndex = 15;
            this.lblCurrentTable.Text = "Chưa chọn bàn";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(10, 527);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 23);
            this.label4.TabIndex = 13;
            this.label4.Text = "Tổng món";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(10, 495);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 23);
            this.label3.TabIndex = 10;
            this.label3.Text = "Tổng giờ chơi";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 461);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 23);
            this.label2.TabIndex = 9;
            this.label2.Text = "Giờ bắt đầu";
            // 
            // btnChuyenBan
            // 
            this.btnChuyenBan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btnChuyenBan.FlatAppearance.BorderSize = 0;
            this.btnChuyenBan.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChuyenBan.Location = new System.Drawing.Point(30, 599);
            this.btnChuyenBan.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnChuyenBan.Name = "btnChuyenBan";
            this.btnChuyenBan.Size = new System.Drawing.Size(157, 41);
            this.btnChuyenBan.TabIndex = 7;
            this.btnChuyenBan.Text = "Chuyển bàn";
            this.btnChuyenBan.UseVisualStyleBackColor = false;
            this.btnChuyenBan.Click += new System.EventHandler(this.btnChuyenBan_Click);
            // 
            // lsvBill
            // 
            this.lsvBill.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colTenMon,
            this.colSoLuong,
            this.colDonGia,
            this.colThanhTien});
            this.lsvBill.ContextMenuStrip = this.contextMenuStrip2;
            this.lsvBill.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lsvBill.GridLines = true;
            this.lsvBill.HideSelection = false;
            this.lsvBill.Location = new System.Drawing.Point(14, 116);
            this.lsvBill.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lsvBill.Name = "lsvBill";
            this.lsvBill.Size = new System.Drawing.Size(367, 329);
            this.lsvBill.TabIndex = 6;
            this.toolTip1.SetToolTip(this.lsvBill, "Nhấp chuột phải để chỉnh sửa món ăn");
            this.lsvBill.UseCompatibleStateImageBehavior = false;
            this.lsvBill.View = System.Windows.Forms.View.Details;
            this.lsvBill.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            this.lsvBill.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lsvBill_MouseDoubleClick);
            // 
            // colTenMon
            // 
            this.colTenMon.Text = "Tên món";
            this.colTenMon.Width = 80;
            // 
            // colSoLuong
            // 
            this.colSoLuong.Text = "Số Lượng";
            this.colSoLuong.Width = 78;
            // 
            // colDonGia
            // 
            this.colDonGia.Text = "Đơn giá";
            this.colDonGia.Width = 70;
            // 
            // colThanhTien
            // 
            this.colThanhTien.Text = "Thành tiền";
            this.colThanhTien.Width = 92;
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.giảm1MónToolStripMenuItem,
            this.xóaHẳnMónNàyToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(194, 52);
            this.contextMenuStrip2.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip2_Opening);
            // 
            // giảm1MónToolStripMenuItem
            // 
            this.giảm1MónToolStripMenuItem.Name = "giảm1MónToolStripMenuItem";
            this.giảm1MónToolStripMenuItem.Size = new System.Drawing.Size(193, 24);
            this.giảm1MónToolStripMenuItem.Text = "Giảm 1 món";
            this.giảm1MónToolStripMenuItem.Click += new System.EventHandler(this.giảm1MónToolStripMenuItem_Click);
            // 
            // xóaHẳnMónNàyToolStripMenuItem
            // 
            this.xóaHẳnMónNàyToolStripMenuItem.Name = "xóaHẳnMónNàyToolStripMenuItem";
            this.xóaHẳnMónNàyToolStripMenuItem.Size = new System.Drawing.Size(193, 24);
            this.xóaHẳnMónNàyToolStripMenuItem.Text = "Xóa hẳn món này";
            this.xóaHẳnMónNàyToolStripMenuItem.Click += new System.EventHandler(this.xóaHẳnMónNàyToolStripMenuItem_Click_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 555);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 28);
            this.label1.TabIndex = 5;
            this.label1.Text = "Tổng tiền";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // btnThanhToan
            // 
            this.btnThanhToan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnThanhToan.FlatAppearance.BorderSize = 0;
            this.btnThanhToan.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnThanhToan.Location = new System.Drawing.Point(207, 599);
            this.btnThanhToan.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnThanhToan.Name = "btnThanhToan";
            this.btnThanhToan.Size = new System.Drawing.Size(157, 41);
            this.btnThanhToan.TabIndex = 4;
            this.btnThanhToan.Text = "Thanh Toán";
            this.btnThanhToan.UseVisualStyleBackColor = false;
            this.btnThanhToan.Click += new System.EventHandler(this.btnThanhToan_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 28);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(796, 660);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.flpTable);
            this.tabPage1.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage1.Size = new System.Drawing.Size(788, 631);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Danh Sách Bàn";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // flpTable
            // 
            this.flpTable.AutoScroll = true;
            this.flpTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpTable.Location = new System.Drawing.Point(3, 2);
            this.flpTable.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.flpTable.Name = "flpTable";
            this.flpTable.Size = new System.Drawing.Size(782, 627);
            this.flpTable.TabIndex = 0;
            this.flpTable.Paint += new System.Windows.Forms.PaintEventHandler(this.flpTable_Paint);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.flpFood);
            this.tabPage2.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage2.Size = new System.Drawing.Size(788, 631);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Thực Đơn";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // flpFood
            // 
            this.flpFood.AutoScroll = true;
            this.flpFood.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpFood.Location = new System.Drawing.Point(3, 2);
            this.flpFood.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.flpFood.Name = "flpFood";
            this.flpFood.Size = new System.Drawing.Size(782, 627);
            this.flpFood.TabIndex = 0;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // txtGioVao
            // 
            this.txtGioVao.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGioVao.Location = new System.Drawing.Point(203, 461);
            this.txtGioVao.Name = "txtGioVao";
            this.txtGioVao.Size = new System.Drawing.Size(158, 23);
            this.txtGioVao.TabIndex = 18;
            this.txtGioVao.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTongGio
            // 
            this.txtTongGio.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTongGio.Location = new System.Drawing.Point(201, 497);
            this.txtTongGio.Name = "txtTongGio";
            this.txtTongGio.Size = new System.Drawing.Size(160, 23);
            this.txtTongGio.TabIndex = 19;
            this.txtTongGio.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.txtTongGio.Click += new System.EventHandler(this.txtTongGio_Click);
            // 
            // txtTongMon
            // 
            this.txtTongMon.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTongMon.Location = new System.Drawing.Point(217, 527);
            this.txtTongMon.Name = "txtTongMon";
            this.txtTongMon.Size = new System.Drawing.Size(144, 23);
            this.txtTongMon.TabIndex = 20;
            this.txtTongMon.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTongTien
            // 
            this.txtTongTien.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTongTien.Location = new System.Drawing.Point(171, 555);
            this.txtTongTien.Name = "txtTongTien";
            this.txtTongTien.Size = new System.Drawing.Size(193, 28);
            this.txtTongTien.TabIndex = 21;
            this.txtTongTien.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // fTableManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1191, 688);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "fTableManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quản lý bàn bida";
            this.Load += new System.EventHandler(this.fTableManager_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.contextMenuStrip2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem adminToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem thôngTinTàiKhoảnToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnThanhToan;
        private System.Windows.Forms.ListView lsvBill;
        private System.Windows.Forms.ColumnHeader colTenMon;
        private System.Windows.Forms.ColumnHeader colSoLuong;
        private System.Windows.Forms.ColumnHeader colDonGia;
        private System.Windows.Forms.ColumnHeader colThanhTien;
        private System.Windows.Forms.Button btnChuyenBan;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblCurrentTable;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.FlowLayoutPanel flpTable;
        private System.Windows.Forms.FlowLayoutPanel flpFood;
        private System.Windows.Forms.ComboBox cbDiscount;
        private System.Windows.Forms.Button btnCancelTable;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem giảm1MónToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xóaHẳnMónNàyToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label txtGioVao;
        private System.Windows.Forms.Label txtTongGio;
        private System.Windows.Forms.Label txtTongMon;
        private System.Windows.Forms.Label txtTongTien;
    }
}