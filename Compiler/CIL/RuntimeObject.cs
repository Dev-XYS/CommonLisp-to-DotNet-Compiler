using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Compiler.CIL
{
    class RuntimeObject : Class
    {
        public Type Type { get; set; }

        public override string Name
        {
            get
            {
                return string.Format("[{0}]{1}", Type.Assembly.GetName().Name, Type.FullName);
            }
        }

        public override string AccessString { get => Name; }

        public override string CtorArgumentList
        {
            get
            {
                var ctorList = Type.GetConstructors();
                ParameterInfo[] args = ctorList[0].GetParameters();
                return string.Join(", ", Array.ConvertAll(args, (ParameterInfo x) =>
                {
                    // For builtin types.
                    if (x.ParameterType == typeof(int))
                    {
                        return "int32";
                    }
                    else if (x.ParameterType == typeof(string))
                    {
                        return "string";
                    }
                    // Assume other types are contained in the namespace `Runtime`.
                    // unnecessary code?
                    else
                    {
                        return string.Format("class [Runtime]{0}", x.ParameterType.FullName);
                    }
                }));
            }
        }
    }
}
