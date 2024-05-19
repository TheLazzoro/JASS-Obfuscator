using JassOptimizer;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;

namespace JASS_Optimizer
{
    internal class CommonJ
    {
        internal List<string> Keywords = new List<string>();
        private string[] startsWithList;

        public CommonJ(string[] commonJLines)
        {
            startsWithList = new string[4];
            startsWithList[0] = "type ";
            startsWithList[1] = "constant native ";
            startsWithList[2] = "constant ";
            startsWithList[3] = "native ";

            for (int i = 0; i < commonJLines.Length; i++)
            {
                string line = commonJLines[i].Trim();
                bool foundKeyword = false;

                // remove excess whitespace from line
                StringBuilder sb = new StringBuilder();
                bool wasWhiteSpace = false;
                for (int j = 0; j < line.Length; j++)
                {
                    char c = line[j];

                    if(char.IsWhiteSpace(c) && wasWhiteSpace)
                    {
                        continue;
                    }
                    else
                    {
                        wasWhiteSpace = false;
                    }

                    if(char.IsWhiteSpace(c))
                    {
                        wasWhiteSpace = true;
                    }
                    sb.Append(c);
                }
                
                line = sb.ToString();


                for (int j = 0; j < startsWithList.Length; j++)
                {
                    var prefix = startsWithList[j];
                    if (line.StartsWith(prefix))
                    {
                        int startIndex = prefix.Length;
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

                                // Little trick that skips the typename after the 'constant' keyword.
                                // We then scan the next name again
                                if (j == 2 && !constantTypeWasSkipped)
                                {
                                    constantTypeWasSkipped = true;
                                    while (JassSymbols.IsSplittingSymbol(c))
                                    {
                                        c = line[index + 1];
                                        index++;
                                        startIndex = index;
                                    }
                                    continue;
                                }

#if DEBUG
                                if (Keywords.Contains(keyword))
                                    throw new Exception($"'{keyword}' already exists!");
#endif

                                Keywords.Add(keyword);
                                break;
                            }

                            index++;
                        }
                    }

                    if (foundKeyword)
                        break;
                }
            }
        }
    }
}
