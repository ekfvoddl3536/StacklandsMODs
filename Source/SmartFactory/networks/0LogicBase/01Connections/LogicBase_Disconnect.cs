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
using System;
using System.Runtime.CompilerServices;

namespace SmartFactory
{
    partial class LogicBase
    {
        protected void DisconnectAll()
        {
            _ResetConnect(Inputs);
            _ResetConnect(Outputs);

            InputDisconnectAll();
            OutputDisconnectAll();

            OnDisconnectedAll();
        }

        protected virtual void OnDisconnectedAll() { }

        protected void InputDisconnectAll()
        {
            var nodes = InputNodes;
            for (int i = 0; i < nodes.Length; ++i)
                if (nodes[i] != (object)null)
                {
                    OutputDisconnect(this, nodes[i]);
                    nodes[i] = null;
                }
        }

        protected void OutputDisconnectAll()
        {
            var nodes = OutputNodes;
            for (int i = 0; i < nodes.Length; ++i)
                if (nodes[i] != (object)null)
                {
                    InputDisconnect(this, nodes[i]);
                    nodes[i] = null;
                }
        }

        /// <summary>
        /// <paramref name="this"/>에서 입력 노드에 연결된 <paramref name="outputCard"/> 개체를 끊는다.
        /// </summary>
        protected static void OutputDisconnect(LogicBase outputCard, LogicBase @this) =>
            _Disconnect(outputCard, @this.OutputNodes, @this.Outputs);

        /// <summary>
        /// <paramref name="this"/>에서 입력 노드에 연결된 <paramref name="inputCard"/> 개체를 끊는다.
        /// </summary>
        protected static void InputDisconnect(LogicBase inputCard, LogicBase @this) =>
            _Disconnect(inputCard, @this.InputNodes, @this.Inputs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static void _Disconnect(LogicBase otherCard, LogicBase[] nodes, LogicType[] types)
        {
            for (int i = 0; i < nodes.Length; ++i)
                if (nodes[i] == (object)otherCard)
                {
                    nodes[i] = null!;

                    // @DISABLE_NO_CHECK
                    // types.refdata(i) &= ~LogicType.Connected;
                    types[i] &= ~LogicType.Connected;

                    break;
                }
        }
    }
}
