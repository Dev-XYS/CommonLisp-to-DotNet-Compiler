using Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Frontend
{
    class Macro
    {
        private static HashSet<Symbol> gml = new HashSet<Symbol>();
        public static bool IsMacro(Symbol s)
        {
            return gml.Contains(s);
        }
        public static Cons FullExpand(Cons form)
        {
            throw new NotImplementedException("Macros are not implemented yet");
        }
    }
}
