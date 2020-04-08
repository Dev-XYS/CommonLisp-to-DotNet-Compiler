using Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Frontend
{
    static class SO
    {
        public enum Type
        {
            BLOCK, CATCH, EVAL_WHEN, FLET, FUNCTION, GO, IF, LABELS, LET, LET_STAR, LOAD_TIME_VALUE, LOCALLY, MACROLET, LAMBDA,
            MULTIPLE_VALUE_CALL, MULTIPLE_VALUE_PROG1, PROGN, PROGY, QUOTE, RETURN_FROM, SETQ, SYMBOL_MACROLET, TAGBODY, THE, THROW, UNWIND_PROTECT
        };
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
        public static void CompileProgn(IType list, Environment e, IL.Function p)
        {
            if (list as TBool == Lisp.nil)
                p.Add(new IL.MoveInstruction(Global.nil, Global.rax));
            while (list is Cons forms)
            {
                var cur = forms.car;
                list = forms.cdr;
                Core.CompileSingleExpr(cur, e, p);
            }
        }
        public static void CompileIf(IType body, Environment e, IL.Function p)
        {
            var param = Util.RequireExactly(body, 3, "IF");
            IType cond = param[0], good = param[1], bad = param[2];
            Core.CompileSingleExpr(cond, e, p);
            var lBadGood = new IL.Label("if:cond bad|good");
            var lGood = new IL.Label("if:cond bad good|");
            p.Add(new IL.ConditionalJumpInstruction(lBadGood, Global.rax, true));
            Core.CompileSingleExpr(bad, e, p);
            p.Add(new IL.UnconditionalJumpInstruction(lGood));
            p.Add(lBadGood);
            Core.CompileSingleExpr(good, e, p);
            p.Add(lGood);
        }
        public static void CompileQuote(IType body, Environment e, IL.Function p)
        {
            var quoted = Util.RequireExactly(body, 1, "QUOTE")[0];
            Core.CompileConstant(quoted, e, p);
        }
        public static void CompileLet(IType body, Environment e, IL.Function p)
        {
            if (!(body is Cons b))
                throw new SyntaxError("LET: insufficient argument");
            var bindings = b.car;
            Environment cure = new Environment(e, 0);
            while (bindings is Cons l)
            {
                var cur = l.car;
                bindings = l.cdr;
                if (cur is Cons c)
                {
                    var temp = Util.RequireExactly(cur, 2, "LET");
                    IType name = temp[0], value = temp[1];
                    if (!(name is Symbol s))
                        throw new SyntaxError("LET: illegal name");
                    Core.CompileSingleExpr(value, e, p);
                    p.Add(new IL.MoveInstruction(Global.rax, cure.AddVariable(s)));
                }
                else if (cur is Symbol s)
                {
                    cure.AddVariable(s); //assume unassigned variable have nil
                }
                else throw new SyntaxError("LET: illegal binding");
            }
            CompileProgn(b.cdr, cure, p);
        }
        public static void CompileLetStar(IType body, Environment e, IL.Function p)
        {
            var (t1, tbody) = Util.RequireAtLeast(body, 1, "LET*");
            var bindings = t1[0];
            Environment cure = new Environment(e, 0);
            while (bindings is Cons c)
            {
                var cur = c.car;
                bindings = c.cdr;
                if (cur is Cons b)
                {
                    var temp = Util.RequireExactly(cur, 2, "LET*");
                    IType name = temp[0], value = temp[1];
                    if (!(name is Symbol s))
                        throw new SyntaxError("LET*: illegal name");
                    Core.CompileSingleExpr(value, cure, p);
                    p.Add(new IL.MoveInstruction(Global.rax, cure.AddVariable(s)));
                }
                else if (cur is Symbol s)
                {
                    cure.AddVariable(s); // assume...
                }
                else throw new SyntaxError("LET*: illegal binding");
            }
            CompileProgn(tbody, cure, p);
        }
        public static void CompileLambda(IType body, Environment e, IL.Function p)
        {
            var (t1, tbody) = Util.RequireAtLeast(body, 1, "LAMBDA");
            Environment cure = new Environment(e, 0);
            Function f = new Function(cure);
            if (t1[0] is Cons c)
                Util.ParseLambdaList(c, cure, f);
            CompileProgn(tbody, cure, f);
            f.Add(new IL.ReturnInstruction(Global.rax));
            p.Add(new IL.FunctionInstruction(f, Global.rax));
        }
        public static void CompileSetq(IType body, Environment e, IL.Function p)
        {
            while (body is Cons c)
            {
                var t1 = c.car;
                if (!(t1 is Symbol s)) throw new SyntaxError("SETQ: illegal name");
                t1 = c.cdr;
                if (!(t1 is Cons t2)) throw new SyntaxError("SETQ: insufficient arguments");
                body = t2.cdr;
                t1 = t2.car;
                Core.CompileSingleExpr(t1, e, p);
                p.Add(new IL.MoveInstruction(Global.rax, e.Find(s)));
            }
            p.Add(new IL.MoveInstruction(Global.nil, Global.rax));
        }
        public static void Dispatch(Cons form, Environment e, IL.Function p)
        {
            switch (GetType((Symbol)form.car))
            {
                case Type.SETQ:
                    CompileSetq(form.cdr, e, p);
                    break;
                case Type.PROGN:
                    CompileProgn(form.cdr, e, p);
                    break;
                case Type.IF:
                    CompileIf(form.cdr, e, p);
                    break;
                case Type.QUOTE:
                    CompileQuote(form.cdr, e, p);
                    break;
                case Type.LET:
                    CompileLet(form.cdr, e, p);
                    break;
                case Type.LET_STAR:
                    CompileLetStar(form.cdr, e, p);
                    break;
                case Type.LAMBDA:
                    CompileLambda(form.cdr, e, p);
                    break;
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
            if (!inited)
            {
                Lisp.Init();
                types = new Dictionary<Symbol, Type>(){
                    {Symbol.FindOrCreate("BLOCK"), Type.BLOCK },
                    {Symbol.FindOrCreate("LAMBDA"), Type.LAMBDA },
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
