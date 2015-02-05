using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Security.Cryptography;

namespace Labyrinth
{
    public partial class Form1 : Form
    {
        String[] answers = new String[50];
        public Form1()
        {

            InitializeComponent();
           
            string message = File.ReadAllText("C:\\questions\\answers.txt");
            answers = EncryptionHelper.Decrypt(message).Split('#');
            MessageBox.Show(File.ReadAllText("C:\\questions\\rules.txt"),"Rules");
            //customPopup popup = new customPopup();
            //popup.msgLabel.Text = "Your message";
            //popup.ShowDialog();
           // Microsoft.VisualBasic.Interaction.InputBox("Question?", "Title", "Default Text");
        }
        int score = 0;
        int skipint = 2;           
        int start = 0;
        int tries = 1;
        private void button1_Click(object sender, EventArgs e)
        {
            string[] subanswers = new string[3];
            if (start == 0)
            {
                start++;
                pictureBox1.Image = new Bitmap("C:\\questions\\1.jpg");
                level.Text = "Level " + start.ToString();
                button1.Text = "Submit Answer";
               
            }
            else
            
            {
                label2.Text = (tries++).ToString() + " number of tries";
                if (answers[start - 1] == "#")
                {
                    skiplabel.Text = "Question Already Solved";
                }
                else
                
                {
                    subanswers[1] = "asjdflakjdflaksjdfla";
                    subanswers[2] = "qweuehjasndf123";
                    subanswers = answers[start - 1].Split('*');
                    if (textBox1.Text == subanswers[0] || textBox1.Text == subanswers[1] )
                    {
                        answers[start - 1] = "#";
                        textBox1.Text = "";
                        start++;
                      //  label3.Text = "Score " + (++score);
                        pictureBox1.Image = new Bitmap("C:\\questions\\" + start + ".jpg");
                        level.Text = "Level " + start.ToString();
                     //   skiplabel.Text = "UnSolved";
                        if (answers[start - 1] == "#")
                        {
                            //skiplabel.Text = "Solved";
                        }
                    }
                    else 
                    {
                       // skiplabel.Text = "UnSolved";
                        textBox1.Text = "";
                    }
                }
                
            }



        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(this, new EventArgs());
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void skip_Click(object sender, EventArgs e)
        {
            if (start == 0)
            {
            }
           
            if (skipint != 0) 
            {
                start++;
                skiplabel.Text = "Skip Tries Left " + --skipint;
                pictureBox1.Image = new Bitmap("C:\\questions\\" + start + ".jpg");
                level.Text = "Level " + start.ToString();
                     
            }

            if (skipint == 0) 
            {
                skiplabel.Text = "No skip tries left ";
            }
        }
        /*

        private void button2_Click(object sender, EventArgs e)
        {

            start++;
            pictureBox1.Image = new Bitmap("C:\\questions\\" + start + ".jpg");
            level.Text = "Level " + start.ToString();
            if (answers[start - 1] == "#")
            {
                label1.Text = "Solved";
            }
            else
            {
                label1.Text = "UnSolved";
                textBox1.Text = "";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            start--;
            pictureBox1.Image = new Bitmap("C:\\questions\\" + start + ".jpg");
            level.Text = "Level " + start.ToString();
            if (answers[start - 1] == "#")
            {
                label1.Text = "Solved";
            }
            else
            {
                label1.Text = "UnSolved";
                textBox1.Text = "";
            }
      
        }
         */
       
    }


    public static class EncryptionHelper
    {
        public static string Encrypt(string clearText)
        {
            string EncryptionKey = "abc123";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = "abc123";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
    }
}
