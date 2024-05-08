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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SmartFactory
{
    internal static class ModPostLoad
    {
        public static void Ready()
        {
            var loader = WorldManager.instance.GameDataLoader;

            // 카드 출현 확률을 설정하기 위해 카드를 등록
            RegisterCards(loader);

            // 카드를 출현시키기 위해 부스터팩에 목록으로 추가
            ApplyBoosterpacks(loader);
        }

        private static void ApplyBoosterpacks(GameDataLoader loader)
        {
            var list = loader.BoosterpackDatas;

            foreach (var w in list.Where(x => x.BoosterLocation == Location.Mainland))
                if (w.BoosterId.Equals("structures", StringComparison.OrdinalIgnoreCase))
                {
                    AddCardBag(w.CardBags, SetCardBagType.AdvancedBuildingIdea);

                    break;
                }
        }

        [SkipLocalsInit]
        private static unsafe void RegisterCards(GameDataLoader loader)
        {
            const ulong K_MAP =
                1ul << (int)SetCardBagType.AdvancedBuildingIdea |
                1ul << (int)SetCardBagType.Island_AdvancedBuildingIdea |
                1ul << (int)SetCardBagType.Happiness_AdvancedBuildingIdea;

            var bags = loader.SetCardBags;

            // 이 모드에서 추가한 모든 블루프린트 프리펩 조회,
            // SimpleCardChance로 변환
            var myModIdeas = 
                loader.CardDataPrefabs
                .Where(x => x.Id.StartsWith("sf_blueprint_"))
                .Select(x => new SimpleCardChance(x.Id, 1))
                .ToArray();

            var found = 0ul;
            foreach (var data in bags)
                if (((int)(K_MAP >> (int)data.SetCardBagType) & 1) != 0)
                {
                    found |= 1ul << (int)data.SetCardBagType;

                    data.Chances.AddRange(myModIdeas);
                }

            // 만약 찾지 못한 테마가 있으면, 강제로 추가한다
            var pList = stackalloc SetCardBagType[3];
            pList[0] = SetCardBagType.AdvancedBuildingIdea;
            pList[1] = SetCardBagType.Island_AdvancedBuildingIdea;
            pList[2] = SetCardBagType.Happiness_AdvancedBuildingIdea;

            for (int i = 0; i < 3; ++i)
                if (((int)(found >> (int)pList[i]) & 1) == 0)
                    bags.Add(new SetCardBagData
                    {
                        SetCardBagType = pList[i],
                        Chances = new List<SimpleCardChance>(myModIdeas)
                    });
        }

        private static void AddCardBag(List<CardBag> myBags, SetCardBagType type)
        {
            if (!myBags.Any(x => x.CardBagType == CardBagType.SetCardBag && x.SetCardBag == type))
                myBags.Add(new CardBag
                {
                    CardBagType = CardBagType.SetCardBag,
                    CardsInPack = 1,
                    SetCardBag = type,
                    UseFallbackBag = false,
                });
        }
    }
}