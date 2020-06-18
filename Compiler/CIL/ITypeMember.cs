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

        public bool IsTemp { get => Name.StartsWith("temp"); }
        public string InternalOrPublic { get => IsTemp ? "assembly" : "public"; }

        public ITypeMember(Environment env, IL.Variable var)
        {
            Environment = env;
            Name = var.Name;
        }
    }
}
