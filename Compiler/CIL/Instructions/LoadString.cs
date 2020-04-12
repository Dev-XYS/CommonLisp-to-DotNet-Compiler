using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.CIL.Instructions
{
    /// <summary>
    /// Loads a string into the runtime stack.
    /// Does not support strings with escape characters or quotation marks.
    /// </summary>
    class LoadString : Instruction
    {
        public string Value { get; set; }

        public override void Emit()
        {
            Emitter.Emit("ldstr \"{0}\"", Value);
        }
    }
}
