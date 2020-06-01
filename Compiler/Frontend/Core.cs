using Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Compiler.Frontend
{
    static class Core
    {
        private static IL.Program prog = new IL.Program();
        public static Function main;
        private static bool inited = false;
        static void CompileFunctionCall(LocalVariable function, IType parameters, Environment e, Function p)
        {
            var args = new List<LocalVariable>();
            while(parameters is Cons para)
            {
                var cur = para.car;
                LocalVariable v = e.AddUnnamedVariable();
                if(cur is Cons c)
                {
                    CompileSingleForm(c, e, p);
                }else if(cur is Symbol s)
                {
                    CompileAtom(s, e, p);
                }else
                {
                    CompileConstant(cur, e, p);
                }
                p.Load(v);
                args.Add(v);
                parameters = para.cdr;
            }
            p.Call(function, args.ToArray());
        }
        static void CompileSingleForm(Cons form, Environment e, Function p)
        {
            var car = form.car;
            if (car is Symbol s)
            {
                if (SO.IsSO(s))
                {
                    SO.Dispatch(form, e, p);
                    return;
                }
                else if (Macro.IsMacro(s))
                {
                    CompileSingleExpr(Macro.FullExpand(form), e, p);
                    return;
                }
                else
                {
                    CompileAtom(s, e, p);
                }
            }
            else if (car is Cons c)
            {
                CompileSingleForm(c, e, p);
            }else 
            throw new SyntaxError(string.Format("Object is not a function, macro or special operator: {0}", car));
            LocalVariable v = e.AddUnnamedVariable();
            p.Load(v);
            CompileFunctionCall(v, form.cdr, e, p);
        }
        static void CompileAtom(Symbol s, Environment e, Function p)
        {
            //currently treat it as variable//todo: Symbol macro
            p.Store(p.FindRight(s));
        }
        public static void CompileConstant(IType value, Environment e, Function p)
        {
            p.Store(value);
        }
        public static void CompileSingleExpr(IType expr, Environment e, Function p)
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
                main = new Function(Global.env);
                prog.Main = main;
                LibraryFunctions.AddAll(Global.env, main);
                Global.Init();
            }
        }
        private static IL.Program CompileFrom(IInputStream input)
        {
            Init();
            IType expr;
            while(true)
            {
                try
                {
                    expr = Reader.Read(input);
                    CompileSingleExpr(expr, Global.env, main);
                }catch(Reader.EOFError)
                {
                    break;
                }
            }
            prog.EnvList = Environment.gel;
            prog.FunctionList = Function.gfl;
            main.Return();
            return prog;
        }
        public static IL.Program CompileFromStdin()
        {
            return CompileFrom(Lisp.stdin);
        }
        public static IL.Program CompileFromFile(string path)
        {
            FileInput fin = new FileInput(path);
            return CompileFrom(fin);
        }
    }
}
