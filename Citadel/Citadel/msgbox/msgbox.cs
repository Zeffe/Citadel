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
    public partial class msgbox : Form
    {

        string msgOut; int _returnVal; bool GO;
        public static int _return = 0;

        public msgbox(String msg, String title, int type, int returnVal)
        {
            //_returnVal = returnVal;
            //_return = 0;
            InitializeComponent();
            int cur = -1;
            for (int j = 1; j < msg.Length; j++)
            {
                for (int i = 0; i < 100; i++)
                {
                    cur++;
                    try
                    {
                        if (i > 50)
                        {
                            if (msg[cur] == ' ')
                            {
                                break;
                            }
                        }
                        msgOut += msg[cur];
                    }
                    catch
                    {
                        j += 100; break;
                    }
                }
                msgOut += "\n";
            }
            lblMessage.Text = msgOut;
            this.Width = lblMessage.Width + 20;
            switch (type)
            {
                case 0: this.Height += lblMessage.Height - 10; break;
                case 1:
                    this.Height += lblMessage.Height + 10; btnOk.Visible = true;
                    btnOk.Location = new System.Drawing.Point(this.Width / 2 - btnOk.Width / 2, this.Height - 30);
                    break;
                case 2:
                    this.Height += lblMessage.Height + 10; btnYes.Visible = true; btnNo.Visible = true;
                    btnYes.Location = new System.Drawing.Point(this.Width / 3 - this.Width/8, this.Height - this.Height / 3);
                    btnNo.Location = new System.Drawing.Point((this.Width / 3) * 2 - this.Width/8, this.Height - this.Height / 3);
                    break;
            }
            // this.Icon later
            thirteenForm1.Text = title; this.Text = title;
            //thirteenControlBox1.Controls.Remove();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            _return = _returnVal;
            this.Close();
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            _return = -1;
            this.Close();
        }
    }
}
