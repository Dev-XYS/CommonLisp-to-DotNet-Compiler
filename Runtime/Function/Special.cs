using System;
using System.Collections.Generic;
using System.Text;

namespace Runtime.Function
{
    public class SpecialGet : IType
    {
        public IType Invoke(IType[] args)
        {
            if (args.Length != 1) throw new RuntimeException("#SPECIAL-GET: Exactly 1 argument required.");
            if (!(args[0] is Symbol s))
                throw new RuntimeException("#SPECIAL-GET: Invalid argument type");
            var ret = Dynamic.Get(s);
            if (ret is null)
                throw new RuntimeException("#SPECIAL-GET: variable {0} not exist or not special", s.Name);
            return ret.Peek();
        }
    }
    public class SpecialSet : IType
    {
        public IType Invoke(IType[] args)
        {
            if (args.Length != 2) throw new RuntimeException("#SPECIAL-SET: Exactly 2 argument required.");
            if (!(args[0] is Symbol s))
                throw new RuntimeException("#SPECIAL-SET: Invalid argument type");
            var ret = Dynamic.Get(s);
            if (ret is null)
                throw new RuntimeException("#SPECIAL-SET: variable {0} not exist or not special", s.Name);
            ret.Pop();
            ret.Push(args[1]);
            return Lisp.nil;
        }
    }
    public class SpecialPush : IType
    {
        public IType Invoke(IType[] args)
        {
            if (args.Length != 2) throw new RuntimeException("#SPECIAL-PUSH: Exactly 2 argument required.");
            if (!(args[0] is Symbol s))
                throw new RuntimeException("#SPECIAL-PUSH: Invalid argument type");
            var ret = Dynamic.Get(s);
            if (ret is null)
                ret = Dynamic.Set(s);
            ret.Push(args[1]);
            return Lisp.nil;
        }
    }
    public class SpecialPop : IType
    {
        public IType Invoke(IType[] args)
        {
            if (args.Length != 1) throw new RuntimeException("#SPECIAL-POP: Exactly 1 argument required.");
            if (!(args[0] is Symbol s))
                throw new RuntimeException("#SPECIAL-POP: Invalid argument type");
            var ret = Dynamic.Get(s);
            if (ret is null)
                throw new RuntimeException("#SPECIAL-POP: variable {0} not exist or not special", s.Name);
            ret.Pop();
            return Lisp.nil;
        }
    }
    public class SpecialReset : IType
    {
        public IType Invoke(IType[] args)
        {
            if (args.Length != 1) throw new RuntimeException("#SPECIAL-RESET: Exactly 1 argument required.");
            if (!(args[0] is Symbol s))
                throw new RuntimeException("#SPECIAL-RESET: Invalid argument type");
            var ret = Dynamic.Get(s);
            if (ret is null)
                Dynamic.Set(s);
            else ret.Clear();
            return Lisp.nil;
        }
    }
}
