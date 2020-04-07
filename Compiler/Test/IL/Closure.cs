using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Test.IL
{
    using Compiler.IL;

    static class Closure
    {
        /* Racket:
         * 
         * (define (create-account balance)
         *   (lambda (x) (set! balance (- balance x)) balance))
         * 
         * (define Alice (create-account 1000))
         * (define Bob (create-account 200))
         * 
         * (displayln (Alice 200))
         * (displayln (Bob 50))
         */

        /* Compiles to:
         * 
         * environment E1 [balance]
         * environment E2 [x]
         * 
         * function anonymous (x) (environment: [E1], scope: E2)
         *     FUNC-CALL - (E1.balance, E2.x) --> temp
         *     MOVE E1.balance = temp
         *     RETURN E1.balance
         * 
         * function create-account (balance) (environment: none, scope: [balance])
         *     CREATE-FUNC anonymous (environment: [E1]) --> temp
         *     RETURN temp
         * 
         * FUNC-CALL create-account (1000)
         */

        public static Program GenerateCode()
        {
            Program p = new Program();

            // environment E0
            Environment E0 = new Environment();
            var minus = new Variable("minus", E0);
            minus.Name = "minus";
            E0.VariableList.Add(minus);

            // environment E1
            Environment E1 = new Environment();
            var balance = new Variable("balance", E1);
            balance.Name = "balance";
            E1.VariableList.Add(balance);

            // environment E2
            Environment E2 = new Environment();
            var x = new Variable("x", E2);
            x.Name = "x";
            E2.VariableList.Add(x);

            p.EnvList.Add(E0);
            p.EnvList.Add(E1);
            p.EnvList.Add(E2);

            // function anonymous (x) (environment: [E0, E1, E2])
            Function anonymous = new ParametersFunction();
            anonymous.Name = "anonymous";
            anonymous.EnvList.Add(E0);
            anonymous.EnvList.Add(E1);
            anonymous.EnvList.Add(E2);
            anonymous.Parameters.Add(x);

            CallInstruction i1 = new CallInstruction(minus/*-*/, E1.VariableList[0]/*balance*/);
            i1.Parameters.Add(E1.VariableList[0]/*balance*/);
            i1.Parameters.Add(E2.VariableList[0]/*x*/);
            ReturnInstruction i2 = new ReturnInstruction(E1.VariableList[0]/*balance*/);
            anonymous.InstructionList.Add(i1);
            anonymous.InstructionList.Add(i2);

            // function create-account (balance) (environment: [E0, E1])
            Function create_account = new ParametersFunction();
            create_account.Name = "create_account";
            create_account.EnvList.Add(E0);
            create_account.EnvList.Add(E1);
            create_account.Parameters.Add(balance);

            var temp = new Variable("temp", E1);
            temp.Name = "temp";
            E1.VariableList.Add(temp);
            FunctionInstruction i3 = new FunctionInstruction(anonymous, temp);
            ReturnInstruction i4 = new ReturnInstruction(temp);
            create_account.InstructionList.Add(i3);
            create_account.InstructionList.Add(i4);

            p.FunctionList.Add(anonymous);
            p.FunctionList.Add(create_account);

            var main0 = new ParametersFunction();
            main0.EnvList.Add(E0);
            main0.InstructionList.Add(new ReturnInstruction(null));
            p.Main = main0;
            p.FunctionList.Add(main0);

            return p;
        }
    }
}
