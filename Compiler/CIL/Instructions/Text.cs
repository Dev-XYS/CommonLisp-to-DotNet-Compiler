using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.CIL.Instructions
{
    class Text : Instruction
    {
        public string Content { get; set; }

        public override void Emit()
        {
            Emitter.Emit(Content);
        }
    }
}
