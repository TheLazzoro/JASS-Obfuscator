using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JASS_Optimizer;

namespace JassOptimizer
{
    public class JassAnalyzer
    {
        private string _script;
        private string _commonJScript;
        private string _blizzardJScript;
        private JassManipulator jassManipulator = new JassManipulator();

        public JassAnalyzer(string script, string pathCommonJ, string pathBlizzardJ)
        {
            _script = script;
            _commonJScript = File.ReadAllText(pathCommonJ);
            _blizzardJScript = File.ReadAllText(pathBlizzardJ);
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
                    if (c_before == '/') // comment
                    {
                        while (!IsNewline(c))
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

                isScanningKeyword = !IsSplittingSymbol(c);
                if (keywordIndexStart < i && IsSplittingSymbol(c))
                {
                    hasKeyword = true;
                    keywordIndexEnd = i;
                }

                if (hasKeyword) // We check the scanned keyword
                {
                    hasKeyword = false;
                    int length = keywordIndexEnd - keywordIndexStart;
                    string keyword = _script.Substring(keywordIndexStart, length);

                    // TODO: Needs to check for all definitions from common.j and blizzard.j
                    if (!IsJassKeyword(keyword))
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

        private bool IsSplittingSymbol(char c)
        {
            switch (c)
            {
                case ' ':
                case '+':
                case '-':
                case '/':
                case '*':
                case '<':
                case '>':
                case '(':
                case ')':
                case '[':
                case ']':
                case '=':
                case ',':
                case '\r':
                case '\n':
                    return true;
            }

            return false;
        }

        private bool IsJassKeyword(string keyword)
        {
            switch (keyword)
            {
                case "and":
                case "array":
                case "boolean":
                case "code":
                case "call":
                case "constant":
                case "debug":
                case "else":
                case "elseif":
                case "endfunction":
                case "endif":
                case "endloop":
                case "endglobals":
                case "extends":
                case "exitwhen":
                case "false":
                case "function":
                case "globals":
                case "handle":
                case "if":
                case "integer":
                case "local":
                case "loop":
                case "native":
                case "not":
                case "nothing":
                case "null":
                case "or":
                case "real":
                case "returns":
                case "return":
                case "set":
                case "string":
                case "takes":
                case "then":
                case "true":
                case "type":
                case ",":
                case "==":
                case "=":
                case "!=":
                case "<=":
                case "<":
                case ">=":
                case ">":
                case "+":
                case "-":
                case "*":
                case "/":
                case "(":
                case ")":
                case "[":
                case "]":
                    return true;
                default:
                    return false;
            }
        }

        private bool IsNewline(char c)
        {
            if (c == '\r' || c == '\n')
                return true;

            return false;
        }
    }
}
