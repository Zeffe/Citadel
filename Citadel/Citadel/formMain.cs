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


        string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        int currentUser; string appData;
        bool logout = false; int perms;

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

        void gbTitle(GroupBox groupbox, Control title)
        {
            Point _temp = groupbox.DisplayRectangle.Location;
            _temp.X += (groupbox.DisplayRectangle.Width - title.Width) / 2;
            _temp.Y = title.Location.Y;
            groupbox.ForeColor = Color.White;
            title.Location = _temp;
        }


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

        Panel activePanel;

        void updateSelected(Panel pnl)
        {
            activePanel.BackColor = Color.FromArgb(50, 50, 50);
            displays[pnl].BringToFront();
            pnl.BackColor = Color.DodgerBlue;
            activePanel = pnl;
            pctPointer.Location = heights[pnl];
        }

        Dictionary<Panel, Panel> displays = new Dictionary<Panel, Panel>();
        Dictionary<Panel, Point> heights = new Dictionary<Panel, Point>();

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

        void updateUserPage(int user)
        {
            lblFirstname.Text = rformLogin.users[user, 2];
            lblLastname.Text = rformLogin.users[user, 3];
            lblEmail.Text = rformLogin.users[user, 4];

            gbTitle(gbCuruser, lblCuruser);
            gbTitle(gbCuruser, lblEmail);

            int _x = gbCuruser.Width - lblFirstname.Width - 15;
            int _x2 = gbCuruser.Width - lblLastname.Width - 15;
            lblFirstname.Location = new Point(_x, lblFirstname.Location.Y);
            lblLastname.Location = new Point(_x2, lblLastname.Location.Y);
        }

        void newUser()
        {
            for (int i = 0; i < rformLogin.users.GetLength(0); i++)
            {
                listUsers.Items.Add(getUser(i));
            }
        }

        void _onClick(object sender, EventArgs e)
        {
            updateSelected(pnlbUsers);
        }

        bool _firstLoad;

        private bool firstLoad()
        {
            bool _first = false;
            if (!File.Exists(appData))
            {
                Directory.CreateDirectory(appData);
                _first = true;
            }
            if (!File.Exists(appData + "/data"))
            {
                Directory.CreateDirectory(appData + "/data");
                File.Create(appData + "/data/students.fbla").Dispose();
                _first = true;
            }
            if (!File.Exists(appData + "/sourceSettings.fbla"))
            {
                File.Create(appData + "/sourceSettings.fbla").Dispose();
                StreamWriter _initial = new StreamWriter(appData + "/sourceSettings.fbla");
                _initial.WriteLine("students.fbla\\0");
                _initial.Close();
                _first = true;
            }
            if (!File.Exists(appData + "/log.fbla"))
            {
                File.Create(appData + "/log.fbla").Dispose();
                _first = true;
            }
            return _first;
        }

        private string getUser(int userNum)
        {
            return rformLogin.users[userNum, 0].Substring(0, rformLogin.users[userNum, 0].Length - 1);
        }

        public static System.Timers.Timer tmrResult = new System.Timers.Timer();

        private void formMain_Load(object sender, EventArgs e)
        {
            appData = Path.Combine(folder, "Citadel");
            _firstLoad = firstLoad();
            tmrResult.Interval = 100;
            tmrResult.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            activePanel = pnlbDashboard;
            pnlDashboard.BringToFront();      
            gbTitle(gbCuruser, btnLogout);
            gbNewuser.ForeColor = Color.White;
            gbUserlist.ForeColor = Color.White;
            lblUser.Click += new System.EventHandler(_onClick);
            pnlContainer.Click += new System.EventHandler(_onClick);
            label1.Click += new System.EventHandler(_onClick);
            int _x; string _user = "";
            if (getUser(currentUser).Length < 12)
            {
                lblUser.Text = getUser(currentUser);
                lblCuruser.Text = getUser(currentUser);
            }
            else
            {
                for (int i = 0; i < 12; i++)
                {
                    _user += getUser(currentUser)[i];
                }
                lblUser.Text = _user + "...";
                lblCuruser.Text = _user + "...";
            }
            pnlContainer.Width = 85 + lblUser.Width;
            _x = pnlContainer.Location.X;
            pnlContainer.Location = new Point((divider1.Location.X - pnlContainer.Width) / 2, pnlContainer.Location.Y);
            _x = pnlContainer.Location.X - _x;
            label1.Location = new Point(label1.Location.X + _x, label1.Location.Y);
            lblUser.Location = new Point(lblUser.Location.X + _x, label1.Location.Y);
            for (int i = 0; i < rformLogin.users.GetLength(0); i++)
            {
                if (rformLogin.users[i, 0] == null) break;
                listUsers.Items.Add(getUser(i));
            }
            panelButton(pnlbDashboard, lblDashboard, pctDashboard, pnlDashboard);
            panelButton(pnlbStats, lblStats, pctStats, pnlStats);
            panelButton(pnlbStudents, lblStudents, pctStudents, pnlStudents);
            panelButton(pnlbSource, lblSource, pctSource, pnlSource);
            panelButton(pnlbUsers, lblUsers, pctUsers, pnlUsers);
            panelButton(pnlbSettings, lblSettings, pctSettings, pnlSettings);
            panelButton(pnlbInfo, lblInfo, pctInfo, pnlInfo);
            if (perms != 1)
            {
                gbNewuser.Enabled = false;
            }
            updateUserPage(currentUser);
        }

        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            switch(msgbox._return)
            {
                case -1: tmrResult.Stop(); break;
                case 1: Application.Exit(); break;
            }        
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            rformLogin _login = new rformLogin();
            _login.Show();
            logout = true;
            this.Close();
        }
    }
}
