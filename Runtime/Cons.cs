using System;
using System.Collections.Generic;
using System.Text;

namespace Runtime
{
    public class Cons : IType
    {
        public IType car, cdr;
        public Cons(IType ia, IType id)
        {
            car = ia;
            cdr = id;
        }
        public Cons() { }
        public override string ToString()
        {
            return " ( " + car.ToString() + " . " + cdr.ToString() + " ) ";
        }

        public IType Invoke(IType[] args)
        {
            throw new RuntimeException(this.ToString() + " cannot be invoked.");
        }
    }
}
