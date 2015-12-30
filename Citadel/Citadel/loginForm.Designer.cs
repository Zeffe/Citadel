namespace Citadel
{
    partial class rformLogin
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
            this.formLogin = new asset.ThirteenForm();
            this.btnLogin = new asset.ThirteenButton();
            this.txtPass = new asset.ThirteenTextBox();
            this.txtUser = new asset.ThirteenTextBox();
            this.thirteenControlBox1 = new asset.ThirteenControlBox();
            this.formLogin.SuspendLayout();
            this.SuspendLayout();
            // 
            // formLogin
            // 
            this.formLogin.AccentColor = System.Drawing.Color.DodgerBlue;
            this.formLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.formLogin.ColorScheme = asset.ThirteenForm.ColorSchemes.Dark;
            this.formLogin.Controls.Add(this.btnLogin);
            this.formLogin.Controls.Add(this.txtPass);
            this.formLogin.Controls.Add(this.txtUser);
            this.formLogin.Controls.Add(this.thirteenControlBox1);
            this.formLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.formLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.formLogin.ForeColor = System.Drawing.Color.White;
            this.formLogin.Location = new System.Drawing.Point(0, 0);
            this.formLogin.Name = "formLogin";
            this.formLogin.Size = new System.Drawing.Size(225, 153);
            this.formLogin.TabIndex = 0;
            this.formLogin.Text = "Citadel - [ Login ]";
            // 
            // btnLogin
            // 
            this.btnLogin.AccentColor = System.Drawing.Color.DodgerBlue;
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnLogin.ColorScheme = asset.ThirteenButton.ColorSchemes.Dark;
            this.btnLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.Location = new System.Drawing.Point(75, 116);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 23);
            this.btnLogin.TabIndex = 3;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // txtPass
            // 
            this.txtPass.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.txtPass.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPass.ColorScheme = asset.ThirteenTextBox.ColorSchemes.Dark;
            this.txtPass.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.txtPass.ForeColor = System.Drawing.Color.White;
            this.txtPass.Location = new System.Drawing.Point(47, 78);
            this.txtPass.MaxLength = 15;
            this.txtPass.Name = "txtPass";
            this.txtPass.Size = new System.Drawing.Size(129, 22);
            this.txtPass.TabIndex = 2;
            // 
            // txtUser
            // 
            this.txtUser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.txtUser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUser.ColorScheme = asset.ThirteenTextBox.ColorSchemes.Dark;
            this.txtUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.txtUser.ForeColor = System.Drawing.Color.White;
            this.txtUser.Location = new System.Drawing.Point(47, 50);
            this.txtUser.MaxLength = 15;
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(129, 22);
            this.txtUser.TabIndex = 1;
            // 
            // thirteenControlBox1
            // 
            this.thirteenControlBox1.AccentColor = System.Drawing.Color.DodgerBlue;
            this.thirteenControlBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.thirteenControlBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.thirteenControlBox1.ColorScheme = asset.ThirteenControlBox.ColorSchemes.Dark;
            this.thirteenControlBox1.ForeColor = System.Drawing.Color.White;
            this.thirteenControlBox1.Location = new System.Drawing.Point(122, 3);
            this.thirteenControlBox1.Name = "thirteenControlBox1";
            this.thirteenControlBox1.Size = new System.Drawing.Size(100, 25);
            this.thirteenControlBox1.TabIndex = 0;
            this.thirteenControlBox1.Text = "thirteenControlBox1";
            // 
            // rformLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(225, 153);
            this.Controls.Add(this.formLogin);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "rformLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Citadel - [ Login ]";
            this.Load += new System.EventHandler(this.rformLogin_Load);
            this.formLogin.ResumeLayout(false);
            this.formLogin.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private asset.ThirteenForm formLogin;
        private asset.ThirteenControlBox thirteenControlBox1;
        private asset.ThirteenTextBox txtPass;
        private asset.ThirteenTextBox txtUser;
        private asset.ThirteenButton btnLogin;
    }
}

