using Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Frontend
{
    static class Global
    {
        public static Environment env = new Environment();
        public static IL.IEntity nil;
        private static bool inited = false;
        public static void Init()
        { 
            if(!inited)
            {
                inited = true;
                Core.Init();
                SO.Init();
                nil = env.FindOrExtern(Symbol.FindOrCreate("NIL"));
                env.Name = "global";
                Macro.Init();
            }
        }
    }
}
