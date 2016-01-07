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
        public static asset.ThirteenTextBox[] textboxA = new asset.ThirteenTextBox[50];
        public static int count = 0;
        formMain main;
        public static msgbox _message;
        public static string PasswordHash; public static string SaltKey = "S@LT&KEY";
        public static string VIKey = "@1B2c3D4e5F6g7H8";
        public static String[,] users = new String[50, 5];

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

        public static Dictionary<TextBox, String> placeText = new Dictionary<TextBox, string>();
        public static Dictionary<TextBox, bool> passChar = new Dictionary<TextBox, bool>();

        public static void placeHolder(asset.ThirteenTextBox textbox, String text, bool pass)
        {
            placeText.Add(textbox, text);
            textbox.Text = text;
            textboxA[count] = textbox;
            passChar[textbox] = pass;
            textbox.ForeColor = SystemColors.WindowFrame;
            textboxA[count].Enter += new System.EventHandler(onEnter);
            textboxA[count].MouseEnter += new System.EventHandler(mouseEnter);
            textboxA[count].MouseLeave += new System.EventHandler(mouseLeave);
            textboxA[count].Leave += new System.EventHandler(onLeave);
            count++;
        }

        public static void mouseEnter(object sender, EventArgs e)
        {
            TextBox textbox = sender as TextBox;
            textbox.Parent.Invalidate();
        }

        public static void mouseLeave(object sender, EventArgs e)
        {
            TextBox textbox = sender as TextBox;
            textbox.Parent.Invalidate();
        }

        public static void onEnter(object sender, EventArgs e)
        {
            TextBox textbox = sender as TextBox;
            if (textbox.Text == placeText[textbox])
            {
                textbox.UseSystemPasswordChar = passChar[textbox];
                textbox.ForeColor = Color.White;
                textbox.Text = "";
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
                textbox.Text = placeText[textbox];
            }
        }

        StreamReader _reader;

        void readToArray(string file, String[,] array2d)
        {
            string str;
            _reader = File.OpenText(file);
            int i = 0;
            int j = 0;
            while ((str = _reader.ReadLine()) != null)
            {
                str = Decrypt(str);
                String[] strArray = new String[str.Split('\\').Length];
                strArray = str.Split('\\');
                foreach (string element in strArray)
                {
                    array2d[i, j] = element;
                    j++;
                }
                i++;
            }
        }

        private void rformLogin_Load(object sender, EventArgs e)
        {
            string appData = Path.Combine(folder, "Citadel");
            PasswordHash = "admin";
            if (!File.Exists(appData + "/users.fbla"))
            {
                File.Create(appData + "/users.fbla").Dispose();
                StreamWriter _initial = new StreamWriter(appData + "/users.fbla");
                _initial.WriteLine(Encrypt("admin1\\password\\First\\Last\\Email") + "\r\n");
                _initial.Close();
            }
            placeHolder(txtUser, "Username", false);
            placeHolder(txtPass, "Password", true);
            readToArray(appData + "/users.fbla", users);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= users.GetLength(0) - 1; i++)
            {
                if (users[i, 0] == txtUser.Text + "1" && users[i, 1] == txtPass.Text)
                {
                    main = new formMain(i, 1);
                    this.Hide();
                    main.Show();
                    break;
                } else if (users[i, 0] == txtUser.Text + "0" && users[i, 1] == txtPass.Text)
                {
                    main = new formMain(i, 0);
                    this.Hide();
                    main.Show();
                    break;
                } else if (i == users.GetLength(0) - 1)
                {
                    message("Invalid username or password.", "Error", 1, -1);
                }
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
