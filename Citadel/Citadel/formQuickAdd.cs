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
            memberNum = currentMemberNum;
        }

        string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData); // %APPDATA% path
        string studentFile;  // Path to students data file.
        string activeSource; // Currently active source.
        string memberNum;    // Gets next member num so it can be set.
        public static bool added = false; // Communicate with formMain to update student list.


        private void formQuickAdd_Load(object sender, EventArgs e)
        {
            studentFile = Path.Combine(folder, "Citadel", "data", activeSource);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            bool worked = false;
            int newMemberNum = Convert.ToInt32(memberNum) - 1;
            try {
                String[] _check = txtAdd.Text.Split('\\');
                string _temp = "";
                _check[0] = memberNum;
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
    }
}
