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

using System.Collections.Generic;
using HarmonyLib;

namespace KivotosLand.Patches
{
    [HarmonyPatch(typeof(EndOfMonthCutscenes), nameof(EndOfMonthCutscenes.AnyVillagerWillChangeLifeStage))]
    public static class EndOfMonthCutscenes_AnyVillagerWillChangeLifeStage_PATCH01
    {
        public static bool Prefix(ref bool __result, List<BaseVillager> villagers)
        {
            var count = villagers.Count;
            for (int i = 0; i < count; ++i)
            {
                var item = villagers[i];
                if (item is Student)
                    continue;

                if (DetermineLifeStageFromAge(item.Age) != DetermineLifeStageFromAge(item.Age + 1))
                {
                    __result = true;
                    return false;
                }
            }

            __result = false;
            return false;
        }

        private static LifeStage DetermineLifeStageFromAge(int age)
        {
            if (age < 2)
                return LifeStage.Teenager;
            
            if (age <= 6)
                return LifeStage.Adult;

            return age <= 8 ? LifeStage.Elderly : LifeStage.Dead;
        }
    }
}
