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

namespace Citadel
{
    public partial class formMain : Form
    {


        string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData); // %APPDATA% path
        int currentUser;       // Usernumber of the logged in user.
        int perms;             // Used to recognize permissions of a user, only 1 = administrator.
        string specificFolder; // A string used to store the Citadel folder in appData.
        Panel activePanel;     // Panel used to find the height to move indicator.
        public static String[,] students = new String[50, 10]; // 2D array that stores students.
        int currentView;       // The Student that is currently being viewed
        int studentLength;     // Number of valid student accounts.

        // Saves the panel tab buttons with their respective display panels.
        Dictionary<Panel, Panel> displays = new Dictionary<Panel, Panel>();
        // Saves the heights necessarry to move the indicator to for a given panel.
        Dictionary<Panel, Point> heights = new Dictionary<Panel, Point>();
        // Saves the student number with its respective property.
        Dictionary<String, int> treeProps = new Dictionary<String, int>();

        public formMain(int user, int _perms)
        {
            InitializeComponent();
            currentUser = user;
            perms = _perms;
        }

        msgbox logConf = new msgbox("Are you sure you wish to exit Citadel?", "Exit", 2);
        bool exit = true;

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
                int j = 1;
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
                    array2d[i, 0] = studentLength.ToString();
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

        void refreshStudentTree()
        {
            tvStudents.Invoke((MethodInvoker)(() => tvStudents.Nodes.Clear()));

            // Add students to the student tab tree view.
            TreeNode student;
            TreeNode display1;
            TreeNode display2;
            TreeNode[] studentChildren;
            for (int i = 0; i < students.GetLength(0); i++)
            {
                if (students[i, 4] == null) break;
                display1 = new TreeNode(cmbTreeview.Text + ": " + students[i, treeProps[cmbTreeview.Text]] + " ");
                display2 = new TreeNode("Grade: " + students[i, 7] + " ");
                studentChildren = new TreeNode[] { display2, display1 };
                student = new TreeNode(students[i, 1] + " " + students[i, 2] + " - " + students[i, 0], studentChildren);
                tvStudents.Invoke((MethodInvoker)(() => tvStudents.Nodes.Add(student)));
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
            refreshStudentTree();

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
            if (_nodeText[_nodeText.Length - 1] != ' ')
            {
                int _memNum = Convert.ToInt32(_nodeText[_nodeText.Length - 1].ToString());
                viewStudent(_memNum - 1);
            }
        }

        private void btnCopyQf_Click(object sender, EventArgs e)
        {
            string _temp = "";
            for (int i = 0; i < 11; i++)
            {
                _temp += students[currentView, i];
                if (i != 10)
                {
                    _temp += "\\";
                }
            }
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
            txtNewComment.Enabled = enable;
        }

        msgbox _conf;
        // Don't prompt user on first time creating new user.
        bool firstNew = true;

        void newStudent()
        {
            _conf = new msgbox("Creating a new user will erase any filled in data.", "Erase Data?", 2);
            if (!firstNew)
            {
                _conf.ShowDialog();
            }
            if (_conf.DialogResult == DialogResult.Yes || firstNew)
            {
                firstNew = false;
                clearNewStudent();
                if (!txtNewFirst.Enabled)
                {
                    enableNewStudent(true);
                    lblNewMemNum.Text = "#" + (studentLength + 1).ToString();
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

        // 0 = Member #, 1 = First Name, 2 = Last Name, 3 = Fees
        // 4 = Year Joined, 5 = Active, 6 = Gender, 7 = Grade
        // 8 = School, 9 = Email, 10 = Comments

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (txtNewFirst.Text != "" && txtNewLast.Text != "" && txtNewSchool.Text != "" && txtNewEmail.Text != "")
            {
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
                string _temp = txtNewFirst.Text + '\\' + txtNewLast.Text + '\\' + txtNewFees.Text 
                    + '\\' + nmNewYear.Text + '\\' + _active + '\\' + _gender + '\\' + _grade
                    + '\\' + txtNewSchool.Text + '\\' + txtNewEmail.Text + '\\' + txtComment.Text;

                try
                {
                    File.AppendAllText(specificFolder + "/data/students.fbla", _temp + "\r\n");
                    studentLength++;
                    readToArray(specificFolder + "/data/students.fbla", students, "NA");
                    rformLogin.message("Successfully added " + txtNewFirst.Text + " " + txtNewLast.Text + ".", "Success", 1);
                    refreshStudentTree();
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

        Double feeDbl = 0;

        private void txtNewFees_Leave(object sender, EventArgs e)
        {
            Double.TryParse(txtNewFees.Text, out feeDbl);
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
            if (txtNewFees.Text.Contains("$"))
            {
                txtNewFees.Text = feeDbl.ToString();
            }
        }

        public static System.Timers.Timer tmrQckAdd = new System.Timers.Timer();

        public static Form quickAdd = null;

        private void btnQuickAdd_Click(object sender, EventArgs e)
        {
            tmrQckAdd.Start();
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
            if (formQuickAdd.added)
            {
                readToArray(specificFolder + "/data/students.fbla", students, "NA");
                refreshStudentTree();
                formQuickAdd.added = false;
            }
        }

        private void cmbTreeview_SelectedIndexChanged(object sender, EventArgs e)
        {
            refreshStudentTree();
        }

        private void btnNew1_Click(object sender, EventArgs e)
        {
            tcNewStudent.SelectedTab = tabPage2;
            newStudent();
        }

        private void btnDelStudent_Click(object sender, EventArgs e)
        {
            msgbox logConf = new msgbox("Are you sure you want to delete " + students[currentView, 1] + " " + students[currentView, 2] + "?", "Delete", 2);
            logConf.ShowDialog();
            if (logConf.DialogResult == DialogResult.Yes) {
                string _contains = students[currentView, 1] + '\\' + students[currentView, 2] + '\\'
                    + students[currentView, 3] + '\\';
                delete(_contains, specificFolder + "/data/students.fbla", false);
                rformLogin.message("Successfully deleted " + students[currentView, 1] + " " + students[currentView, 2] + ".", "Success", 1);
                readToArray(specificFolder + "/data/students.fbla", students, "NA");
                refreshStudentTree();
            }
        }
    }
}
