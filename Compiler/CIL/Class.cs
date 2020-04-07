using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.CIL
{
    abstract class Class
    {
        public abstract string Name { get; }

        public abstract string CtorArgumentList { get; }
    }
}
