using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Extensions;

namespace Citadel
{
    public partial class formMain
    {
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
                // Gets the member number as a string.
                string[] flag = { " - " };
                string tempMemNum = _nodeText.Split(flag, StringSplitOptions.None)[1];
                tempMemNum.Trim(' ');

                // Get the membernum by looking at the last character of the node.
                int _memNum = Convert.ToInt32(tempMemNum.Trim(' '));

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
                switch (!active)
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
                }
                else
                {
                    _grade = "13";
                }

                // The string that will be saved.
                string _temp = nmNewMemNum.Text + '\\' + txtNewFirst.Text + '\\' + txtNewLast.Text + '\\' + txtNewFees.Text
                    + '\\' + nmNewYear.Text + '\\' + _gender + '\\' + _active + '\\' + _grade
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

            if (delConf.DialogResult == DialogResult.Yes)
            {
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
