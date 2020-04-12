using System;
using System.Collections.Generic;
using System.Text;

namespace Runtime.Function.Arith
{
    static class Util
    {
        public static void Contagion(Number[] args)
        {
            int minType = 0;
            foreach(var i in args)
            {
                if(i is TFloat)
                {
                    minType = 3;
                    break;
                }
            }
            if (minType > 0)
                for (int i = 0; i < args.Length; ++i)
                    if (!(args[i] is TFloat))
                        args[i] = TFloat.From(args[i]);
        }
        public static Number[] AllNumbers(IType[] args)
        {
            if (!Array.TrueForAll(args, (IType x) => x is Number)) throw new RuntimeException("Inconsistent type: numbers required");
            return Array.ConvertAll(args, (IType x) => x as Number);
        }
    }
}
