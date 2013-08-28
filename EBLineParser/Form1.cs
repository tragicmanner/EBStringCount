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
        private char[] AllChars;
        private int[] AllWidths;
        private int lineSize;
        private string inputFile;
        private string AllCharsString;

        public Form1()
        {
            inputFile = "widths.yml";
            lineSize = 30;
            InitializeComponent();
            characterInitializer();
            readWidths();
        }

        public Form1(string aFile)
        {
            inputFile = aFile;
            lineSize = 30;
            InitializeComponent();
            characterInitializer();
            readWidths();
        }

        public Form1(string aFile, int aSize)
        {
            inputFile = aFile;
            lineSize = aSize;
            InitializeComponent();
            characterInitializer();
            readWidths();
        }

        public Form1(int aSize)
        {
            inputFile = "widths.yml";
            lineSize = aSize;
            InitializeComponent();
            characterInitializer();
            readWidths();
        }


        private void characterInitializer()
        {
            AllChars = new char[96];
            
            AllChars[0] = ' ';
            AllChars[1] = '!';
            AllChars[2] = '"';
            AllChars[3] = '#';
            AllChars[4] = '$';
            AllChars[5] = '%';
            AllChars[6] = '&';
            AllChars[7] = '\'';
            AllChars[8] = '(';
            AllChars[9] = ')';
            AllChars[10] = '*';
            AllChars[11] = '+';
            AllChars[12] = ',';
            AllChars[13] = '-';
            AllChars[14] = '.';
            AllChars[15] = '/';
            AllChars[16] = '0';
            AllChars[17] = '1';
            AllChars[18] = '2';
            AllChars[19] = '3';
            AllChars[20] = '4';
            AllChars[21] = '5';
            AllChars[22] = '6';
            AllChars[23] = '7';
            AllChars[24] = '8';
            AllChars[25] = '9';
            AllChars[26] = ':';
            AllChars[27] = ';';
            AllChars[28] = '<';
            AllChars[29] = '=';
            AllChars[30] = '>';
            AllChars[31] = '?';
            AllChars[32] = '@';
            AllChars[33] = 'A';
            AllChars[34] = 'B';
            AllChars[35] = 'C';
            AllChars[36] = 'D';
            AllChars[37] = 'E';
            AllChars[38] = 'F';
            AllChars[39] = 'G';
            AllChars[40] = 'H';
            AllChars[41] = 'I';
            AllChars[42] = 'J';
            AllChars[43] = 'K';
            AllChars[44] = 'L';
            AllChars[45] = 'M';
            AllChars[46] = 'N';
            AllChars[47] = 'O';
            AllChars[48] = 'P';
            AllChars[49] = 'Q';
            AllChars[50] = 'R';
            AllChars[51] = 'S';
            AllChars[52] = 'T';
            AllChars[53] = 'U';
            AllChars[54] = 'V';
            AllChars[55] = 'W';
            AllChars[56] = 'X';
            AllChars[57] = 'Y';
            AllChars[58] = 'Z';
            AllChars[59] = '[';
            AllChars[60] = '\\';
            AllChars[61] = ']';
            AllChars[62] = '^';
            AllChars[63] = '_';
            AllChars[64] = '`';
            AllChars[65] = 'a';
            AllChars[66] = 'b';
            AllChars[67] = 'c';
            AllChars[68] = 'd';
            AllChars[69] = 'e';
            AllChars[70] = 'f';
            AllChars[71] = 'g';
            AllChars[72] = 'h';
            AllChars[73] = 'i';
            AllChars[74] = 'j';
            AllChars[75] = 'k';
            AllChars[76] = 'l';
            AllChars[77] = 'm';
            AllChars[78] = 'n';
            AllChars[79] = 'o';
            AllChars[80] = 'p';
            AllChars[81] = 'q';
            AllChars[82] = 'r';
            AllChars[83] = 's';
            AllChars[84] = 't';
            AllChars[85] = 'u';
            AllChars[86] = 'v';
            AllChars[87] = 'w';
            AllChars[88] = 'x';
            AllChars[89] = 'y';
            AllChars[90] = 'z';
            AllChars[91] = '{';
            AllChars[92] = '|';
            AllChars[93] = '}';
            AllChars[94] = '~';
            AllChars[95] = '¬';

            AllCharsString = charArraytoString(AllChars);
        }

        private void readWidths()
        {
            AllWidths = new int[96];
            string line = "";
            string subLine = "";
            int counter = 0;

            try
            {
                using (StreamReader sr = new StreamReader(inputFile))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        subLine = line.Substring(line.IndexOf(':') + 1, line.Length - (line.IndexOf(':') + 1));
                        subLine = subLine.Trim();
                        AllWidths[counter] = Convert.ToInt32(subLine);
                        counter++;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "The file " + inputFile + " could not be read:");
            }
        }

        private string charArraytoString(char[] chars)
        {
            string outString = "";

            for (int i = 0; i < chars.Length; i++)
            {
                outString = outString + "" + chars[i];
            }

            return outString;
        }

        private int calcStringSize(string aString)
        {
            int totalSize = 0;

            foreach (char c in aString)
            {
                //The size of the character being used
                totalSize += AllWidths[AllCharsString.IndexOf(c)];
                //The padding between characters
                totalSize += 1;
            }

            return totalSize;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (richTextBox1.Text.Length > 0)
            {
                totalSizeLabel.Text = calcStringSize(richTextBox1.Text).ToString();
            }
            else
            {
                totalSizeLabel.Text = "0";
            }
        }
    }
}
