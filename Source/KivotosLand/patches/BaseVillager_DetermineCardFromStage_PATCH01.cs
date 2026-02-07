// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

using HarmonyLib;

namespace KivotosLand.Patches;

[HarmonyPatch(typeof(BaseVillager), nameof(BaseVillager.DetermineCardFromStage))]
public static class BaseVillager_DetermineCardFromStage_PATCH01
{
    public static bool Prefix(ref string __result, BaseVillager __instance)
    {
        if (__instance is Student)
        {
            __result = __instance.Id;
            return false;
        }
        else
            return true;
    }
}
