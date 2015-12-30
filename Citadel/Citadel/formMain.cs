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
            lblUser.Text = currentUser;
            pnlContainer.Width = 85 + lblUser.Width;
        }
    }
}
