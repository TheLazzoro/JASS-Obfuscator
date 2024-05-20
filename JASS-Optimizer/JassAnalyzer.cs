using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using JASS_Optimizer;

namespace JassOptimizer
{
    internal class JassAnalyzer
    {
        private string _script;
        private JassManipulator jassManipulator = new JassManipulator();
        private JassDefinitions _jassDefinitions;
        private IFormatProvider _formatProvider;

        internal JassAnalyzer(string script, string pathCommonJ, string pathBlizzardJ)
        {
            _script = script;
            string[] commonJScript = File.ReadAllLines(pathCommonJ);
            string[] blizzardJScript = File.ReadAllLines(pathBlizzardJ);
            _jassDefinitions = new JassDefinitions(commonJScript, blizzardJScript);
            _formatProvider = new CultureInfo("en-US");
        }

        internal string Obfuscate()
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

                // skip string literals
                if (i > 1 && JassSymbols.IsStringLiteral(c))
                {
                    i++;
                    c = _script[i];
                    while (!JassSymbols.IsStringLiteral(c))
                    {
                        if(c == '\\') // escape char
                        {
                            i++;
                        }

                        i++;
                        c = _script[i];
                        keywordIndexStart = i;
                        keywordIndexEnd = i;
                    }
                }

                // skip FourCC literals
                if (i > 0 && JassSymbols.IsFourCCLiteral(c))
                {
                    i++;
                    c = _script[i];
                    while (!JassSymbols.IsFourCCLiteral(c))
                    {
                        c = _script[i];
                        i++;
                        keywordIndexStart = i;
                        keywordIndexEnd = i;
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

                    bool isRawNumber = float.TryParse(keyword, out float val) || int.TryParse(keyword.Replace("0x", string.Empty), NumberStyles.HexNumber, _formatProvider, out int val2);
                    bool isJassKeyword = JassSymbols.IsJassKeyword(keyword);
                    bool isJassDefinition = _jassDefinitions.Keywords.Contains(keyword);

                    // TODO: Needs to check for all definitions from common.j and blizzard.j
                    if ((!isJassKeyword && !isJassDefinition && !isRawNumber) || IsEndOfScript(i))
                    {
                        // We have determined that the keyword is eligible for obfuscation

                        length = keywordIndexStart - offset;
                        string preceedingPart = _script.Substring(offset, length);
                        offset = keywordIndexEnd;

                        JassBlock preceedingBlock = new JassBlock(preceedingPart, false, false);
                        jassManipulator.AddBlock(preceedingBlock);

                        if (!IsEndOfScript(i))
                        {
                            JassBlock keywordBlock = new JassBlock(keyword, true, false);
                            jassManipulator.AddBlock(keywordBlock);
                        }
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
