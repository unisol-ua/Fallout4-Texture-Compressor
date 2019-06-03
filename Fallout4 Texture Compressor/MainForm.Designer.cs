namespace Fallout4_Texture_Compressor
{
    partial class MainForm
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
            this.startbutton = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.browsebutton = new System.Windows.Forms.Button();
            this.pathtextbox = new System.Windows.Forms.TextBox();
            this.compress_check = new System.Windows.Forms.CheckBox();
            this.texturesize_combo = new System.Windows.Forms.ComboBox();
            this.resize_check = new System.Windows.Forms.CheckBox();
            this.backup_check = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.backupname = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.exportlogbutton = new System.Windows.Forms.Button();
            this.aboutbutton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.ignore_sng_maps_check = new System.Windows.Forms.CheckBox();
            this.optionsbutton = new System.Windows.Forms.Button();
            this.compress_lvl_alpha_string_combo = new System.Windows.Forms.ComboBox();
            this.compress_lvl_string_combo = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.BA2btn = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.threading_check = new System.Windows.Forms.CheckBox();
            this.threads_combo = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.ignore_diffuse_check = new System.Windows.Forms.CheckBox();
            this.ignore_face_check = new System.Windows.Forms.CheckBox();
            this.force_compression_check = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // startbutton
            // 
            this.startbutton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.startbutton.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.startbutton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightCyan;
            this.startbutton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Azure;
            this.startbutton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.startbutton.Location = new System.Drawing.Point(306, 446);
            this.startbutton.Name = "startbutton";
            this.startbutton.Size = new System.Drawing.Size(75, 23);
            this.startbutton.TabIndex = 0;
            this.startbutton.Text = "Start";
            this.startbutton.UseVisualStyleBackColor = true;
            this.startbutton.Click += new System.EventHandler(this.startbutton_Click);
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.BackColor = System.Drawing.Color.White;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(12, 41);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(656, 394);
            this.listBox1.TabIndex = 1;
            this.listBox1.Visible = false;
            this.listBox1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBox1_MouseDoubleClick);
            // 
            // browsebutton
            // 
            this.browsebutton.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.browsebutton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightCyan;
            this.browsebutton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Azure;
            this.browsebutton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.browsebutton.Location = new System.Drawing.Point(13, 13);
            this.browsebutton.Name = "browsebutton";
            this.browsebutton.Size = new System.Drawing.Size(75, 23);
            this.browsebutton.TabIndex = 5;
            this.browsebutton.Text = "Browse";
            this.browsebutton.UseVisualStyleBackColor = true;
            this.browsebutton.Click += new System.EventHandler(this.browsebutton_Click);
            // 
            // pathtextbox
            // 
            this.pathtextbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pathtextbox.BackColor = System.Drawing.Color.White;
            this.pathtextbox.Location = new System.Drawing.Point(94, 15);
            this.pathtextbox.Name = "pathtextbox";
            this.pathtextbox.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.pathtextbox.Size = new System.Drawing.Size(574, 20);
            this.pathtextbox.TabIndex = 6;
            // 
            // compress_check
            // 
            this.compress_check.AutoSize = true;
            this.compress_check.BackColor = System.Drawing.Color.White;
            this.compress_check.Checked = true;
            this.compress_check.CheckState = System.Windows.Forms.CheckState.Checked;
            this.compress_check.Location = new System.Drawing.Point(25, 71);
            this.compress_check.Name = "compress_check";
            this.compress_check.Size = new System.Drawing.Size(116, 17);
            this.compress_check.TabIndex = 9;
            this.compress_check.Text = "Compress Textures";
            this.compress_check.UseVisualStyleBackColor = false;
            // 
            // texturesize_combo
            // 
            this.texturesize_combo.BackColor = System.Drawing.Color.White;
            this.texturesize_combo.FormattingEnabled = true;
            this.texturesize_combo.Items.AddRange(new object[] {
            "All",
            "if > 256",
            "if > 512",
            "if > 1024",
            "if > 2048",
            "if > 4096"});
            this.texturesize_combo.Location = new System.Drawing.Point(25, 269);
            this.texturesize_combo.Name = "texturesize_combo";
            this.texturesize_combo.Size = new System.Drawing.Size(121, 21);
            this.texturesize_combo.TabIndex = 10;
            this.texturesize_combo.Text = "if > 1024";
            this.texturesize_combo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.texturesize_combo_KeyDown);
            this.texturesize_combo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.texturesize_combo_KeyPress);
            // 
            // resize_check
            // 
            this.resize_check.AutoSize = true;
            this.resize_check.BackColor = System.Drawing.Color.White;
            this.resize_check.Location = new System.Drawing.Point(25, 246);
            this.resize_check.Name = "resize_check";
            this.resize_check.Size = new System.Drawing.Size(127, 17);
            this.resize_check.TabIndex = 12;
            this.resize_check.Text = "Resize textures down";
            this.resize_check.UseVisualStyleBackColor = false;
            this.resize_check.MouseHover += new System.EventHandler(this.resize_check_MouseHover);
            // 
            // backup_check
            // 
            this.backup_check.AutoSize = true;
            this.backup_check.BackColor = System.Drawing.Color.White;
            this.backup_check.Checked = true;
            this.backup_check.CheckState = System.Windows.Forms.CheckState.Checked;
            this.backup_check.Location = new System.Drawing.Point(25, 301);
            this.backup_check.Name = "backup_check";
            this.backup_check.Size = new System.Drawing.Size(93, 17);
            this.backup_check.TabIndex = 14;
            this.backup_check.Text = "Make Backup";
            this.backup_check.UseVisualStyleBackColor = false;
            this.backup_check.MouseHover += new System.EventHandler(this.backup_check_MouseHover);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 355);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 13);
            this.label3.TabIndex = 16;
            // 
            // backupname
            // 
            this.backupname.BackColor = System.Drawing.Color.White;
            this.backupname.Location = new System.Drawing.Point(25, 324);
            this.backupname.Name = "backupname";
            this.backupname.Size = new System.Drawing.Size(121, 20);
            this.backupname.TabIndex = 17;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(162, 327);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(265, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "Custom name for backup, leave blank for default name";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // exportlogbutton
            // 
            this.exportlogbutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.exportlogbutton.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.exportlogbutton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightCyan;
            this.exportlogbutton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Azure;
            this.exportlogbutton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exportlogbutton.Location = new System.Drawing.Point(593, 446);
            this.exportlogbutton.Name = "exportlogbutton";
            this.exportlogbutton.Size = new System.Drawing.Size(75, 23);
            this.exportlogbutton.TabIndex = 19;
            this.exportlogbutton.Text = "Export log";
            this.exportlogbutton.UseVisualStyleBackColor = true;
            this.exportlogbutton.Visible = false;
            this.exportlogbutton.Click += new System.EventHandler(this.exportlogbutton_Click);
            // 
            // aboutbutton
            // 
            this.aboutbutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.aboutbutton.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.aboutbutton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightCyan;
            this.aboutbutton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Azure;
            this.aboutbutton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.aboutbutton.Location = new System.Drawing.Point(12, 446);
            this.aboutbutton.Name = "aboutbutton";
            this.aboutbutton.Size = new System.Drawing.Size(75, 23);
            this.aboutbutton.TabIndex = 20;
            this.aboutbutton.Text = "About";
            this.aboutbutton.UseVisualStyleBackColor = true;
            this.aboutbutton.Click += new System.EventHandler(this.aboutbutton_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(162, 97);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(149, 13);
            this.label5.TabIndex = 21;
            this.label5.Text = "Textures without transparency";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(22, 347);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(234, 13);
            this.label8.TabIndex = 26;
            this.label8.Text = "Don\'t use special characters like / \\ | : * ? < > \" \'";
            this.label8.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // ignore_sng_maps_check
            // 
            this.ignore_sng_maps_check.AutoSize = true;
            this.ignore_sng_maps_check.BackColor = System.Drawing.Color.White;
            this.ignore_sng_maps_check.Location = new System.Drawing.Point(25, 200);
            this.ignore_sng_maps_check.Name = "ignore_sng_maps_check";
            this.ignore_sng_maps_check.Size = new System.Drawing.Size(207, 17);
            this.ignore_sng_maps_check.TabIndex = 27;
            this.ignore_sng_maps_check.Text = "Ignore specular, normal and glowmaps";
            this.ignore_sng_maps_check.UseVisualStyleBackColor = false;
            this.ignore_sng_maps_check.MouseHover += new System.EventHandler(this.ignore_sng_maps_check_MouseHover);
            // 
            // optionsbutton
            // 
            this.optionsbutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.optionsbutton.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.optionsbutton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightCyan;
            this.optionsbutton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Azure;
            this.optionsbutton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.optionsbutton.Location = new System.Drawing.Point(512, 446);
            this.optionsbutton.Name = "optionsbutton";
            this.optionsbutton.Size = new System.Drawing.Size(75, 23);
            this.optionsbutton.TabIndex = 29;
            this.optionsbutton.Text = "Options";
            this.optionsbutton.UseVisualStyleBackColor = true;
            this.optionsbutton.Visible = false;
            this.optionsbutton.Click += new System.EventHandler(this.optionsbutton_Click);
            // 
            // compress_lvl_alpha_string_combo
            // 
            this.compress_lvl_alpha_string_combo.BackColor = System.Drawing.Color.White;
            this.compress_lvl_alpha_string_combo.FormattingEnabled = true;
            this.compress_lvl_alpha_string_combo.Items.AddRange(new object[] {
            "BC7",
            "BC5",
            "BC3",
            "BC1"});
            this.compress_lvl_alpha_string_combo.Location = new System.Drawing.Point(25, 117);
            this.compress_lvl_alpha_string_combo.Name = "compress_lvl_alpha_string_combo";
            this.compress_lvl_alpha_string_combo.Size = new System.Drawing.Size(121, 21);
            this.compress_lvl_alpha_string_combo.TabIndex = 44;
            this.compress_lvl_alpha_string_combo.Text = "BC3";
            this.compress_lvl_alpha_string_combo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.compress_lvl_alpha_string_combo_KeyDown);
            this.compress_lvl_alpha_string_combo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.compress_lvl_alpha_string_combo_KeyPress);
            // 
            // compress_lvl_string_combo
            // 
            this.compress_lvl_string_combo.BackColor = System.Drawing.Color.White;
            this.compress_lvl_string_combo.FormattingEnabled = true;
            this.compress_lvl_string_combo.Items.AddRange(new object[] {
            "BC7",
            "BC5",
            "BC3",
            "BC1"});
            this.compress_lvl_string_combo.Location = new System.Drawing.Point(25, 94);
            this.compress_lvl_string_combo.Name = "compress_lvl_string_combo";
            this.compress_lvl_string_combo.Size = new System.Drawing.Size(121, 21);
            this.compress_lvl_string_combo.TabIndex = 45;
            this.compress_lvl_string_combo.Text = "BC1";
            this.compress_lvl_string_combo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.compress_lvl_string_combo_KeyDown);
            this.compress_lvl_string_combo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.compress_lvl_string_combo_KeyPress);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(162, 120);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(134, 13);
            this.label6.TabIndex = 46;
            this.label6.Text = "Textures with transparency";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Location = new System.Drawing.Point(473, 75);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(185, 52);
            this.label11.TabIndex = 47;
            this.label11.Text = "BC1 - perfomance, no alpha channel\r\nBC3 - perfomance, has alpha channel\r\nBC5 - me" +
    "dium, has alpha channel\r\nBC7 - quality, has alpha channel";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.BackColor = System.Drawing.Color.Transparent;
            this.linkLabel1.Location = new System.Drawing.Point(539, 130);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(28, 13);
            this.linkLabel1.TabIndex = 49;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "here";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.BackColor = System.Drawing.Color.Transparent;
            this.linkLabel2.Location = new System.Drawing.Point(590, 130);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(28, 13);
            this.linkLabel2.TabIndex = 50;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "here";
            this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // BA2btn
            // 
            this.BA2btn.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.BA2btn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightCyan;
            this.BA2btn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Azure;
            this.BA2btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BA2btn.Location = new System.Drawing.Point(13, 42);
            this.BA2btn.Name = "BA2btn";
            this.BA2btn.Size = new System.Drawing.Size(75, 23);
            this.BA2btn.TabIndex = 51;
            this.BA2btn.Text = "Loose";
            this.BA2btn.UseVisualStyleBackColor = true;
            this.BA2btn.Click += new System.EventHandler(this.BA2btn_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(94, 47);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(230, 13);
            this.label10.TabIndex = 52;
            this.label10.Text = "Choose to compress loose files or BA2 archives";
            this.label10.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // threading_check
            // 
            this.threading_check.AutoSize = true;
            this.threading_check.BackColor = System.Drawing.Color.White;
            this.threading_check.Location = new System.Drawing.Point(25, 372);
            this.threading_check.Name = "threading_check";
            this.threading_check.Size = new System.Drawing.Size(92, 17);
            this.threading_check.TabIndex = 53;
            this.threading_check.Text = "Multithreading";
            this.threading_check.UseVisualStyleBackColor = false;
            this.threading_check.MouseHover += new System.EventHandler(this.threading_check_MouseHover);
            // 
            // threads_combo
            // 
            this.threads_combo.BackColor = System.Drawing.Color.White;
            this.threads_combo.FormattingEnabled = true;
            this.threads_combo.Items.AddRange(new object[] {
            "2",
            "4",
            "6",
            "8",
            "10",
            "12",
            "14",
            "16"});
            this.threads_combo.Location = new System.Drawing.Point(25, 395);
            this.threads_combo.Name = "threads_combo";
            this.threads_combo.Size = new System.Drawing.Size(121, 21);
            this.threads_combo.TabIndex = 54;
            this.threads_combo.Text = "4";
            this.threads_combo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.threads_combo_KeyDown);
            this.threads_combo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.threads_combo_KeyPress);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Location = new System.Drawing.Point(480, 130);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(60, 13);
            this.label7.TabIndex = 55;
            this.label7.Text = "Learn more";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Location = new System.Drawing.Point(565, 130);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(25, 13);
            this.label12.TabIndex = 56;
            this.label12.Text = "and";
            this.label12.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // ignore_diffuse_check
            // 
            this.ignore_diffuse_check.AutoSize = true;
            this.ignore_diffuse_check.BackColor = System.Drawing.Color.White;
            this.ignore_diffuse_check.Location = new System.Drawing.Point(25, 223);
            this.ignore_diffuse_check.Name = "ignore_diffuse_check";
            this.ignore_diffuse_check.Size = new System.Drawing.Size(130, 17);
            this.ignore_diffuse_check.TabIndex = 57;
            this.ignore_diffuse_check.Text = "Ignore diffuse textures";
            this.ignore_diffuse_check.UseVisualStyleBackColor = false;
            this.ignore_diffuse_check.MouseHover += new System.EventHandler(this.ignore_diffuse_check_MouseHover);
            // 
            // ignore_face_check
            // 
            this.ignore_face_check.AutoSize = true;
            this.ignore_face_check.BackColor = System.Drawing.Color.White;
            this.ignore_face_check.Checked = true;
            this.ignore_face_check.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ignore_face_check.Enabled = false;
            this.ignore_face_check.Location = new System.Drawing.Point(25, 177);
            this.ignore_face_check.Name = "ignore_face_check";
            this.ignore_face_check.Size = new System.Drawing.Size(120, 17);
            this.ignore_face_check.TabIndex = 58;
            this.ignore_face_check.Text = "Ignore face textures";
            this.ignore_face_check.UseVisualStyleBackColor = false;
            this.ignore_face_check.MouseHover += new System.EventHandler(this.ignore_face_check_MouseHover);
            // 
            // force_compression_check
            // 
            this.force_compression_check.AutoSize = true;
            this.force_compression_check.BackColor = System.Drawing.Color.White;
            this.force_compression_check.Location = new System.Drawing.Point(25, 154);
            this.force_compression_check.Name = "force_compression_check";
            this.force_compression_check.Size = new System.Drawing.Size(115, 17);
            this.force_compression_check.TabIndex = 59;
            this.force_compression_check.Text = "Force compression";
            this.force_compression_check.UseVisualStyleBackColor = false;
            this.force_compression_check.MouseHover += new System.EventHandler(this.force_compression_check_MouseHover);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(480, 416);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(185, 13);
            this.label1.TabIndex = 60;
            this.label1.Text = "Options have tooltips on mouse hover";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(680, 477);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.force_compression_check);
            this.Controls.Add(this.ignore_face_check);
            this.Controls.Add(this.ignore_diffuse_check);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.threads_combo);
            this.Controls.Add(this.threading_check);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.BA2btn);
            this.Controls.Add(this.linkLabel2);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.compress_lvl_string_combo);
            this.Controls.Add(this.compress_lvl_alpha_string_combo);
            this.Controls.Add(this.optionsbutton);
            this.Controls.Add(this.ignore_sng_maps_check);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.aboutbutton);
            this.Controls.Add(this.exportlogbutton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.backupname);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.backup_check);
            this.Controls.Add(this.resize_check);
            this.Controls.Add(this.texturesize_combo);
            this.Controls.Add(this.compress_check);
            this.Controls.Add(this.pathtextbox);
            this.Controls.Add(this.browsebutton);
            this.Controls.Add(this.startbutton);
            this.Controls.Add(this.listBox1);
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.Text = "Texture Compressor";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button startbutton;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button browsebutton;
        private System.Windows.Forms.TextBox pathtextbox;
        private System.Windows.Forms.CheckBox compress_check;
        private System.Windows.Forms.ComboBox texturesize_combo;
        private System.Windows.Forms.CheckBox resize_check;
        private System.Windows.Forms.CheckBox backup_check;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox backupname;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button exportlogbutton;
        private System.Windows.Forms.Button aboutbutton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox ignore_sng_maps_check;
        private System.Windows.Forms.Button optionsbutton;
        private System.Windows.Forms.ComboBox compress_lvl_alpha_string_combo;
        private System.Windows.Forms.ComboBox compress_lvl_string_combo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.Button BA2btn;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox threading_check;
        private System.Windows.Forms.ComboBox threads_combo;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.CheckBox ignore_diffuse_check;
        private System.Windows.Forms.CheckBox ignore_face_check;
        private System.Windows.Forms.CheckBox force_compression_check;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label1;
    }
}

