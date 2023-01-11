using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Globalization;

namespace TBBL_AES_Encriptor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
            RijndaelManaged algo = new RijndaelManaged();
            algo.KeySize = 256;
                //algo.GenerateKey();
                algo.GenerateIV();
                kunci = algo.Key;
                iv = algo.IV;
                
            
        }

        private void BtExecute_Click(object sender, EventArgs e)
        {
            if (BtExecute.Text == "Encrypt")
            {
                
                kunci= System.Text.Encoding.UTF8.GetBytes(buatKunci(textBox1.Text));

                byte[] hasil_byte=EnkripsiStringKeByte(Text1.Text,kunci,iv);

                StringBuilder hasil_akhir = new StringBuilder();

                foreach (byte data in hasil_byte)
                {
                    hasil_akhir.Append(data.ToString("x2"));
                }
                Text2.Text = hasil_akhir.ToString();
                //textBox1.Text = hasil_akhir.ToString();

            }
            else if (BtExecute.Text == "Decrypt")
            {
                kunci = System.Text.Encoding.UTF8.GetBytes(buatKunci(textBox1.Text));

                string cipher = Text1.Text;
                byte[] chiperb = new byte[cipher.Length / 2];
                for (int a = 0; a < cipher.Length / 2; a++)
                {
                    string byteValue = cipher.Substring(a * 2, 2);
                    chiperb[a] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);

                }
                Text2.Text = DekripsiByteKeString(chiperb, kunci, iv);
            }
        }

        static byte[] EnkripsiStringKeByte(string PlainText, byte[] Kunci, byte[] IV)
        {
            byte[] hasil_enkripsi;
            if (PlainText == null || PlainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Kunci == null || Kunci.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");

            using (RijndaelManaged Algo = new RijndaelManaged())
            {
                Algo.Key = Kunci;
                Algo.IV = IV;
                Algo.Mode = CipherMode.CBC;
                Algo.Padding = PaddingMode.Zeros;

                ICryptoTransform enkrip = Algo.CreateEncryptor(Algo.Key, Algo.IV);

                using (MemoryStream MS = new MemoryStream())
                {
                    using (CryptoStream CS = new CryptoStream(MS,enkrip,CryptoStreamMode.Write))
                    {
                        using(StreamWriter SW = new StreamWriter(CS))
                        {
                            SW.Write(PlainText);
                        }
                        hasil_enkripsi = MS.ToArray();
                    }
                }
            }

                return hasil_enkripsi;
        }

        static string DekripsiByteKeString(byte[] CipherText, byte[] Kunci, byte[] IV)
        {
            string hasilDekripsi=null;
            if (CipherText == null || CipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Kunci == null || Kunci.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");

            using (RijndaelManaged Algo = new RijndaelManaged())
            {
                Algo.Key = Kunci;
                Algo.IV = IV;
                Algo.Mode = CipherMode.CBC;
                Algo.Padding = PaddingMode.Zeros;

                ICryptoTransform dekripsi = Algo.CreateDecryptor(Algo.Key, Algo.IV);

                using (MemoryStream MS = new MemoryStream(CipherText))
                {
                    using (CryptoStream CS = new CryptoStream(MS, dekripsi, CryptoStreamMode.Read))
                    {
                        using (StreamReader SR = new StreamReader(CS))
                        {
                            hasilDekripsi = SR.ReadToEnd();   
                        }
                    }
                }
            }
            return hasilDekripsi;
        }

        private void BtSwitch_Click(object sender, EventArgs e)
        {
            if (BtExecute.Text == "Encrypt")
            {
                BtExecute.Text = "Decrypt";
                BtSwitch.Text = "Encrypt";
            }
            else if (BtExecute.Text == "Decrypt")
            {
                BtSwitch.Text = "Decrypt";
                BtExecute.Text = "Encrypt";
            }
            button1_Click(null, null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Text1.Clear();Text2.Clear();textBox1.Clear(); 
        }


        private static byte[] kunci = new byte[32],iv = new byte[32];

        public static string buatKunci(string s)
        {
            bool a = true; string d=null;
            while (a)
            {
                if (s.Length <= 32)
                { s = s + s; }
                else { d=s.Remove(32);a = false; }
            }
            return d;
        }
    }
}
