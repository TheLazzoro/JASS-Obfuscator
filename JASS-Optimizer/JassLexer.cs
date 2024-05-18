using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JassOptimizer
{
    internal static class JassLexer
    {
        public static List<Tuple<JassSymbol, string>> Run(string script)
        {
            var list = new List<Tuple<JassSymbol, string>>();

            int scriptLength = script.Length;
            int offset = 0;
            int i = 0;
            while (i < scriptLength)
            {
                char c = script[i];

                switch (c)
                {
                    case '+':
                    case '-':
                    case '/':
                    case '*':
                        list.Add(new Tuple<JassSymbol, string>(JassSymbol.MathOperator, c.ToString()));
                        break;
                    default:
                        break;
                }

                i++;
            }

            return list;
        }
    }
}
