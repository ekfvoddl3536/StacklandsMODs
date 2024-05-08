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
using System.Runtime.InteropServices;

namespace SmartFactory
{
    public class ConstantCard : CardData, IComputable, IJsonSaveLoadable
    {
        [ExtraData("myConstValue")]
        public int constValue;

        #region awake
        protected override void Awake()
        {
            UpdateDisplayName();

            base.Awake();
        }
        #endregion

        #region override
        protected override bool CanHaveCard(CardData otherCard) => _CanHaveCard(otherCard);

        [SkipLocalsInit]
        public unsafe void UpdateDisplayName()
        {
            // byte to bits
            const int K_BITS = sizeof(char) * 8;

            const long K_VALUE =
                ((long)'N' << (K_BITS * 0)) |
                ((long)':' << (K_BITS * 1)) |
                ((long)' ' << (K_BITS * 2));

            const int K_LENGTH = 16;
            const int K_OFFSET = 3;

            // 
            // 스택에 할당하여서 GC 힙 쇼크를 줄인다
            // 
            char* buffer = stackalloc char[K_LENGTH];

            *(long*)buffer = K_VALUE;

            var vSpan = MemoryMarshal.CreateSpan(ref buffer[K_OFFSET], K_LENGTH - K_OFFSET);
            
            constValue.TryFormat(vSpan, out int written);
            
            nameOverride = MemoryMarshal.CreateSpan(ref *buffer, written + K_OFFSET).ToString();

            // nameOverride = "N: " + constValue.ToString();
        }
        #endregion
        
        #region interface
        ComputeType IComputable.ComputeType => ComputeType.Constant;
        int IComputable.StackPop => 0;
        int IComputable.ComputeValue(in ComputeStack<int> stack) => constValue;
        #endregion

        void IJsonSaveLoadable.OnSave() { }
        void IJsonSaveLoadable.OnLoad() => UpdateDisplayName();

#pragma warning disable IDE1006
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool _CanHaveCard(CardData otherCard) => otherCard is IComputable;
#pragma warning restore IDE1006
    }
}
