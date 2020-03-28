using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.IL
{
    class CallInstruction : IInstruction
    {
        public Variable Function { get; set; }
        public Variable Destination { get; set; }
        public List<IEntity> Parameters { get; }

        public CallInstruction(Variable function, Variable destination)
        {
            Function = function;
            Destination = destination;
            Parameters = new List<IEntity>();
        }
    }
}
