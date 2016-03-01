using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Extensions;
using cXML = ClosedXML.Excel;
using dXML = DocumentFormat.OpenXml.Spreadsheet;
using System.Diagnostics;

namespace Citadel
{
    public partial class formMain : Form
    {
        string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData); // %APPDATA% path
        int currentUser;       // Usernumber of the logged in user.
        int perms;             // Used to recognize permissions of a user, only 1 = administrator.
        string specificFolder; // A string used to store the Citadel folder in appData.
        Panel activePanel;     // Panel used to find the height to move indicator.
        public static String[,] students = new String[50, 12]; // 2D array that stores students.
        String[,] log = new String[200, 6]; // 2D array that stores log information
        int currentView;       // The Student that is currently being viewed
        int studentLength;     // Number of valid student accounts.
        int searchFor = 0;     // Value used to determine the 2nd dimension to filter by.
        bool exit = true;      // Determine if the entire programming is exiting, or only the form.
        bool firstNew = true;  // Determines whether the user has filled in data on the new student tab.
        Double feeDbl = 0;     // Used as storage for the new student fee double value.
        bool editing = false;  // Determines if the user is editing a user or creating a new one.
        int editStudent;       // Determines the student being edited.
        double males, females; // Amount of males and females for statistics.
        double aYes, aNo;      // Amount of students active and non active.
        double hasFees, noFees;// Amount of students that have or do not have fees.
        double totalFees;      // Total amount due in fees.
        double statHeight;     // Default height of graphs on statistic page.
        int statY;             // Default Y location of the graphs on statistic page.

        // Saves the panel tab buttons with their respective display panels.
        Dictionary<Panel, Panel> displays = new Dictionary<Panel, Panel>();
        // Saves the heights necessarry to move the indicator to for a given panel.
        Dictionary<Panel, Point> heights = new Dictionary<Panel, Point>();
        // Saves the student number with its respective property.
        Dictionary<String, int> treeProps = new Dictionary<String, int>();
        // Saves the amount of students who joined in a specific year.
        Dictionary<String, int> years = new Dictionary<String, int>();
        // Saves the amount of students in a specific state.
        Dictionary<String, int> states = new Dictionary<String, int>();

        // List of panel buttons.
        List<Panel> panelButtons = new List<Panel>();

        // Confirmation box that is displayed when attempting to exit.
        msgbox logConf = new msgbox("Are you sure you wish to exit Citadel?", "Exit", 2);
        // Confirmation box for clearing the log.
        msgbox logClearConf = new msgbox("Are you sure that you want to clear the log?", "Clear", 2);
        // Confirmation box used to confirm that the user wants to clear data on new student tab.
        msgbox _conf;
        // Confirmation box used when deleting a student.
        msgbox delConf;


        public formMain(int user, int _perms)
        {
            InitializeComponent();
            currentUser = user;     // Catch the current user from rformLogin.
            perms = _perms;         // Catch the perms of the current user from rformLogin.
        }


