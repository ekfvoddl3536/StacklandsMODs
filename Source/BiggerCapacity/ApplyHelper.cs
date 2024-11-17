// MIT License
//
// Copyright (c) 2023. SuperComic (ekfvoddl3535@naver.com)
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
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace BiggerCapacity;

internal static class ApplyHelper
{
    private static Func<CardData, bool> _resourceChestFilter;
    private static Func<CardData, bool> _coinChestFilter;

    public static void Initialize()
    {
        _resourceChestFilter = new Func<CardData, bool>(RESOURCE_CHEST_SELECTOR);
        _coinChestFilter = new Func<CardData, bool>(COIN_CHEST_SELECTOR);
    }

    public static void ResourceMax(int changedValue)
    {
        foreach (var c in Filtered(_resourceChestFilter))
            ((ResourceChest)c).MaxResourceCount = changedValue;
    }

    public static void CoinMax(int changedValue)
    {
        foreach (var c in Filtered(_coinChestFilter))
            ((Chest)c).MaxCoinCount = changedValue;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static IEnumerable<CardData> Filtered(Func<CardData, bool> filter) =>
        WorldManager.instance.CardDataPrefabs.Where(filter);

    private static bool COIN_CHEST_SELECTOR(CardData x) => x is Chest;
    private static bool RESOURCE_CHEST_SELECTOR(CardData x) => x is ResourceChest;
}
