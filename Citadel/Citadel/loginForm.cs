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
    public partial class rformLogin : Form
    {
        public rformLogin()
        {
            InitializeComponent();
        }

        public static String[] placeText = new String[100];
        public static asset.ThirteenTextBox[] textboxA = new asset.ThirteenTextBox[50];
        public static int count = 0; public static int cur;
        public static bool[] passChar = new bool[100];
        formMain main;
        public static msgbox _message;

        public static void message(string msg, string title, int type)
        {
            _message = new msgbox(msg, title, type);
            _message.Show();
            //_message.ifExit = ifExit;
            //_message.Start();
        }

        public static void placeHolder(asset.ThirteenTextBox textbox, String text, bool pass)
        {
            placeText[count] = text;
            textbox.Text = text;
            textboxA[count] = textbox;
            passChar[count] = pass;
            textbox.ForeColor = SystemColors.WindowFrame;
            textboxA[count].Enter += new System.EventHandler(onEnter);
            textboxA[count].Leave += new System.EventHandler(onLeave);
            count++;
        }

        public static void onEnter(object sender, EventArgs e)
        {
            TextBox textbox = sender as TextBox;
            for (int i = 0; i < placeText.Length; i++)
            {
                if (textbox.Text == placeText[i])
                {
                    if (passChar[i])
                    {
                        textbox.UseSystemPasswordChar = true;
                    }
                    textbox.ForeColor = Color.White;
                    textbox.Text = "";
                    cur = i;
                }
            }
        }

        public static void onLeave(object sender, EventArgs e)
        {
            TextBox textbox = sender as TextBox;
            if (textbox.Text == "")
            {
                if (textbox.PasswordChar == '*')
                {
                    textbox.UseSystemPasswordChar = false;
                }
                textbox.ForeColor = SystemColors.WindowFrame;
                textbox.Text = placeText[cur];
            }
        }

        private void rformLogin_Load(object sender, EventArgs e)
        {
            placeHolder(txtUser, "Username", false);
            placeHolder(txtPass, "Password", true);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtPass.Text == "password")
            {
                main = new formMain(txtUser.Text);
                this.Hide();
                main.Show();
            } else
            {
                message("Invalid username or password.", "Error", 1);
            }
        }

        private void txtUser_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtPass.Focus();
            }
        }

        private void txtPass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin.PerformClick();
            }
        }
    }
}