        private void formMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!logout && exit)
            {
                if (logConf.ShowDialog() == DialogResult.Yes)
                {
                    exit = false;
                    Application.Exit();
                }
                else
                {
                    e.Cancel = true;
                }
            }

        }

        // Centers a control inside a group box.
        void gbTitle(Control parent, Control title)
        {
            Point _temp = parent.DisplayRectangle.Location;
            _temp.X += (parent.DisplayRectangle.Width - title.Width) / 2;
            _temp.Y = title.Location.Y;
            parent.ForeColor = Color.White;
            title.Location = _temp;
        }

        // Draws an arrow pointing to the selected tab by
        // moving a picture box to the necessarry height.
        void drawIndicator(Panel pnl, bool draw)
        {
            if (draw)
            {
                if (activePanel != pnl)
                {
                    pctPointer2.Visible = true;
                    pctPointer2.Location = heights[pnl];
                    pnl.BackColor = Color.FromArgb(75, 75, 75);
                }
            }
            else
            {
                pctPointer2.Visible = false;
                if (activePanel != pnl)
                {
                    foreach (Panel panel in panelButtons)
                    {
                        if (panel != activePanel)
                        {
                            panel.BackColor = Color.FromArgb(50, 50, 50);
                        }
                    }
                }
                else
                {
                    pnl.BackColor = Color.DodgerBlue;
                }
            }
        }

        StreamReader _reader;

        // Reads a plaintext file into a 2D array.
        void readToArray(string file, String[,] array2d, string hash)
        {
            Array.Clear(array2d, 0, array2d.Length);
            if (array2d == students)
            {
                studentLength = 0;
            }
            if (hash != "NA")
            {
                rformLogin.PasswordHash = hash;
            }
            string str;
            _reader = File.OpenText(file);
            int i = 0;
            while ((str = _reader.ReadLine()) != null)
            {
                int j = 0;
                //str = rformLogin.Decrypt(str);
                String[] strArray = new String[str.Split('\\').Length];
                strArray = str.Split('\\');
                foreach (string element in strArray)
                {
                    try
                    {
                        array2d[i, j] = element;
                    }
                    catch { }
                    j++;
                }

                if (array2d == students)
                {
                    studentLength++;
                }
                i++;
            }
            rformLogin.PasswordHash = "admin";
            _reader.Close();
        }

        // Updates the selected tab and draws the
        // necessarry indicator.
        void updateSelected(Panel pnl)
        {
            activePanel.BackColor = Color.FromArgb(50, 50, 50);
            displays[pnl].BringToFront();
            pnl.BackColor = Color.DodgerBlue;
            activePanel = pnl;
            pctPointer.Location = heights[pnl];
        }

        // Method for highlighting of buttons on mouse enter.
        void pbHighlight(PictureBox pb)
        {
            pb.MouseEnter += new EventHandler(pbEnter);
            pb.MouseLeave += new EventHandler(pbLeave);
        }

        void pbEnter(object sender, EventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            pb.BackColor = Color.DodgerBlue;
        }

        void pbLeave(object sender, EventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            pb.BackColor = pb.Parent.BackColor;
        }

        void panelButton(Panel panelb, Label label, PictureBox picture, Panel display)
        {
            displays.Add(panelb, display);
            panelButtons.Add(panelb);
            heights.Add(panelb, new Point(pctPointer.Location.X, panelb.Location.Y + 12));
            panelb.Click += new System.EventHandler(pnlClick);
            picture.Click += new System.EventHandler(pctClick);
            label.Click += new System.EventHandler(lblClick);
            picture.MouseEnter += new System.EventHandler(pctEnter);
            label.MouseEnter += new System.EventHandler(lblEnter);
            panelb.MouseEnter += new System.EventHandler(pnlEnter);
            panelb.MouseLeave += new System.EventHandler(pnlLeave);
        }


        void pctEnter(object sender, EventArgs e)
        {
            PictureBox _pct = sender as PictureBox;
            drawIndicator(_pct.Parent as Panel, true);
        }

        void lblEnter(object sender, EventArgs e)
        {
            Label _lbl = sender as Label;
            drawIndicator(_lbl.Parent as Panel, true);
        }

        void pnlEnter(object sender, EventArgs e)
        {
            drawIndicator(sender as Panel, true);
        }

        void pnlLeave(object sender, EventArgs e)
        {
            drawIndicator(sender as Panel, false);
        }

        void pnlClick(object sender, EventArgs e)
        {
            Panel _pnl = sender as Panel;
            updateSelected(_pnl);
        }

        void pctClick(object sender, EventArgs e)
        {
            PictureBox _pct = sender as PictureBox;
            updateSelected(_pct.Parent as Panel);
        }

        void lblClick(object sender, EventArgs e)
        {
            Label _lbl = sender as Label;
            updateSelected(_lbl.Parent as Panel);
        }

        void _onClick(object sender, EventArgs e)
        {
            updateSelected(pnlbUsers);
        }

        // Gets the username for a corresponding user number.
        private string getUser(int userNum)
        {
            return rformLogin.users[userNum, 0].Substring(0, rformLogin.users[userNum, 0].Length - 1);
        }

        // Initializes confirmation box panels size and location.
        void confBoxInit(TextBox textbox, Panel box)
        {
            box.Location = new Point(textbox.Location.X - 1, textbox.Location.Y - 1);
            box.Size = new Size(textbox.Width + 2, textbox.Height + 2);
            textbox.BringToFront();
        }

        // Used to change the color of a given panel
        // which acts as a border of a textbox.
        void confBox(Panel box, Color color)
        {
            box.BackColor = color;
            //box.Update();
        }

        void refreshStudentTree(string contains)
        {
            tvStudents.Invoke((MethodInvoker)(() => tvStudents.Nodes.Clear()));

            // Add students to the student tab tree view.
            TreeNode student;
            TreeNode display1;
            TreeNode display2;
            TreeNode[] studentChildren;

            bool skip; // Skips a student when filtering the treeview.

            // Loop through each student.
            for (int i = 0; i < students.GetLength(0); i++)
            {
                skip = false;
                // Break the loop if the student returns a null value.
                if (students[i, 4] == null) break;
                // Only pass if the student contains the content text or
                // don't filter if contains is set to "".
                if (students[i, searchFor].ToLower().Contains(contains.ToLower()) || contains == "") {
                    if (searchFor == 3 && students[i, searchFor] == "$0.00") skip = true;
                    if (!skip)
                    {
                        display1 = new TreeNode(cmbTreeview.Text + ": " + students[i, treeProps[cmbTreeview.Text]] + " ");
                        display2 = new TreeNode("Grade: " + students[i, 7] + " ");
                        studentChildren = new TreeNode[] { display2, display1 };
                        student = new TreeNode(students[i, 1] + " " + students[i, 2] + " - " + students[i, 0], studentChildren);
                        tvStudents.Invoke((MethodInvoker)(() => tvStudents.Nodes.Add(student)));
                    }
                }
            }
        }

        private void formMain_Load(object sender, EventArgs e)
        {
            specificFolder = Path.Combine(folder, "Citadel"); // %APPDATA%/Citadel home path

            // Initialize active panel and bring the dashboard to front.
            activePanel = pnlbDashboard;
            pnlDashboard.BringToFront();

            // Centers controls inside their parents.
            gbTitle(tcUserList, btnDelete);
            gbTitle(tcSelectedUser, btnLogout);
            gbTitle(pnlStats, lblStatsTitle);
            gbTitle(pnlStats, lblReadingFrom2);
            gbTitle(pnlStudents, lblReadingFrom);
            gbTitle(pnlStudents, lblStudentsTitle);

            // Click event handlers that open the user page
            // when the welcome message is clicked.
            lblUser.Click += new System.EventHandler(_onClick);
            pnlContainer.Click += new System.EventHandler(_onClick);
            lblWelcome.Click += new System.EventHandler(_onClick);
            
            // Adjusts the welcome message at the top of the tabs.
            string _user = "";
            if (getUser(currentUser).Length < 12)
            {
                lblUser.Text = getUser(currentUser);
            }
            else
            {
                for (int i = 0; i < 12; i++)
                {
                    _user += getUser(currentUser)[i];
                }
                lblUser.Text = _user + "...";
            }
            lblCuruser.Text = getUser(currentUser);
            pnlContainer.Width = 85 + lblUser.Width;
            int _x = pnlContainer.Location.X;
            pnlContainer.Location = new Point((divider1.Location.X - pnlContainer.Width) / 2, pnlContainer.Location.Y);
            _x = pnlContainer.Location.X - _x;
            lblWelcome.Location = new Point(lblWelcome.Location.X + _x, lblWelcome.Location.Y);
            lblUser.Location = new Point(lblUser.Location.X + _x, lblWelcome.Location.Y);

            // Initialize students tab numeric updown year.
            nmNewYear.Maximum = DateTime.Now.Year;
            nmNewYear.Value = DateTime.Now.Year;

            // Read the students data file into an array.
            readToArray(specificFolder + "/data/students.fbla", students, "NA");

            // Adds all users to the user list on user page.
            for (int i = 0; i < rformLogin.users.GetLength(0); i++)
            {
                if (rformLogin.users[i, 1] == null) break;
                if (!listUsers.Items.Contains(getUser(i)))
                {
                    listUsers.Items.Add(getUser(i));
                }
            }

            // Add properties to a dictionary.
            treeProps.Add("Year Joined", 4);
            treeProps.Add("Email", 9);
            treeProps.Add("School", 8);

            // Initialize student tab combobox values.
            cmbFilterBy.SelectedIndex = 0;
            cmbSearchBy.SelectedIndex = 0;
            cmbTreeview.SelectedIndex = 0;

            // Adds all students to the student tree view.
            refreshStudentTree("");

            // Adds all the event handlers for the tabs.
            panelButton(pnlbDashboard, lblDashboard, pctDashboard, pnlDashboard);
            panelButton(pnlbStats, lblStats, pctStats, pnlStats);
            panelButton(pnlbStudents, lblStudents, pctStudents, pnlStudents);
            panelButton(pnlbSource, lblSource, pctSource, pnlSource);
            panelButton(pnlbUsers, lblUsers, pctUsers, pnlUsers);
            panelButton(pnlbSettings, lblSettings, pctSettings, pnlSettings);
            panelButton(pnlbInfo, lblInfo, pctInfo, pnlInfo);

            // Initialize highlighting for picture box buttons.
            pbHighlight(btnCopyQf);
            pbHighlight(btnEdit);
            pbHighlight(btnDelStudent);
            pbHighlight(btnNew1);
            pbHighlight(btnNew2);
            pbHighlight(btnSave);
            pbHighlight(btnClear);
            pbHighlight(btnGenderNext);
            pbHighlight(btnGenderPrev);
            pbHighlight(btnActiveNext);
            pbHighlight(btnActivePrev);
            pbHighlight(btnGradeNext);
            pbHighlight(btnGradePrev);
            pbHighlight(btnSearch);
            pbHighlight(btnFilter);
            pbHighlight(btnQuickAdd);

            // Moves panels behind the given textboxes in order to
            // easily draw a colored signifier around the textboxes.
            confBoxInit(txtUsername, npnlUser);
            confBoxInit(txtPassword, npnlPass);
            confBoxInit(txtPassconf, npnlPassconf);
            confBoxInit(txtFirstname, npnlFirst);
            confBoxInit(txtLastname, npnlLast);
            confBoxInit(txtEmail, npnlEmail);
            confBoxInit(txtEmailconf, npnlEmailconf);

            // Places place holder text in given textboxes.
            rformLogin.placeHolder(txtUsername, "Username", false);
            rformLogin.placeHolder(txtPassword, "Password", true);
            rformLogin.placeHolder(txtPassconf, "Confirm Password", true);
            rformLogin.placeHolder(txtFirstname, "First Name", false);
            rformLogin.placeHolder(txtLastname, "Last Name", false);
            rformLogin.placeHolder(txtEmail, "Email", false);
            rformLogin.placeHolder(txtEmailconf, "Confirm Email", false);

            // Timer for quickadd that auto updates stuent page.
            tmrQckAdd.Interval = 1000;
            tmrQckAdd.Elapsed += new System.Timers.ElapsedEventHandler(OnTimer);

            // Disables controls on New Student Page.
            enableNewStudent(false);

            // tabStudent events.
            this.btnCopyQf.Click += new System.EventHandler(this.btnCopyQf_Click);
            this.btnQuickAdd.Click += new System.EventHandler(this.btnQuickAdd_Click);
            this.txtNewFees.Enter += new System.EventHandler(this.txtNewFees_Enter);
            this.txtNewFees.Leave += new System.EventHandler(this.txtNewFees_Leave);
            this.btnGradePrev.Click += new System.EventHandler(this.btnGradePrev_Click);
            this.btnGradeNext.Click += new System.EventHandler(this.btnGradeNext_Click);
            this.btnActivePrev.Click += new System.EventHandler(this.btnActivePrev_Click);
            this.btnActiveNext.Click += new System.EventHandler(this.btnActiveNext_Click);
            this.btnGenderPrev.Click += new System.EventHandler(this.btnGenderPrev_Click);
            this.btnGenderNext.Click += new System.EventHandler(this.btnGenderNext_Click);
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            this.btnNew2.Click += new System.EventHandler(this.btnNew2_Click);
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            this.txtFilter.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtFilter_KeyDown);
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            this.btnNew1.Click += new System.EventHandler(this.btnNew1_Click);
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            this.btnDelStudent.Click += new System.EventHandler(this.btnDelStudent_Click);
            this.cmbTreeview.SelectedIndexChanged += new System.EventHandler(this.cmbTreeview_SelectedIndexChanged);

            // tabUser events.
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            this.txtUsername.Leave += new System.EventHandler(this.txtUsername_Leave);
            this.txtPassword.Leave += new System.EventHandler(this.txtPassword_Leave);
            this.txtPassconf.Leave += new System.EventHandler(this.txtPassconf_Leave);
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            this.listUsers.SelectedIndexChanged += new System.EventHandler(this.listUsers_SelectedIndexChanged);
            this.txtEmail.Leave += new System.EventHandler(this.txtEmail_Leave);
            this.txtEmailconf.Leave += new System.EventHandler(this.txtEmailconf_Leave);
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);

            cmbPerms.SelectedIndex = 0;
            cmbState.Text = "MO";

            // Disables new user form and user delete if user is not administrator.
            if (perms != 1)
            {
                tcNewUser.Enabled = false;
                btnDelete.Enabled = false;
            }

            viewStudent(0);
            updateUserPage(currentUser);

            statY = pnlgMale.Location.Y;
            statHeight = pnlgMale.Height;
            updateStatistics();

            readToArray(Path.Combine(specificFolder, "log.fbla"), log, "NA");
            updateLog();

            lblLocal.Text = Application.StartupPath;
            lblData.Text = specificFolder;

        }

        private void lblActiveStudents_Click(object sender, EventArgs e)
        {
            if (lblActiveStudents.Text.Substring(0, 3) == "Act")
            {
                lblActiveStudents.Text = "Non-Active Students: " + aNo.ToString();
            } else
            {
                lblActiveStudents.Text = "Active Students: " + aYes.ToString();
            }
        }

        void updateLog()
        {
            listLog.Items.Clear();
            readToArray(Path.Combine(specificFolder, "log.fbla"), log, "NA");
            for (int i = 0; i < log.GetLength(0); i++)
            {
                if (log[i, 0] == null) break;
                listLog.Items.Add(log[i, 0] + " " + log[i, 1] + " : " + log[i, 2] + " : " + log[i, 4]);
            }
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (logClearConf.ShowDialog() == DialogResult.Yes)
            {
                File.Delete(Path.Combine(specificFolder, "log.fbla"));
                File.Create(Path.Combine(specificFolder, "log.fbla")).Dispose();
                log.Clear();
                msgbox msg = new msgbox("Successfully cleared the log.", "Success", 1);
                msg.Show();
                listLog.Items.Clear();
            }
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            updateLog();
        }

        string logEvent(string str)
        {
            switch (str)
            {
                case "[D]": return "deleted";
                case "[A]": return "added";
                case "[E]": return "edited";
            }
            return "changed";
        }

        private void listLog_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = listLog.SelectedIndex;

            try
            {
                // Show extra log information.
                msgbox msg = new msgbox("'" + log[index, 1] + "' " + logEvent(log[index, 0])
                    + " the " + log[index, 2] + " '" + log[index, 3]
                    + "' on " + log[index, 4] + " at " + log[index, 5]
                    + ".", "Log Info", 1);
                msg.Show();
            }
            catch { }
        }

        private void btnCreateReport_Click(object sender, EventArgs e)
        {
            var wb = new cXML.XLWorkbook();
            var ws = wb.Worksheets.Add("Members");

            string title = "Citadel Report";

            if (cmbReportState.Text != "All") title += " - " + cmbReportState.Text;

            //Title
            ws.Cell("B2").Value = title;

            // Headers
            ws.Cell("B3").Value = "Mem #";
            ws.Cell("C3").Value = "FName";
            ws.Cell("D3").Value = "LName";
            ws.Cell("E3").Value = "Joined";
            ws.Cell("F3").Value = "Grade";
            ws.Cell("G3").Value = "Due";

            int skipped = 0;
            int total = 0;
            int owing = 0;
            int active = 0;
            Double fees = 0;
            Double _temp;

            // Populate the spreadsheet.
            for (int i = 4; i - 4 < students.GetLength(0); i++)
            {
                if (students[i - 4, 0] == null) break;
                if ((students[i - 4, 11] == cmbReportState.Text || cmbReportState.Text == "All") && rbAll.Checked || (rbOwing.Checked && students[i-4, 3] != "$0.00") || (rbPaid.Checked && students[i - 4, 3] == "$0.00"))
                {
                    ws.Cell("B" + (i - skipped).ToString()).Value = students[i - 4, 0]; // Member Numbers
                    ws.Cell("C" + (i - skipped).ToString()).Value = students[i - 4, 1]; // First Names
                    ws.Cell("D" + (i - skipped).ToString()).Value = students[i - 4, 2]; // Last Names
                    ws.Cell("E" + (i - skipped).ToString()).Value = students[i - 4, 4]; // Year Joined
                    ws.Cell("F" + (i - skipped).ToString()).Value = students[i - 4, 7]; // Grade
                    ws.Cell("G" + (i - skipped).ToString()).Value = students[i - 4, 3]; // Amount Due
                    if (students[i - 4, 3] != "$0.00") owing++;
                    if (students[i - 4, 5] == "1") active++;
                    Double.TryParse(students[i - 4, 3].Trim('$'), out _temp);
                    fees += _temp;
                    total++;
                } else
                {
                    skipped++;
                }
            }

            // Members Owing
            ws.Cell("C" + (studentLength + 4 - skipped).ToString()).Value = "Owing:";
            ws.Cell("C" + (studentLength + 4 - skipped).ToString()).Style.Font.SetBold();

            ws.Cell("D" + (studentLength + 4 - skipped).ToString()).Value = owing.ToString() + " of " + total.ToString();
            ws.Cell("D" + (studentLength + 4 - skipped).ToString()).Style.Alignment.Horizontal = cXML.XLAlignmentHorizontalValues.Center;

            // Active Members
            ws.Cell("C" + (studentLength + 5 - skipped).ToString()).Value = "Active:";
            ws.Cell("C" + (studentLength + 5 - skipped).ToString()).Style.Font.SetBold();

            ws.Cell("D" + (studentLength + 5 - skipped).ToString()).Value = active.ToString() + " of " + total.ToString();
            ws.Cell("D" + (studentLength + 5 - skipped).ToString()).Style.Alignment.Horizontal = cXML.XLAlignmentHorizontalValues.Center;

            // Total Fees due
            ws.Cell("F" + (studentLength + 4 - skipped).ToString()).Value = "Total:";
            ws.Cell("F" + (studentLength + 4 - skipped).ToString()).Style.Font.SetBold();

            ws.Cell("G" + (studentLength + 4 - skipped).ToString()).Value = "$" + fees.ToString();
            ws.Cell("G" + (studentLength + 4 - skipped).ToString()).Style.Alignment.Horizontal = cXML.XLAlignmentHorizontalValues.Center;

            //From worksheet
            var rngTable = ws.Range("B2:G" + (studentLength + 5 - skipped).ToString());

            rngTable.FirstCell().Style
                .Font.SetBold()
                .Fill.SetBackgroundColor(cXML.XLColor.CornflowerBlue)
                .Alignment.SetHorizontal(cXML.XLAlignmentHorizontalValues.Center);

            rngTable.FirstRow().Merge();

            var num = rngTable.Range("A1:A" + (studentLength + 2 - skipped).ToString());
            num.Style.Alignment.Horizontal = cXML.XLAlignmentHorizontalValues.Center;

            var due = rngTable.Range("F1:F" + (studentLength + 2 - skipped).ToString());
            due.Style.Alignment.Horizontal = cXML.XLAlignmentHorizontalValues.Center;

            var rngHeaders = rngTable.Range("A2:F2"); // The address is relative to rngTable (NOT the worksheet)
            rngHeaders.Style.Alignment.Horizontal = cXML.XLAlignmentHorizontalValues.Center;
            rngHeaders.Style.Font.Bold = true;
            rngHeaders.Style.Font.FontColor = cXML.XLColor.DarkBlue;
            rngHeaders.Style.Fill.BackgroundColor = cXML.XLColor.Aqua;

            var rngData = ws.Range("B3:G" + (studentLength + 5 - skipped).ToString());
            var excelTable = rngData.CreateTable();

            excelTable.ShowTotalsRow = true;

            //Add thick borders to the contents of the spreadsheet
            ws.RangeUsed().Style.Border.OutsideBorder = cXML.XLBorderStyleValues.Thick;

            ws.Columns().AdjustToContents();

            wb.SaveAs(Path.Combine(Application.StartupPath, "Report" + reportSuffix() + ".xlsx"));

            msgbox msg = new msgbox("Successfully generated report in program's local directory.", "Success", 1);
            msg.Show();
        }

        string reportSuffix()
        {
            if (rbAll.Checked)
            {
                return "All";
            } else if (rbOwing.Checked)
            {
                return "Owing";
            } else if (rbPaid.Checked)
            {
                return "Paid";
            }
            return "1";
        }

        private void tvStudents_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                delStudent();
            }
        }

        private void btnCreateSenior_Click(object sender, EventArgs e)
        {
            var wb = new cXML.XLWorkbook();
            var ws = wb.Worksheets.Add("Members");

            string title = "Senior Report";

            //Title
            ws.Cell("B2").Value = title;

            // Headers
            ws.Cell("B3").Value = "State";
            ws.Cell("C3").Value = "FName";
            ws.Cell("D3").Value = "LName";
            ws.Cell("E3").Value = "Email";

            int skipped = 0;
            int total = 0;

            // Populate the spreadsheet.
            foreach (string item in cmbReportState.Items)
            {
                skipped = 0;
                for (int i = 4; i - 4 < students.GetLength(0); i++)
                {
                    if (students[i - 4, 0] == null) break;
                    if (students[i - 4, 7] == "12" && students[i - 4, 11] == item)
                    {
                        ws.Cell("B" + (i + total - skipped).ToString()).Value = students[i - 4, 11]; // State
                        ws.Cell("C" + (i + total - skipped).ToString()).Value = students[i - 4, 1]; // First Names
                        ws.Cell("D" + (i + total - skipped).ToString()).Value = students[i - 4, 2]; // Last Names
                        ws.Cell("E" + (i + total - skipped).ToString()).Value = students[i - 4, 9]; // Emails
                        total++;
                    } else
                    {
                        skipped++;
                    }
                }
            }

            skipped = studentLength - total;

            //From worksheet
            var rngTable = ws.Range("B2:E" + (studentLength + 5 - skipped).ToString());

            rngTable.FirstCell().Style
                .Font.SetBold()
                .Fill.SetBackgroundColor(cXML.XLColor.CornflowerBlue)
                .Alignment.SetHorizontal(cXML.XLAlignmentHorizontalValues.Center);

            rngTable.FirstRow().Merge();

            var rngHeaders = rngTable.Range("A2:D2"); // The address is relative to rngTable (NOT the worksheet)
            rngHeaders.Style.Alignment.Horizontal = cXML.XLAlignmentHorizontalValues.Center;
            rngHeaders.Style.Font.Bold = true;
            rngHeaders.Style.Font.FontColor = cXML.XLColor.DarkBlue;
            rngHeaders.Style.Fill.BackgroundColor = cXML.XLColor.Aqua;

            var rngData = ws.Range("B3:E" + (studentLength + 5 - skipped).ToString());
            var excelTable = rngData.CreateTable();

            excelTable.ShowTotalsRow = true;

            //Add thick borders to the contents of the spreadsheet
            ws.RangeUsed().Style.Border.OutsideBorder = cXML.XLBorderStyleValues.Thick;

            ws.Columns().AdjustToContents();

            wb.SaveAs(Path.Combine(Application.StartupPath, "SeniorReport.xlsx"));

            msgbox msg = new msgbox("Successfully generated senior report in program's local directory.", "Success", 1);
            msg.Show();
        }

        private void lblLocal_Click(object sender, EventArgs e)
        {
            try
            {
                TextBox textbox = sender as TextBox;
                Process.Start(textbox.Text);
            }
            catch { }
        }

        private void lblData_Click(object sender, EventArgs e)
        {
            try
            {
                TextBox textbox = sender as TextBox;
                Process.Start(textbox.Text);
            }
            catch { }
        }

        private void btnRefreshStats_Click(object sender, EventArgs e)
        {
            updateStatistics();
        }

        void delete(string contains, string path, bool decrypt)
        {
            // Create temporary file.
            var tempFile = Path.GetTempFileName();
            // Create an array of lines to keep if it doesn't contain the selected student.
            IEnumerable<string> linesToKeep;
            if (decrypt)
            {
                linesToKeep = File.ReadLines(path).Where(l => !(rformLogin.Decrypt(l).Contains(contains)));
            }
            else
            {
                linesToKeep = File.ReadLines(path).Where(l => !(l.Contains(contains)));
            }

            // Write the kept lines to the temporary file.
            File.WriteAllLines(tempFile, linesToKeep);

            // Delete current file.
            File.Delete(path);

            //Replace old file with temporary file.
            File.Move(tempFile, path);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            readToArray(Path.Combine(specificFolder, "data", "students.fbla"), students, "NA");
            refreshStudentTree("");
        }
    }
}
