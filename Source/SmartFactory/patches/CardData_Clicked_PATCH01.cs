// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

using HarmonyLib;
using UnityEngine.InputSystem;

namespace SmartFactory;

[HarmonyPatch(typeof(CardData), nameof(CardData.Clicked))]
internal static class CardData_Clicked_PATCH01
{
    public static bool Prefix(CardData __instance)
    {
        var keyboard = Keyboard.current;

        var pressCtrlKey = false;
        if (keyboard != null)
            pressCtrlKey = keyboard.ctrlKey.isPressed;

        return
            !pressCtrlKey ||
            !(LogicNetworkManager.fromCard is IPredCardByRefLogic logic) ||
            !logic.TryLinkByRef(__instance);
    }
}
