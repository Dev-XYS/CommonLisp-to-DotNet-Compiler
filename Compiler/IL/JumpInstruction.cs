using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.IL
{
    abstract class JumpInstruction : IInstruction
    {
        public Label Target { get; }

        public JumpInstruction(Label target)
        {
            Target = target;
        }
    }

    class UnconditionalJumpInstruction : JumpInstruction
    {
        public UnconditionalJumpInstruction(Label target) : base(target)
        {
        }

        public override string ToString()
        {
            return string.Format("[JUMP] {0}", Target.GetHashCode());
        }
    }

    class ConditionalJumpInstruction : JumpInstruction
    {
        public bool Condition { get; }

        public Variable TestVariable { get; }

        public ConditionalJumpInstruction(Label target, Variable test, bool condition) : base(target)
        {
            TestVariable = test;
            Condition = condition;
        }

        public override string ToString()
        {
            return string.Format("[JUMP] {0} if {1} {2}", Target.Name, TestVariable.ToString(), Condition);
        }
    }
}
