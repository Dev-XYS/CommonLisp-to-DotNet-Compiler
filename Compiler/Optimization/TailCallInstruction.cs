using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Optimization
{
    class TailCallInstruction : IL.Instruction
    {
        public List<IL.IEntity> Arguments { get; }

        public TailCallInstruction(List<IL.IEntity> args)
        {
            Arguments = args;
        }

        public override string ToString()
        {
            return string.Format("[TAIL] ({0})", GetParameterList());
        }

        private string GetParameterList()
        {
            return string.Join(", ", Arguments.ConvertAll((IL.IEntity e) => e.ToString()));
        }
    }
}
