using System;
using System.Collections.Generic;
using System.Text;

namespace Runtime
{
    class Add : IType
    {
        public IType Invoke(IType[] args)
        {
            if (args.Length != 2)
            {
                throw new RuntimeException("add: 2 parameters required, {0} given.", args.Length);
            }
            if (!(args[0] is TInteger) || !(args[1] is TInteger))
            {
                throw new RuntimeException("add: integer parameters required.");
            }
            return new TInteger((args[0] as TInteger).Value + (args[1] as TInteger).Value);
        }
    }
}
