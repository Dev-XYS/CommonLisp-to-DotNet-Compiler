using System;
using System.Collections.Generic;
using System.Text;

namespace Runtime
{
    public class Symbol : IType
    {
        private static Dictionary<string, Symbol> gsl;
        private static int gid;
        public string Name { get; set; }
        private int sid;
        private Symbol(string iv, int id)
        {
            Name = iv;
            sid = id;
        }
        public static Symbol Find(string symbol)
        {
            Symbol ret;
            if (gsl.TryGetValue(symbol, out ret))
            {
                return ret;
            }
            else throw new Exception(string.Format("Symbol not found: {0}", symbol));
        }

        public static Symbol FindOrCreate(string symbol)
        {
            Symbol ret;
            if(!gsl.TryGetValue(symbol, out ret))
            {
                ret = new Symbol(symbol, ++gid);
                gsl.Add(symbol, ret);
            }
            return ret;
        }
        public override string ToString()
        {
            return "Symbol(" + Name + ")";
        }
        public override int GetHashCode()
        {
            return sid;
        }
        public static void Init()
        {
            gsl = new Dictionary<string, Symbol>();
            gid = 1;
        }

        public IType Invoke(IType[] args)
        {
            throw new RuntimeException(this.ToString() + " cannot be invoked.");
        }
    }
}
