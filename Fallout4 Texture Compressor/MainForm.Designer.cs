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
            this.startbutton = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.browsebutton = new System.Windows.Forms.Button();
            this.pathtextbox = new System.Windows.Forms.TextBox();
            this.compressbc_check = new System.Windows.Forms.CheckBox();
            this.texturesize_combo = new System.Windows.Forms.ComboBox();
            this.resize_check = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.backup_check = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.backupname = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.exportlogbutton = new System.Windows.Forms.Button();
            this.aboutbutton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.compressother_check = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.safecompress_check = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.ignoresn_check = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.optionsbutton = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.compressother_combo = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // startbutton
            // 
            this.startbutton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.startbutton.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.startbutton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightCyan;
            this.startbutton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Azure;
            this.startbutton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.startbutton.Location = new System.Drawing.Point(306, 413);
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
            this.listBox1.Size = new System.Drawing.Size(656, 355);
            this.listBox1.TabIndex = 1;
            this.listBox1.Visible = false;
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
            // compressbc_check
            // 
            this.compressbc_check.AutoSize = true;
            this.compressbc_check.BackColor = System.Drawing.Color.White;
            this.compressbc_check.Checked = true;
            this.compressbc_check.CheckState = System.Windows.Forms.CheckState.Checked;
            this.compressbc_check.Location = new System.Drawing.Point(36, 62);
            this.compressbc_check.Name = "compressbc_check";
            this.compressbc_check.Size = new System.Drawing.Size(93, 17);
            this.compressbc_check.TabIndex = 9;
            this.compressbc_check.Text = "Compress BC*";
            this.compressbc_check.UseVisualStyleBackColor = false;
            // 
            // texturesize_combo
            // 
            this.texturesize_combo.BackColor = System.Drawing.Color.White;
            this.texturesize_combo.FormattingEnabled = true;
            this.texturesize_combo.Items.AddRange(new object[] {
            "if > 4",
            "if > 8",
            "if > 16",
            "if > 32",
            "if > 64",
            "if > 128",
            "if > 256",
            "if > 512",
            "if > 1024",
            "if > 2048",
            "if > 4096",
            "if > 8192"});
            this.texturesize_combo.Location = new System.Drawing.Point(36, 204);
            this.texturesize_combo.Name = "texturesize_combo";
            this.texturesize_combo.Size = new System.Drawing.Size(121, 21);
            this.texturesize_combo.TabIndex = 10;
            this.texturesize_combo.Text = "if > 2048";
            this.texturesize_combo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.texturesize_combo_KeyDown);
            this.texturesize_combo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.texturesize_combo_KeyPress);
            // 
            // resize_check
            // 
            this.resize_check.AutoSize = true;
            this.resize_check.BackColor = System.Drawing.Color.White;
            this.resize_check.Location = new System.Drawing.Point(36, 181);
            this.resize_check.Name = "resize_check";
            this.resize_check.Size = new System.Drawing.Size(87, 17);
            this.resize_check.TabIndex = 12;
            this.resize_check.Text = "Resize down";
            this.resize_check.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(172, 207);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(228, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Reduces texture dividing by 2 width and height";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // backup_check
            // 
            this.backup_check.AutoSize = true;
            this.backup_check.BackColor = System.Drawing.Color.White;
            this.backup_check.Checked = true;
            this.backup_check.CheckState = System.Windows.Forms.CheckState.Checked;
            this.backup_check.Location = new System.Drawing.Point(36, 236);
            this.backup_check.Name = "backup_check";
            this.backup_check.Size = new System.Drawing.Size(93, 17);
            this.backup_check.TabIndex = 14;
            this.backup_check.Text = "Make Backup";
            this.backup_check.UseVisualStyleBackColor = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(172, 182);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(182, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Compares to the biggest of those two";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
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
            this.backupname.Location = new System.Drawing.Point(36, 259);
            this.backupname.Name = "backupname";
            this.backupname.Size = new System.Drawing.Size(121, 20);
            this.backupname.TabIndex = 17;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(172, 237);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(233, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "Custom name for backup leave blank for default";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // exportlogbutton
            // 
            this.exportlogbutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.exportlogbutton.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.exportlogbutton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightCyan;
            this.exportlogbutton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Azure;
            this.exportlogbutton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exportlogbutton.Location = new System.Drawing.Point(593, 413);
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
            this.aboutbutton.Location = new System.Drawing.Point(12, 413);
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
            this.label5.Location = new System.Drawing.Point(172, 63);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(219, 13);
            this.label5.TabIndex = 21;
            this.label5.Text = "Compress BC2,3,4,5,7 textures to BC1 format";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // compressother_check
            // 
            this.compressother_check.AutoSize = true;
            this.compressother_check.BackColor = System.Drawing.Color.White;
            this.compressother_check.Checked = true;
            this.compressother_check.CheckState = System.Windows.Forms.CheckState.Checked;
            this.compressother_check.Location = new System.Drawing.Point(36, 85);
            this.compressother_check.Name = "compressother_check";
            this.compressother_check.Size = new System.Drawing.Size(101, 17);
            this.compressother_check.TabIndex = 22;
            this.compressother_check.Text = "Compress Other";
            this.compressother_check.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.compressother_check.UseVisualStyleBackColor = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(172, 86);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(213, 13);
            this.label6.TabIndex = 23;
            this.label6.Text = "Compress other formats to BC7, BC5 or BC3";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // safecompress_check
            // 
            this.safecompress_check.AutoSize = true;
            this.safecompress_check.BackColor = System.Drawing.Color.White;
            this.safecompress_check.Checked = true;
            this.safecompress_check.CheckState = System.Windows.Forms.CheckState.Checked;
            this.safecompress_check.Location = new System.Drawing.Point(36, 135);
            this.safecompress_check.Name = "safecompress_check";
            this.safecompress_check.Size = new System.Drawing.Size(111, 17);
            this.safecompress_check.TabIndex = 24;
            this.safecompress_check.Text = "Safe Compressing";
            this.safecompress_check.UseVisualStyleBackColor = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(172, 136);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(436, 13);
            this.label7.TabIndex = 25;
            this.label7.Text = "Textures with alpha channel will be compressed to BC3 (if needed), also it takes " +
    "longer time";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(172, 262);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(382, 13);
            this.label8.TabIndex = 26;
            this.label8.Text = "Don\'t use special characters like / \\ | : * ? < > \" \' unless you want to get an e" +
    "rror";
            this.label8.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // ignoresn_check
            // 
            this.ignoresn_check.AutoSize = true;
            this.ignoresn_check.BackColor = System.Drawing.Color.White;
            this.ignoresn_check.Location = new System.Drawing.Point(36, 158);
            this.ignoresn_check.Name = "ignoresn_check";
            this.ignoresn_check.Size = new System.Drawing.Size(124, 17);
            this.ignoresn_check.TabIndex = 27;
            this.ignoresn_check.Text = "Ignore _s, _n and _g";
            this.ignoresn_check.UseVisualStyleBackColor = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(172, 159);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(251, 13);
            this.label9.TabIndex = 28;
            this.label9.Text = "Completely ignore speculars, normals and glowmaps";
            this.label9.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // optionsbutton
            // 
            this.optionsbutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.optionsbutton.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.optionsbutton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightCyan;
            this.optionsbutton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Azure;
            this.optionsbutton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.optionsbutton.Location = new System.Drawing.Point(512, 413);
            this.optionsbutton.Name = "optionsbutton";
            this.optionsbutton.Size = new System.Drawing.Size(75, 23);
            this.optionsbutton.TabIndex = 29;
            this.optionsbutton.Text = "Options";
            this.optionsbutton.UseVisualStyleBackColor = true;
            this.optionsbutton.Visible = false;
            this.optionsbutton.Click += new System.EventHandler(this.optionsbutton_Click);
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.LightSkyBlue;
            this.label10.Location = new System.Drawing.Point(13, 62);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(17, 17);
            this.label10.TabIndex = 30;
            this.label10.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.PaleGreen;
            this.label11.Location = new System.Drawing.Point(13, 85);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(17, 17);
            this.label11.TabIndex = 31;
            this.label11.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.PaleGreen;
            this.label12.Location = new System.Drawing.Point(13, 135);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(17, 17);
            this.label12.TabIndex = 32;
            this.label12.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.Color.OrangeRed;
            this.label13.Location = new System.Drawing.Point(13, 158);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(17, 17);
            this.label13.TabIndex = 33;
            this.label13.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label14
            // 
            this.label14.BackColor = System.Drawing.Color.Orange;
            this.label14.Location = new System.Drawing.Point(13, 181);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(17, 17);
            this.label14.TabIndex = 34;
            this.label14.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label15
            // 
            this.label15.BackColor = System.Drawing.Color.PaleGreen;
            this.label15.Location = new System.Drawing.Point(13, 236);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(17, 17);
            this.label15.TabIndex = 35;
            this.label15.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label16
            // 
            this.label16.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label16.BackColor = System.Drawing.Color.Orange;
            this.label16.Location = new System.Drawing.Point(13, 340);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(17, 17);
            this.label16.TabIndex = 38;
            this.label16.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label17
            // 
            this.label17.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label17.BackColor = System.Drawing.Color.PaleGreen;
            this.label17.Location = new System.Drawing.Point(13, 317);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(17, 17);
            this.label17.TabIndex = 37;
            this.label17.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label18
            // 
            this.label18.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label18.BackColor = System.Drawing.Color.LightSkyBlue;
            this.label18.Location = new System.Drawing.Point(13, 294);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(17, 17);
            this.label18.TabIndex = 36;
            this.label18.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label19
            // 
            this.label19.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(32, 294);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(134, 13);
            this.label19.TabIndex = 39;
            this.label19.Text = "Mandatory for compressing\r\n";
            this.label19.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label20
            // 
            this.label20.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(32, 317);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(95, 13);
            this.label20.TabIndex = 40;
            this.label20.Text = "Safe setting to use";
            this.label20.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label21
            // 
            this.label21.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(32, 340);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(86, 13);
            this.label21.TabIndex = 41;
            this.label21.Text = "Use with caution\r\n";
            this.label21.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label22
            // 
            this.label22.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(31, 363);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(198, 13);
            this.label22.TabIndex = 43;
            this.label22.Text = "Use only if you know what you are doing";
            this.label22.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label23
            // 
            this.label23.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label23.BackColor = System.Drawing.Color.OrangeRed;
            this.label23.Location = new System.Drawing.Point(13, 363);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(17, 17);
            this.label23.TabIndex = 42;
            this.label23.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // compressother_combo
            // 
            this.compressother_combo.BackColor = System.Drawing.Color.White;
            this.compressother_combo.FormattingEnabled = true;
            this.compressother_combo.Items.AddRange(new object[] {
            "BC7",
            "BC5",
            "BC3"});
            this.compressother_combo.Location = new System.Drawing.Point(36, 108);
            this.compressother_combo.Name = "compressother_combo";
            this.compressother_combo.Size = new System.Drawing.Size(121, 21);
            this.compressother_combo.TabIndex = 44;
            this.compressother_combo.Text = "BC7";
            this.compressother_combo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.compressother_combo_KeyDown);
            this.compressother_combo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.compressother_combo_KeyPress);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(680, 444);
            this.Controls.Add(this.compressother_combo);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.optionsbutton);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.ignoresn_check);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.safecompress_check);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.compressother_check);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.aboutbutton);
            this.Controls.Add(this.exportlogbutton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.backupname);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.backup_check);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.resize_check);
            this.Controls.Add(this.texturesize_combo);
            this.Controls.Add(this.compressbc_check);
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
        private System.Windows.Forms.CheckBox compressbc_check;
        private System.Windows.Forms.ComboBox texturesize_combo;
        private System.Windows.Forms.CheckBox resize_check;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox backup_check;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox backupname;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button exportlogbutton;
        private System.Windows.Forms.Button aboutbutton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox compressother_check;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox safecompress_check;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox ignoresn_check;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button optionsbutton;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.ComboBox compressother_combo;
    }
}

