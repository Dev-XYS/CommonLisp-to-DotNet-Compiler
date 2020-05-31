using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.IL
{
    class Label : Instruction
    {
        private static int glid;
        private int lid;

        public string Name { get; set; }

        public Label(string name)
        {
            lid = glid++;
            Name = "label" + lid;
        }

        public override string ToString()
        {
            return string.Format("[LABEL] {0}", this.GetHashCode());
        }

        public override int GetHashCode()
        {
            return lid;
        }
    }
}
