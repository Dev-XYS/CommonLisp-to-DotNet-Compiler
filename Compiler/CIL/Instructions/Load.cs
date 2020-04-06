using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.CIL.Instructions
{
    class Load : Instruction
    {
        public int Loc { get; set; }

        public override void Emit()
        {
            Emitter.Emit("ldloc.{0}", Loc);
        }
    }
}
