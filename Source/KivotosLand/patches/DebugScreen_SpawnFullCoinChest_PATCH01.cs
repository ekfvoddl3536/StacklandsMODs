// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

using HarmonyLib;

namespace KivotosLand.Patches;

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
