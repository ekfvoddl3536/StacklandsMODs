// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

using HarmonyLib;

namespace KivotosLand.Patches;

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
