// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

using HarmonyLib;

namespace KivotosLand.Patches;

[HarmonyPatch(typeof(BaseVillager), "get_MyLifeStage")]
public static class BaseVillager_MyLifeStage_PATCH01
{
    public static bool Prefix(ref LifeStage __result, BaseVillager __instance)
    {
        if (__instance is Student)
        {
            // "Always"
            __result = LifeStage.Teenager;
            return false;
        }
        else
            return true;
    }
}
