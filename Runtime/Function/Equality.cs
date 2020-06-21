using System;
using System.Collections.Generic;
using System.Text;

namespace Runtime.Function
{
    public class Eq : IType
    {
        public IType Invoke(IType[] args)
        {
            if (args.Length != 2) throw new RuntimeException("EQ: require exactly 2 arguments");
            return args[0].Equals(args[1]) ? Lisp.t : Lisp.nil;
        }
    }
    public class Eql : IType
    {
        public IType Invoke(IType[] args)
        {
            if (args.Length != 2) throw new RuntimeException("EQL: require exactly 2 arguments");
            if (args[0].Equals(args[1])) return Lisp.t;
            if(args[0] is Number x && args[1] is Number y)
            {
                return x.GetType() == y.GetType() && x.Equal(y) ? Lisp.t : Lisp.nil;
            }
            return Lisp.nil;
        }
    }
}
