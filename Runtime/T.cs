using System;
using System.Collections.Generic;
using System.Text;

namespace Runtime
{
    public class T : IType
    {
        public T()
        {
            //do nothing
        }
        public override string ToString()
        {
            return "T";
        }
        public IType Invoke(IType[] args)
        {
            throw new RuntimeException(this.ToString() + " cannot be invoked.");
        }
    }
}
