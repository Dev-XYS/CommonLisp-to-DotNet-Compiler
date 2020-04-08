using System;
using System.Collections.Generic;
using System.Text;

namespace Runtime.Function.Arith
{
    public class Plus : IType
    {
        public IType Invoke(IType[] args)
        {
            if (args.Length == 0)
                return new TInteger(0);
            var nums = Util.AllNumbers(args);
            Util.Contagion(nums);
            var ret = nums[0];
            for (int i = 1; i < nums.Length; ++i)
                ret = ret.Add(nums[i]);
            return ret;
        }
    }
}
