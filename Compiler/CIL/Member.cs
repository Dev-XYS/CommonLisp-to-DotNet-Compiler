using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.CIL
{
    abstract class Member
    {
        public abstract string Name { get; set; }

        public abstract string Type { get; }
    }
}
