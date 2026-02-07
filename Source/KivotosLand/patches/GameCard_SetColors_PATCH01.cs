// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

using HarmonyLib;
using UnityEngine;
using UnityEngine.Rendering;

namespace KivotosLand.Patches;

[HarmonyPatch(typeof(GameCard), "SetColors")]
public static class GameCard_SetColors_PATCH01
{
    public static bool Prepare() => ModOptions.onFCR;

    public static bool Prefix(GameCard __instance)
    {
        var cardData = __instance.CardData;
        if (cardData is Student student)
        {
            __instance.CombatStatusCircle.color = Color.red;

            var palette = new TEMP_ColorPalette(cardData.MyPalette);
            if (__instance.IsHit)
            {
                __instance.CombatStatusCircle.color = Color.white;
                palette.color1 = palette.color2 = palette.color3 = Color.white;
            }

            var cardRenderer = __instance.CardRenderer;
            cardRenderer.shadowCastingMode = __instance.IsEquipped ? ShadowCastingMode.Off : ShadowCastingMode.On;

            var propBlock = student._propBlock;
            cardRenderer.GetPropertyBlock(propBlock);

            propBlock.SetColor("_Color", palette.color1);
            propBlock.SetColor("_Color2", palette.color2);
            propBlock.SetColor("_IconColor", palette.color3);

            if (cardData is ResourceChest)
                SetSecondary(propBlock, SpriteManager.instance.ChestIconSecondary.texture);
            else if (cardData is ResourceMagnet)
                SetSecondary(propBlock, SpriteManager.instance.MagnetIconSecondary.texture);
            else
                propBlock.SetFloat("_HasSecondaryIcon", 0.0f);

            propBlock.SetFloat("_BigShineStrength", 1f);
            propBlock.SetFloat("_ShineStrength", 1f);
            propBlock.SetFloat("_Foil", cardData.IsFoil || cardData.IsShiny ? 1f : 0f);

            propBlock.SetTexture("_IconTex", SpriteManager.instance.EmptyTexture.texture);

            cardRenderer.SetPropertyBlock(propBlock, 2);

            __instance.SpecialText.color = palette.color1;
            __instance.SpecialIcon.color = palette.color3;
            __instance.IconRenderer.color = palette.color3;

            __instance.CoinText.color = palette.color1;

            __instance.CoinIcon.color = palette.color3;

            __instance.EquipmentButton.Color = palette.color1;

            __instance.CardNameText.color = palette.color3;

            return false;
        }

        return true;
    }

    private static void SetSecondary(MaterialPropertyBlock propBlock, Texture2D texture2D)
    {
        propBlock.SetFloat("_HasSecondaryIcon", 1.0f);
        propBlock.SetTexture("_SecondaryTex", texture2D);
    }

    private ref struct TEMP_ColorPalette
    {
        public Color color1, color2, color3;

        public TEMP_ColorPalette(CardPalette cp)
        {
            color1 = cp.Color;
            color2 = cp.Color2;
            color3 = cp.Icon;
        }
    }
}
