using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.IL
{
    class Label : IInstruction
    {
        public string Name { get; set; }

        public Label(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return string.Format("[LABEL] {0}", this.GetHashCode());
        }
    }
}
