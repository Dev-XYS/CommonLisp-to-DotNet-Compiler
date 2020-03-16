using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler
{
    class ILOperationInstruction : IILInstruction
    {
        public string Operand1 { get; }
        public string Operand2 { get; }
        public string Destination { get; }
    }

    class ILMoveInstruction : ILOperationInstruction
    {
    }
}
