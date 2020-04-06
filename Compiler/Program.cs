using Runtime;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            IL.Program program = Test.IL.Closure.GenerateCode();
            CIL.Program target = new CIL.Program(program);
            target.Emit();
        }

        static void f(int x)
        {
            int[] arr = new int[3];
            arr[0] = 10;
            arr[1] = 20;
            arr[2] = 30;
        }

        static void g(int[] args)
        {

        }
    }

    class env0
    {
        public IType var0;
    }

    class func0 : IType
    {
        env0 env0;

        IType Invoke(IType[] args)
        {
            IType t = new TInteger(0);
            env0.var0 = t;
            if (args.Length != 2)
            {
                throw new RuntimeException("minus: require 2 arguments");
            }
            if (!(args[0] is TInteger) && !(args[1] is TInteger))
            {
                throw new RuntimeException("minus: require integer arguments");
            }
            return new TInteger((args[0] as TInteger).Value - (args[1] as TInteger).Value);
        }
    }
}
