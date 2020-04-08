﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Runtime.Function.Arith
{
    public class Minus : IType
    {
        public IType Invoke(IType[] args)
        {
            if (args.Length == 0)
                throw new RuntimeException("-: insufficient arguments");
            var nums = Util.AllNumbers(args);
            Util.Contagion(nums);
            if (nums.Length == 1)
                return nums[0].Negate();
            var ret = nums[0];
            for (int i = 1; i < nums.Length; ++i)
                ret = ret.Subtract(nums[i]);
            return ret;
        }
    }
}
