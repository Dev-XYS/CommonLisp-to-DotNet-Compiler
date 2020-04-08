using System;
using System.Collections.Generic;
using System.Text;

namespace Runtime
{
    public class Symbol : IDataType
    {
        private static Dictionary<string, Symbol> gsl = new Dictionary<string, Symbol>();
        private static int gid = 1;
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

        public IType Invoke(IType[] args)
        {
            throw new RuntimeException(this.ToString() + " cannot be invoked.");
        }
    }
}
