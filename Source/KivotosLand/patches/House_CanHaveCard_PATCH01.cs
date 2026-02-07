// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

using HarmonyLib;

namespace KivotosLand.Patches;

[HarmonyPatch(typeof(House), "CanHaveCard")]
public static class House_CanHaveCard_PATCH01
{
    public static bool Prefix(ref bool __result, House __instance, CardData otherCard)
    {
        __result =
            otherCard is BaseVillager
            ? !(otherCard is Student)
            : (otherCard is Kid || otherCard.Id == __instance.Id);

        return false;
    }
}
