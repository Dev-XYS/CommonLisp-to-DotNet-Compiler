using Runtime;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Compiler.Frontend
{
    static class Core
    {
        private static IL.Program prog = new IL.Program();
        private static bool inited = false;
        static void CompileFunctionCall(IL.Variable function, IType parameters, Environment e, IL.Function p)
        {
            IL.CallInstruction callInstruction = new IL.CallInstruction(function, Global.rax);
            while(parameters is Cons para)
            {
                var cur = para.car;
                if(cur is Cons c)
                {
                    CompileSingleForm(c, e, p);
                    IL.Variable variable = Global.env.AddUnnamedVariable();
                    p.Add(new IL.MoveInstruction(Global.rax, variable));
                    callInstruction.Parameters.Add(variable);
                }else if(cur is Symbol s)
                {
                    callInstruction.Parameters.Add(e.Find(s));
                }else
                {
                    CompileConstant(cur, e, p);
                    IL.Variable variable = Global.env.AddUnnamedVariable();
                    p.Add(new IL.MoveInstruction(Global.rax, variable));
                    callInstruction.Parameters.Add(variable);
                }
                parameters = para.cdr;
            }
            p.Add(callInstruction);
        }
        static void CompileSingleForm(Cons form, Environment e, IL.Function p)
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
                IL.Variable func = Global.env.AddUnnamedVariable();
                p.Add(new IL.MoveInstruction(Global.rax, func));
                CompileFunctionCall(func, form.cdr, e, p);
            }else 
            throw new SyntaxError(string.Format("Object is not a function, macro or special operator: {0}", car));
        }
        static void CompileAtom(Symbol s, Environment e, IL.Function p)
        {
            //currently treat it as variable
            p.Add(new IL.MoveInstruction(e.Find(s), Global.rax));
        }
        public static void CompileConstant(IType value, Environment e, IL.Function p)
        {
            p.Add(new IL.MoveInstruction(new IL.ImmediateNumber(value), Global.rax));
        }
        public static void CompileSingleExpr(IType expr, Environment e, IL.Function p)
        {
            if (expr is Cons form)
            {
                CompileSingleForm(form, e, p);
            }else if(expr is Symbol s)
            {
                CompileAtom(s, e, p);
            }else 
            {
                CompileConstant(expr, e, p);
            }
        }
        public static void Init()
        {
            if(!inited)
            {
                inited = true;
                prog.Main = new IL.ParametersFunction();
                LibraryFunctions.AddAll(Global.env, prog.Main);
                prog.Main.EnvList.Add(Global.env);
                Global.Init();
            }
        }
        public static IL.Program CompileFromStdin()
        {
            Init();
            IType expr;
            while(true)
            {
                try
                {
                    expr = Reader.Read(Lisp.stdin);
                    CompileSingleExpr(expr, Global.env, prog.Main);
                }catch(Reader.EOFError)
                {
                    break;
                }
            }
            prog.EnvList = IL.Environment.gel;
            prog.FunctionList = IL.ParametersFunction.gfl;
            prog.Main.Add(new IL.ReturnInstruction(Global.rax));
            return prog;
        }
    }
}
