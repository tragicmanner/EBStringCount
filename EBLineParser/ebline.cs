using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private int lineSize { get; set; }
        //The File we are reading Characters and Widths from
        private string inputFile { get; set; }

        //A list of all the Characters supported by the EBLINE class
        private char[] AllChars;
        //A list of all the widths, with the indexes corresponding with AllChars
        private int[] AllWidths;
        //A string that contains all the characters, with the same order as AllChars
        private string AllCharsString;

        //The Maximum pixels you can have in a row
        private const int rowMax = 132;
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
                //The size of the character being used
                totalSize += AllWidths[AllCharsString.IndexOf(c)];
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

                while (lineNotFound)
                {
                    aWord = nextWord(tempString);
                    theLine = theLine + aWord;
                    tempString = tempString.Replace(aWord, "");
                    if (calcStringSize(theLine) > lineSize)
                    {
                        theLine = theLine.Replace(aWord, "");
                        lineNotFound = false;
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
    }
}
