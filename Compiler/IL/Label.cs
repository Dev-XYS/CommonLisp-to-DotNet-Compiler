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
    }
}
