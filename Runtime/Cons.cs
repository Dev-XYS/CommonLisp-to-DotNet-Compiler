using System;
using System.Collections.Generic;
using System.Text;

namespace Runtime
{
    class Cons : IType
    {
        public IType car, cdr;
        public Cons(IType ia, IType id)
        {
            car = ia;
            cdr = id;
        }
        public Cons() { }
    }
}
