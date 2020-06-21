using System;
using System.Collections.Generic;
using System.Text;

namespace Runtime
{
    class Util
    {
        public static IType[] RequireExactly(IType list, int n, string name = "unknown")
        {
            List<IType> ret = new List<IType>();
            while (n-- > 0)
            {
                if (!(list is Cons c))
                    throw new RuntimeException(string.Format("{0}: Insufficient arguments", name));
                ret.Add(c.car);
                list = c.cdr;
            }
            if (list != Lisp.nil) throw new RuntimeException(string.Format("{0}: Too many arguments", name));
            return ret.ToArray();
        }
        public static (IType[], IType) RequireAtLeast(IType list, int n, string name = "unknown")
        {
            List<IType> ret1 = new List<IType>();
            while (n-- > 0)
            {
                if (!(list is Cons c))
                    throw new RuntimeException(string.Format("{0}: Insufficient arguments", name));
                ret1.Add(c.car);
                list = c.cdr;
            }
            return (ret1.ToArray(), list);
        }
        public static IType[] ListToArray(IType list)
        {
            List<IType> ret = new List<IType>();
            while (list is Cons c)
            {
                ret.Add(c.car);
                list = c.cdr;
            }
            return ret.ToArray();
        }
    }
}
