using Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Frontend
{
    static class SO
    {
        public enum Type { BLOCK, CATCH, EVAL_WHEN, FLET, FUNCTION, GO, IF, LABELS, LET, LET_STAR, LOAD_TIME_VALUE, LOCALLY, MACROLET, 
        MULTIPLE_VALUE_CALL, MULTIPLE_VALUE_PROG1, PROGN, PROGY, QUOTE, RETURN_FROM, SETQ, SYMBOL_MACROLET, TAGBODY, THE, THROW, UNWIND_PROTECT };
        private static Dictionary<Symbol, Type> types;
        private static bool inited = false;
        private static Type GetType(Symbol s)
        {
            Type ret;
            if (types.TryGetValue(s, out ret))
            {
                return ret;
            }
            else throw new Exception(string.Format("Unknown special operator {0}", s));
        }
        public static void Dispatch(Cons form, Environment e, IL.Program prog)
        {
            switch(GetType((Symbol)form.car))
            {
                default:
                    throw new NotImplementedException(string.Format("Not Implemented Special Operator {0}", (Symbol)form.car));
            }
        }
        public static bool IsSO(Symbol s)
        {
            return types.ContainsKey(s);
        }
        public static void Init()
        {
            if(!inited)
            {
                Lisp.Init();
                types = new Dictionary<Symbol, Type>(){
                    {Symbol.FindOrCreate("BLOCK"), Type.BLOCK },
                    {Symbol.FindOrCreate("TAGBODY"), Type.TAGBODY },
                    {Symbol.FindOrCreate("LET"), Type.LET },
                    {Symbol.FindOrCreate("LET*"), Type.LET_STAR },
                    {Symbol.FindOrCreate("MACROLET"), Type.MACROLET },
                    {Symbol.FindOrCreate("SYMBOL-MACROLET"), Type.SYMBOL_MACROLET },
                    {Symbol.FindOrCreate("GO"), Type.GO },
                    {Symbol.FindOrCreate("IF"), Type.IF },
                    {Symbol.FindOrCreate("FLET"), Type.FLET },
                    {Symbol.FindOrCreate("FUNCTION"), Type.FUNCTION },
                    {Symbol.FindOrCreate("CATCH"), Type.CATCH },
                    {Symbol.FindOrCreate("EVAL-WHEN"), Type.EVAL_WHEN },
                    {Symbol.FindOrCreate("LABELS"), Type.LABELS },
                    {Symbol.FindOrCreate("LOAD-TIME-VALUE"), Type.LOAD_TIME_VALUE },
                    {Symbol.FindOrCreate("LOCALLY"), Type.LOCALLY },
                    {Symbol.FindOrCreate("MULTIPLE-VALUE-CALL"), Type.MULTIPLE_VALUE_CALL },
                    {Symbol.FindOrCreate("MULTIPLE-VALUE-PROG1"), Type.MULTIPLE_VALUE_PROG1 },
                    {Symbol.FindOrCreate("PROGN"), Type.PROGN },
                    {Symbol.FindOrCreate("PROGY"), Type.PROGY },
                    {Symbol.FindOrCreate("QUOTE"), Type.QUOTE },
                    {Symbol.FindOrCreate("RETURN-FROM"), Type.RETURN_FROM },
                    {Symbol.FindOrCreate("SETQ"), Type.SETQ },
                    {Symbol.FindOrCreate("THE"), Type.THE },
                    {Symbol.FindOrCreate("THROW"), Type.THROW },
                    {Symbol.FindOrCreate("UNWIND-PROTECT"), Type.UNWIND_PROTECT } };
            }
            inited = true;
        }
    }
}
