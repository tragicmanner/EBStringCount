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
            lineCalc.readWidths();
        }

        public Form1(string aFile)
        {
            lineCalc = new ebline(aFile);
            InitializeComponent();
            lineCalc.readWidths();
        }

        public Form1(string aFile, int aSize)
        {
            lineCalc = new ebline(aFile, aSize);
            InitializeComponent();
            lineCalc.readWidths();
        }

        public Form1(int aSize)
        {
            lineCalc = new ebline(aSize);
            InitializeComponent();
            lineCalc.readWidths();
        }

        private void ccsPathButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            DialogResult result = fbd.ShowDialog();

            if (result == DialogResult.OK)
            {
                ccsPathBox.Text = fbd.SelectedPath;
            }
        }

        private void generateButton_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(ccsPathBox.Text) &&
                Directory.Exists(logPathBox.Text))
            {
                lineCalc.startCCS(ccsPathBox.Text,logPathBox.Text);
                MessageBox.Show("Report has been completed", "Complete");
            }
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

        
    }
}
