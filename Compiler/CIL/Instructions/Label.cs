using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.CIL.Instructions
{
    class Label : Instruction
    {
        public IL.Label ILLabel { get; set; }

        public override void Emit()
        {
            Emitter.Emit("label{0}:", ILLabel.GetHashCode().ToString());
        }
    }
}
