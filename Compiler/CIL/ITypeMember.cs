using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.CIL
{
    class ITypeMember : Member
    {
        public override string Name { get; set; }

        public override string Type
        {
            get
            {
                return "[Runtime]Runtime.IType";
            }
        }

        public Environment Environment { get; }

        public ITypeMember(Environment env, IL.Variable var)
        {
            Environment = env;
            Name = "'" + var.Name + "'";
        }
    }
}
