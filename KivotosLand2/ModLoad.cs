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
using System.Reflection;
using SuperComicLib.Stacklands;

namespace KivotosLand
{
    public sealed class ModLoad : Mod
    {
        private const int MAX_PHASE = 4;
        private const int MAX_COIN_COUNT = 1200;

        public override void Ready()
        {
            var logger = Logger;
            logger.Log(nameof(KivotosLand) + " MOD Loading... by 'SuperComic (ekfvoddl3535@naver.com)'");

            try
            {
                LogPhase(logger,1);

                this.LoadFallbackTerms();

                ModOptions.Load(Config);
                GlobalValues.Load();

                LogPhase(logger,2);

                var t = typeof(Combatable);

                const BindingFlags FLAGS = BindingFlags.NonPublic | BindingFlags.Instance;
                Student._AttackSpecialHit = t.GetField("AttackSpecialHit", FLAGS);
                Student._PerformSpecialHit = (PerformSpecialHitDelegate)t.GetMethod("PerformSpecialHit", FLAGS).CreateDelegate(typeof(PerformSpecialHitDelegate));
                Student._ShowHitText = (ShowHitTextDelegate)t.GetMethod("ShowHitText", FLAGS).CreateDelegate(typeof(ShowHitTextDelegate));

                LogPhase(logger,3);

                var temp_selector = new Func<CardData, bool>(CHEST_SELECTOR);
                var chestPrefabs = WorldManager.instance.CardDataPrefabs.AsParallel().AsUnordered().Where(temp_selector);

                foreach (var prefab in chestPrefabs)
                {
                    ref int maxCoins = ref ((Chest)prefab).MaxCoinCount;

                    if (maxCoins < MAX_COIN_COUNT)
                        maxCoins = MAX_COIN_COUNT;
                }

                LogPhase(logger, 4);

                this.PatchAllWithDependencies(Harmony, false);
                Harmony.PatchAll();

                logger.Log(nameof(KivotosLand) + " MOD Loaded!");
            }
            catch (Exception e)
            {
                logger.Log(nameof(KivotosLand) + " MOD Load FAIL! -> " + e.ToString());
            }
        }

        private static void LogPhase(ModLogger logger, int phase) =>
            logger.Log(nameof(KivotosLand) + $" MOD Initialize... Phase {phase} / {MAX_PHASE}");

        private static bool CHEST_SELECTOR(CardData x) => x is Chest;
    }
}
