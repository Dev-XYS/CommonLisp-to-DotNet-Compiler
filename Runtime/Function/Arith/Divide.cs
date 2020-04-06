using System;
using System.Collections.Generic;
using System.Text;

namespace Runtime.Function.Arith
{
    public class Divide : IType
    {
        public IType Invoke(IType[] args)
        {
            if (args.Length == 0)
                throw new RuntimeException("/: insufficient arguments");
            if (!(args is Number[] nums))
                throw new RuntimeException("/: Invalid argument type");
            Util.Contagion(nums);
            if (nums.Length == 1)
                return nums[0].Reciprocal();
            var ret = nums[0];
            for (int i = 1; i < nums.Length; ++i)
                ret = ret.Divide(nums[i]);
            return ret;
        }
    }
}
