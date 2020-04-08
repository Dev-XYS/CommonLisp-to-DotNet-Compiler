using System;
using System.Collections.Generic;
using System.Text;

namespace Runtime.Function.Arith
{
    public class Less : IType
    {
        public IType Invoke(IType[] args)
        {
            if (args.Length < 1)
                throw new RuntimeException("<: At least 1 argument required, {0} given", args.Length);
            var nums = Util.AllNumbers(args);
            Util.Contagion(nums);
            for (int i = 1; i < args.Length; ++i)
                if (!nums[i - 1].LessThan(nums[i]))
                    return Lisp.nil;
            return Lisp.t;
        }
    }
    public class Greater : IType
    {
        public IType Invoke(IType[] args)
        {
            if (args.Length < 1)
                throw new RuntimeException(">: At least 1 argument required, {0} given", args.Length);
            var nums = Util.AllNumbers(args);
            Util.Contagion(nums);
            Array.Reverse(nums);
            for (int i = 1; i < args.Length; ++i)
                if (!nums[i - 1].LessThan(nums[i]))
                    return Lisp.nil;
            return Lisp.t;
        }
    }
    public class GreaterEqual : IType
    {
        public IType Invoke(IType[] args)
        {
            if (args.Length < 1)
                throw new RuntimeException(">=: At least 1 argument required, {0} given", args.Length);
            var nums = Util.AllNumbers(args);
            Util.Contagion(nums);
            for (int i = 1; i < args.Length; ++i)
                if (nums[i - 1].LessThan(nums[i]))
                    return Lisp.nil;
            return Lisp.t;
        }
    }
    public class LessEqual : IType
    {
        public IType Invoke(IType[] args)
        {
            if (args.Length < 1)
                throw new RuntimeException("<=: At least 1 argument required, {0} given", args.Length);
            var nums = Util.AllNumbers(args);
            Util.Contagion(nums);
            Array.Reverse(nums);
            for (int i = 1; i < args.Length; ++i)
                if (nums[i - 1].LessThan(nums[i]))
                    return Lisp.nil;
            return Lisp.t;
        }
    }
    public class NumberEqual: IType
    {
        public IType Invoke(IType[] args)
        {
            if (args.Length < 1)
                throw new RuntimeException("=: At least 1 argument required, {0} given", args.Length);
            var nums = Util.AllNumbers(args);
            Util.Contagion(nums);
            for (int i = 1; i < args.Length; ++i)
                if (!nums[i - 1].Equal(nums[i]))
                    return Lisp.nil;
            return Lisp.t;
        }
    }
    public class NumberNotEqual : IType
    {
        public IType Invoke(IType[] args)
        {
            if (args.Length < 1)
                throw new RuntimeException("/=: At least 1 argument required, {0} given", args.Length);
            var nums = Util.AllNumbers(args);
            Util.Contagion(nums);
            for (int i = 1; i < args.Length; ++i)
                if (nums[i - 1].Equal(nums[i]))
                    return Lisp.nil;
            return Lisp.t;
        }
    }

}
