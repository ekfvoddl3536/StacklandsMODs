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
using System.Runtime.CompilerServices;

namespace KivotosLand
{
    [HarmonyPatch(typeof(CardopediaScreen), "SetTempDemoCard")]
    internal static class CardopediaScreen_SetTempDemoCard_PATCH01
    {
        public static bool Prefix(CardopediaScreen __instance, CardData data)
        {
            var demoCard = CardopediaHelper.RefreshDemoCard(data);
            var cardData = demoCard.CardData;

            var cardDesc = __instance.CardDescription;
            cardDesc.transform.gameObject.SetActive(true);

            cardData.UpdateCardText();

            var dropSummary = CardopediaHelper.GetDropSummaryFromCard(cardData);
            string str;

            if (cardData is Blueprint blueprint)
                str = blueprint.GetText();
            else
            {
                str = cardData.Description.Replace("\\d", "\n\n");

                if (cardData is Combatable combatable)
                    str += combatable.GetCombatableDescriptionAdvanced();

                if (dropSummary != null && cardData.MyCardType != CardType.Locations)
                    str += "\n\n" + dropSummary;
            }

            cardDesc.text = str;

            CardopediaHelper._demoCard = demoCard;

            return false;
        }
    }
}
