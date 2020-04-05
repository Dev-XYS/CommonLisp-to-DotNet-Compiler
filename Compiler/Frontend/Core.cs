using Runtime;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Frontend
{
    static class Core
    {
        static void CompileSingle(IType expr, Environment e, IL.Program p)
        {
            if (expr is Cons form)
            {
                var car = form.car;
                if (car is Symbol s)
                {
                    if(SO.IsSO(s))
                    {
                        SO.Dispatch(form, e, p);
                        return;
                    }else if(Macro.IsMacro(s))
                    {
                        form = Macro.FullExpand(form);
                    }

                }
                else throw new SyntaxError(string.Format("Object is not a function, macro or special operator: {0}", car));
            }
        }
        static IL.Program CompileFromStdin()
        {
            IL.Program ret = new IL.Program();
            IType expr;
            while(true)
            {
                try
                {
                    expr = Reader.Read(Lisp.stdin);
                    CompileSingle(expr, Global.env, ret);
                }catch(Reader.EOFError)
                {
                    return ret;
                }
            }
        }
    }
}
