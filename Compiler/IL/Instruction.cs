using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.IL
{
    public abstract class Instruction
    {
        public virtual Variable DefinedVariable { get => null; }

        public virtual List<Variable> UsedVariables { get => new List<Variable>(); }

        public virtual void ReplaceUsedValue(IEntity original, IEntity alternative)
        {
            // This method is overwritten by inherited classes.
        }
    }
}
