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

using HarmonyLib;

namespace KivotosLand.Patches
{
    [HarmonyPatch(typeof(DebugScreen), nameof(DebugScreen.SpawnFullCoinChest))]
    public static class DebugScreen_SpawnFullCoinChest_PATCH01
    {
        public static bool Prepare() => ModOptions.onDbg;

        public static bool Prefix()
        {
            var wminst = WorldManager.instance;
            var id =
                wminst.CurrentBoard.BoardOptions.UsesShells
                ? Cards.shell_chest
                : Cards.coin_chest;

            var chest = (Chest)wminst.CreateCard(wminst.MiddleOfBoard(), id, true, false);
            chest.CoinCount = chest.MaxCoinCount;

            return false;
        }
    }
}
