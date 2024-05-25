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

using System;
using System.Runtime.CompilerServices;

namespace SmartFactory
{
    public class LogicSwitch : LogicPredCardByRefBase
    {
        [ExtraData("switchValue")]
        public bool m_ison;
        
        protected override void Awake()
        {
            if (Inputs == null || Inputs.Length != 1)
                Inputs = new LogicType[1] { LogicType.Any };
        
            Outputs = Array.Empty<LogicType>();
        
            base.Awake();
        }

        protected override void OnUpdateCard()
        {
            if (_target != (object)null)
                NetworkArrows.DrawArrow(this, _target);

            base.OnUpdateCard();
        }
        
        protected override int GetNextOutputValue() => 0;
        protected override void SetInputValue(int newInputValue)
        {
            var v = newInputValue != 0;
        
            if (m_ison != v)
            {
                m_ison = v;
        
                if (_target != (object)null)
                    SetOperation(_target, v);
            }
        }
        
        protected override void OnUnlinkedTarget(CardData other)
        {
            m_ison = true;
            SetOperation(other, true);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SetOperation(CardData c, bool ison)
        {
            for (var g = c.MyGameCard; g != (object)null; g = g.Child)
            {
                g.CardData.enabled = ison;
                g.enabled = ison;
            }
        }
    }
}
