using JassOptimizer;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
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
        private JassDefinitions _jassDefinitions;

        internal JassManipulator(JassDefinitions jassDefinitions)
        {
            _jassDefinitions = jassDefinitions;
        }

        internal void AddBlock(JassBlock jassBlock)
        {
            _jassBlocks.Add(jassBlock);

            // handle function/variable references
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

            // obfuscate blocks
            chars.Add('a'); // init
            foreach (var jassblock in references)
            {
                var list = jassblock.Value;
                if (!list[0].Obfuscate || list[0].WasObfuscated)
                {
                    continue;
                }

                string obfuscatedName = GenerateObfuscatedName();
                for (int i = 0; i < list.Count; i++)
                {
                    var block = list[i];
                    block.Block = obfuscatedName;
                    block.WasObfuscated = true;
                }
            }

            // Append to script
            foreach (var jassBlock in _jassBlocks)
            {
                optimized.Append(jassBlock.Block);
            }

            return optimized.ToString();
        }

        List<char> chars = new List<char>();
        private string GenerateObfuscatedName()
        {
            string name = string.Empty;
            bool validName = false;

            while (!validName)
            {
                char letter = chars[0];
                letter = NextLetter(letter);
                chars[0] = letter;

                int index = 1;
                while (letter == 'Z' && index < chars.Count)
                {
                    letter = chars[index];
                    letter = NextLetter(letter);
                    chars[index] = letter;

                    index++;
                }

                bool addNew = chars[chars.Count - 1] == 'Z';

                for (int i = 0; i < chars.Count; i++)
                {
                    name += chars[i];
                }

                if (addNew)
                {
                    chars.Add('a');
                }

                if(!JassSymbols.IsJassKeyword(name) && !_jassDefinitions.Keywords.Contains(name))
                {
                    validName = true;
                }
            }

            return name;
        }

        private char NextLetter(char letter)
        {
            if (letter == 'z')
            {
                letter = 'A';
            }
            else if (letter == 'Z')
            {
                letter = 'a';
            }
            else
            {
                letter = (char)(((int)letter) + 1);
            }

            return letter;
        }
    }
}