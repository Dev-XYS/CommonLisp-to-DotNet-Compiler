using System.Collections.Generic;

namespace Runtime
{
    public static class Lisp
    {
        public static TBool t, nil;
        public static StandardInput stdin;
        private static bool inited = false;
        public static void Init()
        {
            if (!inited)
            {
                t = TBool.T();
                nil = null;
                stdin = new StandardInput();
                Symbol.Init();
            }
            inited = true;
        }
    }
}