namespace VimeoAlbum
{
    partial class frmOnay
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
            this.btnOnay = new System.Windows.Forms.Button();
            this.txtOnay = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnOnay
            // 
            this.btnOnay.Location = new System.Drawing.Point(268, 53);
            this.btnOnay.Name = "btnOnay";
            this.btnOnay.Size = new System.Drawing.Size(88, 27);
            this.btnOnay.TabIndex = 0;
            this.btnOnay.Text = "ONAY";
            this.btnOnay.UseVisualStyleBackColor = true;
            this.btnOnay.Click += new System.EventHandler(this.btnOnay_Click);
            // 
            // txtOnay
            // 
            this.txtOnay.Location = new System.Drawing.Point(55, 57);
            this.txtOnay.Name = "txtOnay";
            this.txtOnay.Size = new System.Drawing.Size(207, 20);
            this.txtOnay.TabIndex = 1;
            this.txtOnay.UseSystemPasswordChar = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(52, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Lütfen onay şifresini giriniz";
            // 
            // frmOnay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(418, 174);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtOnay);
            this.Controls.Add(this.btnOnay);
            this.MaximizeBox = false;
            this.Name = "frmOnay";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmOnay";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOnay;
        private System.Windows.Forms.TextBox txtOnay;
        private System.Windows.Forms.Label label1;
    }
}