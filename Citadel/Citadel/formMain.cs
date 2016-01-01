﻿using System;
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

        bool closing = false;

        private void formMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
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

        public static System.Timers.Timer tmrResult = new System.Timers.Timer();

        private void formMain_Load(object sender, EventArgs e)
        {
            tmrResult.Interval = 100;
            tmrResult.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            activePanel = pnlbDashboard;
            pnlDashboard.BringToFront();
            int _x; string _user = "";
            if (currentUser.Length < 12)
            {
                lblUser.Text = currentUser;
            }
            else
            {
                for (int i = 0; i < 12; i++)
                {
                    _user += currentUser[i];
                }
                lblUser.Text = _user + "...";
            }
            pnlContainer.Width = 85 + lblUser.Width;
            _x = pnlContainer.Location.X;
            pnlContainer.Location = new Point((divider1.Location.X - pnlContainer.Width) / 2, pnlContainer.Location.Y);
            _x = pnlContainer.Location.X - _x;
            label1.Location = new Point(label1.Location.X + _x, label1.Location.Y);
            lblUser.Location = new Point(lblUser.Location.X + _x, label1.Location.Y);
            panelButton(pnlbDashboard, lblDashboard, pctDashboard, pnlDashboard);
            panelButton(pnlbStats, lblStats, pctStats, pnlStats);
        }

        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            switch(msgbox._return)
            {
                case -1: tmrResult.Stop(); break;
                case 1: Application.Exit(); break;
            }        
        }
    }
}
