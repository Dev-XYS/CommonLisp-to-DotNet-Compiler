using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.CIL.Instructions
{
    /// <summary>
    /// This instruction loads a static field.
    /// The instruction can only be used to load fields in the special class `Constants`.
    /// </summary>
    class LoadStaticField : Instruction
    {
        public string StaticClass { get; set; }

        public string Name { get; set; }

        public override void Emit()
        {
            Emitter.Emit("ldsfld class [Runtime]Runtime.IType {0}::{1}", StaticClass, Name);
        }
    }
}
