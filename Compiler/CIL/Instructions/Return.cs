using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.CIL.Instructions
{
    class Return : Instruction
    {
        public override void Emit()
        {
            Emitter.Emit("ret");
        }
    }
}
