namespace Citadel
{
    partial class formQuickAdd
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formQuickAdd));
            this.thirteenForm1 = new asset.ThirteenForm();
            this.tcMain = new asset.ThirteenTabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnAddAll = new asset.ThirteenButton();
            this.dQuickList = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAdd = new asset.ThirteenButton();
            this.txtAdd = new asset.ThirteenTextBox();
            this.thirteenControlBox1 = new asset.ThirteenControlBox();
            this.hMemNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hFirstName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hLastName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hFees = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hYearJoined = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.hActive = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.hSex = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.hGrade = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.hSchool = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hEmail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hComment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.thirteenForm1.SuspendLayout();
            this.tcMain.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dQuickList)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // thirteenForm1
            // 
            this.thirteenForm1.AccentColor = System.Drawing.Color.DodgerBlue;
            this.thirteenForm1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.thirteenForm1.ColorScheme = asset.ThirteenForm.ColorSchemes.Dark;
            this.thirteenForm1.Controls.Add(this.tcMain);
            this.thirteenForm1.Controls.Add(this.thirteenControlBox1);
            this.thirteenForm1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.thirteenForm1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.thirteenForm1.ForeColor = System.Drawing.Color.White;
            this.thirteenForm1.Location = new System.Drawing.Point(0, 0);
            this.thirteenForm1.Name = "thirteenForm1";
            this.thirteenForm1.Size = new System.Drawing.Size(886, 632);
            this.thirteenForm1.TabIndex = 0;
            this.thirteenForm1.Text = "Quick Add";
            // 
            // tcMain
            // 
            this.tcMain.AccentColor = System.Drawing.Color.DodgerBlue;
            this.tcMain.ColorScheme = asset.ThirteenTabControl.ColorSchemes.Dark;
            this.tcMain.Controls.Add(this.tabPage1);
            this.tcMain.Controls.Add(this.tabPage2);
            this.tcMain.ForeColor = System.Drawing.Color.White;
            this.tcMain.Location = new System.Drawing.Point(12, 37);
            this.tcMain.Name = "tcMain";
            this.tcMain.SelectedIndex = 0;
            this.tcMain.Size = new System.Drawing.Size(867, 588);
            this.tcMain.TabIndex = 5;
            this.tcMain.SelectedIndexChanged += new System.EventHandler(this.tcMain_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.tabPage1.Controls.Add(this.btnAddAll);
            this.tabPage1.Controls.Add(this.dQuickList);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(859, 559);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Add Multiple";
            // 
            // btnAddAll
            // 
            this.btnAddAll.AccentColor = System.Drawing.Color.DodgerBlue;
            this.btnAddAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnAddAll.ColorScheme = asset.ThirteenButton.ColorSchemes.Dark;
            this.btnAddAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.btnAddAll.ForeColor = System.Drawing.Color.White;
            this.btnAddAll.Location = new System.Drawing.Point(777, 529);
            this.btnAddAll.Name = "btnAddAll";
            this.btnAddAll.Size = new System.Drawing.Size(75, 23);
            this.btnAddAll.TabIndex = 5;
            this.btnAddAll.Text = "Add All";
            this.btnAddAll.UseVisualStyleBackColor = false;
            this.btnAddAll.Click += new System.EventHandler(this.btnAddAll_Click);
            // 
            // dQuickList
            // 
            this.dQuickList.AllowUserToResizeColumns = false;
            this.dQuickList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dQuickList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.dQuickList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dQuickList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.hMemNum,
            this.hFirstName,
            this.hLastName,
            this.hFees,
            this.hYearJoined,
            this.hActive,
            this.hSex,
            this.hGrade,
            this.hSchool,
            this.hEmail,
            this.hComment});
            this.dQuickList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dQuickList.GridColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.dQuickList.Location = new System.Drawing.Point(3, 6);
            this.dQuickList.Name = "dQuickList";
            this.dQuickList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dQuickList.Size = new System.Drawing.Size(853, 516);
            this.dQuickList.TabIndex = 0;
            this.dQuickList.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dQuickList_CellBeginEdit);
            this.dQuickList.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dQuickList_CellEndEdit);
            this.dQuickList.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dQuickList_CellEnter);
            this.dQuickList.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dQuickList_RowsAdded);
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.btnAdd);
            this.tabPage2.Controls.Add(this.txtAdd);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(859, 559);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Quick Add Codes";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(343, 32);
            this.label1.TabIndex = 2;
            this.label1.Text = "Please only use quickadd codes that have been copied \r\nfrom the students tab.";
            // 
            // btnAdd
            // 
            this.btnAdd.AccentColor = System.Drawing.Color.DodgerBlue;
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnAdd.ColorScheme = asset.ThirteenButton.ColorSchemes.Dark;
            this.btnAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(281, 69);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(58, 27);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // txtAdd
            // 
            this.txtAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.txtAdd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAdd.ColorScheme = asset.ThirteenTextBox.ColorSchemes.Dark;
            this.txtAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.txtAdd.ForeColor = System.Drawing.Color.White;
            this.txtAdd.Location = new System.Drawing.Point(6, 41);
            this.txtAdd.Name = "txtAdd";
            this.txtAdd.Size = new System.Drawing.Size(333, 22);
            this.txtAdd.TabIndex = 1;
            // 
            // thirteenControlBox1
            // 
            this.thirteenControlBox1.AccentColor = System.Drawing.Color.DodgerBlue;
            this.thirteenControlBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.thirteenControlBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.thirteenControlBox1.ColorScheme = asset.ThirteenControlBox.ColorSchemes.Dark;
            this.thirteenControlBox1.ForeColor = System.Drawing.Color.White;
            this.thirteenControlBox1.Location = new System.Drawing.Point(783, 3);
            this.thirteenControlBox1.Name = "thirteenControlBox1";
            this.thirteenControlBox1.Size = new System.Drawing.Size(100, 25);
            this.thirteenControlBox1.TabIndex = 0;
            this.thirteenControlBox1.Text = "thirteenControlBox1";
            // 
            // hMemNum
            // 
            this.hMemNum.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.hMemNum.HeaderText = "#";
            this.hMemNum.Name = "hMemNum";
            this.hMemNum.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.hMemNum.Width = 40;
            // 
            // hFirstName
            // 
            this.hFirstName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.hFirstName.HeaderText = "First Name";
            this.hFirstName.Name = "hFirstName";
            this.hFirstName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.hFirstName.Width = 98;
            // 
            // hLastName
            // 
            this.hLastName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.hLastName.HeaderText = "Last Name";
            this.hLastName.Name = "hLastName";
            this.hLastName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.hLastName.Width = 98;
            // 
            // hFees
            // 
            this.hFees.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.hFees.HeaderText = "Fees";
            this.hFees.Name = "hFees";
            this.hFees.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.hFees.Width = 64;
            // 
            // hYearJoined
            // 
            this.hYearJoined.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.hYearJoined.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.hYearJoined.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.hYearJoined.HeaderText = "Joined";
            this.hYearJoined.Name = "hYearJoined";
            this.hYearJoined.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.hYearJoined.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.hYearJoined.Width = 74;
            // 
            // hActive
            // 
            this.hActive.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.hActive.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.hActive.HeaderText = "Active";
            this.hActive.Items.AddRange(new object[] {
            "Yes",
            "No"});
            this.hActive.Name = "hActive";
            this.hActive.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.hActive.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.hActive.Width = 70;
            // 
            // hSex
            // 
            this.hSex.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.hSex.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.hSex.HeaderText = "Sex";
            this.hSex.Items.AddRange(new object[] {
            "M",
            "F"});
            this.hSex.Name = "hSex";
            this.hSex.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.hSex.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.hSex.Width = 56;
            // 
            // hGrade
            // 
            this.hGrade.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.hGrade.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.hGrade.HeaderText = "Grade";
            this.hGrade.Items.AddRange(new object[] {
            "9",
            "10",
            "11",
            "12",
            "13+"});
            this.hGrade.Name = "hGrade";
            this.hGrade.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.hGrade.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.hGrade.Width = 71;
            // 
            // hSchool
            // 
            this.hSchool.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.hSchool.HeaderText = "School";
            this.hSchool.Name = "hSchool";
            this.hSchool.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.hSchool.Width = 75;
            // 
            // hEmail
            // 
            this.hEmail.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.hEmail.HeaderText = "Email";
            this.hEmail.Name = "hEmail";
            this.hEmail.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.hEmail.Width = 67;
            // 
            // hComment
            // 
            this.hComment.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.hComment.HeaderText = "Comments";
            this.hComment.Name = "hComment";
            this.hComment.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.hComment.Width = 97;
            // 
            // formQuickAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(886, 632);
            this.Controls.Add(this.thirteenForm1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "formQuickAdd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quick Add";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.formQuickAdd_FormClosing);
            this.Load += new System.EventHandler(this.formQuickAdd_Load);
            this.thirteenForm1.ResumeLayout(false);
            this.tcMain.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dQuickList)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private asset.ThirteenForm thirteenForm1;
        private asset.ThirteenButton btnAdd;
        private System.Windows.Forms.Label label1;
        private asset.ThirteenTextBox txtAdd;
        private asset.ThirteenControlBox thirteenControlBox1;
        private asset.ThirteenTabControl tcMain;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private asset.ThirteenButton btnAddAll;
        private System.Windows.Forms.DataGridView dQuickList;
        private System.Windows.Forms.DataGridViewTextBoxColumn hMemNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn hFirstName;
        private System.Windows.Forms.DataGridViewTextBoxColumn hLastName;
        private System.Windows.Forms.DataGridViewTextBoxColumn hFees;
        private System.Windows.Forms.DataGridViewComboBoxColumn hYearJoined;
        private System.Windows.Forms.DataGridViewComboBoxColumn hActive;
        private System.Windows.Forms.DataGridViewComboBoxColumn hSex;
        private System.Windows.Forms.DataGridViewComboBoxColumn hGrade;
        private System.Windows.Forms.DataGridViewTextBoxColumn hSchool;
        private System.Windows.Forms.DataGridViewTextBoxColumn hEmail;
        private System.Windows.Forms.DataGridViewTextBoxColumn hComment;
    }
}