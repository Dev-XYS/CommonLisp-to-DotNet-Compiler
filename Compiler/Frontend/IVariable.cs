using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Frontend
{
    interface IVariable
    {
        public void Load(Function f);
        public void Store(Function f);
    }
}
