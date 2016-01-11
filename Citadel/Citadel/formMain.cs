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
        bool logout = false;   // Checks if application needs to exit or only form.
        Panel activePanel;     // Panel used to find the height to move indicator.
        bool _emails = false;  // Checks that both emails have been filled out in new user form.
        bool _pass = false;    // Checks that both passwords have been filled out in new user form.

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

        // Changes selected user information to match
        // a given user number.
        void updateUserPage(int user)
        {
            lblFirstname.Text = rformLogin.users[user, 2];
            lblLastname.Text = rformLogin.users[user, 3];
            lblEmail.Text = rformLogin.users[user, 4];
            lblCuruser.Text = getUser(user);

            gbTitle(gbCuruser, lblCuruser);
            gbTitle(gbCuruser, lblEmail);

            int _x = gbCuruser.Width - lblFirstname.Width - 15;
            int _x2 = gbCuruser.Width - lblLastname.Width - 15;
            lblFirstname.Location = new Point(_x, lblFirstname.Location.Y);
            lblLastname.Location = new Point(_x2, lblLastname.Location.Y);
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
                if (rformLogin.users[i, 0] == null) break;
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

            cmbPerms.SelectedIndex = 0;

            // Disables new user form and user delete if user is not administrator.
            if (perms != 1)
            {
                gbNewuser.Enabled = false;
                btnDelete.Enabled = false;
            }

            updateUserPage(currentUser);
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            rformLogin _login = new rformLogin();
            _login.Show();
            logout = true;
            this.Close();
        }

        // Checks if the given fields meet their requirements.
        bool userOk, passOk, emailOk;

        private void txtUsername_Leave(object sender, EventArgs e)
        {
            // Checks that username is not already taken.
            if (txtUsername.Text.Length >= 5 && txtUsername.Text != "Username")
            {
                for (int i = 0; i < rformLogin.users.GetLength(0); i++)
                {

                    if (rformLogin.users[i, 0] == null)
                    {
                        confBox(npnlUser, Color.DodgerBlue);
                        userOk = true;
                        break;
                    }
                    else if (txtUsername.Text == getUser(i))
                    {
                        confBox(npnlUser, Color.Red);
                        userOk = false;
                        break;
                    }
                }
            }
        }

        private void txtPassword_Leave(object sender, EventArgs e)
        {
            // Checks that password is greater than 5 characters.
            if (txtPassword.Text.Length >= 5)
            {
                confBox(npnlPass, Color.DodgerBlue);
                ttMaster.Hide(txtPassword);
            } else
            {
                confBox(npnlPass, Color.Red);
                ttMaster.Show("Passwords must be at least 5 characters long.", txtPassword);
            }
            if (_pass)
            {
                if (txtPassconf.Text == txtPassword.Text && txtPassword.Text.Length >= 5)
                {
                    confBox(npnlPassconf, Color.DodgerBlue);
                    passOk = true;
                }
                else
                {
                    confBox(npnlPassconf, Color.Red);
                    passOk = false;
                }
            }
        }

        private void txtPassconf_Leave(object sender, EventArgs e)
        {
            // Checks that password fields match.
            if (txtPassconf.Text == txtPassword.Text && txtPassword.Text.Length >= 5)
            {
                confBox(npnlPassconf, Color.DodgerBlue);
                passOk = true;
            } else
            {
                confBox(npnlPassconf, Color.Red);
                passOk = false;
            }
            _pass = true;
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Removes the selected item from the
            // user file, user array, and listbox.
            if (listUsers.SelectedItem != null)
            {
                int _userNum = rformLogin.userNums[listUsers.SelectedItem.ToString()];
                delete(rformLogin.users[_userNum, 0] + '\\' + rformLogin.users[_userNum, 1] + '\\' + rformLogin.users[_userNum, 2], specificFolder + "/users.fbla", true);
                rformLogin.users[rformLogin.userNums[listUsers.SelectedItem.ToString()], 1] = rformLogin.Encrypt("disable");
                updateUserPage(currentUser);
                listUsers.Items.Remove(listUsers.SelectedItem);
            }
        }

        private void listUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Displays user information when clicking
            // on a user in user list.
            try
            {
                updateUserPage(rformLogin.userNums[listUsers.SelectedItem.ToString()]);
            } catch { }
        }

        private void txtEmail_Leave(object sender, EventArgs e)
        {
            // Checks that email is filled out correctly.
            if (_emails)
            {
                if (txtEmail.Text != txtEmailconf.Text)
                {
                    confBox(npnlEmailconf, Color.Red);
                    confBox(npnlEmail, Color.Red);
                    emailOk = false;
                }
                else
                {
                    confBox(npnlEmailconf, Color.DodgerBlue);
                    confBox(npnlEmail, Color.DodgerBlue);
                    emailOk = true;
                }
            }
        }

        private void txtEmailconf_Leave(object sender, EventArgs e)
        {
            // Checks that emails match.
            if (txtEmail.Text != txtEmailconf.Text)
            {
                confBox(npnlEmailconf, Color.Red);
                confBox(npnlEmail, Color.Red);
                emailOk = false;
            } else
            {
                confBox(npnlEmailconf, Color.DodgerBlue);
                confBox(npnlEmail, Color.DodgerBlue);
                emailOk = true;
            }
            _emails = true;
        }

        void writeUser(string user, string pass, string first, string last, string email)
        {
            // Writes the text to the user file.
            File.AppendAllText(specificFolder + "/users.fbla", rformLogin.Encrypt(user + '\\' + pass + '\\' + first + '\\' + last + '\\' + email + "\r\n"));

            // Sets the user number for the new user and the
            // username without the permission number.
            int newUserNum = rformLogin.userNums.Count() + 1;
            string rawUser = user.Substring(0, user.Length - 1);     
                 
            // Adds the new user to the users array and listbox.
            listUsers.Items.Add(rawUser);
            rformLogin.userNums.Add(rawUser, newUserNum);
            rformLogin.users[newUserNum, 0] = user; rformLogin.users[newUserNum, 1] = pass; rformLogin.users[newUserNum, 2] = first;
            rformLogin.users[newUserNum, 3] = last; rformLogin.users[newUserNum, 4] = email;
            rformLogin.message(txtUsername.Text + " was successfully created!", "Success", 1, -1);
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            // Creates the new user if all fields throw no errors.
            if (userOk && passOk && emailOk && txtFirstname.Text !=
                rformLogin.placeText[txtFirstname] && txtLastname.Text != rformLogin.placeText[txtLastname])
            {
                writeUser(txtUsername.Text + cmbPerms.SelectedIndex.ToString(), txtPassconf.Text, txtFirstname.Text, txtLastname.Text, txtEmail.Text);
            } else
            {
                rformLogin.message("Please make sure all entries are complete and correct.", "Error", 1, -1);
            }
        }

        private void btnCopyQf_MouseEnter(object sender, EventArgs e)
        {
            btnCopyQf.BackColor = Color.DodgerBlue;
        }

        private void btnCopyQf_MouseLeave(object sender, EventArgs e)
        {
            btnCopyQf.BackColor = Color.FromArgb(35,35,35);
        }
    }
}
