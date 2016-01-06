using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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

        string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static String[] placeText = new String[100];
        public static asset.ThirteenTextBox[] textboxA = new asset.ThirteenTextBox[50];
        public static int count = 0; public static int cur;
        public static bool[] passChar = new bool[100];
        formMain main;
        public static msgbox _message;
        public static string PasswordHash; public static string SaltKey = "S@LT&KEY";
        public static string VIKey = "@1B2c3D4e5F6g7H8";

        public static string Encrypt(string plainText)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));

            byte[] cipherTextBytes;

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                    cryptoStream.Close();
                }
                memoryStream.Close();
            }
            return Convert.ToBase64String(cipherTextBytes);
        }

        public static string Decrypt(string encryptedText)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
            byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

            var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
            var memoryStream = new MemoryStream(cipherTextBytes);
            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
        }

        public static void message(string msg, string title, int type, int returnVal)
        {
            _message = new msgbox(msg, title, type, returnVal);
            _message.Show();
            formMain.tmrResult.Start();
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
                if (textbox.UseSystemPasswordChar == true)
                {
                    textbox.UseSystemPasswordChar = false;
                }
                textbox.ForeColor = SystemColors.WindowFrame;
                textbox.Text = placeText[cur];
            }
        }

        private void rformLogin_Load(object sender, EventArgs e)
        {
            string appData = Path.Combine(folder, "Citadel");
            if(!File.Exists(appData + "/users.fbla"))
            {
                File.Create(appData + "/users.fbla").Dispose();
                StreamWriter _initial = new StreamWriter(appData + "/sourceSettings.fbla");
                PasswordHash = "admin";
                _initial.WriteLine(Encrypt("admin\\password\\First\\Last\\Email"));
                _initial.Close();
            }
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
                message("Invalid username or password.", "Error", 1, -1);
            }
        }

        private void txtUser_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                txtPass.Focus();
            }
        }

        private void txtPass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                btnLogin.PerformClick();
            }
        }

        private void rformLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
