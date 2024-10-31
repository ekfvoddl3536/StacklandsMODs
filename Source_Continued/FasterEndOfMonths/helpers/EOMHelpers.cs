// MIT License
//
// Copyright (c) 2022 Benedikt Werner
// Copyright (c) 2024 SuperComic (ekfvoddl3535@naver.com)
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

// -- alias --
using System;
using System.Runtime.CompilerServices;

namespace FasterEndOfMonths;

internal static class EOMHelpers // EndOfMonth
{
    public static ulong _fastModMultiplier;

    internal static void Autosave()
    {
        if (IntPtr.Size == sizeof(ulong))
        {
            // we can use fast mod
            var mon = WorldManager.instance.CurrentMonth;
            if (FastMod((uint)mon, (uint)ModConfig.autosaveFrequency, _fastModMultiplier) == 0)
                SaveManager.instance.Save(true);
        }
        else
        {
            if (WorldManager.instance.CurrentMonth % ModConfig.autosaveFrequency == 0)
                SaveManager.instance.Save(true);
        }
    }

    public static void OnAutosaveFrequencyChanged(int changedValue)
    {
        // only 64-bit system
        if (IntPtr.Size == sizeof(ulong))
            _fastModMultiplier = ulong.MaxValue / (uint)changedValue + 1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint FastMod(uint value, uint divisor, ulong multiplier)
    {
        uint hb = (uint)(((((multiplier * value) >> 32) + 1) * divisor) >> 32);
        return hb;
    }
}
