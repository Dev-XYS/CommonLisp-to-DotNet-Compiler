using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.CIL
{
    abstract class Instruction
    {
        public abstract void Emit();
    }
}
