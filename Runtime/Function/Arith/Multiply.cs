using System;
using System.Collections.Generic;
using System.Text;

namespace Runtime.Function.Arith
{
    public class Multiply : IType
    {
        public IType Invoke(IType[] args)
        {
            if (args.Length == 0)
                return new TInteger(1);
            var nums = Util.AllNumbers(args);
            Util.Contagion(nums);
            var ret = nums[0];
            for (int i = 1; i < nums.Length; ++i)
                ret = ret.Multiply(nums[i]);
            return ret;
        }
    }
}
