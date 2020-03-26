using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Runtime
{
    static class Reader
    {
        public class ReaderError: Exception
        {
#nullable enable
            public ReaderError(string? message) : base(message) { }
            public ReaderError() : base() { }
        }
        public class EOFError : ReaderError
        {
            public EOFError(string? message) : base(message) { }
            public EOFError() : base() { }
        }
        private class EndOfListError : ReaderError
        {
           // public EndOfListError(string? message) : base(message) { }
#nullable disable
        }
        private static HashSet<char> MACROTERM = new HashSet<char>(";'`\",()");
        private static HashSet<char> MACRONONTERM = new HashSet<char>("#");

        private static Regex INTREGEX = new Regex(@"^[+-]?\d+\.?$", RegexOptions.Compiled);
        private static Regex RATIOREGEX = new Regex(@"^(?<numerator>[+-]?\d+)/(?<denominator>\d+)$", RegexOptions.Compiled);
        private static Regex FLOATREGEX = new Regex(@"^(?<base>[+-]?\d*\.\d+)((?<type>[DEFLS])(?<exponent>[+-]?\d+))?|(?<base>[+-]?\d+\.?\d*)(?<type>[DEFLS])(?<exponent>[+-]?\d+)$", RegexOptions.Compiled | RegexOptions.ExplicitCapture);
        private static char ReadNextChar(IInputStream input)
        {
            int next = input.ReadChar();
            if (next == -1) throw new EOFError("Error: Early EOF");
            return (char)next;
        }
        private static char WithCase(char ch)
        {
            return char.ToUpper(ch); // feat: support case modify
        }
        private static void ReadComment(IInputStream input)
        {
            while(true)
            {
                int next = input.ReadChar();
                if (next == -1 || (char)next == '\n') return;
            }
        }
        private static Cons ReadWith(Symbol sym, IInputStream input)
        {
            Cons ret = new Cons();
            ret.car = sym;
            ret.cdr = new Cons(Read(input), Lisp.nil);
            return ret;
        }
        private static TString ReadString(IInputStream input)
        {
            return new TString();
        }
        private static IType ReadList(IInputStream input) // todo: support ( car . cdr ) notation
        {
            Cons ret = new Cons();
            Cons cur = ret;
            try
            {
                IType next = Read(input);
                cur.car = next;
                cur.cdr = Lisp.nil;
            }
            catch(EndOfListError)
            {
                return Lisp.nil;
            }
            while(true)
            {
                try
                {
                    IType next = Read(input);
                    var cdr = new Cons(next, Lisp.nil);
                    cur.cdr = cdr;
                    cur = cdr;
                }
                catch(EndOfListError)
                {
                    return ret;
                }
            }
        }
        /*
         * @return true if the token could be a number
         */
        private static bool ReadToken(IInputStream input, List<char> token)
        {
            bool couldBeNumber = true, escape = false;
            char cur = '~';
            while (true)
            {
                try
                {
                    cur = ReadNextChar(input);
                } catch (EOFError e)
                {
                    if (escape) throw e;
                    return couldBeNumber;
                }
                if (cur == '\\')
                {
                    cur = ReadNextChar(input);
                    if (char.IsWhiteSpace(cur)) throw new ReaderError("Error: Invalid Char");
                    token.Add(cur);
                    couldBeNumber = false;
                }
                else if (cur == '|')
                {
                    escape = !escape;
                    couldBeNumber = false;
                }
                else if (!escape && MACROTERM.Contains(cur))
                {
                    input.UnReadChar();
                    return couldBeNumber;
                }
                else if(!escape && char.IsWhiteSpace(cur))
                {
                    return couldBeNumber;
                }else
                {
                    token.Add(escape ? cur : WithCase(cur));
                }
            }
        }
        private static IType ToNumOrSymbol(char[] input, bool couldBeNumber)
        {
            string output = string.Join("", input);
            if (!couldBeNumber) return Symbol.FindOrCreate(output);
            var m = INTREGEX.Match(output);
            if (m.Success) return new TInteger(Convert.ToInt32(output));
            m = FLOATREGEX.Match(output);
            if (m.Success)
            {
                if (m.Groups["type"].Success)
                    return new TFloat(Convert.ToDouble(m.Groups["base"].Value + "E" + m.Groups["exponent"].Value)); //todo: support other precision
                return new TFloat(Convert.ToDouble(output));
            }
            return Symbol.FindOrCreate(output); //todo: support rational
        }
#nullable enable
        private static IType? ReadMacro(IInputStream input, char macrochar)
        {
            switch(macrochar)
            {
                case ';':
                    ReadComment(input);
                    return null;
                case '\'':
                    return ReadWith(Symbol.Find("QUOTE"), input);
                case '`':
                    return ReadWith(Symbol.Find("QUASIQUOTE"), input);
                case '\"':
                    return ReadString(input);
                case ',':
                    return ReadWith(Symbol.Find("UNQUOTE"), input); //todo: support ,@
                case '(':
                    return ReadList(input);
                case ')':
                    throw new EndOfListError();
                case '#':
                    return Lisp.nil;//ReadSharp(input); todo
                default:
                    throw new ReaderError(string.Format("Unrecognized Macro Character {0}", macrochar)); //feature: add user-implemented reader macros
            }
        }
#nullable disable
        public static IType Read(IInputStream input)
        {
            List<char> token = new List<char>();
            while (true)
            {
                char cur = ReadNextChar(input);
                if (char.IsWhiteSpace(cur)) continue;
                if (MACROTERM.Contains(cur) || MACRONONTERM.Contains(cur))
                {
                    var ret = ReadMacro(input, cur);
                    if (ret is null)
                        continue;
                    else return ret;
                }
                bool couldBeNumber = true;
                input.UnReadChar();
                ReadToken(input, token);
                return ToNumOrSymbol(token.ToArray(), couldBeNumber);
            }
        }
    }
}
