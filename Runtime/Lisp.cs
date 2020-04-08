using System.Collections.Generic;

namespace Runtime
{
    public static class Lisp
    {
        public static T t, nil;
        public static StandardInput stdin;
        private static bool inited = false;
        public static void Init()
        {
            if (!inited)
            {
                t = new T();
                nil = null;
                stdin = new StandardInput();
                Symbol.Init();
            }
            inited = true;
        }
    }
}