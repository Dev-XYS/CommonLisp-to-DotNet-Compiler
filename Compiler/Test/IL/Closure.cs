﻿using System;
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
            var minus = new Variable(E0);
            E0.VariableList.Add(minus);

            // environment E1
            Environment E1 = new Environment();
            var balance = new Variable(E1);
            E1.VariableList.Add(balance);

            // environment E2
            Environment E2 = new Environment();
            var x = new Variable(E2);
            E2.VariableList.Add(x);

            // function anonymous (x) (environment: [E0, E1, E2])
            Function anonymous = new ParametersFunction();
            anonymous.EnvList.Add(E0);
            anonymous.EnvList.Add(E1);
            anonymous.EnvList.Add(E2);

            CallInstruction i1 = new CallInstruction(minus/*-*/, E1.VariableList[0]/*balance*/);
            i1.Parameters.Add(E1.VariableList[0]/*balance*/);
            i1.Parameters.Add(E2.VariableList[0]/*x*/);
            ReturnInstruction i2 = new ReturnInstruction(E1.VariableList[0]/*balance*/);
            anonymous.InstructionList.Add(i1);
            anonymous.InstructionList.Add(i2);

            // function create-account (balance) (environment: [E0, E1])
            Function create_account = new ParametersFunction();
            create_account.EnvList.Add(E0);
            create_account.EnvList.Add(E1);

            var temp = new Variable(E1);
            FunctionInstruction i3 = new FunctionInstruction(anonymous, temp);
            ReturnInstruction i4 = new ReturnInstruction(temp);
            create_account.InstructionList.Add(i3);
            create_account.InstructionList.Add(i4);

            return p;
        }
    }
}
