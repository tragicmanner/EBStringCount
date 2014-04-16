using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Threading.Tasks;

namespace EBLineParser
{
    class ebline
    {
        /*//////////////////////////////////////////////////////////////////////////////
         * ERROR CODES
         * 
         * ERROR_LINE_OVERFLOW - String contains a single word that is longer than a line
         *      allows
         * 
         * ///////////////////////////////////////////////////////////////////////////*/


        //The maximum pixels a line can have in the text box.
        private int lineSize;
        //The File we are reading Characters and Widths from
        private string inputFile;
        //A string that represents the path where your CCS project is kept
        private string ccsPath;
        //A string that represents the path where your log will be generated
        private string logPath;
        
        //A list of all the Characters supported by the EBLINE class
        private char[] AllChars;
        //A list of all the widths, with the indexes corresponding with AllChars
        private int[] AllWidths;
        //A string that contains all the characters, with the same order as AllChars
        private string AllCharsString;

        //The Maximum pixels you can have in a row
        private const int rowMax = 132;
        //The Maximum pixels you can have in an item name
        private const int itemMax = 71;
        //Forgot what this was for
        private const int dftLineSize = 30;
        //The default file that contains characters and their widths
        private const string dftFileName = "widths.cfg";

        public ebline()
        {
            inputFile = dftFileName;
            lineSize = rowMax;
        }

        public ebline(string aFile)
        {
            inputFile = aFile;
            lineSize = rowMax;
        }

        public ebline(string aFile, int aSize)
        {
            inputFile = aFile;
            lineSize = aSize;
        }

        public ebline(int aSize)
        {
            inputFile = dftFileName;
            lineSize = aSize;
        }

        public int getRowMax()
        {
            return rowMax;
        }

        public int getItemMax()
        {
            return itemMax;
        }

        public int getDefaultLineSize()
        {
            return dftLineSize;
        }

        //Reads all the characters and widths from the widths file
        public string readWidths()
        {
            AllWidths = new int[96];
            AllChars = new char[96];
            string line = "";
            string subLine = "";
            string subLine2 = "";
            int counter = 0;

            try
            {
                using (StreamReader sr = new StreamReader(inputFile))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        AllChars[counter] = line[0];
                        if (line.Contains("::"))
                        {
                            subLine2 = line.Substring(line.IndexOf(':') + 1, line.Length - (line.IndexOf(':') + 1));
                            subLine = subLine2.Substring(subLine2.IndexOf(':') + 1, subLine2.Length - (subLine2.IndexOf(':') + 1));
                        }
                        else
                        {
                            subLine = line.Substring(line.IndexOf(':') + 1, line.Length - (line.IndexOf(':') + 1));
                        }
                        subLine = subLine.Trim();
                        AllWidths[counter] = Convert.ToInt32(subLine);
                        counter++;
                    }
                }
            }
            catch (Exception e)
            {
                return "" + e.Message + ": The file " + inputFile + " could not be read:";
            }

            AllCharsString = charArraytoString(AllChars);
            return "";
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

        //Calcs the number of pixels a string of characters will take up
        //Does not worry about max line size and words being carried to newline
        public int calcStringSize(string aString)
        {
            int totalSize = 0;

            aString = RemoveDiacritics(aString);

            foreach (char c in aString)
            {
                if (AllCharsString.IndexOf(c) > -1)
                {
                    //The size of the character being used
                    totalSize += AllWidths[AllCharsString.IndexOf(c)];
                }
                //The padding between characters
                totalSize += 1;
            }

            return totalSize;
        }

        //Returns the first string that cleanly fits on one line
        public string lineExtractor(string aString)
        {
            if (aString.IndexOf(' ') == 0 | aString.IndexOf('@') == 0)
            {
                aString = aString.Substring(1);
            }

            if (calcStringSize(aString) > lineSize &&
                !aString.Contains(' '))
            {
                return "ERROR_LINE_OVERFLOW";
            }

            if (calcStringSize(aString) <= lineSize)
            {
                return aString;
            }
            else
            {
                string theLine = "";
                string tempString = aString;
                string aWord = "";
                bool lineNotFound = true;

                while (lineNotFound && !tempString.Equals(""))
                {
                    aWord = nextWord(tempString);
                    tempString = tempString.Substring(aWord.Length);
                    if (calcStringSize(theLine + aWord) > lineSize)
                    {

                        lineNotFound = false;
                    }
                    else
                    {
                        theLine = theLine + aWord;
                    }

                    if (aWord.Equals(tempString))
                    {
                        lineNotFound = false;
                    }
                }

                return theLine;
                
            }
        }

