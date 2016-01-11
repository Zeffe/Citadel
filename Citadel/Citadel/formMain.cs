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

        // Saves the panel tab buttons with their respective display panels.
        Dictionary<Panel, Panel> displays = new Dictionary<Panel, Panel>();
        // Saves the heights necessarry to move the indicator to for a given panel.
        Dictionary<Panel, Point> heights = new Dictionary<Panel, Point>();

        public formMain(int user, int _perms)
        {
            InitializeComponent();
            currentUser = user;
            perms = _perms;
        }

        private void formMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!logout)
            {
                Application.Exit();
            }
        }

        // Centers a control inside a group box.
        void gbTitle(GroupBox groupbox, Control title)
        {
            Point _temp = groupbox.DisplayRectangle.Location;
            _temp.X += (groupbox.DisplayRectangle.Width - title.Width) / 2;
            _temp.Y = title.Location.Y;
            groupbox.ForeColor = Color.White;
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

        private void formMain_Load(object sender, EventArgs e)
        {
            specificFolder = Path.Combine(folder, "Citadel"); // %APPDATA%/Citadel home path

            // Initialize active panel and bring the dashboard to front.
            activePanel = pnlbDashboard;
            pnlDashboard.BringToFront();

            // Centers and colors the user page groupboxes and controls.
            gbTitle(gbUserlist, btnDelete);
            gbTitle(gbCuruser, btnLogout);
            gbNewuser.ForeColor = Color.White;
            gbUserlist.ForeColor = Color.White;
            gbSourceinfo.ForeColor = Color.White;
            gbStudentList.ForeColor = Color.White;
            gbNewStudent.ForeColor = Color.White;

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

            // Adds all users to the user list on user page.
            for (int i = 0; i < rformLogin.users.GetLength(0); i++)
            {
                if (rformLogin.users[i, 1] == null) break;
                listUsers.Items.Add(getUser(i));
            }

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
            pbHighlight(btnView);

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

            updateUserPage(currentUser);
        }   

        void delete(string contains, string path, bool decrypt)
        {
            // Create temporary file.
            var tempFile = Path.GetTempFileName();
            // Create an array of lines to keep if it doesn't contain the selected student.
            var linesToKeep = File.ReadLines(path);
            if (decrypt)
            {
                linesToKeep = File.ReadLines(path).Where(l => !(rformLogin.Decrypt(l).Contains(contains)));
            } else
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
    }
}
