using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.CIL
{
    class Program
    {
        private List<Environment> EnvList { get; }
        public Dictionary<IL.Environment, Environment> EnvMap { get; }

        private List<Function> FuncList { get; }
        public Dictionary<IL.Function, Function> FuncMap { get; }

        private Function Main { get; }

        public ConstantClass Const { get; }

        public Program(IL.Program prog)
        {
            EnvList = new List<Environment>();
            EnvMap = new Dictionary<IL.Environment, Environment>();
            FuncList = new List<Function>();
            FuncMap = new Dictionary<IL.Function, Function>();

            Const = new ConstantClass();

            foreach (IL.Environment env in prog.EnvList)
            {
                Environment e = new Environment(env);
                EnvList.Add(e);
                EnvMap[env] = e;
            }

            foreach (IL.Function func in prog.FunctionList)
            {
                Function f = new Function(this, func);
                FuncList.Add(f);
                FuncMap[func] = f;
            }
            foreach (Function f in FuncList)
            {
                f.Generate();
            }

            Main = FuncMap[prog.Main];

            Const.Generate();
        }

        public void Emit(System.IO.TextWriter writer, EmissionType type)
        {
            Emitter.Writer = writer;

            EmitHeader(type);

            EmitMain(type);

            Const.Emit();

            foreach (Environment env in EnvList)
            {
                env.Emit();
            }
            foreach (Function func in FuncList)
            {
                func.Emit();
            }
        }

        private void EmitHeader(EmissionType type)
        {
            if (type == EmissionType.Program)
            {
                Emitter.EmitRaw(global::Compiler.Properties.Resources.CILHeader);
            }
            else if (type == EmissionType.Library)
            {
                Emitter.EmitRaw(global::Compiler.Properties.Resources.CILLibHeader);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private void EmitMain(EmissionType type)
        {
            if (type == EmissionType.Program)
            {
                Emitter.Emit("");
                Emitter.Emit(global::Compiler.Properties.Resources.CILMain, Main.Name);
            }
            else if (type == EmissionType.Library)
            {
                // Do nothing.
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
