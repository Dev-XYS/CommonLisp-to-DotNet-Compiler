using System;
using System.Collections.Generic;
using System.Text;

namespace Runtime
{
    public class TFloat : Number
    {
        public double Value { get; set; }
        public TFloat(double x)
        {
            Value = x;
        }
        public TFloat(Number src, int _)
        {
            if (src is TFloat f)
                Value = f.Value;
            else if (src is TInteger i)
                Value = i.Value;
            else throw new NotImplementedException(string.Format("Not implemented conversion: {0} to TFloat", src));
        }
        public override string ToString()
        {
            return Value.ToString();
        }

        public override Number Add(Number rhs)
        {
            if (!(rhs is TFloat r))
                throw new RuntimeException("Invalid call: Add(TFloat)");
            return new TFloat(Value + r.Value);
        }

        public override Number Subtract(Number rhs)
        {
            if (!(rhs is TFloat r))
                throw new RuntimeException("Invalid call: Subtract(TFloat)");
            return new TFloat(Value - r.Value);
        }

        public override Number Multiply(Number rhs)
        {
            if (!(rhs is TFloat r))
                throw new RuntimeException("Invalid call: Multiply(TFloat)");
            return new TFloat(Value * r.Value);
        }

        public override Number Divide(Number rhs)
        {
            if (!(rhs is TFloat r))
                throw new RuntimeException("Invalid call: Divide(TFloat)");
            return new TFloat(Value / r.Value);
        }

        public override Number Negate()
        {
            return new TFloat(-Value);
        }

        public override bool LessThan(Number rhs)
        {
            if (!(rhs is TFloat r))
                throw new RuntimeException("Invalid call: LessThan(TFloat)");
            return Value < r.Value;
        }

        public override Number Reciprocal()
        {
            return new TFloat(1.0 / Value);
        }
        public override bool Equal(Number rhs)
        {
            if (!(rhs is TFloat r))
                throw new RuntimeException("Invalid call: Equal(TFloat)");
            return Value == r.Value;
        }
    }
}
