// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

using HarmonyLib;

namespace KivotosLand.Patches;

[HarmonyPatch(typeof(BaseVillager), nameof(BaseVillager.DetermineLifeStageFromAge))]
public static class BaseVillager_DetermineLifeStageFromAge_PATCH01
{
    public static bool Prefix(ref LifeStage __result, BaseVillager __instance)
    {
        if (__instance is Student)
        {
            __instance.Age = 0;
            __result = LifeStage.Teenager;
            return false;
        }
        else
            return true;
    }
}
