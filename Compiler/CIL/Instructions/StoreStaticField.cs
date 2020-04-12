using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.CIL.Instructions
{
    /// <summary>
    /// This instruction stores a static field.
    /// The instruction can only be used to store fields in the special class `Constants`.
    /// </summary>
    class StoreStaticField : Instruction
    {
        public string Name { get; set; }

        public override void Emit()
        {
            Emitter.Emit("stsfld class [Runtime]Runtime.IType Constants::{0}", Name);
        }
    }
}
