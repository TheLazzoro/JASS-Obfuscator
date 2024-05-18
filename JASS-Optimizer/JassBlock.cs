using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JASS_Optimizer
{
    /// <summary>
    /// Represents a block of code to be appended in the optimized script.
    /// </summary>
    public class JassBlock
    {
        internal string Block { get; private set; }
        private bool _obfuscate;
        private bool _isLocalVariable;

        public JassBlock(string block, bool obfuscate, bool isLocalVariable)
        {
            Block = block;
            _obfuscate = obfuscate;
            _isLocalVariable = isLocalVariable;
        }
    }
}