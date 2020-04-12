using System;
using System.Collections.Generic;
using System.Text;

namespace Runtime
{
    public static class Dynamic
    {
        private static Dictionary<Symbol, Stack<IType>> list = new Dictionary<Symbol, Stack<IType>>();
        public static Stack<IType> Get(Symbol s)
        {
            Stack<IType> ret;
            if (list.TryGetValue(s, out ret))
                return ret;
            return null;
        }
        public static Stack<IType> Set(Symbol s)
        {
            return list[s] = new Stack<IType>();
        }
    }
}
