// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

using HarmonyLib;

namespace SmartFactory;

[HarmonyPatch(typeof(Conveyor), "CanBeInputCard")]
internal static class Conveyor_CanBeInputCard_PATCH01
{
    public static bool Prefix(ref bool __result, CardData card) =>
        __result = !(card is LogicBase);
}
