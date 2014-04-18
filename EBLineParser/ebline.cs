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
        //A string that represents the path where your CCS project is kept
        private string coilPath;
        //A string that represents the path where your log will be generated
        private string logPath;
        //A boolean to determine whether we are using Saturn font or not
        private bool isSaturn;
        //The tile height of our text window
        private int windowHeight;
        //The tile width of our text window
        private int windowWidth;
        //The X offset of our window
        private int windowXOffset;
        //The Y offset of our window
        private int windowYOffset;
        //The number of rows that fit in a window
        private int numRows;
        
        //A list of all the Characters supported by the EBLINE class
        private char[] AllChars;
        //A list of all the widths, with the indexes corresponding with AllChars
        private int[] AllWidths;
        //A list of all the Saturn widths
        private int[] AllWidthsSaturn;

        //A string that contains all the characters, with the same order as AllChars
        private const string AllCharsString =  "!\"#$%&\\()*+,-./0123456789:;<=>?" + 
            "@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~¬";
        //The Default pixels you can have in a row
        private const int rowDefault = 132;
        //The normal width/height of a tile
        private const int tilePixels = 8;

        //The Maximum pixels you can have in an item name
        //private const int itemMax = 71;
        //Forgot what this was for
        //private const int dftLineSize = 30;
        //The default file that contains characters and their widths
        //private const string dftFileName = "widths.cfg";

        public ebline()
        {
            lineSize = rowDefault;
        }

        public ebline(int aSize)
        {
            lineSize = aSize;
        }

        public int getRowMax()
        {
            return rowDefault;
        }

        public void SetCoilPath(string aPath)
        {
            coilPath = aPath;
        }

        public void SetLogPath(string aPath)
        {
            logPath = aPath;
        }

        //Reads all the characters and widths from the widths file
        public string readWidths()
        {
            AllWidths = new int[96];
            AllWidthsSaturn = new int[96];
            AllChars = new char[96];
            string line = "";
            string subLine = "";
            string subLine2 = "";
            int counter = 0;
            string aFile;

            AllChars = AllCharsString.ToCharArray();

            aFile = Path.Combine(coilPath, "Fonts\\0_widths.yml");

            try
            {
                using (StreamReader sr = new StreamReader(aFile))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        //AllChars[counter] = line[0];
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
                    sr.Close();
                }
            }
            catch (Exception e)
            {
                return "" + e.Message + ": The file " + aFile + " could not be read:";
            }

            aFile = Path.Combine(coilPath, "Fonts\\1_widths.yml");
            counter = 0;

            try
            {
                using (StreamReader sr = new StreamReader(aFile))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        //AllChars[counter] = line[0];
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
                        AllWidthsSaturn[counter] = Convert.ToInt32(subLine);
                        counter++;
                    }
                    sr.Close();
                }
            }
            catch (Exception e)
            {
                return "" + e.Message + ": The file " + aFile + " could not be read:";
            }

            return "";
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

            string[] lineEnders = new string[4];
            Regex controlCode = new Regex("\\[[0-9A-Fa-f]{2}( [0-9A-Fa-f]{2})*" +
                "( \\{e\\((data_[0-9]{2}.)*l_0x[0-9A-Fa-f]{6}\\)\\})*\\]");
            Regex CCScript = new Regex("[a-z_]+\\([a-z_0-9.]+\\)");
            Regex CCScriptInline = new Regex("{[a-z_\\(\\)0-9 ]+}");


            //Setup line enders to be removed in string length assessment
            lineEnders[0] = "\" next";
            lineEnders[1] = "\" eob";
            lineEnders[2] = "\" end";
            lineEnders[3] = "\" linebreak";

            if (aString.Length - aString.Replace("\"", "").Length >= 2)
            {

                foreach (string ender in lineEnders)
                {
                    aString = aString.Replace(ender, "");
                }

                aString = aString.Replace("\"", "");
                aString = aString.Replace("  ", " ");

                //Handle Control Codes
                foreach (Match match in controlCode.Matches(aString))
                {
                    if (match.Success)
                    {
                        aString = aString.Replace(match.Value, loadControlCode(match.Value));
                    }
                }

                //Handle Inline CCScript
                foreach (Match match in CCScriptInline.Matches(aString))
                {
                    if (match.Success)
                    {
                        aString = aString.Replace(match.Value, loadCCScript(match.Value));
                    }
                }

                //Handle other CCScript
                foreach (Match match in CCScript.Matches(aString))
                {
                    if (match.Success)
                    {
                        aString = aString.Replace(match.Value, loadCCScript(match.Value));
                    }
                }

                return aString;
            }



            return "";
        }

        public void startCCS(string a_ccsPath, string a_logPath)
        {
            logPath = a_logPath;
            coilPath = Path.Combine(a_ccsPath);

            calculateWindowSize();

            readCCS();
        }

        private void readCCS()
        {
            //Setup log file
            string fileName = "cssOverflow_" + DateTime.Now.ToString("MMyyHHmmss") + ".log";
            string fullFile = Path.Combine(logPath, fileName);

            StreamWriter sw = new StreamWriter(@fullFile, false);
            
            //Setup reader
            //TODO Setup Bad Path handling here
            string[] dataFiles = Directory.GetFiles(coilPath, "ccscript\\Data_*.ccs");

            string[] rows = new string[numRows];

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

                    if (lineExtractor(tempString).Length > 0)
                    {
                        sw.WriteLine("=====================");
                        sw.WriteLine("File: " + aFile);
                        sw.WriteLine("Line: " + line);
                        for (int p = 0; p < numRows; p++)
                        {
                            sw.WriteLine("Row {0}: {1}", p, rows[p]);
                        }
                        sw.WriteLine("Excess Text: " + tempString);
                        sw.WriteLine("Excess Pixels: " + calcStringSize(tempString).ToString());
                    }
                }
            }
            sw.Close();
        }

        private string loadControlCode(string aCode)
        {
            Regex[] loadThese = new Regex[10];

            //Setup regular expressions that need to be replaced w/ strings from another location
            loadThese[0] = new Regex("call\\([A-Za-z]_0x[0-9A-Fa-f]{6}\\)");
            loadThese[1] = new Regex("\\[1C 02 [0-9]{2}\\]");
            loadThese[2] = new Regex("\\[1C 05 [0-9A-Fa-f]{2}\\]");
            //TODO [06 ...]
            //TODO [08 ...]
            //TODO [09 ...]
            //TODO [0A ...]

            

            //If the code doesn't match any of the above, pass an empty string which will be used
            //to likely remove the code from the string to be assessed.
            return "";
        }

        private string loadCCScript(string aCode)
        {
            if(aCode.Contains("name("))
            {
                //TODO Handle naming_skip.yml
                //StreamReader skipReader = new StreamReader(Path.Combine(coilPath,"\\naming_skip.yml"));
                string wideName = String.Format("{0}{0}{0}{0}{0}",AllCharsString[AllCharsString.IndexOf(AllChars.Max())]);
                return wideName;    
            }

            if(aCode.Contains("itemname("))
            {

            }

            return "";
        }

        private void calculateWindowSize()
        {
            string aPath = Path.Combine(coilPath, "window_configuration_table.yml");
            Regex aNum = new Regex("[0-9]+");

            StreamReader windowStream = new StreamReader(aPath);

            string curLine;

            while (!windowStream.EndOfStream)
            {
                curLine = windowStream.ReadLine();

                if (curLine.Equals("1:"))
                {
                    windowHeight = Convert.ToInt32(aNum.Match(windowStream.ReadLine()).Value);

                    windowWidth = Convert.ToInt32(aNum.Match(windowStream.ReadLine()).Value);

                    windowXOffset = Convert.ToInt32(aNum.Match(windowStream.ReadLine()).Value);

                    windowYOffset = Convert.ToInt32(aNum.Match(windowStream.ReadLine()).Value);

                    windowStream.Close();

                    //20 is the number of pixels lost to indentation and window borders
                    lineSize = ( windowWidth * tilePixels) - 20;

                    //13 is the number of pixels lost to indentation and window borders
                    numRows = (( windowHeight * tilePixels) - 13) / 16;

                    break;
                }
            }
        }
    }
}
