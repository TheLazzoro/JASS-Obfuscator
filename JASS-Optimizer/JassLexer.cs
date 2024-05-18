using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JassOptimizer
{
    public class JassLexer
    {
        private string _script;

        public JassLexer(string script)
        {
            _script = script;
        }

        public string Optimize()
        {
            string optimized = string.Empty;
            var list = new List<Tuple<JassSymbol, string>>();

            int scriptLength = _script.Length;
            int offset = 0;
            int i = 0;
            while (i < scriptLength)
            {
                char c = _script[i];

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

            return optimized;
        }
    }
}
