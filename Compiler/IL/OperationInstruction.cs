using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.IL
{
    class OperationInstruction : IInstruction
    {
        public IEntity Operand1 { get; }
        public IEntity Operand2 { get; }
        public Variable Destination { get; }

        public bool IsTwoImm()
        {
            return Operand1 is ImmediateNumber && Operand2 is ImmediateNumber;
        }
    }
}
