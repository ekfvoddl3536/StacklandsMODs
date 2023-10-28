// MIT License
//
// Copyright (c) 2023. SuperComic (ekfvoddl3535@naver.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using HarmonyLib;
using UnityEngine;
using UnityEngine.Rendering;

namespace KivotosLand.Patches
{
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
}
