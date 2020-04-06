using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.CIL.Instructions
{
    class LoadInt : Instruction
    {
        public int Value { get; set; }

        public override void Emit()
        {
            Emitter.Emit("ldc.i32 {0}", Value);
        }
    }
}
