using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.IL
{
    class FunctionInstruction : IInstruction
    {
        public Function Function { get; set; }

        public Variable Destination { get; set; }

        public FunctionInstruction(Function function, Variable destination)
        {
            Function = function;
            Destination = destination;
        }
    }
}
