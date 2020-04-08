using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.CIL.Instructions
{
    class BranchTrue : Instruction
    {
        public IL.Label Target { get; set; }

        public override void Emit()
        {
            Emitter.Emit("brtrue label{0}", Target.GetHashCode().ToString());
        }
    }
}
