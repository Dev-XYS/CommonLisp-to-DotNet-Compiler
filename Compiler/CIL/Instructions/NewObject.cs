using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.CIL.Instructions
{
    class NewObject : Instruction
    {
        public Class Type { get; set; }

        public override void Emit()
        {
            Emitter.Emit("newobj instance void {0}::.ctor()", Type.Name);
        }
    }
}
