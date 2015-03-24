using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EBLineParser
{
    public partial class DefineString : Form
    {
        public DefineString()
        {
            InitializeComponent();
        }

        private void DefineString_Load(object sender, EventArgs e)
        {
            textString.Text = Properties.Settings.Default.fontString;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textString.Text.Length > 128)
                MessageBox.Show("The font string cannot exceed 128 characters");
            else if (textString.Text.Length < 96)
                MessageBox.Show("The font string should be at least 96 characters long");
            else
                Properties.Settings.Default.fontString = textString.Text;
        }

        private void DefineString_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.Save();
        }
    }
}
