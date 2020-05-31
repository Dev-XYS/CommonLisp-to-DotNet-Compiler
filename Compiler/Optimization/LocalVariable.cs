using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Optimization
{
    // LocalVariable should not inherited from IL.Variable.
    // A common base class needed.
    class LocalVariable : IL.Variable
    {
        public int LocSlot { get; }

        public LocalVariable(int loc) : base(null)
        {
            LocSlot = loc;
        }

        public override string ToString()
        {
            return string.Format("LOC({0})", LocSlot);
        }
    }
}