        public string nextWord(string aString)
        {
            string nextWord = "";
            int wordBoundary = 0;

            if (!aString.Contains(' ') |
                !aString.Substring(1).Contains(' '))
                return aString;

            wordBoundary = aString.Substring(1).IndexOf(' ') + 1;
            nextWord = aString.Substring(0, wordBoundary);
            return nextWord;
        }

        public string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public string RemoveCCS(string text)
        {
            string aString = text;

            string itemName = "Mr. Baseball cap";
            string charName = "ABCDE";
            string[] lineEnders = new string[4];

            lineEnders[0] = "\" next";
            lineEnders[1] = "\" eob";
            lineEnders[2] = "\" end";
            lineEnders[3] = "\" linebreak";

            Regex callEx = new Regex("call\\([A-Za-z]_0x[0-9A-Fa-f]{6}\\)");
            Regex nameEx = new Regex("\\[1C 02 [0-9]{2}\\]");
            Regex itemEx = new Regex("\\[1C 05 [0-9A-Fa-f]{2}\\]");

            if (aString.Length - aString.Replace("\"", "").Length >= 2)
            {

                foreach (string ender in lineEnders)
                {
                    aString = aString.Replace(ender, "");
                }

                aString = aString.Replace("\"", "");
                aString = aString.Replace("  ", " ");

                aString = itemEx.Replace(aString, itemName);
                aString = nameEx.Replace(aString, charName);

                //Call replacement is going to be difficult because we need to
                //reference lines in other files at times. For now, just leaving it
                //for after I assess how much this can lengthen out a single line.
                /*if (callEx.IsMatch(aString))
                {

                }*/

                return aString;
            }



            return "";
        }

        public void startCCS(string a_ccsPath, string a_logPath)
        {
            logPath = a_logPath;
            ccsPath = a_ccsPath;

            readCCS();
        }

        private void readCCS()
        {
            //Setup log file
            string fileName = "cssOverflow_" + DateTime.Now.ToString("MMYYHHmmss") + ".log";
            string fullFile = Path.Combine(logPath, fileName);

            StreamWriter sw = new StreamWriter(@fullFile, false);
            
            //Setup reader
            string[] dataFiles = Directory.GetFiles(ccsPath, "Data_*.ccs");
            string row1 = "";
            string row2 = "";
            string row3 = "";

            string[] rows = new string[3] {"","",""};

            foreach (string aFile in dataFiles)
            {
                StreamReader sr = new StreamReader(aFile);
                string curString = "";
                int line = 0;

                while (!sr.EndOfStream)
                {
                    curString = sr.ReadLine();
                    line++;
                    string tempString = curString;

                    tempString = RemoveCCS(tempString);

                    tempString = RemoveDiacritics(tempString);

                    for (int i = 0; i < 3; i++)
                    {
                        
                        while (tempString.IndexOf(' ') == 0 | tempString.IndexOf('@') == 0)
                        {
                            tempString = tempString.Substring(1);
                        }

                        rows[i] = lineExtractor(tempString);

                        if(rows[i].Length > 0)
                            tempString = tempString.Substring(rows[i].Length);

                        if (tempString.Length == 0)
                        {
                            i = 3;
                        }
                    }

                    if (rows[0].Length > 0)
                    {
                        row1 = calcStringSize(rows[0]).ToString();
                    }

                    if (rows[1].Length > 0)
                    {
                        row2 = calcStringSize(rows[1]).ToString();
                    }

                    if (rows[2].Length > 0)
                    {
                        row3 = calcStringSize(rows[2]).ToString();
                    }

                    if (lineExtractor(tempString).Length > 0)
                    {
                        sw.WriteLine("=====================");
                        sw.WriteLine("File: " + aFile);
                        sw.WriteLine("Line: " + line);
                        sw.WriteLine("Row 1: " + rows[0]);
                        sw.WriteLine("Row 2: " + rows[1]);
                        sw.WriteLine("Row 3: " + rows[2]);
                        sw.WriteLine("Excess Text: " + tempString);
                        sw.WriteLine("Excess Pixels: " + calcStringSize(tempString).ToString());
                    }
                }
            }
            
        }
    }
}
