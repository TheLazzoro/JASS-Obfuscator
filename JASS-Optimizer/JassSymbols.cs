namespace JassOptimizer
{
    internal static class JassSymbols
    {
        internal static bool IsSplittingSymbol(char c)
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
                case '.':
                case '\r':
                case '\n':
                case '\t':
                    return true;
            }

            return false;
        }

        internal static bool IsJassKeyword(string keyword)
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
                case ".":
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

        internal static bool IsNewline(char c)
        {
            if (c == '\r' || c == '\n')
                return true;

            return false;
        }

    }
}
