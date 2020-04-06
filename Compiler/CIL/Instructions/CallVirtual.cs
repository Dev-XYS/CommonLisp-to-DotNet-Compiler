using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.CIL.Instructions
{
    class CallVirtual : Instruction
    {
        public override void Emit()
        {
            Emitter.Emit("callvirt instance class [Runtime]Runtime.IType [Runtime]Runtime.IType::Invoke(class [Runtime]Runtime.IType[])");
        }
    }
}
