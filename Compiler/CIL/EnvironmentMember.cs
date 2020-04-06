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

        public EnvironmentMember(Function func, IL.Environment env)
        {
            Name = env.Name;
            Function = func;
            Environment = Function.Program.EnvMap[env];
        }
    }
}
