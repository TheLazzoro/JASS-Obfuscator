using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace JassObfuscator
{
    internal class JassAnalyzer
    {
        private string _script;
        private JassManipulator _jassManipulator;
        private JassDefinitions _jassDefinitions;
        private IFormatProvider _formatProvider;

        private HashSet<string> _functionIdentifiers;

        internal JassAnalyzer(string script, string pathCommonJ, string pathBlizzardJ)
        {
            _script = script;
            string[] commonJScript = File.ReadAllLines(pathCommonJ);
            string[] blizzardJScript = File.ReadAllLines(pathBlizzardJ);
            _jassDefinitions = new JassDefinitions(commonJScript, blizzardJScript);
            _jassManipulator = new JassManipulator(_jassDefinitions);
            _formatProvider = new CultureInfo("en-US");
            _functionIdentifiers = new HashSet<string>();

            // Collect all function declarations.
            string[] scriptLines = _script.Split(
                new string[] { Environment.NewLine },
                StringSplitOptions.None);
            string functionKeyword = "function ";
            for (int i = 0; i < scriptLines.Length; i++)
            {
                string line = scriptLines[i].Trim();
                if (line.StartsWith(functionKeyword))
                {
                    int index = functionKeyword.Length;
                    while (index < line.Length && !JassSymbols.IsSplittingSymbol(line[index]))
                    {
                        index++;
                    }

                    int length = index - functionKeyword.Length;
                    string fuctionName = line.Substring(functionKeyword.Length, length);
                    _functionIdentifiers.Add(fuctionName);
                }
            }
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

            int stringLiteralStart = 0;
            int stringLiteralEnd = 0;
            bool hasStringLiteral = false;

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
                    hasStringLiteral = true;
                    i++;
                    stringLiteralStart = i;
                    stringLiteralEnd = i;
                    c = _script[i];
                    while (!JassSymbols.IsStringLiteral(c))
                    {
                        if (c == '\\') // escape char
                        {
                            i++;
                        }

                        i++;
                        c = _script[i];
                        stringLiteralEnd = i;
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

                if (hasKeyword || hasStringLiteral || IsEndOfScript(i)) // We check the scanned keyword
                {
                    hasKeyword = false;
                    int length = keywordIndexEnd - keywordIndexStart;
                    string keyword = _script.Substring(keywordIndexStart, length);

                    bool isRawNumber = float.TryParse(keyword, out float val);
                    if (keyword.StartsWith("0x") || keyword.StartsWith("$"))
                    {
                        isRawNumber = int.TryParse(keyword.Replace("0x", string.Empty), NumberStyles.HexNumber, _formatProvider, out int val2)
                                   || int.TryParse(keyword.Replace("$", string.Empty), NumberStyles.HexNumber, _formatProvider, out int val3);
                    }

                    bool isJassKeyword = JassSymbols.IsJassKeyword(keyword);
                    bool isJassDefinition = _jassDefinitions.Keywords.Contains(keyword);
                    bool stringLiteralIsFunctionCall = true;

                    if (hasStringLiteral) // replace 'ExecuteFunc' string with the transformed function name.
                    {
                        bool isExecuteFunc = _script.Substring(stringLiteralStart - 13, 11) == "ExecuteFunc";
                        length = stringLiteralEnd - stringLiteralStart;
                        keyword = _script.Substring(stringLiteralStart, length);
                        stringLiteralIsFunctionCall = _functionIdentifiers.Contains(keyword) && isExecuteFunc;

                        if (stringLiteralIsFunctionCall) // is string in 'ExecuteFunc' call
                        {
                            keywordIndexStart -= length;
                            keywordIndexEnd -= length;
                            //offset = stringLiteralStart - 1;
                        }
                    }

                    if ((!isJassKeyword && !isJassDefinition && !isRawNumber && stringLiteralIsFunctionCall) || IsEndOfScript(i))
                    {
                        // We have determined that the keyword is eligible for obfuscation

                        length = keywordIndexStart - offset;
                        string preceedingPart = _script.Substring(offset, length);
                        offset = keywordIndexEnd;

                        if(hasStringLiteral && stringLiteralIsFunctionCall)
                        {
                            offset = stringLiteralEnd;
                        }

                        JassBlock preceedingBlock = new JassBlock(preceedingPart, false);
                        _jassManipulator.AddBlock(preceedingBlock);

                        if (!IsEndOfScript(i))
                        {
                            JassBlock keywordBlock = new JassBlock(keyword, true);
                            _jassManipulator.AddBlock(keywordBlock);
                        }
                    }

                    hasStringLiteral = false;
                }

                i++;
            }

            return _jassManipulator.GetOptimizedJASS();
        }

        private bool IsEndOfScript(int index)
        {
            return index == _script.Length - 1;
        }
    }
}
