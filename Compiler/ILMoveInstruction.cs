using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler
{
    class ILMoveInstruction : IILInstruction
    {
        public IILEntity Source { get; set; }
        public ILVariable Destination { get; set; }
    }
}
