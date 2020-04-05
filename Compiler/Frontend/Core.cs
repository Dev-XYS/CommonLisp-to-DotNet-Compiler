using Runtime;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Frontend
{
    static class Core
    {
        static void CompileFunctionCall(IL.Variable function, IType parameters, Environment e, IL.IProcedure p)
        {
            Environment inner = new Environment(e, 0);
            IL.CallInstruction callInstruction = new IL.CallInstruction(function, Global.rax);
            while(parameters is Cons para)
            {
                var cur = para.car;
                if(cur is Cons c)
                {
                    CompileSingleForm(c, e, p);
                    IL.Variable variable = new IL.Variable(inner);
                    p.Add(new IL.MoveInstruction(Global.rax, variable));
                    callInstruction.Parameters.Add(variable);
                }else if(cur is Symbol s)
                {
                    callInstruction.Parameters.Add(e.Find(s));
                }else
                {
                    CompileConstant(cur, e, p);
                    IL.Variable variable = new IL.Variable(inner);
                    p.Add(new IL.MoveInstruction(Global.rax, variable));
                    callInstruction.Parameters.Add(variable);
                }
                parameters = para.cdr;
            }
            p.Add(callInstruction);
        }
        static void CompileSingleForm(Cons form, Environment e, IL.IProcedure p)
        {
            var car = form.car;
            if (car is Symbol s)
            {
                if (SO.IsSO(s))
                {
                    SO.Dispatch(form, e, p);
                }
                else if (Macro.IsMacro(s))
                {
                    CompileSingleForm(Macro.FullExpand(form), e, p);
                }
                else
                {
                    CompileFunctionCall(e.Find(s), form.cdr, e, p);
                }
            }
            else if (car is Cons c)
            {
                CompileSingleForm(c, e, p);
                CompileFunctionCall(Global.rax, form.cdr, e, p);
            }else 
            throw new SyntaxError(string.Format("Object is not a function, macro or special operator: {0}", car));
        }
        static void CompileConstant(IType value, Environment e, IL.IProcedure p)
        {
            p.Add(new IL.MoveInstruction(Constant.New(value), Global.rax));
        }
        static void CompileSingleExpr(IType expr, Environment e, IL.IProcedure p)
        {
            if (expr is Cons form)
            {
                CompileSingleForm(form, e, p);
            }else
            {
                CompileConstant(expr, e, p);
            }
        }
        public static IL.Program CompileFromStdin()
        {
            IL.Program ret = new IL.Program();
            IType expr;
            while(true)
            {
                try
                {
                    expr = Reader.Read(Lisp.stdin);
                    CompileSingleExpr(expr, Global.env, ret);
                }catch(Reader.EOFError)
                {
                    return ret;
                }
            }
        }
    }
}
