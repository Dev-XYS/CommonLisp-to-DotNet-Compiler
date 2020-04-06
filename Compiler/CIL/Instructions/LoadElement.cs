using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.CIL.Instructions
{
    class LoadElement : Instruction
    {
        public override void Emit()
        {
            Emitter.Emit("ldelem.ref");
        }
    }
}
