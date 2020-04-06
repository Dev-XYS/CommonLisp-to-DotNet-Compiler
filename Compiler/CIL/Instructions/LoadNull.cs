using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.CIL.Instructions
{
    class LoadNull : Instruction
    {
        public override void Emit()
        {
            Emitter.Emit("ldnull");
        }
    }
}
