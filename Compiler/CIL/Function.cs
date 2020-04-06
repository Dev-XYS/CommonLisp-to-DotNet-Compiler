using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.CIL
{
    using I = Instructions;

    partial class Function : Class
    {
        public override string Name { get; }

        public Program Program { get; }

        public List<Instruction> CtorInstructionList { get; }
        public List<Instruction> InstructionList { get; }

        private List<EnvironmentMember> EnvList { get; }
        public Dictionary<IL.Environment, EnvironmentMember> EnvMap { get; }

        public int VarNo { get; set; }
        public Dictionary<IL.Variable, int> VarMap { get; }

        public Function(Program prog, IL.Function func)
        {
            Name = func.Name;
            Program = prog;

            CtorInstructionList = new List<Instruction>();
            InstructionList = new List<Instruction>();
            EnvList = new List<EnvironmentMember>();
            EnvMap = new Dictionary<IL.Environment, EnvironmentMember>();
            VarMap = new Dictionary<IL.Variable, int>();

            foreach (IL.Environment env in func.EnvList)
            {
                EnvironmentMember m = new EnvironmentMember(this, env);
                EnvList.Add(m);
                EnvMap[env] = m;
            }

            GenCtor();
            GenBody(func);
        }

        private void CtorGen(Instruction instr)
        {
            CtorInstructionList.Add(instr);
        }

        private void GenCtor()
        {
            CtorGen(new I.LoadArgument { ArgNo = 0 });
            CtorGen(new I.CallObjectCtor { });

            for (int i = 0; i < EnvList.Count - 1; i++)
            {
                CtorGen(new I.LoadArgument { ArgNo = 0 });
                CtorGen(new I.LoadArgument { ArgNo = i + 1 });
                CtorGen(new I.StoreField { Field = EnvList[i] });
            }

            CtorGen(new I.Return { });
        }

        private void GenBody(IL.Function func)
        {
            foreach (ITypeMember m in EnvList[EnvList.Count - 1].Environment.VarMap.Values)
            {
                Gen(new I.LoadArgument { ArgNo = 0 });
                Gen(new I.LoadField { Field = EnvList[EnvList.Count - 1] });
                Gen(new I.LoadNull { });
                Gen(new I.StoreField { Field = m });
            }

            foreach (IL.IInstruction instr in func.InstructionList)
            {
                GenInstruction(instr);
            }
        }

        private string GetCtorArgumentList()
        {
            string r = string.Format("class {0} @{1}", EnvList[0].Type, EnvList[0].Name);
            for (int i = 1; i < EnvList.Count - 1; i++)
            {
                r += string.Format(", class {0} @{1}", EnvList[i].Type, EnvList[i].Name);
            }
            return r;
        }

        public void Emit()
        {
            Emitter.Emit(".class private auto ansi beforeinit {0} extends [System.Runtime]System.Object implements [Runtime]Runtime.IType", Name);
            Emitter.BeginBlock();

            foreach (EnvironmentMember m in EnvList)
            {
                Emitter.Emit(".field private class {0} {1}", m.Environment.Name, m.Name);
            }

            Emitter.Emit(".method public hidebysig specialname rtspecialname instance void .ctor({0}) cil managed", GetCtorArgumentList());
            Emitter.BeginBlock();
            foreach (Instruction instr in CtorInstructionList)
            {
                instr.Emit();
            }
            Emitter.EndBlock();

            Emitter.Emit(".method public hidebysig newslot virtual final instance class [Runtime]Runtime.IType Invoke(class [Runtime]Runtime.IType[] args) cil managed");
            Emitter.BeginBlock();
            foreach (Instruction instr in InstructionList)
            {
                instr.Emit();
            }
            Emitter.EndBlock();

            Emitter.EndBlock();
        }
    }
}
