using System;
using System.IO;
using System.Collections;
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
    public partial class VisualInterface : Form
    {
        private ebline localLine;
        public VisualInterface()
        {
            InitializeComponent();
            localLine = new ebline();
        }

        public VisualInterface(ebline anEbline)
        {
            InitializeComponent();
            localLine = anEbline;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            string plainLine = richTextBox1.Text;
            ArrayList rows = localLine.ParseString(plainLine);
            int maxRows = localLine.getRowMax();
            // Here we calculate the number of pixels due to indents. One is subtracted to account for no forced indent on the first line
            int indents = (rows.Count * localLine.getIndent()) - localLine.getIndent();
            totalLengthValue.Text = (localLine.calcStringSize(plainLine) + indents).ToString();

            if (rows.Count > maxRows)
            {
                rowsValue.Text = String.Format("String exceeds {0} rows by {1} pixels", maxRows, 
                    localLine.calcStringSize(rows[maxRows].ToString()));
                overflowText.Text = rows[maxRows].ToString();

            }
            else
            {
                rowsValue.Text = rows.Count.ToString();
                overflowText.Text = "";
            }
        }

        private void VisualInterface_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < localLine.getNumFonts(); i++)
            {
                fontBox.Items.Add(i);
            }
            fontBox.SelectedIndex = 0;
        }

        private void fontBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            pictureBox1.ImageLocation = Path.Combine(localLine.getCoilPath(), string.Format("Fonts\\{0}.png", fontBox.SelectedIndex));
            localLine.SetCurrentFont(fontBox.SelectedIndex);
            richTextBox1_TextChanged(sender, e);
        }

        private void richTextBox1_SelectionChanged(object sender, EventArgs e)
        {
            selectedLengthValue.Text = localLine.calcStringSize(richTextBox1.SelectedText).ToString();
        }

    }
}
