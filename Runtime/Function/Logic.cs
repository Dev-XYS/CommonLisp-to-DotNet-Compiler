using System;
using System.Collections.Generic;
using System.Text;

namespace Runtime.Function
{
    public class LogicNot : IType
    {
        public IType Invoke(IType[] args)
        {
            if (args.Length != 1)
                throw new RuntimeException("NOT: Exactly 1 argument required.");
            return args[0] is null ? Lisp.t : Lisp.nil;
        }
    }
}
