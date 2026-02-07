// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

using HarmonyLib;

namespace SmartFactory;

[HarmonyPatch(typeof(Conveyor), "OutputCardAllowed")]
internal static class Conveyor_OutputCardAllowed_PATCH01
{
    public static bool Prefix(ref bool __result, GameCard gameCard) =>
        __result = !(gameCard.CardData is LogicBase);
}
