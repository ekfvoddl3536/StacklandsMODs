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

#pragma warning disable IDE1006
using System;
using System.Runtime.CompilerServices;

namespace SmartFactory
{
    public class LogicCounter : LogicPredCardByRefBase
    {
        protected override void Awake()
        {
            if (Outputs == null || Outputs.Length != 1)
                Outputs = new LogicType[1] { LogicType.Any };

            Inputs = Array.Empty<LogicType>();

            base.Awake();
        }


        protected override void OnUpdateCard()
        {
            if (_target != (object)null)
                NetworkArrows.DrawArrow(_target, this);

            base.OnUpdateCard();
        }

        protected override void SetInputValue(int newInputValue) { }
        protected override int GetNextOutputValue() => 
            _target != (object)null
            ? OnCount(_target)
            : 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static int OnCount(CardData card)
        {
            var root = card.MyGameCard;

            int sum = 0;
            for (; root; root = root.Child)
                sum += 1 + _ItemCount(root.CardData);

            return sum;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static int _ItemCount(CardData c) =>
            c is ResourceChest rc
            ? rc.ResourceCount
            : c is Chest cs
            ? cs.CoinCount
            : 0;
    }
}
