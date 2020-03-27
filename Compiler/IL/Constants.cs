using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.IL
{
    static class Constants
    {
        public static List<Runtime.IType> ConstantList { get; set; }

        static Constants()
        {
            ConstantList = new List<Runtime.IType>();
        }
    }
}
