using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Citadel
{
    public partial class formMain
    {

        bool _emails = false;  // Checks that both emails have been filled out in new user form.
        bool _pass = false;    // Checks that both passwords have been filled out in new user form.
        bool logout = false;   // Checks if application needs to exit or only form.

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

        private void btnLogout_Click(object sender, EventArgs e)
        {
            rformLogin _login = new rformLogin();
            if (!logout)
            {
                _login.Show();
            }
            logout = true;
            exit = false;
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
            }
            else
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
            }
            else
            {
                confBox(npnlPassconf, Color.Red);
                passOk = false;
            }
            _pass = true;
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
            }
            catch { }
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
            }
            else
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
            File.AppendAllText(specificFolder + "/users.fbla", rformLogin.Encrypt(user + '\\' + pass + '\\' + first + '\\' + last + '\\' + email) + "\r\n");

            // Sets the user number for the new user and the
            // username without the permission number.
            int newUserNum = rformLogin.userNums.Count() + 1;
            string rawUser = user.Substring(0, user.Length - 1);

            // Adds the new user to the users array and listbox.
            listUsers.Items.Add(rawUser);
            rformLogin.userNums.Add(rawUser, newUserNum);
            rformLogin.users[newUserNum, 0] = user; rformLogin.users[newUserNum, 1] = pass; rformLogin.users[newUserNum, 2] = first;
            rformLogin.users[newUserNum, 3] = last; rformLogin.users[newUserNum, 4] = email;
            rformLogin.message(txtUsername.Text + " was successfully created!", "Success", 1);
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            // Creates the new user if all fields throw no errors.
            if (userOk && passOk && emailOk && txtFirstname.Text !=
                rformLogin.placeText[txtFirstname] && txtLastname.Text != rformLogin.placeText[txtLastname])
            {
                writeUser(txtUsername.Text + cmbPerms.SelectedIndex.ToString(), txtPassconf.Text, txtFirstname.Text, txtLastname.Text, txtEmail.Text);
            }
            else
            {
                rformLogin.message("Please make sure all entries are complete and correct.", "Error", 1);
            }
        }
    }
}
