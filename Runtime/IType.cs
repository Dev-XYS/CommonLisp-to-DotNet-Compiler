using System;

namespace Runtime
{
    public interface IType
    {
        public IType Invoke(IType[] args)
        {
            throw new RuntimeException(this.ToString() + " cannot be invoked.");
        }
    }
}
