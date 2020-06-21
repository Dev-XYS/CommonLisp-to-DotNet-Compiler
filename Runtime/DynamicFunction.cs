using System;
using System.Collections.Generic;
using System.Text;

namespace Runtime
{
    public class DynamicFunction : IType
    {
        Func<IType[], IType> func;
        public DynamicFunction(Func<IType[], IType> f)
        {
            func = f;
        }
        public IType Invoke(IType[] args)
        {
            return func(args);
        }
    }
}
