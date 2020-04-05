using Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Frontend
{
    class Constant
    {
        public static IL.Variable New(IType value)
        {
            //todo: create a new constant with value
            var ret = new IL.Variable(Global.env);
            return ret;
        }
    }
}
