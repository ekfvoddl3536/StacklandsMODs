﻿// MIT License
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

using SuperComicLib.Runtime;
using System.Runtime.CompilerServices;

namespace SmartFactory
{
    partial class LogicBase
    {
        public sealed override void UpdateCardText() { }

        public sealed override void UpdateCard()
        {
            if (MyGameCard.IsDemoCard || !MyGameCard.MyBoard.IsCurrent)
                return;

            base.UpdateCard();

            OnUpdateCard();
        }

        protected virtual void OnUpdateCard()
        {
            NetworkArrows.ReDraw(this);

            CheckNodeLength();
        }

        protected virtual void CheckNodeLength()
        {
            if (IsDraggingMe())
            {
                ConnectionUpdate(InputNodes, Inputs);

                ConnectionUpdate(OutputNodes, Outputs);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected bool IsDraggingMe()
        {
            var dragCard = WorldManager.instance.DraggingDraggable;
            return
                dragCard != (object)null && 
                dragCard == (object)MyGameCard;
        }

        public void NetworkUpdate()
        {
            // 초기 값
            int nextOutputValue = GetNextOutputValue();

            var outputs = Outputs;
            // @DISABLE_NO_CHECK
            // // next types
            // ref var ntFirst = ref outputs.refdata();
            // // next nodes
            // ref var nnFirst = ref OutputNodes.refdata();

            // 값 전파
            for (int i = 0; i < outputs.Length; ++i)
                if (outputs[i] < 0) // isConnected?
                {
                    OutputNodes[i].SetInputValue(nextOutputValue);
                }

                // @DISABLE_NO_CHECK
                // if (Unsafe.Add(ref ntFirst, i) < 0) // isConnected?
                // {
                //     ref var item = ref Unsafe.Add(ref nnFirst, i);
                // 
                //     item.SetInputValue(nextOutputValue);
                // }
        }
    }
}
