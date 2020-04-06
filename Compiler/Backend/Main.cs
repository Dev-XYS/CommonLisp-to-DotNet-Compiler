using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Backend
{
    using Compiler.CIL;
    using I = Compiler.CIL.Instructions;

    static partial class Main
    {
        /// <summary>
        /// 在后端编译过程中，始终使用此静态对象作为输出的CIL程序。
        /// </summary>
        private static Program Target { get; set; }

        private static int EnvNo { get; set; }
        private static Dictionary<IL.Environment, string> EnvName { get; set; }
        private static Dictionary<IL.Environment, Class> EnvClass { get; set; }

        private static Dictionary<IL.Function, Class> FuncClass { get; set; }

        public static Program Compile(IL.Program program)
        {
            Target = new Program(program);
            EnvClass = new Dictionary<IL.Environment, Class>();
            FuncClass = new Dictionary<IL.Function, Class>();
            //CompileEnvironments(program);
            //CompileFunctions(program);

            return Target;
        }

        private static void CompileEnvironments(IL.Program program)
        {
            foreach (IL.Environment env in program.EnvList)
            {
                Class c = new Class(env.Name, true);
                foreach (IL.Variable var in env.VariableList)
                {
                    Member m = new Member("Runtime.IType", var.Name);
                    c.MemberList.Add(m);
                    c.VarMap[var] = m;
                }
                Target.ClassList.Add(c);
                EnvClass[env] = c;
            }
        }

        private static void CompileFunctions(IL.Program program)
        {
            foreach (IL.Function func in program.FunctionList)
            {
                Class c = new Class(func.Name);

                foreach (IL.Environment env in func.EnvList)
                {
                    Member m = new Member(env.Name, "@" + env.Name);
                    c.MemberList.Add(m);
                    c.EnvMap[env] = m;
                    c.EnvClass[env] = EnvClass[env];
                }

                Function f = new Function();
                foreach (IL.IInstruction instr in func.InstructionList)
                {
                    f.InstructionList.AddRange(CompileInstruction(c, instr));
                }
                c.Function = f;

                Target.ClassList.Add(c);
                FuncClass[func] = c;
            }
        }

        private static List<Instruction> CompileInstruction(Class c, IL.IInstruction instr)
        {
            Console.WriteLine("hello");
            List<Instruction> result = new List<Instruction>();

            if (instr is IL.CallInstruction)
            {
                IL.CallInstruction ins = instr as IL.CallInstruction;
                result.Add(new I.LoadArgument { ArgNo = 0 });
                result.Add(new I.LoadField { Field = c.EnvMap[ins.Function.Env] });
                result.Add(new I.LoadInt { Value = ins.Parameters.Count });
                result.Add(new I.NewArray { Type = "Runtime.IType" });
                for (int i = 0; i < ins.Parameters.Count; i++)
                {
                    result.Add(new I.LoadInt { Value = i });
                    result.AddRange(CompileLoadEntity(c, ins.Parameters[i]));
                }
                result.Add(new I.Call { });
            }
            else if (instr is IL.FunctionInstruction)
            {
                IL.FunctionInstruction ins = instr as IL.FunctionInstruction;
                for (int i = 0; i < ins.Function.EnvList.Count - 1; i++)
                {
                    IL.Environment env = ins.Function.EnvList[i];
                    result.Add(new I.LoadField { Field = c.EnvMap[env] });
                }
                result.Add(new I.NewObject { Type = FuncClass[ins.Function] });
            }
            else if (instr is IL.MoveInstruction)
            {
                IL.MoveInstruction ins = instr as IL.MoveInstruction;
                if (ins.Destination.Env == null)
                {
                    result.AddRange(CompileLoadEntity(c, ins.Source));
                    if (!c.Function.VarMap.ContainsKey(ins.Destination))
                    {
                        c.Function.VarMap[ins.Destination] = c.Function.VarNo++;
                    }
                    result.Add(new I.Store { Loc = c.Function.VarMap[ins.Destination] });
                }
                else
                {
                    result.Add(new I.LoadField { Field = c.EnvMap[ins.Destination.Env] });
                    result.AddRange(CompileLoadEntity(c, ins.Source));
                    result.Add(new I.StoreField { Field = c.EnvClass[ins.Destination.Env].VarMap[ins.Destination] });
                }
            }
            else if (instr is IL.ReturnInstruction)
            {
                IL.ReturnInstruction ins = instr as IL.ReturnInstruction;
                result.AddRange(CompileLoadEntity(c, ins.Value));
                result.Add(new I.Return { });
            }

            return result;
        }

        private static List<Instruction> CompileLoadEntity(Class c, IL.IEntity entity)
        {
            List<Instruction> result = new List<Instruction>();

            if (entity is IL.ImmediateNumber)
            {
                throw new NotImplementedException();
            }
            else if (entity is IL.Variable)
            {
                IL.Variable var = entity as IL.Variable;
                if (var.Env == null)
                {
                    result.Add(new I.Load { Loc = c.Function.VarMap[var] });
                }
                else
                {
                    result.Add(new I.LoadArgument { ArgNo = 0 });
                    result.Add(new I.LoadField { Field = c.EnvMap[var.Env] });
                    result.Add(new I.LoadField { Field = c.EnvClass[var.Env].VarMap[var] });
                }
            }

            return result;
        }

        private static List<Instruction> CompileStore(Class c, IL.Variable var)
        {
            List<Instruction> result = new List<Instruction>();

            if (var.Env == null)
            {
                result.Add(new I.Store { Loc = c.Function.VarMap[var] });
            }
            else
            {
                result.Add(new I.LoadArgument { ArgNo = 0 });
                result.Add(new I.LoadField { Field = c.EnvMap[var.Env] });
                result.Add(new I.LoadField { Field = c.EnvClass[var.Env].VarMap[var] });
            }

            return result;
        }
    }
}
