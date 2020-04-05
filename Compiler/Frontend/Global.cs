using Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Frontend
{
    static class Global
    {
        public static Environment env;
        public static IL.Variable rax;
        private static bool inited = false;
        public static void Init()
        { 
            if(!inited)
            {
                Lisp.Init();
                SO.Init();
                env = new Environment();
                rax = new IL.Variable(env.ilenv);
            }
            inited = true;
        }
    }
}
