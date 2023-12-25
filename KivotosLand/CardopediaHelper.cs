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

using System.Runtime.CompilerServices;

namespace KivotosLand
{
    internal static class CardopediaHelper
    {
        public static GameCard _demoCard;
        public static CardopediaEntryElement _lastHoveredEntry;

        public static GameCard RefreshDemoCard(CardData data, bool faceUp = true)
        {
            var demoCard = _demoCard;
            if (demoCard != null)
                UnityEngine.Object.Destroy(demoCard.gameObject);

            demoCard = UnityEngine.Object.Instantiate(PrefabManager.instance.GameCardPrefab);

            var cardData = UnityEngine.Object.Instantiate(data);
            cardData.transform.SetParent(demoCard.transform);

            demoCard.CardData = cardData;
            cardData.MyGameCard = demoCard;

            demoCard.FaceUp = faceUp;
            demoCard.IsDemoCard = true;

            demoCard.SetDemoCardRotation();
            demoCard.UpdateCardPalette();

            if (cardData is Student student)
                student.Initialize();

            cardData.UpdateCard();

            demoCard.ForceUpdate();

            return demoCard;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetDropSummaryFromCard(CardData cardData) =>
            cardData is Harvestable || cardData is Enemy
            ? BoosterpackData.GetSummaryFromAllCards(cardData.GetPossibleDrops(), SokTerms.label_can_drop)
            : string.Empty;
    }
}
