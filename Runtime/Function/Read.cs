using System;
using System.Collections.Generic;
using System.Text;

namespace Runtime.Function
{
    public class Read : IType
    {
        public IType Invoke(IType[] args)
        {
            return Reader.Read(Lisp.stdin);
        }
    }
}
