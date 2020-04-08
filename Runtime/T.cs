using System;
using System.Collections.Generic;
using System.Text;

namespace Runtime
{
    public class T : IType
    {
        private static T t = new T();
        private T()
        {
            //do nothing
        }
        public override string ToString()
        {
            return "T";
        }
        public static T GetT()
        {
            return t;
        }
        public IType Invoke(IType[] args)
        {
            throw new RuntimeException(this.ToString() + " cannot be invoked.");
        }
    }
}
