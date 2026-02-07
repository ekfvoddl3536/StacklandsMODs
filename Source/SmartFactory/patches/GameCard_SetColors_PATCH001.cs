// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

using HarmonyLib;
using UnityEngine;
using UnityEngine.Rendering;

namespace SmartFactory;

[HarmonyPatch(typeof(GameCard), "SetColors")]
internal static class GameCard_SetColors_PATCH001
{
    public static bool Prefix(GameCard __instance)
    {
        if (__instance.CardData is LogicBase gate)
        {
            var myCardPalette = gate.MyPalette;

            var color1 = myCardPalette.Color;
            var color2 = myCardPalette.Color2;
            var color3 = myCardPalette.Icon;

            var useRectBG = gate.RectBackground;

            var propBlock = GameCardAccessor.propBlock.Value<MaterialPropertyBlock>(__instance);

            // optimize: logic gates not equipable
            __instance.CardRenderer.shadowCastingMode = ShadowCastingMode.On;

            // optimize: logic gates not combatable
            //  (code removed)
            __instance.CombatStatusCircle.color = Color.red;

            __instance.CardRenderer.GetPropertyBlock(propBlock, 2);

            propBlock.SetColor("_Color", color1);
            propBlock.SetColor("_Color2", color2);
            propBlock.SetColor("_IconColor", color3);

            // optimize: logic gates has always secondary texture
            var emptyTexture = SpriteManager.instance.EmptyTexture.texture;

            if (useRectBG)
            {
                propBlock.SetFloat("_HasSecondaryIcon", 1f);
                propBlock.SetTexture("_SecondaryTex", emptyTexture);
            }

            // optimize: logic gates can't be equipable
            propBlock.SetFloat("_BigShineStrength", 1f);
            propBlock.SetFloat("_ShineStrength", 1f);
            // optimize: logic gates can't be foil, shiny
            propBlock.SetFloat("_Foil", 0f);

            var temp = __instance.IconRenderer.sprite;
            var texture =
                temp
                ? temp.texture
                : emptyTexture;

            propBlock.SetTexture("_IconTex", texture);

            __instance.CardRenderer.SetPropertyBlock(propBlock, 2);

            // optimize: re-sort assign order
            __instance.CoinText.color = color1;
            __instance.SpecialText.color = color1;
            __instance.EquipmentButton.Color = color1;

            __instance.CoinIcon.color = color3;
            __instance.SpecialIcon.color = color3;
            __instance.IconRenderer.color = color3;
            __instance.CardNameText.color = color3;

            return false;
        }

        return true;
    }
}
