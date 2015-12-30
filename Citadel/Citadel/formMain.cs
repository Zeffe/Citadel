using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Citadel
{
    public partial class formMain : Form
    {

        string currentUser;

        public formMain(string user)
        {
            InitializeComponent();
            currentUser = user;
        }

        private void formMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void formMain_Load(object sender, EventArgs e)
        {
            int _x; string _user = "";
            if (currentUser.Length < 12)
            {
                lblUser.Text = currentUser;
            } else
            {
                for (int i = 0; i < 12; i++)
                {
                    _user += currentUser[i];
                }
                lblUser.Text = _user + "...";
            }
            pnlContainer.Width = 85 + lblUser.Width;
            _x = pnlContainer.Location.X;
            pnlContainer.Location = new Point( (divider1.Location.X - pnlContainer.Width)/2, pnlContainer.Location.Y);
            _x = pnlContainer.Location.X - _x;
            label1.Location = new Point(label1.Location.X + _x, label1.Location.Y);
            lblUser.Location = new Point(lblUser.Location.X + _x, label1.Location.Y);
        }
    }
}
