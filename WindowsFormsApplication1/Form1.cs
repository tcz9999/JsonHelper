using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            richTextBox1.Text = @"  {""adff"":'aa'}
         {sdf:aaa}";
            textBox2.Text = @"(?<=^\s*{\s*"").*?(?=""\s*:)";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string str1 = richTextBox1.Text;
            string pattern = textBox2.Text;
            try
            {
                MatchCollection Matches = Regex.Matches(str1,pattern);
                label1.Text = "";
                foreach(Match m in Matches)
                {
                    label1.Text +=string.Format("位置：{0}，内容：{1}\n", m.Index.ToString(),m.Value.ToString() );
                }
                label1.Text += string.Format("替换后：{0}",Regex.Replace(str1,@"\\.",(m)=>m.Value[1].ToString()));
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label1.Text = Regex.Unescape(richTextBox1.Text);
        }
    }
}
