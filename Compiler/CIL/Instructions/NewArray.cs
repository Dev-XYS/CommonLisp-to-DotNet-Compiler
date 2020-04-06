using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.CIL.Instructions
{
    class NewArray : Instruction
    {
        public string Type { get; set; }

        public override void Emit()
        {
            Emitter.Emit("newarr {0}", Type);
        }
    }
}
