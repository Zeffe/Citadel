namespace Citadel
{
    partial class formMain
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
            this.thirteenForm1 = new asset.ThirteenForm();
            this.thirteenControlBox1 = new asset.ThirteenControlBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblUser = new System.Windows.Forms.Label();
            this.pnlContainer = new System.Windows.Forms.Panel();
            this.thirteenForm1.SuspendLayout();
            this.SuspendLayout();
            // 
            // thirteenForm1
            // 
            this.thirteenForm1.AccentColor = System.Drawing.Color.DodgerBlue;
            this.thirteenForm1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.thirteenForm1.ColorScheme = asset.ThirteenForm.ColorSchemes.Dark;
            this.thirteenForm1.Controls.Add(this.lblUser);
            this.thirteenForm1.Controls.Add(this.label1);
            this.thirteenForm1.Controls.Add(this.thirteenControlBox1);
            this.thirteenForm1.Controls.Add(this.pnlContainer);
            this.thirteenForm1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.thirteenForm1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.thirteenForm1.ForeColor = System.Drawing.Color.White;
            this.thirteenForm1.Location = new System.Drawing.Point(0, 0);
            this.thirteenForm1.Name = "thirteenForm1";
            this.thirteenForm1.Size = new System.Drawing.Size(841, 638);
            this.thirteenForm1.TabIndex = 0;
            this.thirteenForm1.Text = "Citadel - [ Panel ]";
            // 
            // thirteenControlBox1
            // 
            this.thirteenControlBox1.AccentColor = System.Drawing.Color.DodgerBlue;
            this.thirteenControlBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.thirteenControlBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.thirteenControlBox1.ColorScheme = asset.ThirteenControlBox.ColorSchemes.Dark;
            this.thirteenControlBox1.ForeColor = System.Drawing.Color.White;
            this.thirteenControlBox1.Location = new System.Drawing.Point(738, 3);
            this.thirteenControlBox1.Name = "thirteenControlBox1";
            this.thirteenControlBox1.Size = new System.Drawing.Size(100, 25);
            this.thirteenControlBox1.TabIndex = 0;
            this.thirteenControlBox1.Text = "thirteenControlBox1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.label1.Location = new System.Drawing.Point(12, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Welcome";
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.BackColor = System.Drawing.Color.DodgerBlue;
            this.lblUser.Location = new System.Drawing.Point(84, 40);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(83, 16);
            this.lblUser.TabIndex = 2;
            this.lblUser.Text = "placeHolder";
            // 
            // pnlContainer
            // 
            this.pnlContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.pnlContainer.Location = new System.Drawing.Point(6, 34);
            this.pnlContainer.Name = "pnlContainer";
            this.pnlContainer.Size = new System.Drawing.Size(78, 28);
            this.pnlContainer.TabIndex = 3;
            // 
            // formMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(841, 638);
            this.Controls.Add(this.thirteenForm1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "formMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Citadel - [ Panel ]";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.formMain_FormClosing);
            this.Load += new System.EventHandler(this.formMain_Load);
            this.thirteenForm1.ResumeLayout(false);
            this.thirteenForm1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private asset.ThirteenForm thirteenForm1;
        private asset.ThirteenControlBox thirteenControlBox1;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlContainer;
    }
}