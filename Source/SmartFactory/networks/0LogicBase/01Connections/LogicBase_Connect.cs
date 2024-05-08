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

namespace SmartFactory
{
    partial class LogicBase
    {
        /// <summary>
        /// <paramref name="this"/>에서 출력 노드에 <paramref name="outputCard"/> 개체를 연결한다.
        /// </summary>
        protected static void OutputConnect(LogicBase outputCard, LogicBase @this) =>
            _Connect(outputCard, @this.OutputNodes, @this.Outputs);

        /// <summary>
        /// <paramref name="this"/>에서 입력 노드에 <paramref name="inputCard"/> 개체를 연결한다.
        /// </summary>
        protected static void InputConnect(LogicBase inputCard, LogicBase @this) =>
            _Connect(inputCard, @this.InputNodes, @this.Inputs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static void _Connect(LogicBase otherCard, LogicBase[] nodes, LogicType[] types)
        {
            for (int i = 0; i < nodes.Length; ++i)
                if (nodes[i] == (object)null)
                {
                    nodes[i] = otherCard;

                    types.refdata(i) |= LogicType.Connected;

                    break;
                }
        }
    }
}
