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

namespace SmartFactory
{
    partial class LogicBase
    {
        protected override void Awake()
        {
            if (_EnsureNodeSizes(Inputs, ref InputNodes))
                OnReadyInputs();

            if (_EnsureNodeSizes(Outputs, ref OutputNodes))
                OnReadyOutputs();

            Inputs ??= Array.Empty<LogicType>();
            Outputs ??= Array.Empty<LogicType>();

            InputNodes ??= Array.Empty<LogicBase>();
            OutputNodes ??= Array.Empty<LogicBase>();

            LogicNetworkManager.NextVersion();
        }

        private static bool _EnsureNodeSizes(LogicType[] types, ref LogicBase[] nodes)
        {
            if (types != null && types.Length > 0)
            {
                // reset connected info
                _ResetConnect(types);

                var vs = nodes;
                if (vs == null || vs.Length != types.Length)
                    nodes = new LogicBase[types.Length];

                // okay ready.
                return true;
            }
            
            // no
            return false;
        }

        /// <summary>
        /// 이 논리 카드에는 입력이 하나 이상 존재합니다. 필요한 초기화를 수행하세요.
        /// </summary>
        protected virtual void OnReadyInputs() { }
        /// <summary>
        /// 이 논리 카드에는 출력이 하나 이상 존재합니다. 필요한 초기화를 수행하세요.
        /// </summary>
        protected virtual void OnReadyOutputs() { }
    }
}
