using System;
using System.Text;

namespace JassObfuscator
{
    public static class Obfuscator
    {
        /// <summary>
        /// Obfuscates the input jass script.
        /// </summary>
        /// <param name="script">Input script to be obfuscated</param>
        /// <param name="pathCommonJ">File path to Common.j</param>
        /// <param name="pathBlizzardJ">File path to Blizzard.j</param>
        /// <returns>Obfuscated script</returns>
        public static string Obfuscate(string script, string pathCommonJ, string pathBlizzardJ)
        {
            var analyzer = new JassAnalyzer(script, pathCommonJ, pathBlizzardJ);
            script = analyzer.Analyze();
            var lines = script.Split(
                new string[] { Environment.NewLine },
                StringSplitOptions.None
            );

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                line = line.Trim();
                line = Utility.RemoveExcessWhitespace(line);
                if (line.Length > 0 && !line.StartsWith("//"))
                {
                    sb.Append(line + Environment.NewLine);
                }
            }

            return sb.ToString();
        }
    }
}
