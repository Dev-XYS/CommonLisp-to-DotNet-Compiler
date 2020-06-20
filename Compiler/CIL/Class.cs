using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.CIL
{
    abstract class Class
    {
        public abstract string Name { get; }

        // Same as `Name`, possibly with quotation marks.
        public abstract string AccessString { get; }

        public abstract string CtorArgumentList { get; }
    }
}
