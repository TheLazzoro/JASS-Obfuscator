using JassOptimizer;
using System;
using System.Collections.Generic;
using System.Text;

namespace JASS_Optimizer
{
    public static class Optimizer
    {
        /// <summary>
        /// Optimizes the input jass script
        /// </summary>
        /// <param name="script">Input script to be optimized</param>
        /// <param name="pathCommonJ">File path to Common.j</param>
        /// <param name="pathBlizzardJ">File path to Blizzard.j</param>
        /// <returns>Optimized script</returns>
        public static string Optimize(string script, string pathCommonJ, string pathBlizzardJ)
        {
            var analyzer = new JassAnalyzer(script, pathCommonJ, pathBlizzardJ);
            script = analyzer.Obfuscate();
            var lines = script.Split(
                new string[] { Environment.NewLine },
                StringSplitOptions.None
            );

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                line = line.Trim();
                if (!line.StartsWith("//"))
                {
                    sb.Append(line + Environment.NewLine);
                }
            }

            return sb.ToString();
        }
    }
}
