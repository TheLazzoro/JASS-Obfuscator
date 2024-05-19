using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using JASS_Optimizer;

namespace JassOptimizer
{
    public class JassAnalyzer
    {
        private string _script;
        private string _blizzardJScript;
        private JassManipulator jassManipulator = new JassManipulator();
        private CommonJ _commonJ;

        public JassAnalyzer(string script, string pathCommonJ, string pathBlizzardJ)
        {
            _script = script;
            string[] commonJScript = File.ReadAllLines(pathCommonJ);
            _commonJ = new CommonJ(commonJScript);

            //_blizzardJScript = File.ReadAllText(pathBlizzardJ);
        }

        public string Optimize()
        {
            int scriptLength = _script.Length;
            int offset = 0;
            int i = 0;
            bool isScanningKeyword = false;
            bool hasKeyword = false;
            int keywordIndexStart = 0;
            int keywordIndexEnd = 0;

            while (i < scriptLength)
            {
                char c = _script[i];

                // skip comments
                if (c == '/' && i > 0)
                {
                    char c_before = _script[i - 1];
                    if (c_before == '/') // is a comment
                    {
                        while (!JassSymbols.IsNewline(c))
                        {
                            c = _script[i];
                            i++;
                        }
                    }
                }

                if (!isScanningKeyword)
                {
                    keywordIndexStart = i;
                    keywordIndexEnd = i;
                }

                isScanningKeyword = !JassSymbols.IsSplittingSymbol(c);
                if (keywordIndexStart < i && JassSymbols.IsSplittingSymbol(c))
                {
                    hasKeyword = true;
                    keywordIndexEnd = i;
                }

                if (hasKeyword || IsEndOfScript(i)) // We check the scanned keyword
                {
                    hasKeyword = false;
                    int length = keywordIndexEnd - keywordIndexStart;
                    string keyword = _script.Substring(keywordIndexStart, length);

                    // TODO: Needs to check for all definitions from common.j and blizzard.j
                    if (!JassSymbols.IsJassKeyword(keyword) || IsEndOfScript(i))
                    {
                        // We have determined that the keyword is eligible for obfuscation

                        length = keywordIndexStart - offset;
                        string preceedingPart = _script.Substring(offset, length);
                        offset = keywordIndexEnd;

                        JassBlock preceedingBlock = new JassBlock(preceedingPart, false, false);
                        jassManipulator.AddBlock(preceedingBlock);

                        JassBlock keywordBlock = new JassBlock(keyword, true, false);
                        jassManipulator.AddBlock(keywordBlock);
                    }
                }

                i++;
            }

            return jassManipulator.GetOptimizedJASS();
        }

        private bool IsEndOfScript(int index)
        {
            return index == _script.Length - 1;
        }
    }
}
