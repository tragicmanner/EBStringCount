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


        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            resetUI();
            string curString = richTextBox1.Text;
            string tempString = curString;

            for (int i = 0; i < 3; i++)
            {
                rows[i] = lineCalc.lineExtractor(tempString);

                if(rows[i].Length > 0)
                    tempString = tempString.Replace(rows[i], "");

                if (tempString.Length == 0)
                {
                    i = 3;
                }
            }

            if (rows[0].Length > 0)
            {
                firstSizeLabel.Text = lineCalc.calcStringSize(rows[0]).ToString();
            }

            if (rows[1].Length > 0)
            {
                secondSizeLabel.Text = lineCalc.calcStringSize(rows[1]).ToString();
            }

            if (rows[2].Length > 0)
            {
                thirdSizeLabel.Text = lineCalc.calcStringSize(rows[2]).ToString();
            }

            if (lineCalc.lineExtractor(tempString).Length > 0)
            {
                errorLabel.Text = "Current line exceeds 3 rows by " +
                    lineCalc.calcStringSize(tempString).ToString() +
                    "pixels. Extra is: \n" + tempString;
            }
        }

        private void resetUI()
        {
            firstSizeLabel.Text = "";
            secondSizeLabel.Text = "";
            thirdSizeLabel.Text = "";

            errorLabel.Text = "Current line does not exceed 3 rows";

        }

        private void visualizeButton_Click(object sender, EventArgs e)
        {
            rows[0] = lineCalc.RemoveDiacritics(rows[0]);
            rows[1] = lineCalc.RemoveDiacritics(rows[1]);
            rows[2] = lineCalc.RemoveDiacritics(rows[2]);
            Visualizer aVis = new Visualizer(rows[0], rows[1], rows[2]);
            aVis.Show();
        }
    }
}
