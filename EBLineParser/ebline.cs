using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Threading.Tasks;

namespace EBLineParser
{
    public class ebline
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
        //The font number currently in use
        private int fontNum = 0;
        
        //A list of all the Characters supported by the EBLINE class
        private char[] AllChars;
        //A list of all the widths, with the indexes corresponding with AllChars
        private List<List<int>> AllWidths;
        //A dictionary of all the item names as keys and their types as values
        Dictionary<string, string> AllItems;

        // The place we will keep all the ccs files
        Dictionary<string, Dictionary<string, string>> CCSFiles;

        //A string that contains all the characters, with the same order as AllChars
        private string AllCharsString =  Properties.Settings.Default.fontString;
        //A string that contains all the expanded font codes
        private const string ExpandedCharsString = "" +
        "[b0],[b1],[b2],[b3],[b4],[b5],[b6],[b7],[b8],[b9],[ba],[bb],[bc],[bd],[be],[bf]," +
        "[c0],[c1],[c2],[c3],[c4],[c5],[c6],[c7],[c8],[c9],[ca],[cc],[cc],[cd],[ce],[cf]";
        //The Default pixels you can have in a row
        private const int rowDefault = 132;
        //The normal width/height of a tile
        private const int tilePixels = 8;
        // The amount of pixels we need to indent by if not the first line
        private const int indent = 6;
        // The number of pixels wide an equippable item can be
        private const int equipSize = 71;
        // The number of pixels wide a normal item can be
        private const int itemSize = 79;

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

        public string getCoilPath()
        {
            return coilPath;
        }

        public int getIndent()
        {
            return indent;
        }

        public int getNumFonts()
        {
            return AllWidths.Count;
        }

        public int getRowMax()
        {
            return numRows;
        }

        public void SetCoilPath(string aPath)
        {
            if (!aPath.Equals(string.Empty))
                coilPath = aPath;
        }

        public void SetCurrentFont(int aFont)
        {
            fontNum = aFont;
            calculateWindowSize(1);
        }

        public void SetLogPath(string aPath)
        {
            logPath = aPath;
        }

