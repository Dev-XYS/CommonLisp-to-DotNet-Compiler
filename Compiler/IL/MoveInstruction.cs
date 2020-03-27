using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.IL
{
    class MoveInstruction : IInstruction
    {
        public IEntity Source { get; set; }
        public Variable Destination { get; set; }
    }
}
