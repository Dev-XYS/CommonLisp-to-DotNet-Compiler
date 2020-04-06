using System;

namespace Runtime
{
    public interface IType
    {
        public IType Invoke(IType[] args);
    }
}
