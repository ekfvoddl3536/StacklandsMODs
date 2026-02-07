// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

using HarmonyLib;

namespace KivotosLand.Patches;

[HarmonyPatch(typeof(BaseVillager), "CanHaveCard")]
public static class BaseVillager_CanHaveCard_PATCH01
{
    public static bool Prefix(ref bool __result, CardData otherCard) =>
        !(otherCard is Student) || (__result = false);
}
