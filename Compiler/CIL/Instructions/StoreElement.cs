using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.CIL.Instructions
{
    class StoreElement : Instruction
    {
        public override void Emit()
        {
            Emitter.Emit("stelem");
        }
    }
}
