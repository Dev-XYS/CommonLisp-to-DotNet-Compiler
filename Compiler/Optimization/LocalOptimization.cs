using Compiler.IL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Optimization
{
    static class LocalOptimization
    {
        static private Program Program;

        static public Program Optimize(Program program)
        {
            Program = program;
            OptimizeAllFunctions();
            return Program;
        }

        static private void OptimizeAllFunctions()
        {
            Initialize();
            foreach (Function func in Program.FunctionList)
            {
                func.OptimizeLocalVariables();
            }
        }

        static private void Initialize()
        {
            GetLocalVariables();
        }

        /// <summary>
        /// Initialize non-local variable list.
        /// </summary>
        static private void GetLocalVariables()
        {
            // For every defined and used variable which is not declared in local
            // environment, it is non-local and cannot be optimized.
            foreach (Function func in Program.FunctionList)
            {
                foreach (IL.Instruction instr in func.InstructionList)
                {
                    if (instr.DefinedVariable != null && !func.LocalEnv.ContainsVariable(instr.DefinedVariable))
                    {
                        Variable.NonLocalVariables.Add(instr.DefinedVariable);
                    }
                    foreach (var v in instr.UsedVariables)
                    {
                        // Null should not count as variable.
                        if (v == null) continue;

                        if (!func.LocalEnv.ContainsVariable(v))
                        {
                            Variable.NonLocalVariables.Add(v);
                        }
                    }
                }
            }
            Console.WriteLine("\nClosure variables:");
            foreach (IL.Variable var in Variable.NonLocalVariables)
            {
                Console.WriteLine(var);
            }
            Console.WriteLine();

            // Allocate a local variable number for each local variable.
            foreach (Function func in Program.FunctionList)
            {
                foreach (IL.Variable var in func.LocalEnv.VariableList)
                {
                    if (!Variable.NonLocalVariables.Contains(var))
                    {
                        func.RegisterLocalVariable(var);
                    }
                }
            }
        }
    }

    partial class Function
    {
        public Dictionary<IL.IEntity, LocalVariable> Loc { get; } = new Dictionary<IEntity, LocalVariable>();
        private int LocNum { get; set; } = 3;
        public int LocCount { get => LocNum - 3; }

        public bool RegisterLocalVariable(IL.Variable var)
        {
            if (!Loc.ContainsKey(var))
            {
                Loc[var] = new LocalVariable(LocNum++);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void OptimizeLocalVariables()
        {
            // Optimize function parameters.
            for (int i = 0; i < Parameters.Count; i++)
            {
                if (!Variable.NonLocalVariables.Contains(Parameters[i]))
                {
                    Parameters[i] = Loc[Parameters[i]];
                }
            }

            // Optimize variables in instructions.
            foreach (IL.Instruction instr in InstructionList)
            {
                if (instr is IL.CallInstruction call)
                {
                    opt_Local_Call(call);
                }
                else if (instr is IL.FunctionInstruction func)
                {
                    opt_Local_Func(func);
                }
                else if (instr is IL.MoveInstruction move)
                {
                    opt_Local_Move(move);
                }
                else if (instr is IL.ConditionalJumpInstruction jump)
                {
                    opt_Local_Jump(jump);
                }
                else if (instr is IL.ReturnInstruction ret)
                {
                    opt_Local_Ret(ret);
                }
                else if (instr is IL.UnconditionalJumpInstruction || instr is IL.Label)
                {
                    // No optimization.
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        private void opt_Local_Call(IL.CallInstruction instr)
        {
            if (Loc.ContainsKey(instr.Function)) instr.Function = Loc[instr.Function];
            if (Loc.ContainsKey(instr.Destination)) instr.Destination = Loc[instr.Destination];
            for (int i = 0; i < instr.Parameters.Count; i++) if (Loc.ContainsKey(instr.Parameters[i])) instr.Parameters[i] = Loc[instr.Parameters[i]];
        }

        private void opt_Local_Func(IL.FunctionInstruction instr)
        {
            if (Loc.ContainsKey(instr.Destination)) instr.Destination = Loc[instr.Destination];
        }

        private void opt_Local_Jump(IL.ConditionalJumpInstruction instr)
        {
            if (Loc.ContainsKey(instr.TestVariable)) instr.TestVariable = Loc[instr.TestVariable];
        }

        private void opt_Local_Move(IL.MoveInstruction instr)
        {
            if (Loc.ContainsKey(instr.Source)) instr.Source = Loc[instr.Source];
            if (Loc.ContainsKey(instr.Destination)) instr.Destination = Loc[instr.Destination];
        }

        private void opt_Local_Ret(IL.ReturnInstruction instr)
        {
            if (Loc.ContainsKey(instr.Value)) instr.Value = Loc[instr.Value];
        }
    }
}
