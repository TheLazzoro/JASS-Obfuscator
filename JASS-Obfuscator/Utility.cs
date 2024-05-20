using System.Text;

namespace JassObfuscator
{
    internal static class Utility
    {
        internal static string RemoveExcessWhitespace(string input)
        {
            StringBuilder sb = new StringBuilder();
            bool wasWhiteSpace = false;
            for (int j = 0; j < input.Length; j++)
            {
                char c = input[j];

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

            return sb.ToString();
        }
    }
}
