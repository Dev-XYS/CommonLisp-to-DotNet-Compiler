using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.IL
{
    class MoveInstruction : Instruction
    {
        public IEntity Source { get; set; }
        public Variable Destination { get; set; }

        public MoveInstruction(IEntity source, Variable destination)
        {
            Source = source;
            Destination = destination;
        }

        public override string ToString()
        {
            return string.Format("[MOVE] {0} -> {1}", Source.ToString(), Destination.ToString());
        }

        public override Variable DefinedVariable
        {
            get
            {
                return Destination;
            }
        }

        public override List<Variable> UsedVariables
        {
            get
            {
                if (Source is Variable v)
                {
                    return new List<Variable>() { v };
                }
                else
                {
                    return base.UsedVariables;
                }
            }
        }

        public override void ReplaceUsedValue(IEntity original, IEntity alternative)
        {
            if (Source == original)
            {
                Source = alternative;
            }
        }
    }
}
