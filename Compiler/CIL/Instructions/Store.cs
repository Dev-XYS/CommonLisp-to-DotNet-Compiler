using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.CIL.Instructions
{
    class Store : Instruction
    {
        public int Loc { get; set; }

        public override void Emit()
        {
            Emitter.Emit("stloc.{0}", Loc);
        }
    }
}
