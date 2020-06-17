using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.IL
{
    class ReturnInstruction : Instruction
    {
        public IEntity Value { get; set; }

        public ReturnInstruction(IEntity value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return string.Format("[RET] {0}", Value.ToString());
        }

        public override List<Variable> UsedVariables
        {
            get
            {
                if (Value is Variable v)
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
            if (Value == original)
            {
                Value = alternative;
            }
        }
    }
}
