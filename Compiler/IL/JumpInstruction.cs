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
    }

    class ConditionalJumpInstruction : JumpInstruction
    {
        public bool Condition { get; }

        public ConditionalJumpInstruction(Label target, bool condition) : base(target)
        {
            Condition = condition;
        }
    }
}
