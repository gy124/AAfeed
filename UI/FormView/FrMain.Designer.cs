namespace UI
{
    partial class FrMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrMain));
            this.tbl_main = new System.Windows.Forms.TableLayoutPanel();
            this.pnl_top_menu = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lb_Time = new System.Windows.Forms.Label();
            this.lb_Date = new System.Windows.Forms.Label();
            this.rbtn_rst = new System.Windows.Forms.RadioButton();
            this.img_list_main = new System.Windows.Forms.ImageList(this.components);
            this.rbtn_user = new System.Windows.Forms.RadioButton();
            this.btn_quit = new System.Windows.Forms.Button();
            this.rbtn_sys = new System.Windows.Forms.RadioButton();
            this.rbtn_product = new System.Windows.Forms.RadioButton();
            this.rbtn_run = new System.Windows.Forms.RadioButton();
            this.pnl_sub = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.timer_update = new System.Windows.Forms.Timer(this.components);
            this.timer_key = new System.Windows.Forms.Timer(this.components);
            this.ts_manual = new System.Windows.Forms.ToolStrip();
            this.tsc_step_sel = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator17 = new System.Windows.Forms.ToolStripSeparator();
            this.tsb_xn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator16 = new System.Windows.Forms.ToolStripSeparator();
            this.tsb_xp = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
            this.tsb_yn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator27 = new System.Windows.Forms.ToolStripSeparator();
            this.tsb_yp = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
            this.tsb_zn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator28 = new System.Windows.Forms.ToolStripSeparator();
            this.tsb_zp = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            this.tsb_rn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsb_rp = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsb_x1_n = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsb_x1_p = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.tsb_stop = new System.Windows.Forms.ToolStripButton();
            this.axTcpClient_cmd = new SocketHelper.AxTcpClient(this.components);
            this.axTcpClient_status = new SocketHelper.AxTcpClient(this.components);
            this.timer_reconnect = new System.Windows.Forms.Timer(this.components);
            this.axTcpServer1 = new SocketHelper.AxTcpServer(this.components);
            this.tbl_main.SuspendLayout();
            this.pnl_top_menu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.ts_manual.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbl_main
            // 
            this.tbl_main.ColumnCount = 1;
            this.tbl_main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tbl_main.Controls.Add(this.pnl_top_menu, 0, 0);
            this.tbl_main.Controls.Add(this.pnl_sub, 0, 1);
            this.tbl_main.Controls.Add(this.panel1, 0, 2);
            this.tbl_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbl_main.Location = new System.Drawing.Point(0, 0);
            this.tbl_main.Name = "tbl_main";
            this.tbl_main.RowCount = 3;
            this.tbl_main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tbl_main.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tbl_main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tbl_main.Size = new System.Drawing.Size(1280, 700);
            this.tbl_main.TabIndex = 0;
            // 
            // pnl_top_menu
            // 
            this.pnl_top_menu.Controls.Add(this.button2);
            this.pnl_top_menu.Controls.Add(this.button1);
            this.pnl_top_menu.Controls.Add(this.pictureBox1);
            this.pnl_top_menu.Controls.Add(this.label1);
            this.pnl_top_menu.Controls.Add(this.lb_Time);
            this.pnl_top_menu.Controls.Add(this.lb_Date);
            this.pnl_top_menu.Controls.Add(this.rbtn_rst);
            this.pnl_top_menu.Controls.Add(this.rbtn_user);
            this.pnl_top_menu.Controls.Add(this.btn_quit);
            this.pnl_top_menu.Controls.Add(this.rbtn_sys);
            this.pnl_top_menu.Controls.Add(this.rbtn_product);
            this.pnl_top_menu.Controls.Add(this.rbtn_run);
            this.pnl_top_menu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_top_menu.Location = new System.Drawing.Point(3, 3);
            this.pnl_top_menu.Name = "pnl_top_menu";
            this.pnl_top_menu.Size = new System.Drawing.Size(1274, 74);
            this.pnl_top_menu.TabIndex = 0;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(850, 20);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 37);
            this.button2.TabIndex = 70;
            this.button2.Text = "报警清除";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(753, 20);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 37);
            this.button1.TabIndex = 69;
            this.button1.Text = "报警";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(976, 9);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(64, 55);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 68;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Gray;
            this.label1.Location = new System.Drawing.Point(1046, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 20);
            this.label1.TabIndex = 67;
            this.label1.Text = "AA上料  V1.0";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lb_Time
            // 
            this.lb_Time.BackColor = System.Drawing.Color.Transparent;
            this.lb_Time.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lb_Time.ForeColor = System.Drawing.Color.Gray;
            this.lb_Time.Location = new System.Drawing.Point(1165, 34);
            this.lb_Time.Name = "lb_Time";
            this.lb_Time.Size = new System.Drawing.Size(91, 26);
            this.lb_Time.TabIndex = 65;
            this.lb_Time.Text = "21:09";
            this.lb_Time.Click += new System.EventHandler(this.lb_Date_Click);
            // 
            // lb_Date
            // 
            this.lb_Date.BackColor = System.Drawing.Color.Transparent;
            this.lb_Date.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lb_Date.ForeColor = System.Drawing.Color.Gray;
            this.lb_Date.Location = new System.Drawing.Point(1165, 6);
            this.lb_Date.Name = "lb_Date";
            this.lb_Date.Size = new System.Drawing.Size(106, 61);
            this.lb_Date.TabIndex = 66;
            this.lb_Date.Text = "2017-10-31\r\n";
            this.lb_Date.Click += new System.EventHandler(this.lb_Date_Click);
            // 
            // rbtn_rst
            // 
            this.rbtn_rst.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbtn_rst.BackColor = System.Drawing.Color.Transparent;
            this.rbtn_rst.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.rbtn_rst.FlatAppearance.BorderSize = 0;
            this.rbtn_rst.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(144)))), ((int)(((byte)(217)))));
            this.rbtn_rst.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSkyBlue;
            this.rbtn_rst.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbtn_rst.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbtn_rst.ForeColor = System.Drawing.Color.Gray;
            this.rbtn_rst.ImageIndex = 0;
            this.rbtn_rst.ImageList = this.img_list_main;
            this.rbtn_rst.Location = new System.Drawing.Point(6, 0);
            this.rbtn_rst.Name = "rbtn_rst";
            this.rbtn_rst.Size = new System.Drawing.Size(125, 71);
            this.rbtn_rst.TabIndex = 48;
            this.rbtn_rst.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbtn_rst.UseVisualStyleBackColor = false;
            this.rbtn_rst.Click += new System.EventHandler(this.rbtn_run_Click);
            // 
            // img_list_main
            // 
            this.img_list_main.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("img_list_main.ImageStream")));
            this.img_list_main.TransparentColor = System.Drawing.Color.Transparent;
            this.img_list_main.Images.SetKeyName(0, "home.ico");
            this.img_list_main.Images.SetKeyName(1, "product.ico");
            this.img_list_main.Images.SetKeyName(2, "quit..ico");
            this.img_list_main.Images.SetKeyName(3, "quit2.ico");
            this.img_list_main.Images.SetKeyName(4, "set.ico");
            this.img_list_main.Images.SetKeyName(5, "user.ico");
            // 
            // rbtn_user
            // 
            this.rbtn_user.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbtn_user.BackColor = System.Drawing.Color.Transparent;
            this.rbtn_user.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.rbtn_user.FlatAppearance.BorderSize = 0;
            this.rbtn_user.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(144)))), ((int)(((byte)(217)))));
            this.rbtn_user.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSkyBlue;
            this.rbtn_user.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbtn_user.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbtn_user.ForeColor = System.Drawing.Color.Gray;
            this.rbtn_user.ImageIndex = 5;
            this.rbtn_user.ImageList = this.img_list_main;
            this.rbtn_user.Location = new System.Drawing.Point(510, 0);
            this.rbtn_user.Name = "rbtn_user";
            this.rbtn_user.Size = new System.Drawing.Size(125, 71);
            this.rbtn_user.TabIndex = 47;
            this.rbtn_user.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbtn_user.UseVisualStyleBackColor = false;
            this.rbtn_user.Click += new System.EventHandler(this.rbtn_run_Click);
            // 
            // btn_quit
            // 
            this.btn_quit.BackColor = System.Drawing.Color.Transparent;
            this.btn_quit.FlatAppearance.BorderSize = 0;
            this.btn_quit.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(144)))), ((int)(((byte)(217)))));
            this.btn_quit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSkyBlue;
            this.btn_quit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_quit.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_quit.ForeColor = System.Drawing.Color.Gray;
            this.btn_quit.ImageIndex = 2;
            this.btn_quit.ImageList = this.img_list_main;
            this.btn_quit.Location = new System.Drawing.Point(636, 0);
            this.btn_quit.Name = "btn_quit";
            this.btn_quit.Size = new System.Drawing.Size(125, 71);
            this.btn_quit.TabIndex = 46;
            this.btn_quit.UseVisualStyleBackColor = false;
            this.btn_quit.Click += new System.EventHandler(this.btn_quit_Click);
            // 
            // rbtn_sys
            // 
            this.rbtn_sys.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbtn_sys.BackColor = System.Drawing.Color.Transparent;
            this.rbtn_sys.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.rbtn_sys.FlatAppearance.BorderSize = 0;
            this.rbtn_sys.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(144)))), ((int)(((byte)(217)))));
            this.rbtn_sys.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSkyBlue;
            this.rbtn_sys.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbtn_sys.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbtn_sys.ForeColor = System.Drawing.Color.Gray;
            this.rbtn_sys.ImageIndex = 4;
            this.rbtn_sys.ImageList = this.img_list_main;
            this.rbtn_sys.Location = new System.Drawing.Point(384, 0);
            this.rbtn_sys.Name = "rbtn_sys";
            this.rbtn_sys.Size = new System.Drawing.Size(125, 71);
            this.rbtn_sys.TabIndex = 44;
            this.rbtn_sys.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbtn_sys.UseVisualStyleBackColor = false;
            this.rbtn_sys.CheckedChanged += new System.EventHandler(this.rbtn_sys_CheckedChanged_1);
            this.rbtn_sys.Click += new System.EventHandler(this.rbtn_run_Click);
            // 
            // rbtn_product
            // 
            this.rbtn_product.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbtn_product.BackColor = System.Drawing.Color.Transparent;
            this.rbtn_product.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.rbtn_product.FlatAppearance.BorderSize = 0;
            this.rbtn_product.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(144)))), ((int)(((byte)(217)))));
            this.rbtn_product.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSkyBlue;
            this.rbtn_product.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbtn_product.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbtn_product.ForeColor = System.Drawing.Color.Gray;
            this.rbtn_product.ImageIndex = 1;
            this.rbtn_product.ImageList = this.img_list_main;
            this.rbtn_product.Location = new System.Drawing.Point(258, 0);
            this.rbtn_product.Name = "rbtn_product";
            this.rbtn_product.Size = new System.Drawing.Size(125, 71);
            this.rbtn_product.TabIndex = 43;
            this.rbtn_product.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbtn_product.UseVisualStyleBackColor = false;
            this.rbtn_product.Click += new System.EventHandler(this.rbtn_run_Click);
            // 
            // rbtn_run
            // 
            this.rbtn_run.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbtn_run.BackColor = System.Drawing.Color.DarkGray;
            this.rbtn_run.Checked = true;
            this.rbtn_run.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.rbtn_run.FlatAppearance.BorderSize = 0;
            this.rbtn_run.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(144)))), ((int)(((byte)(217)))));
            this.rbtn_run.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSkyBlue;
            this.rbtn_run.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbtn_run.Font = new System.Drawing.Font("Microsoft Sans Serif", 22F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbtn_run.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.rbtn_run.ImageIndex = 3;
            this.rbtn_run.ImageList = this.img_list_main;
            this.rbtn_run.Location = new System.Drawing.Point(132, 0);
            this.rbtn_run.Name = "rbtn_run";
            this.rbtn_run.Size = new System.Drawing.Size(125, 71);
            this.rbtn_run.TabIndex = 42;
            this.rbtn_run.TabStop = true;
            this.rbtn_run.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbtn_run.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rbtn_run.UseVisualStyleBackColor = false;
            this.rbtn_run.Click += new System.EventHandler(this.rbtn_run_Click);
            // 
            // pnl_sub
            // 
            this.pnl_sub.BackColor = System.Drawing.SystemColors.Control;
            this.pnl_sub.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_sub.Location = new System.Drawing.Point(3, 83);
            this.pnl_sub.Name = "pnl_sub";
            this.pnl_sub.Size = new System.Drawing.Size(1274, 600);
            this.pnl_sub.TabIndex = 1;
            this.pnl_sub.Paint += new System.Windows.Forms.PaintEventHandler(this.pnl_sub_Paint);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 689);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1274, 14);
            this.panel1.TabIndex = 2;
            // 
            // timer_update
            // 
            this.timer_update.Enabled = true;
            this.timer_update.Interval = 1000;
            this.timer_update.Tick += new System.EventHandler(this.timer_update_Tick);
            // 
            // timer_key
            // 
            this.timer_key.Tick += new System.EventHandler(this.timer_key_Tick);
            // 
            // ts_manual
            // 
            this.ts_manual.BackColor = System.Drawing.SystemColors.Control;
            this.ts_manual.Dock = System.Windows.Forms.DockStyle.None;
            this.ts_manual.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ts_manual.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ts_manual.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsc_step_sel,
            this.toolStripSeparator17,
            this.tsb_xn,
            this.toolStripSeparator16,
            this.tsb_xp,
            this.toolStripSeparator15,
            this.tsb_yn,
            this.toolStripSeparator27,
            this.tsb_yp,
            this.toolStripSeparator14,
            this.tsb_zn,
            this.toolStripSeparator28,
            this.tsb_zp,
            this.toolStripSeparator13,
            this.tsb_rn,
            this.toolStripSeparator2,
            this.tsb_rp,
            this.toolStripSeparator1,
            this.tsb_x1_n,
            this.toolStripSeparator3,
            this.tsb_x1_p,
            this.toolStripSeparator12,
            this.tsb_stop});
            this.ts_manual.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.ts_manual.Location = new System.Drawing.Point(1150, 60);
            this.ts_manual.Name = "ts_manual";
            this.ts_manual.Size = new System.Drawing.Size(122, 915);
            this.ts_manual.TabIndex = 0;
            this.ts_manual.Visible = false;
            // 
            // tsc_step_sel
            // 
            this.tsc_step_sel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tsc_step_sel.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.tsc_step_sel.Font = new System.Drawing.Font("Microsoft Sans Serif", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tsc_step_sel.Items.AddRange(new object[] {
            "0.01mm",
            "0.1mm",
            "1mm",
            "10mm"});
            this.tsc_step_sel.Name = "tsc_step_sel";
            this.tsc_step_sel.Size = new System.Drawing.Size(118, 44);
            this.tsc_step_sel.SelectedIndexChanged += new System.EventHandler(this.tsc_step_sel_SelectedIndexChanged);
            // 
            // toolStripSeparator17
            // 
            this.toolStripSeparator17.Name = "toolStripSeparator17";
            this.toolStripSeparator17.Size = new System.Drawing.Size(120, 6);
            // 
            // tsb_xn
            // 
            this.tsb_xn.AutoSize = false;
            this.tsb_xn.BackColor = System.Drawing.SystemColors.Control;
            this.tsb_xn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tsb_xn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tsb_xn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tsb_xn.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsb_xn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_xn.Name = "tsb_xn";
            this.tsb_xn.Size = new System.Drawing.Size(121, 70);
            this.tsb_xn.Text = "X向左";
            this.tsb_xn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tsb_xn_MouseDown);
            this.tsb_xn.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tsb_xn_MouseUp);
            // 
            // toolStripSeparator16
            // 
            this.toolStripSeparator16.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripSeparator16.Name = "toolStripSeparator16";
            this.toolStripSeparator16.Size = new System.Drawing.Size(120, 6);
            // 
            // tsb_xp
            // 
            this.tsb_xp.AutoSize = false;
            this.tsb_xp.BackColor = System.Drawing.SystemColors.Control;
            this.tsb_xp.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tsb_xp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tsb_xp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tsb_xp.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsb_xp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_xp.Name = "tsb_xp";
            this.tsb_xp.Size = new System.Drawing.Size(121, 70);
            this.tsb_xp.Text = "X向右";
            this.tsb_xp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tsb_xn_MouseDown);
            this.tsb_xp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tsb_xn_MouseUp);
            // 
            // toolStripSeparator15
            // 
            this.toolStripSeparator15.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripSeparator15.Name = "toolStripSeparator15";
            this.toolStripSeparator15.Size = new System.Drawing.Size(120, 6);
            // 
            // tsb_yn
            // 
            this.tsb_yn.AutoSize = false;
            this.tsb_yn.BackColor = System.Drawing.SystemColors.Control;
            this.tsb_yn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tsb_yn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tsb_yn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tsb_yn.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsb_yn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_yn.Name = "tsb_yn";
            this.tsb_yn.Size = new System.Drawing.Size(121, 70);
            this.tsb_yn.Text = "Y向前";
            this.tsb_yn.ToolTipText = "Y向前";
            this.tsb_yn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tsb_xn_MouseDown);
            this.tsb_yn.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tsb_xn_MouseUp);
            // 
            // toolStripSeparator27
            // 
            this.toolStripSeparator27.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripSeparator27.Name = "toolStripSeparator27";
            this.toolStripSeparator27.Size = new System.Drawing.Size(120, 6);
            // 
            // tsb_yp
            // 
            this.tsb_yp.AutoSize = false;
            this.tsb_yp.BackColor = System.Drawing.SystemColors.Control;
            this.tsb_yp.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tsb_yp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tsb_yp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tsb_yp.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsb_yp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_yp.Name = "tsb_yp";
            this.tsb_yp.Size = new System.Drawing.Size(121, 70);
            this.tsb_yp.Text = "Y向后";
            this.tsb_yp.ToolTipText = "Y向后";
            this.tsb_yp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tsb_xn_MouseDown);
            this.tsb_yp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tsb_xn_MouseUp);
            // 
            // toolStripSeparator14
            // 
            this.toolStripSeparator14.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripSeparator14.Name = "toolStripSeparator14";
            this.toolStripSeparator14.Size = new System.Drawing.Size(120, 6);
            // 
            // tsb_zn
            // 
            this.tsb_zn.AutoSize = false;
            this.tsb_zn.BackColor = System.Drawing.SystemColors.Control;
            this.tsb_zn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tsb_zn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tsb_zn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tsb_zn.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsb_zn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_zn.Name = "tsb_zn";
            this.tsb_zn.Size = new System.Drawing.Size(121, 70);
            this.tsb_zn.Text = "Z向上";
            this.tsb_zn.ToolTipText = "Z向上";
            this.tsb_zn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tsb_xn_MouseDown);
            this.tsb_zn.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tsb_xn_MouseUp);
            // 
            // toolStripSeparator28
            // 
            this.toolStripSeparator28.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripSeparator28.Name = "toolStripSeparator28";
            this.toolStripSeparator28.Size = new System.Drawing.Size(120, 6);
            // 
            // tsb_zp
            // 
            this.tsb_zp.AutoSize = false;
            this.tsb_zp.BackColor = System.Drawing.SystemColors.Control;
            this.tsb_zp.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tsb_zp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tsb_zp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tsb_zp.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsb_zp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_zp.Name = "tsb_zp";
            this.tsb_zp.Size = new System.Drawing.Size(121, 70);
            this.tsb_zp.Text = "Z向下";
            this.tsb_zp.ToolTipText = "Z向下";
            this.tsb_zp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tsb_xn_MouseDown);
            this.tsb_zp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tsb_xn_MouseUp);
            // 
            // toolStripSeparator13
            // 
            this.toolStripSeparator13.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            this.toolStripSeparator13.Size = new System.Drawing.Size(120, 6);
            // 
            // tsb_rn
            // 
            this.tsb_rn.AutoSize = false;
            this.tsb_rn.BackColor = System.Drawing.SystemColors.Control;
            this.tsb_rn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tsb_rn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tsb_rn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tsb_rn.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsb_rn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_rn.Name = "tsb_rn";
            this.tsb_rn.Size = new System.Drawing.Size(121, 70);
            this.tsb_rn.Text = "R顺时针";
            this.tsb_rn.ToolTipText = "R1顺时针";
            this.tsb_rn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tsb_xn_MouseDown);
            this.tsb_rn.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tsb_xn_MouseUp);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(120, 6);
            // 
            // tsb_rp
            // 
            this.tsb_rp.AutoSize = false;
            this.tsb_rp.BackColor = System.Drawing.SystemColors.Control;
            this.tsb_rp.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tsb_rp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tsb_rp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tsb_rp.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsb_rp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_rp.Name = "tsb_rp";
            this.tsb_rp.Size = new System.Drawing.Size(121, 70);
            this.tsb_rp.Text = "R逆时针";
            this.tsb_rp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tsb_xn_MouseDown);
            this.tsb_rp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tsb_xn_MouseUp);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(120, 6);
            // 
            // tsb_x1_n
            // 
            this.tsb_x1_n.AutoSize = false;
            this.tsb_x1_n.BackColor = System.Drawing.SystemColors.Control;
            this.tsb_x1_n.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tsb_x1_n.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tsb_x1_n.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tsb_x1_n.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsb_x1_n.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_x1_n.Name = "tsb_x1_n";
            this.tsb_x1_n.Size = new System.Drawing.Size(121, 70);
            this.tsb_x1_n.Text = "X2 向左";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(120, 6);
            // 
            // tsb_x1_p
            // 
            this.tsb_x1_p.AutoSize = false;
            this.tsb_x1_p.BackColor = System.Drawing.SystemColors.Control;
            this.tsb_x1_p.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tsb_x1_p.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tsb_x1_p.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tsb_x1_p.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsb_x1_p.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_x1_p.Name = "tsb_x1_p";
            this.tsb_x1_p.Size = new System.Drawing.Size(121, 70);
            this.tsb_x1_p.Text = "X2 向右";
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new System.Drawing.Size(120, 6);
            // 
            // tsb_stop
            // 
            this.tsb_stop.AutoSize = false;
            this.tsb_stop.BackColor = System.Drawing.Color.Gold;
            this.tsb_stop.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tsb_stop.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tsb_stop.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tsb_stop.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsb_stop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_stop.Name = "tsb_stop";
            this.tsb_stop.Size = new System.Drawing.Size(121, 70);
            this.tsb_stop.Text = "停止";
            this.tsb_stop.ToolTipText = "停止";
            this.tsb_stop.Click += new System.EventHandler(this.tsb_stop_Click);
            // 
            // axTcpClient_cmd
            // 
            this.axTcpClient_cmd.Isclosed = false;
            this.axTcpClient_cmd.IsStartTcpthreading = false;
            this.axTcpClient_cmd.Receivestr = null;
            this.axTcpClient_cmd.ReConectedCount = 0;
            this.axTcpClient_cmd.ReConnectionTime = 3000;
            this.axTcpClient_cmd.ServerIp = null;
            this.axTcpClient_cmd.ServerPort = 0;
            this.axTcpClient_cmd.Tcpclient = null;
            this.axTcpClient_cmd.Tcpthread = null;
            this.axTcpClient_cmd.OnReceviceByte += new SocketHelper.AxTcpClient.ReceviceByteEventHandler(this.axTcpClient_cmd_OnReceviceByte);
            // 
            // axTcpClient_status
            // 
            this.axTcpClient_status.Isclosed = false;
            this.axTcpClient_status.IsStartTcpthreading = false;
            this.axTcpClient_status.Receivestr = null;
            this.axTcpClient_status.ReConectedCount = 0;
            this.axTcpClient_status.ReConnectionTime = 3000;
            this.axTcpClient_status.ServerIp = null;
            this.axTcpClient_status.ServerPort = 0;
            this.axTcpClient_status.Tcpclient = null;
            this.axTcpClient_status.Tcpthread = null;
            // 
            // timer_reconnect
            // 
            this.timer_reconnect.Interval = 5000;
            this.timer_reconnect.Tick += new System.EventHandler(this.timer_reconnect_Tick);
            // 
            // axTcpServer1
            // 
            this.axTcpServer1.ServerIp = "127.0.0.1";
            this.axTcpServer1.ServerPort = 5000;
            this.axTcpServer1.OnStateInfo += new SocketHelper.AxTcpServer.StateInfoEventHandler(this.axTcpServer1_OnStateInfo);
            // 
            // FrMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 700);
            this.Controls.Add(this.ts_manual);
            this.Controls.Add(this.tbl_main);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrMain";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrMain";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrMain_FormClosing);
            this.Load += new System.EventHandler(this.FrMain_Load);
            this.tbl_main.ResumeLayout(false);
            this.pnl_top_menu.ResumeLayout(false);
            this.pnl_top_menu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ts_manual.ResumeLayout(false);
            this.ts_manual.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tbl_main;
        private System.Windows.Forms.Panel pnl_sub;
        private System.Windows.Forms.ImageList img_list_main;
        private System.Windows.Forms.Panel pnl_top_menu;
        private System.Windows.Forms.RadioButton rbtn_rst;
        private System.Windows.Forms.RadioButton rbtn_user;
        private System.Windows.Forms.Button btn_quit;
        private System.Windows.Forms.RadioButton rbtn_sys;
        public System.Windows.Forms.RadioButton rbtn_product;
        public System.Windows.Forms.RadioButton rbtn_run;
        private System.Windows.Forms.Label lb_Time;
        private System.Windows.Forms.Label lb_Date;
        private System.Windows.Forms.Timer timer_update;
        private System.Windows.Forms.Timer timer_key;
        private System.Windows.Forms.ToolStrip ts_manual;
        public System.Windows.Forms.ToolStripComboBox tsc_step_sel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator17;
        private System.Windows.Forms.ToolStripButton tsb_xn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator16;
        private System.Windows.Forms.ToolStripButton tsb_xp;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator15;
        private System.Windows.Forms.ToolStripButton tsb_yn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator27;
        private System.Windows.Forms.ToolStripButton tsb_yp;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator14;
        private System.Windows.Forms.ToolStripButton tsb_zn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator28;
        private System.Windows.Forms.ToolStripButton tsb_zp;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
        private System.Windows.Forms.ToolStripButton tsb_rn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsb_rp;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsb_stop;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStripButton tsb_x1_n;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton tsb_x1_p;
        private SocketHelper.AxTcpClient axTcpClient_status;
        private System.Windows.Forms.Timer timer_reconnect;
        private SocketHelper.AxTcpServer axTcpServer1;
        public SocketHelper.AxTcpClient axTcpClient_cmd;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Panel panel1;
    }
}

