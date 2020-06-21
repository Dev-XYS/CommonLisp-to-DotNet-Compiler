using System;
using System.Collections.Generic;
using System.Text;

namespace Runtime
{
    class DummyFunction : IType
    {
        public IType Invoke(IType[] args)
        {
            return Lisp.nil;
        }
    }
}
