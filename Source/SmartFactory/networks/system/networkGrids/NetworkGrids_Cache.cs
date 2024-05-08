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

using SuperComicLib;
using System.Linq;
using System;
using SuperComicLib.Runtime;
using UnityEngine;

namespace SmartFactory
{
    static partial class NetworkGrids
    {
        public static void UpdateCache()
        {
            var list = WorldManager.instance.AllCards;

            cachedCards =
                list.Count < 32
                ?
                    list
                    .Where(x => x.CardData is LogicBase lb && lb.Inputs.Length == 0 && lb.MyGameCard.Parent == false)
                    .Select(x => (LogicBase)x.CardData)
                    .ToArray()
                :
                    // 순서 신경 안씀
                    list
                    .AsParallel()
                    .WithDegreeOfParallelism(Environment.ProcessorCount)
                    .AsUnordered()
                    .Where(x => x.CardData is LogicBase lb && lb.Inputs.Length == 0 && lb.MyGameCard.Parent == false)
                    .Select(x => (LogicBase)x.CardData)
                    .ToArray();

            if (L1Nodes._items.Length < cachedCards.Length)
            {
                int next = L1Nodes._items.Length << 1;

                int pow2 = cachedCards.Length;
                if ((pow2 & (pow2 - 1)) != 0)
                    pow2 = (int)BitMath.SetUnderbits((uint)pow2) + 1;

                // 더 큰 크기로 할당하려고 시도하고, 실패하면
                // 조금 더 작은 크기로 다시 시도
                // 그것조차 실패하면 오류를 내보낸다
                int size = Mathf.Max(pow2, next);
                for (int retry = 0; ; ++retry)
                    try
                    {
                        // fast allocation
                        XUnsafe.AsRef(in L1Nodes._items) = new LogicBase[size];
                        XUnsafe.AsRef(in L2Nodes._items) = new LogicBase[size];

                        currentHash.ForceSetCapacity(size);
                        break;
                    }
                    catch (OutOfMemoryException OutOfMem)
                    {
                        // is insuffiction memory?
                        if (retry > 0)
                            throw OutOfMem;

                        size = Mathf.Min(pow2, next);
                    }
            }
        }
    }
}
