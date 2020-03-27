using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.IL
{
    class IL
    {
        /* 
         * OPERATION: $RIP unchanged
         * CONTROL: may change $RIP
         */
        public enum Type { OPERATION, CONTROL };
        /*
         * ADD: dst = sr1 + sr2
         * SUB: dst = sr1 - sr2
         * MUL: dst = sr1 * sr2
         * DIV: dst = sr1 / sr2
         * MOD: dst = sr1 % sr2
         * SHL: dst = sr1 << sr2
         * SHR: dst = sr1 (unsigned>>) sr2
         * SAR: dst = sr1 (signed>>) sr2
         * AND: dst = sr1 bitand sr2
         * OR:  dst = sr1 bitor sr2
         * XOR: dst = sr1 bitxor sr2
         * MOV: dst = sr1
         * TAG: add tag dst here
         */
        public enum Operator { ADD, SUB, MUL, DIV, MOD, SHL, SAR, SHR, AND, OR, XOR, MOV, TAG };
        /*
         * JMP: goto dst
         * JLE: if sr1 <= sr2 goto dst
         * JGE: if sr1 >= sr2 goto dst
         * JL:  if sr1 < sr2 goto dst
         * JG:  if sr1 > sr2 goto dst
         * JE:  if sr1 == sr2 goto dst
         * JNE: if sr1 != sr2 goto dst
         */
        public enum Control { JMP, JLE, JGE, JL, JG, JE, JNE }
        public Type type;
        public int op;
        public ulong? sr1, sr2, dst;
        public IL(Operator i_op, ulong? i_sr1, ulong? i_sr2, ulong? i_dst)
        {
            type = Type.OPERATION;
            op = (int)i_op;
            sr1 = i_sr1;
            sr2 = i_sr2;
            dst = i_dst;
        }
        public IL(Control i_ctrl, ulong? i_sr1, ulong? i_sr2, ulong? i_dst)
        {
            type = Type.CONTROL;
            op = (int)i_ctrl;
            sr1 = i_sr1;
            sr2 = i_sr2;
            dst = i_dst;
        }
    }
}
