using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Frontend
{
    class Function : IL.ParametersFunction
    {
        public Function(Environment e) : base()
        {
            EnvList.Add(e);
            while(e != e.outer)
            {
                e = e.outer;
                EnvList.Add(e);
            }
        }
    }
}
