using System;
using System.Collections.Generic;
using System.Text;

namespace Runtime
{
    public abstract class Number : IDataType
    {
        public abstract Number Add(Number _);
        public abstract Number Subtract(Number _);
        public abstract Number Multiply(Number _);
        public abstract Number Divide(Number _);
        public abstract Number Negate();
        public abstract T LessThan(Number _);
        public abstract Number Reciprocal();
        public IType Invoke(IType[] _)
        {
            throw new RuntimeException("Number cannot be invoked.");
        }
    }
}
