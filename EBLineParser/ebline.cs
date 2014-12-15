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
        private List<List<int>> AllWidths;

        // The place we will keep all the ccs files
        Dictionary<string, Dictionary<string, string>> CCSFiles;

        //A string that contains all the characters, with the same order as AllChars
        private const string AllCharsString =  " !\"#$%&\\()*+,-./0123456789:;<=>?" + 
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
            if (!aPath.Equals(string.Empty))
                coilPath = aPath;
        }

        public void SetLogPath(string aPath)
        {
            logPath = aPath;
        }

        //Reads all the characters and widths from the widths file
        public string readWidths()
        {
            string[] widthFiles = Directory.GetFiles(Path.Combine(coilPath, "Fonts"), "*.yml");
            AllWidths = new List<List<int>>();
            AllChars = new char[96];
            string line = "";
            string subLine = "";
            string subLine2 = "";

            AllChars = AllCharsString.ToCharArray();

            foreach (string aFile in widthFiles)
            {
                try
                {
                    List<int> tempList = new List<int>();
                    using (StreamReader sr = new StreamReader(aFile))
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
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
                            tempList.Add(Convert.ToInt32(subLine));
                        }
                        sr.Close();
                    }
                    AllWidths.Add(tempList);
                }
                catch (Exception e)
                {
                    return "" + e.Message + ": The file " + aFile + " could not be read:";
                }
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
                    totalSize += AllWidths[0][AllCharsString.IndexOf(c)];
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

            Regex charCode = new Regex("\\[[aA5-9][a-fA-F0-9]\\]");

            // Replace character codes
            foreach (Match match in charCode.Matches(normalizedString))
            {
                if (match.Success)
                {
                    normalizedString = normalizedString.Replace(match.Value, AllCharsString[AllCharsString.IndexOf(AllChars.Max())].ToString());
                }
            }

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

            string[] lineEnders = new string[10];
            //Setup line enders to be removed in string length assessment
            lineEnders[0] = "\" next";
            lineEnders[1] = "\" eob";
            lineEnders[2] = "\" end";
            lineEnders[3] = "\" linebreak";
            lineEnders[4] = "\" newline";
            lineEnders[5] = "  next";
            lineEnders[6] = "  eob";
            lineEnders[7] = "  end";
            lineEnders[8] = "  linebreak";
            lineEnders[9] = "  newline";

            //Regex patterns to completely remove
            Regex controlCode = new Regex("(\\[[0-9A-Fa-f]{2}( [0-9A-Fa-f]{2})* |goto|call)" +
                "((\\{e)*\\(.*\\)(\\})*)+(\\])*");
            Regex rawCC = new Regex("\\[[ ]*([0-9A-Fa-f]{2}[ ]*?|\\{\\}[ ]*?)*\\]");
            Regex CCScript = new Regex("[a-z_]+\\([a-z_0-9.]+\\)");
            Regex CCScriptInline = new Regex("{[a-z_\\(\\)0-9 ]+}");
            Regex extraCode = new Regex("\\[([0-9A-F]{2}[ ]*)+\\]");

            if (aString.Replace("\"", "").Length >= 2)
            {

                foreach (string ender in lineEnders)
                {
                    aString = aString.Replace(ender, "");
                }

                aString = aString.Replace("\"", "");
                aString = aString.Replace("  ", " ");

                // Replace control codes
                foreach (Match match in controlCode.Matches(aString))
                {
                    if (match.Success)
                    {
                        aString = aString.Replace(match.Value, "");
                    }
                }

                // Replace raw control codes
                foreach (Match match in rawCC.Matches(aString))
                {
                    if (match.Success)
                    {
                        aString = aString.Replace(match.Value, "");
                    }
                }

                //Handle Inline CCScript
                foreach (Match match in CCScriptInline.Matches(aString))
                {
                    if (match.Success)
                    {
                        aString = aString.Replace(match.Value, "");
                    }
                }

                //Handle other CCScript
                foreach (Match match in CCScript.Matches(aString))
                {
                    if (match.Success)
                    {
                        aString = aString.Replace(match.Value, "");
                    }
                }

                
                // Get rid of any left over CC stuff
                foreach (Match match in extraCode.Matches(aString))
                {
                    if (match.Success)
                    {
                        aString = aString.Replace(match.Value, "");
                    }
                }

                return aString;
            }
            return "";
        }

        public string ReplaceCCS(string file, string text, int runs)
        {
            string aString = text;

            //Regex patterns to completely remove
            Regex controlCode = new Regex("(\\[[0-9A-Fa-f]{2}( [0-9A-Fa-f]{2})* |goto|call)" +
                "((\\{e)*\\(.*\\)(\\})*)+(\\])*");
            Regex name = new Regex("name\\([0-9]+?\\)");
            Regex nameCC = new Regex("\\[1C 02 [0-9ABCDEF]{2}\\]");
            Regex item = new Regex("itemname\\([0-9]+\\)");
            Regex itemCC = new Regex("\\[1C 05 [0-9ABCDEF]{2}\\]");

            foreach (Match match in controlCode.Matches(aString))
            {
                if (match.Success)
                {
                    aString = aString.Replace(match.Value, loadControlCode(file, match.Value, runs));
                }
            }

            // Replace item stuff
            foreach (Match match in item.Matches(aString))
            {
                if (match.Success)
                {
                    int number = Convert.ToInt32(match.Value.Substring(match.Value.IndexOf("itemname(") + 
                        9, match.Value.IndexOf(')') - match.Value.IndexOf("itemname(")));
                    aString = aString.Replace(match.Value, loadItem(number));
                }
            }

            foreach (Match match in itemCC.Matches(aString))
            {
                if (match.Success)
                {
                    int number = Convert.ToInt32(match.Value.Substring(match.Value.IndexOf("1C 05") + 6, 2), 16);
                    aString = aString.Replace(match.Value, loadItem(number));
                }
            }

            // Replace Name stuff
            foreach (Match match in name.Matches(aString))
            {
                if (match.Success)
                {
                    aString = aString.Replace(match.Value, String.Format("{0}{0}{0}{0}{0}", AllCharsString[AllCharsString.IndexOf(AllChars.Max())]));
                }
            }

            foreach (Match match in nameCC.Matches(aString))
            {
                if (match.Success)
                {
                    aString = aString.Replace(match.Value, String.Format("{0}{0}{0}{0}{0}", AllCharsString[AllCharsString.IndexOf(AllChars.Max())]));
                }
            }

            return aString;
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
            string[] delimiters = new string[] { Environment.NewLine, "{wait}", "{prompt}", "{next}", "[03]" };
            CCSFiles = loadCCScriptFiles();

            StreamWriter sw = new StreamWriter(@fullFile, false);

            string[] rows = new string[numRows];

            foreach (KeyValuePair <string, Dictionary<string, string>> kvp in CCSFiles)
            {
                foreach (KeyValuePair <string, string> svp in kvp.Value)
                {
                    string aString = svp.Value;

                    // Load all the referenced text into the string
                    aString = ReplaceCCS(kvp.Key, aString, 0);

                    string[] lines = aString.Split(delimiters, StringSplitOptions.None);

                    foreach (string curString in lines)
                    {
                        string tempString = curString;

                        // Get rid of accented characters
                        tempString = RemoveDiacritics(tempString);

                        // Get rid of any extra codes at this point
                        tempString = RemoveCCS(tempString);

                        for (int i = 0; i < 3; i++)
                        {

                            while (tempString.IndexOf(' ') == 0 | tempString.IndexOf('@') == 0)
                            {
                                tempString = tempString.Substring(1);
                            }

                            rows[i] = lineExtractor(tempString);

                            if (rows[i].Length > 0)
                                tempString = tempString.Substring(rows[i].Length);

                            if (tempString.Length == 0)
                            {
                                i = 3;
                            }
                        }

                        if (lineExtractor(tempString).Length > 0)
                        {
                            sw.WriteLine("=====================");
                            sw.WriteLine("File: " + kvp.Key);
                            sw.WriteLine("Label: " + svp.Key);
                            for (int p = 0; p < numRows; p++)
                            {
                                sw.WriteLine("Row {0}: {1}", p, rows[p]);
                            }
                            sw.WriteLine("Excess Text: " + tempString);
                            sw.WriteLine("Excess Pixels: " + calcStringSize(tempString).ToString());
                        }

                    }
                }
            }
            sw.Close();
        }

        private string loadControlCode(string file, string aCode, int runs)
        {
            Match aMatch;
            Match aLabel;

            string newFile = "";
            string label = "";

            Regex[] replaces = new Regex[3];
            //Regex patterns to be replaced with text from somewhere else:
            replaces[0] = new Regex("call\\(.*\\)");
            replaces[1] = new Regex("goto\\(.*\\)");
            replaces[2] = new Regex("\\[09.*\\]");

            //Regex pattern to use to pull out label references
            Regex labelRef = new Regex("\\([^\\)]*\\)");

            foreach (Regex aReg in replaces)
            {
                newFile = "";
                aMatch = aReg.Match(aCode);

                if (aMatch.Success)
                {
                    aLabel = labelRef.Match(aCode);

                    if (aLabel.Success)
                    {
                        if (aLabel.Value.Contains('.'))
                        {
                            string[] split = aLabel.Value.Split('.');
                            newFile = split[0].Replace("(", "");
                            label = split[1].Replace("(", "").Replace(")", "");
                        }
                        else
                        {
                            newFile = file;
                            label = aLabel.Value.Replace("(", "").Replace(")", "");
                        }

                        if (runs < 10)
                            return ReplaceCCS(newFile, CCSFiles[newFile][label], ++runs);
                        else
                            return RemoveCCS(CCSFiles[newFile][label]);
                    }
                }
            }

            //If the code doesn't match any of the above, pass the original code back in case it's needed to split the string
            return aCode;
        }

        private Dictionary<string, Dictionary<string, string>> loadCCScriptFiles()
        {
            //Setup reader
            //TODO Setup Bad Path handling here
            var CCSFiles = new Dictionary<string, Dictionary<string, string>>();
            string[] dataFiles = Directory.GetFiles(coilPath, "ccscript\\*.ccs");
            string aLabel = "";
            Regex label = new Regex("^[a-zA-Z]+[_a-zA-Z0-9]*:");

            foreach (string aFile in dataFiles)
            {
                StreamReader sr = new StreamReader(aFile);
                string curString = "";

                string fileName = Path.GetFileNameWithoutExtension(aFile);

                CCSFiles.Add(fileName, new Dictionary<string,string>());

                curString = sr.ReadLine();

                while (!sr.EndOfStream)
                {
                    if (label.Match(curString).Success)
                    {
                        aLabel = curString.Replace(":", "");
                        curString = sr.ReadLine();
                        string content = "";

                        while (!label.Match(curString).Success & !sr.EndOfStream)
                        {
                            content += curString + Environment.NewLine;
                            curString = sr.ReadLine();
                        }
                        CCSFiles[fileName].Add(aLabel, content);
                    }
                    else
                    {
                        curString = sr.ReadLine();
                    }
                }
            }

            return CCSFiles;
        }

        private string loadItem(int aNum)
        {
            string aPath = Path.Combine(coilPath, "item_configuration_table.yml");
            string curLine = "";
            string name = "Name:";
            StreamReader itemStream = new StreamReader(aPath);

            while (!itemStream.EndOfStream)
            {
                curLine = itemStream.ReadLine();

                if(curLine.Equals(aNum + ":"))
                {
                    while (!itemStream.EndOfStream)
                    {
                        curLine = itemStream.ReadLine();
                        if(curLine.Contains(name))
                        {
                            return curLine.Substring(curLine.IndexOf(name) + name.Length);
                        }
                    }
                }
            }

            return "AAAAAAAAAAAAAAAAAAAAAAAAA";
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
