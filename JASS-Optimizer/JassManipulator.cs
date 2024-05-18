using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JASS_Optimizer
{
    /// <summary>
    /// Keeps a collection of JASS blocks in the script,
    /// and is responsible for obfuscating function- and variable names.
    /// </summary>
    public class JassManipulator
    {
        private List<JassBlock> _jassBlocks = new List<JassBlock>();
        private Dictionary<string, List<JassBlock>> references = new Dictionary<string, List<JassBlock>>();

        internal void AddBlock(JassBlock jassBlock)
        {
            string identifier = jassBlock.Block;
            List<JassBlock> blocks;
            if (!references.TryGetValue(identifier, out blocks))
            {
                blocks = new List<JassBlock>();
                references.Add(identifier, blocks);
            }

            blocks.Add(jassBlock);
        }

        internal string GetOptimizedJASS()
        {
            StringBuilder optimized = new StringBuilder();

            foreach (var reference in references)
            {
                var list = reference.Value;
                var block = list[0];
                optimized.Append(block.Block);
            }

            return optimized.ToString();
        }
    }
}