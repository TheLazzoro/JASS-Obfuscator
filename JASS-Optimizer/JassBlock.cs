using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JASS_Optimizer
{
    /// <summary>
    /// Represents a block of code to be appended in the optimized script.
    /// </summary>
    internal class JassBlock
    {
        internal string Block { get; set; }
        internal bool Obfuscate { get; }
        private bool _isLocalVariable;

        internal JassBlock(string block, bool obfuscate, bool isLocalVariable)
        {
            Block = block;
            Obfuscate = obfuscate;
            _isLocalVariable = isLocalVariable;
        }
    }
}