using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.CIL.Instructions
{
    class NotImplemented : Instruction
    {
        public override void Emit()
        {
            Emitter.Emit("[not implemented]");
        }
    }
}
