using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.CIL.Instructions
{
    class Call : Instruction
    {
        public override void Emit()
        {
            Emitter.Emit("call: no emit");
        }
    }
}
