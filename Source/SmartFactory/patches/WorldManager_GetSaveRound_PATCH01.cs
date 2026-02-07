// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

using HarmonyLib;

namespace SmartFactory;

[HarmonyPatch(typeof(WorldManager), nameof(WorldManager.GetSaveRound))]
internal static class WorldManager_GetSaveRound_PATCH01
{
    public static void Prefix(WorldManager __instance)
    {
        var allCards =
            __instance.AllCards
            .Where(x => x.CardData is IJsonSaveLoadable);

        foreach (var card in allCards)
            ((IJsonSaveLoadable)card.CardData).OnSave();
    }
}
