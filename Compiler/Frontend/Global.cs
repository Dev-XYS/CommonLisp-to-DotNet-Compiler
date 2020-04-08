using Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Frontend
{
    static class Global
    {
        public static Environment env = new Environment();
        public static IL.Variable rax, nil;
        private static bool inited = false;
        public static void Init()
        { 
            if(!inited)
            {
                inited = true;
                Core.Init();
                Lisp.Init();
                SO.Init();
                rax = env.AddUnnamedVariable();
                nil = env.Find(Symbol.Find("NIL"));
            }
        }
    }
}
