using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.CIL.Instructions
{
    class LoadArgument : Instruction
    {
        public int ArgNo { get; set; }

        public override void Emit()
        {
            Emitter.Emit("ldarg.{0}", ArgNo);
        }
    }
}
