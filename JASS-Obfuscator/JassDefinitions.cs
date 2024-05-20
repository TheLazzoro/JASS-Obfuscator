using System;
using System.Collections.Generic;

namespace JassObfuscator
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

            #region Reserved keywords

            AddReservedKeyword("InitCustomTriggers");
            AddReservedKeyword("RunInitializationTriggers");
            AddReservedKeyword("InitCustomPlayerSlots");
            AddReservedKeyword("InitCustomTeams");
            AddReservedKeyword("InitAllyPriorities");
            AddReservedKeyword("main");
            AddReservedKeyword("InitSounds");
            AddReservedKeyword("CreateRegions");
            AddReservedKeyword("CreateCameras");
            AddReservedKeyword("CreateAllItems");
            AddReservedKeyword("CreateAllUnits");
            AddReservedKeyword("CreateAllDestructables");
            AddReservedKeyword("InitGlobals");
            AddReservedKeyword("InitCustomTriggers");
            AddReservedKeyword("RunInitializationTriggers");
            AddReservedKeyword("config");
            AddReservedKeyword("InitCustomPlayerSlots");
            AddReservedKeyword("InitCustomTeams");
            AddReservedKeyword("InitAllyPriorities");

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
            line = line + " "; // hack. padding when the keyword has the last char in the line
            line = Utility.RemoveExcessWhitespace(line);

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

        private void AddReservedKeyword(string keyword)
        {
            Keywords.Add(keyword);
        }
    }
}
