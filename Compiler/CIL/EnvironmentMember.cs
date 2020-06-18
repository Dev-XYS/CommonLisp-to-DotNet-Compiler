using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.CIL
{
    class EnvironmentMember : Member
    {
        public override string Name { get; set; }

        public override string Type
        {
            get
            {
                return Environment.Name;
            }
        }

        public Environment Environment { get; }

        public Function Function { get; }

        public bool IsGlobal { get => Environment.IsGlobal; }
        public bool IsLibGlobal { get => Function.IsLibMain && IsGlobal; }
        public string PrivateOrPublic { get => IsLibGlobal ? "public" : "private"; }

        public EnvironmentMember(Function func, IL.Environment env)
        {
            // We use the same name as the environment.
            // e.g. ".field private class env0 env0"
            Name = env.Name;

            Function = func;
            Environment = Function.Program.EnvMap[env];
        }
    }
}
