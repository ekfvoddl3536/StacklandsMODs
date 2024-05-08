// MIT License
//
// Copyright (c) 2024. SuperComic (ekfvoddl3535@naver.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SmartFactory
{
    public class ComputeOperator : CardData, IComputable, IJsonSaveLoadable
    {
        public OperatorType opType;

        [ExtraData("operatorType")]
        public int m_saveData_opType;

        protected override void Awake()
        {
            // random select
            opType = (OperatorType)Random.Range(0, (int)OperatorType.Abs + 1);

            UpdateDisplayName();

            base.Awake();
        }

        protected void UpdateDisplayName()
        {
            nameOverride =
                (uint)opType <= (uint)OperatorType.Abs
                ? OPNameTable.Names[(int)opType]
                : GetNameFallback(opType);
        }

        public override void Clicked()
        {
            var keyboard = Keyboard.current;
            if (keyboard == null)
                return;

            if (keyboard.ctrlKey.isPressed)
            {
                if (keyboard.shiftKey.isPressed)
                    opType = GetNextOpType(opType, 5);
                else
                    opType = GetNextOpType(opType);

                UpdateDisplayName();
            }
        }

        protected virtual OperatorType GetNextOpType(OperatorType old, int shift = 1) =>
            old + shift <= OperatorType.Abs
            ? (old + shift)
            : OperatorType.Add;

        protected override bool CanHaveCard(CardData otherCard) => ConstantCard._CanHaveCard(otherCard);
        protected virtual string GetNameFallback(OperatorType t) => "NOP";

        #region interface (+method)
        ComputeType IComputable.ComputeType => ComputeType.Constant;
        int IComputable.StackPop => GetStackPopCount(opType);
        int IComputable.ComputeValue(in ComputeStack<int> stack) => ComputeValue(in stack, opType);

        protected virtual int GetStackPopCount(OperatorType t) =>
            (uint)t < (uint)OperatorType.Not
            ? 2
            : 1;

        protected virtual int ComputeValue(in ComputeStack<int> stack, OperatorType t)
        {
            int y =
                (uint)t < (uint)OperatorType.Not
                ? stack.Pop()
                : 0;
            int x = stack.Pop();

            return t switch
            {
                // def
                OperatorType.Add => x + y,
                OperatorType.Sub => x - y,
                OperatorType.Mul => x * y,
                OperatorType.Div => x / y,
                OperatorType.Mod => x % y,

                // bit
                OperatorType.And => x & y,
                OperatorType.Or => x | y,
                OperatorType.Xor => x ^ y,

                // compare
                OperatorType.CmpEQ => ToInt(x == y),
                OperatorType.CmpNE => ToInt(x != y),
                OperatorType.CmpLE => ToInt(x <= y),
                OperatorType.CmpGE => ToInt(x >= y),
                OperatorType.CmpLT => ToInt(x < y),
                OperatorType.CmpGT => ToInt(x > y),

                // unary
                OperatorType.Not => ~x,
                OperatorType.Neg => -x,
                OperatorType.Abs => Mathf.Abs(x),

                // NOP
                _ => 0
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static int ToInt(bool v) => v ? 1 : 0;

        void IJsonSaveLoadable.OnSave()
        {
            m_saveData_opType = (int)opType;
        }

        void IJsonSaveLoadable.OnLoad()
        {
            var v = (OperatorType)m_saveData_opType;
            if (v != opType)
            {
                opType = v;
                UpdateDisplayName();
            }
        }
        #endregion
    }
}
