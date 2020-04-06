using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.CIL
{
    static class Emitter
    {
        private static string Tab = "";

        public static void Emit(string format, params object[] values)
        {
            Console.WriteLine(Tab + format, values);
        }
        public static void EmitRaw(string str)
        {
            Console.WriteLine(str);
        }

        public static void BeginBlock()
        {
            Emit("{{");
            Tab += '\t';
        }

        public static void EndBlock()
        {
            Tab = Tab.Substring(0, Tab.Length - 1);
            Emit("}}");
        }
    }
}
