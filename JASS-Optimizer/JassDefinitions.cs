using JassOptimizer;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;

namespace JASS_Optimizer
{
    internal class JassDefinitions
    {
        internal List<string> Keywords = new List<string>();
        private List<string> TypeWords = new List<string>();
        private string[] startsWithList;

        private bool isScanningTypewordLine = false;

        internal JassDefinitions(string[] commonJLines, string[] blizzardJLines)
        {
            startsWithList = new string[5];
            startsWithList[0] = "type";
            startsWithList[1] = "constant native";
            startsWithList[2] = "constant";
            startsWithList[3] = "native";
            startsWithList[4] = "function";

            #region Generic types

            AddTypewordManual("boolean");
            AddTypewordManual("integer");
            AddTypewordManual("real");
            AddTypewordManual("string");
            AddTypewordManual("array");

            #endregion


            for (int i = 0; i < commonJLines.Length; i++)
            {
                PrepareLine(commonJLines[i]);
            }

            for (int i = 0; i < blizzardJLines.Length; i++)
            {
                PrepareLine(blizzardJLines[i]);
            }
        }

        private void PrepareLine(string line)
        {
            line = line.Trim();

            // remove excess whitespace from line
            StringBuilder sb = new StringBuilder();
            bool wasWhiteSpace = false;
            for (int j = 0; j < line.Length; j++)
            {
                char c = line[j];

                if (char.IsWhiteSpace(c) && wasWhiteSpace)
                {
                    continue;
                }
                else
                {
                    wasWhiteSpace = false;
                }

                if (char.IsWhiteSpace(c))
                {
                    wasWhiteSpace = true;
                }
                sb.Append(c);
            }

            line = sb.ToString();

            bool wasKeywordFound = false;
            for (int j = 0; j < startsWithList.Length; j++)
            {
                isScanningTypewordLine = j == 0;
                wasKeywordFound = ScanLine(line, startsWithList[j]);
                if (wasKeywordFound)
                {
                    break;
                }
            }

            // Case where a line starts with a typeword, e.g. integer, rect, boolean etc. etc.
            if (!wasKeywordFound)
            {
                for (int i = 0; i < TypeWords.Count; i++)
                {
                    var typeWord = TypeWords[i];
                    wasKeywordFound = ScanLine(line, typeWord);
                    if (wasKeywordFound)
                    {
                        break;
                    }
                }
            }
        }

        private bool ScanLine(string line, string startWithWord)
        {
            bool foundKeyword = false;
            var prefix = startWithWord;
            if (line.StartsWith(prefix))
            {
                // makes sure the prefix didn't cut off parts of the actual prefix.
                // e.g. a type of 'timerdialog' can be mistaken for 'timer' due to
                // the nature of the StartsWith method.
                char contolChar = line[prefix.Length];
                if (contolChar != ' ')
                {
                    return false;
                }

                int startIndex = prefix.Length + 1;
                int index = startIndex;
                bool constantTypeWasSkipped = false;
                while (index < line.Length)
                {
                    char c = line[index];

                    if (JassSymbols.IsSplittingSymbol(c))
                    {
                        foundKeyword = true;
                        int length = index - startIndex;
                        string keyword = line.Substring(startIndex, length);

                        if (TypeWords.Contains(keyword))
                        {
                            index++;
                            startIndex = index;
                            continue;
                        }

#if DEBUG
                        if (Keywords.Contains(keyword))
                        {
                            throw new Exception($"'{keyword}' already exists!");
                        }
#endif

                        Keywords.Add(keyword);
                        if (isScanningTypewordLine)
                        {
                            TypeWords.Add(keyword);
                        }

                        break;
                    }

                    index++;
                }
            }

            return foundKeyword;
        }

        private void AddTypewordManual(string keyword)
        {
            Keywords.Add(keyword);
            TypeWords.Add(keyword);
        }
    }
}
