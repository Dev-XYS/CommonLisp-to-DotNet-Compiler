using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.CIL.Instructions
{
    /// <summary>
    /// Represents the `call` instruction.
    /// This instruction should only be used for calling static functions.
    /// </summary>
    /// <remarks>
    /// Can this instruction used for calling non-static functions?
    /// </remarks>
    class Call : Instruction
    {
        public string FullName { get; set; }

        public override void Emit()
        {
            Emitter.Emit("call class {0}", FullName);
        }
    }
}
