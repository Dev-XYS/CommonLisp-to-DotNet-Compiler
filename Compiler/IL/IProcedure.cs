using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.IL
{
    interface IProcedure
    {
        public void Add(IInstruction _);
    }
}
