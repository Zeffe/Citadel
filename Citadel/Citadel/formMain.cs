﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Citadel
{
    public partial class formMain : Form
    {


        string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData); // %APPDATA% path
        int currentUser;       // Usernumber of the logged in user.
        int perms;             // Used to recognize permissions of a user, only 1 = administrator.
        string specificFolder; // A string used to store the Citadel folder in appData.
        Panel activePanel;     // Panel used to find the height to move indicator.
        public static String[,] students = new String[50, 11]; // 2D array that stores students.
        int currentView;       // The Student that is currently being viewed
        int studentLength;     // Number of valid student accounts.
        int searchFor = 0;     // Value used to determine the 2nd dimension to filter by.
        bool exit = true;      // Determine if the entire programming is exiting, or only the form.
        bool firstNew = true;  // Determines whether the user has filled in data on the new student tab.
        Double feeDbl = 0;     // Used as storage for the new student fee double value.
        bool editing = false;  // Determines if the user is editing a user or creating a new one.
        int editStudent;       // Determines the student being edited.

        // Saves the panel tab buttons with their respective display panels.
        Dictionary<Panel, Panel> displays = new Dictionary<Panel, Panel>();
        // Saves the heights necessarry to move the indicator to for a given panel.
        Dictionary<Panel, Point> heights = new Dictionary<Panel, Point>();
        // Saves the student number with its respective property.
        Dictionary<String, int> treeProps = new Dictionary<String, int>();

        // Confirmation box that is displayed when attempting to exit.
        msgbox logConf = new msgbox("Are you sure you wish to exit Citadel?", "Exit", 2);
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
                    pnl.BackColor = Color.FromArgb(50, 50, 50);
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
                if (students[i, searchFor].Contains(contains) || contains == "") {
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
            gbTitle(gbUserlist, btnDelete);
            gbTitle(gbCuruser, btnLogout);
            gbTitle(pnlStudents, lblReadingFrom);
            gbTitle(pnlStudents, lblStudentsTitle);

            // Sets the text color of group boxes to white.
            gbNewuser.ForeColor = Color.White;
            gbUserlist.ForeColor = Color.White;
            gbSearch.ForeColor = Color.White;
            gbStudentList.ForeColor = Color.White;
            gbNewStudent.ForeColor = Color.White;
            gbVersion.ForeColor = Color.White;
            gbProjSumm.ForeColor = Color.White;

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
            nmNewYear.Maximum = DateTime.Now.Year + 1;
            nmNewYear.Value = DateTime.Now.Year;

            // Read the students data file into an array.
            readToArray(specificFolder + "/data/students.fbla", students, "NA");

            // Adds all users to the user list on user page.
            for (int i = 0; i < rformLogin.users.GetLength(0); i++)
            {
                if (rformLogin.users[i, 1] == null) break;
                listUsers.Items.Add(getUser(i));
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

            // Disables new user form and user delete if user is not administrator.
            if (perms != 1)
            {
                gbNewuser.Enabled = false;
                btnDelete.Enabled = false;
            }

            viewStudent(0);
            updateUserPage(currentUser);
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

        private void pnlStudents_LocationChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < rformLogin.users.GetLength(0); i++)
            {
                if (rformLogin.users[i, 1] == null) break;
                listUsers.Items.Add(getUser(i));
            }
        }

        bool gender = false; bool active = false; int grade = 9;

        void changeGender()
        {
            if (txtFirstname.Enabled)
            {
                if (!gender)
                {
                    pbGenderSel.BackgroundImage = global::Citadel.Properties.Resources.female128;
                    ttMaster.SetToolTip(pbGenderSel, "Female");
                }
                else
                {
                    pbGenderSel.BackgroundImage = global::Citadel.Properties.Resources.male244;
                    ttMaster.SetToolTip(pbGenderSel, "Male");
                }

                gender = !gender;
            }
        }

        void changeActive()
        {
            if (txtFirstname.Enabled)
            {
                if (!active)
                {
                    pbActiveSel.BackgroundImage = global::Citadel.Properties.Resources.cancel30;
                    ttMaster.SetToolTip(pbActiveSel, "Not Active");
                }
                else
                {
                    pbActiveSel.BackgroundImage = global::Citadel.Properties.Resources.checked21;
                    ttMaster.SetToolTip(pbActiveSel, "Active");
                }

                active = !active;
            }
        }

        void changeGrade(int _grade)
        {
            if (txtFirstname.Enabled)
            {
                switch (_grade)
                {
                    case 9:
                        lblGradeSel.Text = "9";
                        ttMaster.SetToolTip(lblGradeSel, "Freshman");
                        break;
                    case 10:
                        lblGradeSel.Text = "10";
                        ttMaster.SetToolTip(lblGradeSel, "Sophomore");
                        break;
                    case 11:
                        lblGradeSel.Text = "11";
                        ttMaster.SetToolTip(lblGradeSel, "Junior");
                        break;
                    case 12:
                        lblGradeSel.Text = "12";
                        ttMaster.SetToolTip(lblGradeSel, "Senior");
                        break;
                    case 13:
                        lblGradeSel.Text = "13+";
                        ttMaster.SetToolTip(lblGradeSel, "College Level");
                        break;
                }
            }
        }

        private void btnGenderNext_Click(object sender, EventArgs e)
        {
            changeGender();
        }

        private void btnGenderPrev_Click(object sender, EventArgs e)
        {
            changeGender();
        }

        private void btnActiveNext_Click(object sender, EventArgs e)
        {
            changeActive();
        }

        private void btnActivePrev_Click(object sender, EventArgs e)
        {
            changeActive();
        }

        private void btnGradeNext_Click(object sender, EventArgs e)
        {
            if (grade != 13)
            {
                grade++;
            }
            else
            {
                grade = 9;
            }
            changeGrade(grade);
        }

        private void btnGradePrev_Click(object sender, EventArgs e)
        {
            if (grade != 9)
            {
                grade--;
            }
            else
            {
                grade = 13;
            }
            changeGrade(grade);
        }

        void viewStudent(int studentNum)
        {
            try
            {
                // 0 = Member #, 1 = First Name, 2 = Last Name, 3 = Fees
                // 4 = Year Joined, 5 = Active, 6 = Gender, 7 = Grade
                // 8 = School, 9 = Email, 10 = Comments

                tcNewStudent.SelectedTab = tabPage1;
                currentView = studentNum;
                txtMemberNum.Text = students[studentNum, 0];
                txtFullName.Text = students[studentNum, 1] + " " + students[studentNum, 2];
                txtFees.Text = students[studentNum, 3];
                txtYearJoined.Text = students[studentNum, 4];
                switch (Convert.ToInt32(students[studentNum, 5]))
                {
                    case 0:
                        pbNotActive.BackColor = Color.DodgerBlue;
                        pbActive.BackColor = pbActive.Parent.BackColor;
                        break;
                    case 1:
                        pbActive.BackColor = Color.DodgerBlue;
                        pbNotActive.BackColor = pbNotActive.Parent.BackColor;
                        break;
                }
                switch (Convert.ToInt32(students[studentNum, 6]))
                {
                    case 0:
                        pbFemale.BackColor = Color.DodgerBlue;
                        pbMale.BackColor = pbMale.Parent.BackColor;
                        break;
                    case 1:
                        pbMale.BackColor = Color.DodgerBlue;
                        pbFemale.BackColor = pbFemale.Parent.BackColor;
                        break;
                }

                lblGrade.Text = students[studentNum, 7];
                lblSchool.Text = "School: " + students[studentNum, 8];
                lblEmail2.Text = "Email: " + students[studentNum, 9];
                txtComment.Text = students[studentNum, 10];
            }
            catch { }
        }

        private void tvStudents_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string _nodeText = e.Node.Text;

            // Check that the selected node is the parent node.
            if (_nodeText[_nodeText.Length - 1] != ' ')
            {
                // Get the membernum by looking at the last character of the node.
                int _memNum = Convert.ToInt32(_nodeText[_nodeText.Length - 1].ToString());

                for (int i = 0; i < students.GetLength(0); i++)
                {
                    if (students[i, 0] == null) break;
                    if (Convert.ToInt32(students[i, 0]) == _memNum)
                    {
                        viewStudent(i);
                    }
                }
            }
        }

        private void btnCopyQf_Click(object sender, EventArgs e)
        {
            string _temp = "";

            // Generate the necesarry quickFind string.
            for (int i = 0; i < 11; i++)
            {
                _temp += students[currentView, i];
                if (i != 10)
                {
                    _temp += "\\";
                }
            }

            // Copy the string to the clipboard.
            Clipboard.SetText(_temp);
            rformLogin.message("Successfully copied " + students[currentView, 1] + " " + students[currentView, 2] + " to clipboard!", "Success", 1);
        }

        void clearNewStudent()
        {
            txtNewFirst.Clear(); txtNewLast.Clear(); txtNewSchool.Clear();
            txtNewEmail.Clear(); txtNewFees.Text = "$0.00"; txtNewComment.Clear();
        }

        void enableNewStudent(bool enable)
        {
            txtNewFirst.Enabled = enable; txtNewLast.Enabled = enable; txtNewSchool.Enabled = enable;
            txtNewEmail.Enabled = enable; nmNewYear.Enabled = enable; txtNewFees.Enabled = enable;
            txtNewComment.Enabled = enable; nmNewMemNum.Enabled = enable;
        }

        void newStudent()
        {
            _conf = new msgbox("Creating a new user will erase any filled in data.", "Erase Data?", 2);
            if (!firstNew)
            {
                _conf.ShowDialog();
            }
            if (_conf.DialogResult == DialogResult.Yes || firstNew)
            {
                editing = false;
                firstNew = false;
                clearNewStudent();
                if (!txtNewFirst.Enabled)
                {
                    enableNewStudent(true);
                    // Generate next member number in the chain (Probably temporary).
                    nmNewMemNum.Value = Convert.ToDecimal(studentLength + 1);
                }
            }
        }

        private void btnNew2_Click(object sender, EventArgs e)
        {
            newStudent();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            _conf = new msgbox("Are you sure you want to clear all data?", "Erase Data?", 2);
            _conf.ShowDialog();
            if (_conf.DialogResult == DialogResult.Yes)
            {
                clearNewStudent();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Check that all required fields have been filled.
            if (txtNewFirst.Text != "" && txtNewLast.Text != "" && txtNewSchool.Text != "" && txtNewEmail.Text != "")
            {

                if (editing)
                {
                    int _student = editStudent;
                    string _del = "";
                    for (int i = 0; i < 5; i++)
                    {
                        _del += students[_student, i];
                        _del += "\\";
                    }
                    delete(_del, specificFolder + "/data/students.fbla", false);
                }

                // Temporary strings for parsing the integer values to strings.
                string _active = ""; string _gender = ""; string _grade;
                switch(!active)
                {
                    case true: _active = "1"; break;
                    case false: _active = "0"; break;
                }
                switch (!gender)
                {
                    case true: _gender = "1"; break;
                    case false: _gender = "0"; break;
                }
                if (lblGradeSel.Text != "13+")
                {
                    _grade = lblGradeSel.Text;
                } else
                {
                    _grade = "13";
                }

                // The string that will be saved.
                string _temp = nmNewMemNum.Text + '\\' + txtNewFirst.Text + '\\' + txtNewLast.Text + '\\' + txtNewFees.Text 
                    + '\\' + nmNewYear.Text + '\\' + _active + '\\' + _gender + '\\' + _grade
                    + '\\' + txtNewSchool.Text + '\\' + txtNewEmail.Text + '\\' + txtNewComment.Text;

                try
                {
                    // Write the student to the source.
                    File.AppendAllText(specificFolder + "/data/students.fbla", _temp + "\r\n");

                    // Add the new student to the array.
                    studentLength++;
                    readToArray(specificFolder + "/data/students.fbla", students, "NA");
                    refreshStudentTree("");
                    if (!editing)
                    {
                        rformLogin.message("Successfully added " + txtNewFirst.Text + " " + txtNewLast.Text + ".", "Success", 1);
                    }

                    // Reset the new student form.
                    editing = false;
                    clearNewStudent();
                    enableNewStudent(false);
                    firstNew = true;       
                }
                catch
                {
                    rformLogin.message("Error writing student to the selected source.", "Error", 1);
                }
            }
            else
            {
                rformLogin.message("Please ensure that all of the required entries have been filled in.", "Error", 1);
            }
        }

        private void txtNewFees_Leave(object sender, EventArgs e)
        {
            Double.TryParse(txtNewFees.Text, out feeDbl);

            // Format the number, so the user doesn't have to.
            if (!txtNewFees.Text.Contains("$"))
            {
                txtNewFees.Text = "$" + txtNewFees.Text;
            }
            if (!txtNewFees.Text.Contains("."))
            {
                txtNewFees.Text += ".00";
            }
            String[] temp = txtNewFees.Text.Split('.');
            if (temp[1].Length == 1)
            {
                temp[1] += "0";
                txtNewFees.Text = temp[0] + "." + temp[1];
            }
        }

        private void txtNewFees_Enter(object sender, EventArgs e)
        {
            // Display only the double value of the fee.
            if (txtNewFees.Text.Contains("$"))
            {
                txtNewFees.Text = feeDbl.ToString();
            }
        }

        public static System.Timers.Timer tmrQckAdd = new System.Timers.Timer();

        public static Form quickAdd = null;

        private void btnQuickAdd_Click(object sender, EventArgs e)
        {
            // Start the timer that updates when a student is added.
            tmrQckAdd.Start();

            // Stop the user from opening multiple quickAdd forms.
            if (quickAdd != null)
            {
                quickAdd.BringToFront();
            }
            else
            {
                quickAdd = new formQuickAdd("students.fbla", (studentLength + 1).ToString());
                quickAdd.Show();
            }
        }

        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            // Check if any students have been added in formQuickAdd.
            // Refresh the students array and treeview if so.
            if (formQuickAdd.added)
            {
                readToArray(specificFolder + "/data/students.fbla", students, "NA");
                refreshStudentTree("");
                formQuickAdd.added = false;
            }
        }

        private void cmbTreeview_SelectedIndexChanged(object sender, EventArgs e)
        {
            refreshStudentTree("");
        }

        private void btnNew1_Click(object sender, EventArgs e)
        {
            // View the new student form and toggle newStudent()
            tcNewStudent.SelectedTab = tabPage2;
            newStudent();
        }

        private void btnDelStudent_Click(object sender, EventArgs e)
        {
            // Prompt the user before deleting student.
            delConf = new msgbox("Are you sure you want to delete " + students[currentView, 1] + " " + students[currentView, 2] + "?", "Delete", 2);
            delConf.ShowDialog();

            if (delConf.DialogResult == DialogResult.Yes) {
                // String used to find the student inside the student file.
                string _contains = students[currentView, 1] + '\\' + students[currentView, 2] + '\\'
                    + students[currentView, 3] + '\\';
                // Delete the line that contains _contains in the given source.
                delete(_contains, specificFolder + "/data/students.fbla", false);

                // Reset the students array and treeview.
                readToArray(specificFolder + "/data/students.fbla", students, "NA");
                refreshStudentTree("");
            }
        }

        void search(int field)
        {
            // Converts combobox data into array positions.
            switch (field)
            {
                case 0: searchFor = 2; break;
                case 1: searchFor = 1; break;
                case 2: searchFor = 4; break;
                case 3: searchFor = 0; break;
                case 4: searchFor = 9; break;
                case 5: searchFor = 8; break;
                case 6: searchFor = 7; break;
            }

            // Loop through all the students.
            for (int i = 0; i < students.GetLength(0); i++)
            {
                // Break the loop if the student has a null value.
                if (students[i, searchFor] == null) break;
                // View the student if they contain the search text.
                if (students[i, searchFor].Contains(txtSearch.Text))
                {
                    viewStudent(i);
                }
            }

        }

        void filter(int field)
        {
            // Converts combobox data into array positions.
            switch (field)
            {
                case 0: searchFor = 2; break;
                case 1: searchFor = 1; break;
                case 2: searchFor = 4; break;
                case 3: searchFor = 0; break;
                case 4: searchFor = 9; break;
                case 5: searchFor = 8; break;
                case 6: searchFor = 7; break;
                case 7: searchFor = 5; break;
                case 8: searchFor = 3; break;
            }

            // Update the treeview with every value that contains
            // the text in the txtFilter field.
            if (searchFor != 5 || searchFor != 3)
            {
                // Update the treeview with every value that contains
                // the text in the txtFilter field.
                refreshStudentTree(txtFilter.Text);
            }
            else if (searchFor == 5)
            {
                // If searching for "Is Active", search for 1, which is
                // the value of an active member.
                refreshStudentTree("1");
            }
            else if (searchFor == 3)
            {
                // Pass $0.00 if searching for "Has Fees".
                refreshStudentTree("$0.00");
            }
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            // Filter the student array using cmbFilterBy's selected
            // index in order to determine value of the 2nd dimension.
            filter(cmbFilterBy.SelectedIndex);

            // Reset searchFor in order to prevent errors when
            // creating new users.
            searchFor = 0;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            // View in the view menu instead of filtering treeview.
            search(cmbSearchBy.SelectedIndex);
            searchFor = 0;
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            // Search on enter press.
            if (e.KeyCode == Keys.Enter)
            {
                search(cmbSearchBy.SelectedIndex);
                searchFor = 0;
            }
        }

        private void txtFilter_KeyDown(object sender, KeyEventArgs e)
        {
            // Filter on enter press.
            if (e.KeyCode == Keys.Enter)
            {
                filter(cmbFilterBy.SelectedIndex);
                searchFor = 0;
            }
        }

        // 0 = Member #, 1 = First Name, 2 = Last Name, 3 = Fees
        // 4 = Year Joined, 5 = Active, 6 = Gender, 7 = Grade
        // 8 = School, 9 = Email, 10 = Comments

        void edit(int _studentNum)
        {
            tcNewStudent.SelectedTab = tabPage2;
            editing = true;
            nmNewMemNum.Value = Convert.ToDecimal(students[_studentNum, 0]);
            txtNewFirst.Text = students[_studentNum, 1];
            txtNewLast.Text = students[_studentNum, 2];
            txtNewFees.Text = students[_studentNum, 3];
            nmNewYear.Value = Convert.ToDecimal(students[_studentNum, 4]);
            if (students[_studentNum, 5] == "1" && active) changeActive();
            if (students[_studentNum, 6] == "1" && gender) changeGender();
            changeGrade(Convert.ToInt32(students[_studentNum, 7]));
            txtNewSchool.Text = students[_studentNum, 8];
            txtNewEmail.Text = students[_studentNum, 9];
            txtNewComment.Text = students[_studentNum, 10];
            enableNewStudent(true);
            editStudent = _studentNum;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            edit(currentView);
        }
    }
}
