// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

using HarmonyLib;

namespace KivotosLand.Patches;

[HarmonyPatch(typeof(GameCard), nameof(GameCard.StartDragging))]
public static class GameCard_StartDragging_PATCH01
{
    public static bool Prepare() => DraggableUnsafeAccessor._isSupported;

    public static bool Prefix(GameCard __instance)
    {
        if (__instance.CardData is Student)
        {
            Student.GameCard_OnStartDragging(__instance);
            return false;
        }

        return true;
    }
}
