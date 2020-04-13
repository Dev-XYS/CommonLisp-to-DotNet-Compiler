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
        public static void AddAll(Environment e, Function f)
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
            AddOne(">", new Greater());
            AddOne("<=", new LessEqual());
            AddOne(">=", new GreaterEqual());
            AddOne("=", new NumberEqual());
            AddOne("/=", new NumberNotEqual());
            AddOne("NOT", new LogicNot());
            AddOne("EQ", new Eq());
            AddOne("EQL", new Eql());
            AddOne("LIST", new List());
            AddOne("CAR", new Car());
            AddOne("CDR", new Cdr());
            AddOne("READ", new Read());
            AddOne("WRITE", new Write());
            AddOne("WRITELN", new WriteLn());
            AddOne("#SPECIAL-GET", new SpecialGet());
            AddOne("#SPECIAL-SET", new SpecialSet());
            AddOne("#SPECIAL-PUSH", new SpecialPush());
            AddOne("#SPECIAL-POP", new SpecialPop());
            AddOne("#SPECIAL-RESET", new SpecialReset());
        }
    }
}
