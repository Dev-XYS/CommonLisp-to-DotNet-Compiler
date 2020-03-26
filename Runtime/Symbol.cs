using System;
using System.Collections.Generic;
using System.Text;

namespace Runtime
{
    class Symbol : IType
    {
        string Value;
        public Symbol(string iv)
        {
            Value = iv;
        }
        public static Symbol Find(string symbol)
        {
            //todo:find from symbol list
            return new Symbol(symbol); //todo: fixme
        }
        public static Symbol FindOrCreate(string symbol)
        {
            return new Symbol(symbol);//todo : fixme
        }
        public override string ToString()
        {
            return "Symbol(" + Value + ")";
        }
    }
}