        //Reads all the item names from item_configuration_table.yml
        public string readItems()
        {
            string itemPath = Path.Combine(coilPath, "item_configuration_table.yml");
            string line = "";
            string modifiedLine = "";
            string typeValue = "";
            int type = 0;
            string warning = "";
            AllItems = new Dictionary<string, string>();

            try
            {
                using (StreamReader sr = new StreamReader(itemPath))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Contains("Name: "))
                        {
                            modifiedLine = line.Replace("  Name: ", "");
                            modifiedLine = modifiedLine.Replace("\"", "");
                            typeValue = sr.ReadLine();

                            typeValue = typeValue.Replace("  Type: ", "");
                            type = Convert.ToInt32(typeValue);

                            if (type > 8 && type < 32)
                            {
                                try
                                {
                                    AllItems.Add(modifiedLine, "equip");
                                }
                                catch (System.ArgumentException e)
                                {
                                    // Do nothing, we will skip duplicates
                                }

                            }
                            else
                            {
                                try
                                {
                                    AllItems.Add(modifiedLine, "normal");
                                }
                                catch (System.ArgumentException e)
                                {
                                    // Do Nothing, we'll skip any items with duplicate names
                                }
                                
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return "" + e.Message + ": The file " + itemPath + " could not be read:";
            }
            return "";
        }

        //Reads all the characters and widths from the widths file
        public string readWidths()
        {
            string[] widthFiles = Directory.GetFiles(Path.Combine(coilPath, "Fonts"), "*.yml");
            AllWidths = new List<List<int>>();
            AllChars = AllCharsString.ToCharArray();
            string line = "";
            string subLine = "";
            string subLine2 = "";
            string lineComment = "#.*";

            foreach (string aFile in widthFiles)
            {
                try
                {
                    List<int> tempList = new List<int>();
                    using (StreamReader sr = new StreamReader(aFile))
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            line = Regex.Replace(line, lineComment, ""); // Removing single-line comments
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
            char c = ' ';
            string expanded = "";
            Regex expandedChar = new Regex("\\[[5-9A-Ca-c][0-9A-Fa-f]\\]");

            aString = RemoveDiacritics(aString);

            for (int i = 0; i < aString.Length; i++)
            {
                c = aString.ToArray()[i];
                if (i + 3 < aString.Length)
                    expanded = aString.Substring(i, 4);
                else
                    expanded = "";

                if (expandedChar.IsMatch(expanded))
                {
                    totalSize += AllWidths[fontNum][Array.IndexOf(ExpandedCharsString.Split(','), expanded.ToLower()) + 96];
                    i += 3;
                }
                else if (AllCharsString.IndexOf(c) > -1)
                {
                    //The size of the character being used
                    totalSize += AllWidths[fontNum][AllCharsString.IndexOf(c)];
                }
                //The padding between characters
                totalSize += 1;
            }

            return totalSize;
        }

        //Returns the first string that cleanly fits on one line
        public string lineExtractor(string aString, bool firstLine)
        {
            int padding = 0;

            if (calcStringSize(aString) + padding > lineSize &&
                !aString.Contains(' '))
            {
                return "ERROR_LINE_OVERFLOW";
            }

            if (!firstLine)
                padding = indent;

            if (calcStringSize(aString) + padding < lineSize)
            {
                return aString;
            }
            else
            {
                string theLine = "";
                string tempString = aString;
                string aWord = "";
                bool lineNotFound = true;

                // Remove spaces when wrapping to the next row just like the game does
                if (!firstLine)
                {
                    while (tempString.IndexOf(' ') == 0)
                    {
                        tempString = tempString.Substring(1);
                    }
                }

                while (lineNotFound && !tempString.Equals(""))
                {
                    aWord = nextWord(tempString);
                    tempString = tempString.Substring(aWord.Length);
                    if (calcStringSize(theLine + aWord) + padding > lineSize)
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

        public ArrayList ParseString(string text)
        {
            string tempString = text;
            bool firstLine;
            ArrayList rows = new ArrayList();

            // Get rid of accented characters
            tempString = RemoveDiacritics(tempString);

            // Get rid of any extra codes at this point
            tempString = RemoveCCS(tempString);

            for (int i = 0; i < numRows; i++)
            {
                if (i == 0)
                    firstLine = true;
                else
                    firstLine = false;

                rows.Add(lineExtractor(tempString, firstLine));

                if (rows[i].ToString().Length > 0)
                    tempString = tempString.Substring(rows[i].ToString().Length);

                if (tempString.Length == 0)
                {
                    i = numRows;
                }
            }
            if (tempString.Length != 0)
                rows.Add(tempString);
            return rows;
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
            Regex rawCC = new Regex("\\[[ ]*?([1-4DEFdef][0-9A-Fa-f])[ ]*?([0-9A-Fa-f]{2}[ ]*?|\\{\\}[ ]*?)*\\]");
            Regex CCScript = new Regex("[a-z_]+\\([a-z_0-9.]+\\)");
            Regex CCScriptInline = new Regex("{[a-z_\\(\\)0-9 ]+}");
            Regex extraCode = new Regex("\\[([0-4DEFdef]{2}[ ]*)+\\]");

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

        public void startCCS(string a_coilPath, string a_logPath)
        {
            logPath = a_logPath;
            coilPath = Path.Combine(a_coilPath);

            calculateWindowSize(1);

            logCCS();
        }

        public void startItems(string a_coilPath, string a_logPath)
        {
            logPath = a_logPath;
            coilPath = Path.Combine(a_coilPath);

            calculateWindowSize(1);

            logItems();
        }

        private void logCCS()
        {
            //Setup log file
            string fileName = "cssOverflow_" + DateTime.Now.ToString("MMyyHHmmss") + ".log";
            string fullFile = Path.Combine(logPath, fileName);
            string[] delimiters = new string[] { Environment.NewLine, "{wait}", "{prompt}", "{next}", "[03]" };
            bool firstLine;
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

                        // Get rid of leading spaces from line indentation
                        while (tempString.IndexOf(' ') == 0)
                        {
                            tempString = tempString.Substring(1);
                        }
                        // Get rid of accented characters
                        tempString = RemoveDiacritics(tempString);

                        // Get rid of any extra codes at this point
                        tempString = RemoveCCS(tempString);

                        for (int i = 0; i < numRows; i++)
                        {
                            if (i == 0)
                                firstLine = true;
                            else
                                firstLine = false;

                            rows[i] = lineExtractor(tempString, firstLine);

                            if (rows[i].Length > 0)
                                tempString = tempString.Substring(rows[i].Length);

                            if (tempString.Length == 0)
                            {
                                i = numRows;
                            }
                        }

                        if (calcStringSize(tempString) > 0)
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

        private void logItems()
        {
            //Setup log file
            string fileName = "itemOverflow_" + DateTime.Now.ToString("MMyyHHmmss") + ".log";
            string fullFile = Path.Combine(logPath, fileName);
            string[] delimiters = new string[] { Environment.NewLine, "{wait}", "{prompt}", "{next}", "[03]" };

            StreamWriter sw = new StreamWriter(@fullFile, false);

            foreach (KeyValuePair<string, string> item in AllItems)
            {
                if (item.Value.Equals("normal"))
                {
                    if (calcStringSize(item.Key) > itemSize)
                    {
                        sw.WriteLine("=====================");
                        sw.WriteLine("Item Name: " + item.Key);
                        sw.WriteLine("Type: Non-Equipment");
                        sw.WriteLine("Name Size: " + calcStringSize(item.Key).ToString());
                        sw.WriteLine("Excess Pixels: " + (calcStringSize(item.Key) - itemSize).ToString());
                    }
                }

                if (item.Value.Equals("equip"))
                {
                    if (calcStringSize(item.Key) > equipSize)
                    {
                        sw.WriteLine("=====================");
                        sw.WriteLine("Item Name: " + item.Key);
                        sw.WriteLine("Type: Equipment");
                        sw.WriteLine("Name Size: " + calcStringSize(item.Key).ToString());
                        sw.WriteLine("Excess Pixels: " + (calcStringSize(item.Key) - equipSize).ToString());
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
            Regex label = new Regex("^[^0-9]\\S*:");
            Regex commentBlockStart = new Regex("\\/\\*");
            Regex commentBlockEnd = new Regex("\\*\\/");
            string upUntilBlockEnd = ".*\\*\\/";
            string afterBlockStart = "\\/\\*.*";
            bool inCommentBlock = false;

            foreach (string aFile in dataFiles)
            {
                StreamReader sr = new StreamReader(aFile);
                string curString = "";

                string fileName = Path.GetFileNameWithoutExtension(aFile);

                CCSFiles.Add(fileName, new Dictionary<string,string>());

                curString = sr.ReadLine();

                while (!sr.EndOfStream)
                {
                    if (commentBlockStart.Match(curString).Success | inCommentBlock)
                    {
                        if(commentBlockStart.Match(curString).Success && !inCommentBlock)
                        {
                            inCommentBlock = true;
                            curString = Regex.Replace(curString, afterBlockStart, "");
                        }
                        if (commentBlockEnd.Match(curString).Success && inCommentBlock)
                        {
                            inCommentBlock = false;
                            curString = Regex.Replace(curString, upUntilBlockEnd, "");
                        }
                    }
                    Match match = label.Match(curString);
                    if (match.Success)
                    {
                        aLabel = match.Value;
                        aLabel = aLabel.Replace(":", "");
                        curString = sr.ReadLine();
                        string content = "";

                        while (!label.Match(curString).Success & !sr.EndOfStream)
                        {
                            content += curString + Environment.NewLine;
                            curString = sr.ReadLine();
                        }
                        try
                        {
                            CCSFiles[fileName].Add(aLabel, content);
                        }
                        catch (ArgumentException ex)
                        {
                            // Skip the duplicate
                        }
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

        private void calculateWindowSize(int window)
        {
            string aPath = Path.Combine(coilPath, "window_configuration_table.yml");
            Regex aNum = new Regex("[0-9]+");

            StreamReader windowStream = new StreamReader(aPath);

            string curLine;

            while (!windowStream.EndOfStream)
            {
                curLine = windowStream.ReadLine();

                if (curLine.Equals(window.ToString() + ":"))
                {
                    windowHeight = Convert.ToInt32(aNum.Match(windowStream.ReadLine()).Value);

                    windowWidth = Convert.ToInt32(aNum.Match(windowStream.ReadLine()).Value);

                    windowXOffset = Convert.ToInt32(aNum.Match(windowStream.ReadLine()).Value);

                    windowYOffset = Convert.ToInt32(aNum.Match(windowStream.ReadLine()).Value);

                    windowStream.Close();

                    //22 is the number of pixels lost to indentation and window borders
                    lineSize = ( windowWidth * tilePixels) - 16;

                    //13 is the number of pixels lost to indentation and window borders
                    numRows = (( windowHeight * tilePixels) - 13) / 16;

                    break;
                }
            }
        }
    }
}
