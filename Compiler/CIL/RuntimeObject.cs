using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.CIL
{
    class RuntimeObject : Class
    {
        public Type Type { get; set; }

        public override string CtorArgumentList
        {
            get
            {
                string r = "";
                var ctorList = Type.GetConstructors();
                var args = ctorList[0].GetParameters();
                if (args.Length > 0)
                {
                    r = args[0].ParameterType.Name.ToLower();
                }
                for (int i = 1; i < args.Length; i++)
                {
                    r += string.Format(", {0}", args[i].ParameterType.Name.ToLower());
                }
                return r;
            }
        }

        public override string Name
        {
            get
            {
                return string.Format("[Runtime]{0}", Type.FullName);
            }
        }
    }
}
