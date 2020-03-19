using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler
{
    class ILOperationInstruction : IILInstruction
    {
        public IILEntity Operand1 { get; }
        public IILEntity Operand2 { get; }
        public ILVariable Destination { get; }

        public bool IsTwoImm()
        {
            return Operand1 is ILImmediateNumber && Operand2 is ILImmediateNumber;
        }
    }
}
