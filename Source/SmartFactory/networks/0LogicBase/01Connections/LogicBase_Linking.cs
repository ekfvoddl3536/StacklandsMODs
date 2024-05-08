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
using SuperComicLib.Runtime;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SmartFactory
{
    partial class LogicBase
    {
        public virtual void ConnectFrom(LogicBase inputCard)
        {
            OutputConnect(this, inputCard);

            InputConnect(inputCard, this);
        }

        public virtual void DisconnectFrom(LogicBase inputCard)
        {
            OutputDisconnect(this, inputCard);

            InputDisconnect(inputCard, this);
        }

        /// <summary>
        /// check connections is valid
        /// </summary>
        public override void StoppedDragging()
        {
            if (!(this is IComputable) && MyGameCard.Parent != (object)null)
                DisconnectAll();
        }

        protected void ConnectionUpdate(LogicBase[] nodes, LogicType[] types)
        {
            var myPos = MyGameCard.transform.position;

            ref var ntFirst = ref types.refdata();
            for (int i = 0; i < nodes.Length; ++i)
            {
                var item = nodes[i];
                if (item == (object)null ||
                    _GetDistance(item, myPos) <= ModOptions.maxConnLen)
                    continue;

                Unsafe.Add(ref ntFirst, i) &= ~LogicType.Connected;
                nodes[i] = default;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static float _GetDistance(CardData dst, Vector3 src)
        {
            var tmp = dst.MyGameCard.transform.position;

            float dx = tmp.x - src.x;
            float dy = tmp.z - src.z;

            return dx * dx + dy * dy;
        }
    }
}
