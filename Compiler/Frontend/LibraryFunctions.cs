using System;
using System.Collections.Generic;
using System.Text;
using Runtime.Function.Arith;
using Runtime.Function;
using Runtime;

namespace Compiler.Frontend
{
    static class LibraryFunctions
    {
        public static void AddAll(Environment e, IL.Function f)
        {
            void AddOne(string name, IType value)
            {
                f.Add(new IL.MoveInstruction(new IL.ImmediateNumber(value), e.AddVariable(Symbol.FindOrCreate(name))));
            }
            AddOne("T", Lisp.t);
            AddOne("NIL", Lisp.nil);
            AddOne("+", new Plus());
            AddOne("-", new Minus());
            AddOne("*", new Multiply());
            AddOne("/", new Divide());
            AddOne("<", new Less());
            AddOne("WRITE", new Write());
            AddOne("WRITELN", new WriteLn());
        }
    }
}
