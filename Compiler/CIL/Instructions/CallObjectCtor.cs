using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.CIL.Instructions
{
    class CallObjectCtor : Instruction
    {
        public override void Emit()
        {
            Emitter.Emit("call instance void [System.Runtime]System.Object::.ctor()");
        }
    }
}
