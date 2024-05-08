// MIT License
//
// Copyright (c) 2024. SuperComic (ekfvoddl3535@naver.com)
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

namespace SmartFactory
{
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
}
