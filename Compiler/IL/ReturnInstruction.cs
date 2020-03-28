using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.IL
{
    class ReturnInstruction:IInstruction
    {
        public IEntity Value { get; set; }

        public ReturnInstruction(IEntity value)
        {
            Value = value;
        }
    }
}
