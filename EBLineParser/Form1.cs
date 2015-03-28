using System;
using System.IO;
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
    public partial class Form1 : Form
    {

        private ebline lineCalc;
        private string[] rows = new string[3] { "", "", "" };

        public Form1()
        {
            lineCalc = new ebline();
            InitializeComponent();
        }

        public Form1(int aSize)
        {
            lineCalc = new ebline(aSize);
            InitializeComponent();
        }

        private void ccsPathButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            DialogResult result = fbd.ShowDialog();

            if (result == DialogResult.OK)
            {
                ccsPathBox.Text = fbd.SelectedPath;
                string widthResult = lineCalc.readWidths();
                if (!widthResult.Equals(""))
                {
                    MessageBox.Show(widthResult, "ERROR");
                    ccsPathBox.Text = "";
                }
                else
                {
                    testFontsBut.Enabled = true;
                }
                string itemResult = lineCalc.readItems();
                if (!itemResult.Equals(""))
                {
                    MessageBox.Show(itemResult, "ERROR");
                }
            }

        }

        private void generateButton_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            if (Directory.Exists(ccsPathBox.Text) &&
                Directory.Exists(logPathBox.Text))
            {
                string widthResult = lineCalc.readWidths();

                if (!widthResult.Equals(""))
                {
                    MessageBox.Show(widthResult, "ERROR");
                }

                string itemResult = lineCalc.readItems();
                if (!itemResult.Equals(""))
                {
                    MessageBox.Show(itemResult, "ERROR");
                }

                lineCalc.startItems(ccsPathBox.Text, logPathBox.Text);
                lineCalc.startCCS(ccsPathBox.Text,logPathBox.Text);
                MessageBox.Show("Report has been completed", "Complete");
            }
            this.Enabled = true;
        }

        private void logPathButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            DialogResult result = fbd.ShowDialog();

            if (result == DialogResult.OK)
            {
                logPathBox.Text = fbd.SelectedPath;
            }
        }

        private void ccsPathBox_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(ccsPathBox.Text) || String.IsNullOrEmpty(logPathBox.Text))
                generateButton.Enabled = false;
            else
                generateButton.Enabled = true;

            lineCalc.SetCoilPath(ccsPathBox.Text);
        }

        private void logPathBox_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(ccsPathBox.Text) || String.IsNullOrEmpty(logPathBox.Text))
                generateButton.Enabled = false;
            else
                generateButton.Enabled = true;

            lineCalc.SetLogPath(logPathBox.Text);
        }

        private void ccsPathBox_Leave(object sender, EventArgs e)
        {
            if(Directory.Exists(Path.Combine(ccsPathBox.Text,"Fonts")))
            {

                string widthResult = lineCalc.readWidths();

                if (!widthResult.Equals(""))
                {
                    MessageBox.Show(widthResult, "ERROR");
                    ccsPathBox.Text = "";
                }
                else
                {
                    testFontsBut.Enabled = true;
                }

                string itemResult = lineCalc.readItems();
                if (!itemResult.Equals(""))
                {
                    MessageBox.Show(itemResult, "ERROR");
                }
            }
        }

        private void testFontsBut_Click(object sender, EventArgs e)
        {
            VisualInterface vis = new VisualInterface(lineCalc);
            vis.Show();
        }

        private void setTextValuesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DefineString DefString = new DefineString();

            DefString.Show();
        }
    }
}
