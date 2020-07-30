namespace VimeoAlbum
{
    partial class Form1
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
            this.btnAlbumOlustur = new System.Windows.Forms.Button();
            this.btnExcelVimeo = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lblTemizle = new System.Windows.Forms.Label();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.renklendirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lblOrbimTemizle = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSiraNumarasi = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lstLog = new System.Windows.Forms.ListBox();
            this.btnEkleOrbim = new System.Windows.Forms.Button();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.btnExcelOrbim = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.btnExcelExport = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAlbumOlustur
            // 
            this.btnAlbumOlustur.Location = new System.Drawing.Point(134, 16);
            this.btnAlbumOlustur.Name = "btnAlbumOlustur";
            this.btnAlbumOlustur.Size = new System.Drawing.Size(135, 40);
            this.btnAlbumOlustur.TabIndex = 1;
            this.btnAlbumOlustur.Text = "Album Oluştur";
            this.btnAlbumOlustur.UseVisualStyleBackColor = true;
            this.btnAlbumOlustur.Click += new System.EventHandler(this.btnAlbumOlustur_Click);
            // 
            // btnExcelVimeo
            // 
            this.btnExcelVimeo.Location = new System.Drawing.Point(10, 16);
            this.btnExcelVimeo.Name = "btnExcelVimeo";
            this.btnExcelVimeo.Size = new System.Drawing.Size(118, 40);
            this.btnExcelVimeo.TabIndex = 4;
            this.btnExcelVimeo.Text = "Excel İmport";
            this.btnExcelVimeo.UseVisualStyleBackColor = true;
            this.btnExcelVimeo.Click += new System.EventHandler(this.btnExcelVimeo_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(1, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1052, 506);
            this.tabControl1.TabIndex = 7;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.lblTemizle);
            this.tabPage1.Controls.Add(this.listBox2);
            this.tabPage1.Controls.Add(this.btnExcelVimeo);
            this.tabPage1.Controls.Add(this.listBox1);
            this.tabPage1.Controls.Add(this.btnAlbumOlustur);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1044, 480);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "VİMEO ALBUM OLUŞTUR";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // lblTemizle
            // 
            this.lblTemizle.AutoSize = true;
            this.lblTemizle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblTemizle.ForeColor = System.Drawing.Color.OrangeRed;
            this.lblTemizle.Location = new System.Drawing.Point(290, 30);
            this.lblTemizle.Name = "lblTemizle";
            this.lblTemizle.Size = new System.Drawing.Size(53, 13);
            this.lblTemizle.TabIndex = 9;
            this.lblTemizle.Text = "TEMİZLE";
            this.lblTemizle.Click += new System.EventHandler(this.lblTemizle_Click);
            // 
            // listBox2
            // 
            this.listBox2.FormattingEnabled = true;
            this.listBox2.Location = new System.Drawing.Point(8, 67);
            this.listBox2.Name = "listBox2";
            this.listBox2.ScrollAlwaysVisible = true;
            this.listBox2.Size = new System.Drawing.Size(432, 407);
            this.listBox2.TabIndex = 8;
            // 
            // listBox1
            // 
            this.listBox1.ContextMenuStrip = this.contextMenuStrip1;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(439, 67);
            this.listBox1.Name = "listBox1";
            this.listBox1.ScrollAlwaysVisible = true;
            this.listBox1.Size = new System.Drawing.Size(599, 407);
            this.listBox1.TabIndex = 7;
            this.listBox1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBox1_DrawItem);
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            this.listBox1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBox1_MouseDoubleClick);
            this.listBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listBox1_MouseDown);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.renklendirToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(131, 26);
            // 
            // renklendirToolStripMenuItem
            // 
            this.renklendirToolStripMenuItem.Name = "renklendirToolStripMenuItem";
            this.renklendirToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.renklendirToolStripMenuItem.Text = "Renklendir";
            this.renklendirToolStripMenuItem.Click += new System.EventHandler(this.renklendirToolStripMenuItem_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.lblOrbimTemizle);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.txtSiraNumarasi);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.lstLog);
            this.tabPage2.Controls.Add(this.btnEkleOrbim);
            this.tabPage2.Controls.Add(this.treeView1);
            this.tabPage2.Controls.Add(this.btnExcelOrbim);
            this.tabPage2.Controls.Add(this.comboBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1044, 480);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "ORBİM YÜKLEME";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lblOrbimTemizle
            // 
            this.lblOrbimTemizle.AutoSize = true;
            this.lblOrbimTemizle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblOrbimTemizle.ForeColor = System.Drawing.Color.Red;
            this.lblOrbimTemizle.Location = new System.Drawing.Point(636, 22);
            this.lblOrbimTemizle.Name = "lblOrbimTemizle";
            this.lblOrbimTemizle.Size = new System.Drawing.Size(60, 13);
            this.lblOrbimTemizle.TabIndex = 16;
            this.lblOrbimTemizle.Text = "TEMİZLE";
            this.lblOrbimTemizle.Click += new System.EventHandler(this.lblOrbimTemizle_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(224, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "SIRA GİRİN";
            // 
            // txtSiraNumarasi
            // 
            this.txtSiraNumarasi.Location = new System.Drawing.Point(295, 41);
            this.txtSiraNumarasi.Name = "txtSiraNumarasi";
            this.txtSiraNumarasi.Size = new System.Drawing.Size(76, 20);
            this.txtSiraNumarasi.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "SEÇİLEN ID :";
            // 
            // lstLog
            // 
            this.lstLog.FormattingEnabled = true;
            this.lstLog.Location = new System.Drawing.Point(378, 63);
            this.lstLog.Name = "lstLog";
            this.lstLog.Size = new System.Drawing.Size(568, 407);
            this.lstLog.TabIndex = 12;
            // 
            // btnEkleOrbim
            // 
            this.btnEkleOrbim.Location = new System.Drawing.Point(501, 6);
            this.btnEkleOrbim.Name = "btnEkleOrbim";
            this.btnEkleOrbim.Size = new System.Drawing.Size(117, 44);
            this.btnEkleOrbim.TabIndex = 11;
            this.btnEkleOrbim.Text = "Ekle";
            this.btnEkleOrbim.UseVisualStyleBackColor = true;
            this.btnEkleOrbim.Click += new System.EventHandler(this.btnEkleOrbim_Click);
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(7, 63);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(365, 407);
            this.treeView1.TabIndex = 10;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // btnExcelOrbim
            // 
            this.btnExcelOrbim.Location = new System.Drawing.Point(378, 6);
            this.btnExcelOrbim.Name = "btnExcelOrbim";
            this.btnExcelOrbim.Size = new System.Drawing.Size(117, 44);
            this.btnExcelOrbim.TabIndex = 9;
            this.btnExcelOrbim.Text = "Excel Import";
            this.btnExcelOrbim.UseVisualStyleBackColor = true;
            this.btnExcelOrbim.Click += new System.EventHandler(this.btnExcelOrbim_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(7, 17);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(365, 21);
            this.comboBox1.TabIndex = 3;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.btnExcelExport);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1044, 480);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "MP4 TO EXCEL";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // btnExcelExport
            // 
            this.btnExcelExport.Location = new System.Drawing.Point(94, 74);
            this.btnExcelExport.Name = "btnExcelExport";
            this.btnExcelExport.Size = new System.Drawing.Size(132, 37);
            this.btnExcelExport.TabIndex = 1;
            this.btnExcelExport.Text = "Videoları Excel Yap";
            this.btnExcelExport.UseVisualStyleBackColor = true;
            this.btnExcelExport.Click += new System.EventHandler(this.btnExcelExport_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(532, 16);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1054, 530);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Vimeo & Orbim Video";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnAlbumOlustur;
        private System.Windows.Forms.Button btnExcelVimeo;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button btnExcelExport;
        private System.Windows.Forms.ListBox lstLog;
        private System.Windows.Forms.Button btnEkleOrbim;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Button btnExcelOrbim;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem renklendirToolStripMenuItem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSiraNumarasi;
        private System.Windows.Forms.Label lblTemizle;
        private System.Windows.Forms.Label lblOrbimTemizle;
        private System.Windows.Forms.Button button1;
    }
}

