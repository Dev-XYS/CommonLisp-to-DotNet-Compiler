using System;

namespace Compiler.Frontend
{
        public class SyntaxError : Exception
        {
            public SyntaxError() : base() { }
#nullable enable
            public SyntaxError(string? message) : base(message) { }
#nullable disable
        }
    }
