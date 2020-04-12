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
        public string Name { get; set; }

        public override void Emit()
        {
            Emitter.Emit("ldsfld class [Runtime]Runtime.IType Constants::{0}", Name);
        }
    }
}
