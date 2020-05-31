using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.IL
{
    public abstract class Instruction
    {
        public virtual Variable DefinedVariable { get => null; }

        public virtual List<Variable> UsedVariables { get => new List<Variable>(); }
    }
}
