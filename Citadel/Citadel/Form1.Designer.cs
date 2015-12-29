namespace Citadel
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
            this.thirteenForm1 = new asset.ThirteenForm();
            this.thirteenTextBox2 = new asset.ThirteenTextBox();
            this.thirteenTextBox1 = new asset.ThirteenTextBox();
            this.thirteenControlBox1 = new asset.ThirteenControlBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.thirteenForm1.SuspendLayout();
            this.SuspendLayout();
            // 
            // thirteenForm1
            // 
            this.thirteenForm1.AccentColor = System.Drawing.Color.DodgerBlue;
            this.thirteenForm1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.thirteenForm1.ColorScheme = asset.ThirteenForm.ColorSchemes.Dark;
            this.thirteenForm1.Controls.Add(this.richTextBox1);
            this.thirteenForm1.Controls.Add(this.thirteenTextBox2);
            this.thirteenForm1.Controls.Add(this.thirteenTextBox1);
            this.thirteenForm1.Controls.Add(this.thirteenControlBox1);
            this.thirteenForm1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.thirteenForm1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.thirteenForm1.ForeColor = System.Drawing.Color.White;
            this.thirteenForm1.Location = new System.Drawing.Point(0, 0);
            this.thirteenForm1.Name = "thirteenForm1";
            this.thirteenForm1.Size = new System.Drawing.Size(611, 505);
            this.thirteenForm1.TabIndex = 0;
            this.thirteenForm1.Text = "Citadel - [ Login ]";
            // 
            // thirteenTextBox2
            // 
            this.thirteenTextBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.thirteenTextBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.thirteenTextBox2.ColorScheme = asset.ThirteenTextBox.ColorSchemes.Dark;
            this.thirteenTextBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.thirteenTextBox2.ForeColor = System.Drawing.Color.White;
            this.thirteenTextBox2.Location = new System.Drawing.Point(47, 89);
            this.thirteenTextBox2.Name = "thirteenTextBox2";
            this.thirteenTextBox2.Size = new System.Drawing.Size(100, 22);
            this.thirteenTextBox2.TabIndex = 2;
            // 
            // thirteenTextBox1
            // 
            this.thirteenTextBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.thirteenTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.thirteenTextBox1.ColorScheme = asset.ThirteenTextBox.ColorSchemes.Dark;
            this.thirteenTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.thirteenTextBox1.ForeColor = System.Drawing.Color.White;
            this.thirteenTextBox1.Location = new System.Drawing.Point(47, 61);
            this.thirteenTextBox1.Name = "thirteenTextBox1";
            this.thirteenTextBox1.Size = new System.Drawing.Size(100, 22);
            this.thirteenTextBox1.TabIndex = 1;
            // 
            // thirteenControlBox1
            // 
            this.thirteenControlBox1.AccentColor = System.Drawing.Color.DodgerBlue;
            this.thirteenControlBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.thirteenControlBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.thirteenControlBox1.ColorScheme = asset.ThirteenControlBox.ColorSchemes.Dark;
            this.thirteenControlBox1.ForeColor = System.Drawing.Color.White;
            this.thirteenControlBox1.Location = new System.Drawing.Point(508, 3);
            this.thirteenControlBox1.Name = "thirteenControlBox1";
            this.thirteenControlBox1.Size = new System.Drawing.Size(100, 25);
            this.thirteenControlBox1.TabIndex = 0;
            this.thirteenControlBox1.Text = "thirteenControlBox1";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(153, 100);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(412, 393);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(611, 505);
            this.Controls.Add(this.thirteenForm1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.Text = "Form1";
            this.thirteenForm1.ResumeLayout(false);
            this.thirteenForm1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private asset.ThirteenForm thirteenForm1;
        private asset.ThirteenControlBox thirteenControlBox1;
        private asset.ThirteenTextBox thirteenTextBox2;
        private asset.ThirteenTextBox thirteenTextBox1;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}

