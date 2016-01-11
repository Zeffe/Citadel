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

        string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData); // %APPDATA% path
        string specificFolder; // A string used to store the Citadel folder in appData.
        public static asset.ThirteenTextBox[] textboxA = new asset.ThirteenTextBox[50]; // Array of textboxes for placeholder method
        public static int count = 0; // Counts placeholder array members.
        formMain main;
        public static msgbox _message;
        public static string PasswordHash; public static string SaltKey = "S@LT&KEY"; // Encryption keys.
        public static string VIKey = "@1B2c3D4e5F6g7H8";
        public static String[,] users = new String[50, 5];

        // Method used to encrypt a string.
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

        // Method used to decrypt a string.
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

        // Used to call the custom message box.
        public static void message(string msg, string title, int type, int returnVal)
        {
            _message = new msgbox(msg, title, type, returnVal);
            _message.Show();
        }

        // Stores the place text for the respective TextBoxes.
        public static Dictionary<TextBox, String> placeText = new Dictionary<TextBox, string>();
        // Stores whether or not the respective TextBox uses password characters.
        public static Dictionary<TextBox, bool> passChar = new Dictionary<TextBox, bool>();
        // Stores usernames with their respective user numbers.
        public static Dictionary<String, int> userNums = new Dictionary<String, int>();

        // Declares all necessarry events for placeholder TextBoxes to work.
        public static void placeHolder(asset.ThirteenTextBox textbox, String text, bool pass)
        {
            placeText.Add(textbox, text);
            textbox.Text = text;
            textboxA[count] = textbox;
            passChar[textbox] = pass;
            textbox.ForeColor = SystemColors.WindowFrame;
            textboxA[count].Enter += new System.EventHandler(onEnter);
            textboxA[count].Leave += new System.EventHandler(onLeave);
            count++;
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

        StreamReader _reader; // Reader used when reading to array.

        // Reads a plaintext file into a 2D array.
        void readToArray(string file, String[,] array2d)
        {
            string str;
            _reader = File.OpenText(file);
            int i = 0;
            while ((str = _reader.ReadLine()) != null)
            {
                int j = 0;
                str = Decrypt(str);
                String[] strArray = new String[str.Split('\\').Length];
                strArray = str.Split('\\');
                foreach (string element in strArray)
                {
                    try
                    { 
                        array2d[i, j] = element;
                    }
                    catch { }
                    j++;
                }
                if (array2d == users)
                {
                    // Match usernames with their respective user numbers
                    // if being read to the users array.
                    try
                    {
                        userNums.Add(strArray[0].Substring(0, strArray[0].Length - 1), i);
                    }
                    catch { }
                }
                i++;
            }
            _reader.Close();
        }

        // Checks if it is a users first time loading the
        // program and initializes necessarry files.
        private bool firstLoad()
        {
            bool _first = false;
            if (!File.Exists(specificFolder))
            {
                Directory.CreateDirectory(specificFolder);
                _first = true;
            }
            if (!File.Exists(specificFolder + "/data"))
            {
                Directory.CreateDirectory(specificFolder + "/data");
                File.Create(specificFolder + "/data/students.fbla").Dispose();
                _first = true;
            }
            if (!File.Exists(specificFolder + "/sourceSettings.fbla"))
            {
                File.Create(specificFolder + "/sourceSettings.fbla").Dispose();
                StreamWriter _initial = new StreamWriter(specificFolder + "/sourceSettings.fbla");
                _initial.WriteLine("students.fbla\\0");
                _initial.Close();
                _first = true;
            }
            if (!File.Exists(specificFolder + "/log.fbla"))
            {
                File.Create(specificFolder + "/log.fbla").Dispose();
                _first = true;
            }
            return _first;
        }

        private void rformLogin_Load(object sender, EventArgs e)
        {
            specificFolder = Path.Combine(folder, "Citadel"); // %APPDATA%/Citadel home path
            // Checks that all existing files are created.
            firstLoad();

            // Sets the hash for encryption.
            PasswordHash = "admin";

            // Initializes an administrator account if the 
            // users file does not exist. 
            // User: admin  Pass: password
            if (!File.Exists(specificFolder + "/users.fbla"))
            {
                File.Create(specificFolder + "/users.fbla").Dispose();
                File.AppendAllText(specificFolder + "/users.fbla", Encrypt("admin1\\password\\First\\Last\\Email") + "\r\n");
            }

            // Places the placeholder text into given TextBoxes.
            placeHolder(txtUser, "Username", false);
            placeHolder(txtPass, "Password", true);

            // Reads the data from users.fbla to the users array.
            readToArray(specificFolder + "/users.fbla", users);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            // Loops through each user.
            for (int i = 0; i <= users.GetLength(0) - 1; i++)
            {
                // If the user matches and ends in 1, give admin permissions.
                if (users[i, 0] == txtUser.Text + "1" && users[i, 1] == txtPass.Text)
                {
                    main = new formMain(i, 1);
                    main.Show();
                    this.Hide();
                    break;
                // If the user matches and ends in 0, give normal user permissions.
                } else if (users[i, 0] == txtUser.Text + "0" && users[i, 1] == txtPass.Text)
                {
                    main = new formMain(i, 0);
                    main.Show();
                    this.Hide();
                    break;
                } else if (i == users.GetLength(0) - 1)
                {
                    message("Invalid username or password.", "Error", 1, -1);
                }
            }
        }

        // Focus password TextBox on enter key press.
        private void txtUser_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                txtPass.Focus();
            }
        }

        // Simulate a login click on enter key press.
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
