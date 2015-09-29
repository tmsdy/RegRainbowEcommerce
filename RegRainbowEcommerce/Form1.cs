using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace RegRainbowEcommerce
{ 
    public partial class Form1 : Form
    {
       string sVersionPrefix = "ES";
       string sVersion = "ADV"; 
        public Form1()
        {
            InitializeComponent();
            radioButton2.Checked = true;

            radioButton4.Checked = true;
        }
 
        public static string GetAuthKey(string prodName, string hardwareID)
        {
            string s = "";
            int startIndex = 0;
            do
            {
                s = s + Strings.Asc(prodName.Substring(startIndex, 1)).ToString();
                startIndex++;
            }
            while (startIndex <= 3);
            long num4 = long.Parse(s);
            long num = long.Parse(hardwareID) ^ num4;
            if (num < 0x3b9aca00L)
            {
                num += 0x3b9aca00L;
            }
            return num.ToString();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                MessageBox.Show("請輸入硬體ID");
                return;

            }
            string sKey0Key3 = GetAuthKey(sVersionPrefix+sVersion, textBox1.Text);
            if (sKey0Key3.Length < 10)
            {
                MessageBox.Show("計算出錯");
                return;

            }
            textBox2.Text = sKey0Key3.Substring(0, 5) + "-" + sVersion + "-1234567-" + sKey0Key3.Substring(5, 5);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            sVersion = "ENT";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            sVersion = "ADV";
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
              sVersionPrefix = "ES";
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
              sVersionPrefix = "CS";
        }
    }
}
