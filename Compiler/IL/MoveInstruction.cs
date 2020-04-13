using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.IL
{
    class MoveInstruction : IInstruction
    {
        public IEntity Source { get; set; }
        public Variable Destination { get; set; }

        public MoveInstruction(IEntity source, Variable destination)
        {
            Source = source;
            Destination = destination;
        }

        public override string ToString()
        {
            return string.Format("[MOVE] {0} -> {1}", Source.ToString(), Destination.ToString());
        }
    }
}
