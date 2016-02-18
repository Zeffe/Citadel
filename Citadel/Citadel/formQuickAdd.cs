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
    public partial class formQuickAdd : Form
    {
        public formQuickAdd(string source, string currentMemberNum)
        {
            InitializeComponent();
            activeSource = source;
            memberNum = Convert.ToInt32(currentMemberNum);
        }

        string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData); // %APPDATA% path
        string studentFile;  // Path to students data file.
        string activeSource; // Currently active source.
        int  memberNum;      // Gets next member num so it can be set.
        public static bool added = false; // Communicate with formMain to update student list.

        private void formQuickAdd_Load(object sender, EventArgs e)
        {
            studentFile = Path.Combine(folder, "Citadel", "data", activeSource);
            dQuickList.Columns[3].DefaultCellStyle.NullValue = "$0.00";
            for (int i = DateTime.Now.Year; i > 1950; i--)
            {
                hYearJoined.Items.Add(i.ToString());
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            bool worked = false;
            int newMemberNum = memberNum - 1;
            try {
                String[] _check = txtAdd.Text.Split('\\');
                string _temp = "";
                _check[0] = memberNum.ToString();
                if (_check.Length == 11)
                {
                    for (int i = 0; i < 11; i++)
                    {
                        _temp += _check[i];
                        formMain.students[newMemberNum, i] = _check[i];
                        if (i != 10)
                        {
                            _temp += '\\';
                        }
                    }
                    File.AppendAllText(studentFile, _temp + "\r\n");
                    txtAdd.Clear();
                    rformLogin.message("Successfully added " + _check[1] + " " + _check[2] + ".", "Success", 1);
                    added = true;
                    memberNum++;
                    worked = true;
                }
            } catch { }

            if (!worked)
            {
                rformLogin.message("The data you have entered is not formatted correctly, please be sure you are only using data that has been copied from any copy of Citadel.", "Error", 1);
            }
        }

        private void formQuickAdd_FormClosing(object sender, FormClosingEventArgs e)
        {
            formMain.tmrQckAdd.Stop();
            formMain.quickAdd = null;
        }

        private void tcMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tcMain.SelectedTab == tabPage2)
            {
                this.Size = new Size(387, 184);
                tcMain.Size = new Size(362, 133);
            } else
            {
                this.Size = new Size(616, 521);
                tcMain.Size = new Size(592, 472);
            }
        }

        private void dQuickList_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            dQuickList.SelectedCells[0].Style = new DataGridViewCellStyle { ForeColor = Color.Black };
        }

        private void dQuickList_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            Extensions.extensions.dFormat(e, dQuickList);
        }

        private void dQuickList_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                DataGridViewCell cell = dQuickList.Rows[e.RowIndex].Cells[e.ColumnIndex];
                string cellText;
                try {
                    cellText = cell.Value.ToString();
                } catch
                {
                    cellText = "$0.00";
                }
                Double feeDbl = 0;
                Double.TryParse(cellText.Substring(1, cellText.Length - 1), out feeDbl);

                if (cellText.Contains("$"))
                {
                    cell.Value = feeDbl.ToString();
                }
            }
        }

        private void dQuickList_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            dQuickList.Columns[3].DefaultCellStyle.NullValue = "$0.00";
        }

        private void btnAddAll_Click(object sender, EventArgs e)
        {
            bool GO = false;
            int rowsToUse = dQuickList.Rows.Count - 1;
            String[,] tempStudents = new String[rowsToUse, 11];
            String[] newStudents = new String[rowsToUse];
            for (int i = 0; i < rowsToUse; i++)
            {
                for (int j = 0; j < tempStudents.GetLength(1) - 1; j++)
                {
                    try {
                        if ((tempStudents[i, j] = dQuickList.Rows[i].Cells[j].Value.ToString()) != "" || j == 10)
                        {
                            if (j == 5 || j == 6)
                            {
                                switch (tempStudents[i, j])
                                {
                                    case "M": tempStudents[i, j] = "1"; break;
                                    case "F": tempStudents[i, j] = "0"; break;
                                    case "Yes": tempStudents[i, j] = "1"; break;
                                    case "No": tempStudents[i, j] = "0"; break;
                                }
                            }
                            GO = true;
                        } 
                    } catch
                    {
                        rformLogin.message("Please ensure all required entries are filled.", "Error", 1);
                        GO = false; break;
                    }
                }
            }

            if (GO)
            {
                for (int j = 0; j < tempStudents.GetLength(0); j++)
                {
                    for (int i = 0; i < tempStudents.GetLength(1) - 1; i++)
                    {
                        newStudents[j] += tempStudents[j, i] + "\\";
                    }
                    newStudents[j] += tempStudents[j, tempStudents.GetLength(1) - 1] + "\r\n";
                }
                
                foreach (String str in newStudents)
                {
                    File.AppendAllText(studentFile, str);
                }

                dQuickList.Rows.Clear();
                rformLogin.message("Successfully added " + rowsToUse.ToString() + " student(s).", "Sucess", 1);
            }
        }
    }
}
