using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler
{
    static class ILConstants
    {
        public static List<Runtime.IType> Constants { get; set; }

        static ILConstants()
        {
            Constants = new List<Runtime.IType>();
        }
    }
}
