using System;
using System.Collections.Generic;
using System.Text;

namespace Runtime.Function.Arith
{
    public class Less : IType
    {
        public IType Invoke(IType[] args)
        {
            if (args.Length != 2)
                throw new RuntimeException("Less: 2 arguments required, {0} given", args.Length);
            var nums = Util.AllNumbers(args);
            Util.Contagion(nums);
            return nums[0].LessThan(nums[1]);
        }
    }
}
