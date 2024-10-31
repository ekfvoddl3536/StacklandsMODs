// MIT License
//
// Copyright (c) 2022 Benedikt Werner
// Copyright (c) 2024 SuperComic (ekfvoddl3535@naver.com)
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

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FasterEndOfMonths;

internal static class EOMCHelpers // EndOfMonthCutscenes
{
    internal static IEnumerator FeedVillagers()
    {
        AudioManager.me.PlaySound2D(AudioManager.me.Eat, UnityEngine.Random.Range(0.8f, 1.2f), 0.3f);

        var wminst = WorldManager.instance;

        int requiredFoodCount = wminst.GetRequiredFoodCount();
        var cardsToFeed = EndOfMonthCutscenes.GetCardsToFeed();

        var fedCards = new List<CardData>();
        for (int i = 0; i < cardsToFeed.Count; i++)
        {
            CardData cardToFeed = cardsToFeed[i];
            if (cardToFeed is BaseVillager baseVillager)
                baseVillager.AteUncookedFood = false;
            
            int foodForVillager = wminst.GetCardRequiredFoodCount(cardToFeed.MyGameCard);
            for (int j = 0; j < foodForVillager; j++)
            {
                Food food = EndOfMonthCutscenes.GetFoodToUseUp();
                if (food == null)
                    break;
                GameCard foodCard = food.MyGameCard;
                food.FoodValue--;
                requiredFoodCount--;
                if (cardToFeed is BaseVillager vill)
                {
                    vill.HealthPoints = UnityEngine.Mathf.Min(vill.HealthPoints + 3, vill.ProcessedCombatStats.MaxHealth);
                    food.ConsumedBy(vill);
                    EndOfMonthCutscenes.TryCreatePoop(vill);
                        
                    vill.AteUncookedFood = !food.IsCookedFood;
                }

                if (food.FoodValue <= 0 &&
                    food.Id != "compactstorage.food_warehouse" &&
                    food is not Hotpot)
                {
                    var originalStack = foodCard.GetAllCardsInStack();
                    foodCard.RemoveFromStack();
                    food.FullyConsumed(cardToFeed);
                    originalStack.Remove(foodCard);
                    wminst.Restack(originalStack);
                    foodCard.DestroyCard(true, true);
                }

                if (j == foodForVillager - 1)
                    fedCards.Add(cardToFeed);
            }
        }

        if (requiredFoodCount <= 0) 
            yield break;

        var unfedVillagers = cardsToFeed.Where(dx => !fedCards.Contains(dx) && dx is not Kid).ToList();
        var humansToDie = unfedVillagers.Count;
        
        EndOfMonthCutscenes.SetStarvingHumanStatus(humansToDie);
        yield return Cutscenes.WaitForContinueClicked(SokLoc.Translate(SokTerms.label_uh_oh));

        for (int i = 0; i < unfedVillagers.Count; i++)
        {
            CardData cardData2 = unfedVillagers[i];
            if (cardData2 is not Kid)
            {
                yield return wminst.KillVillagerCoroutine(
                    cardData2 as Villager,
                    null,
                    null
                );
                EndOfMonthCutscenes.SetStarvingHumanStatus(humansToDie - i);
            }
        }

        if (wminst.CheckAllVillagersDead())
        {
            wminst.VillagersStarvedAtEndOfMoon = true;

            var boid = wminst.CurrentBoard.Id;
            if (boid == Board.Mainland)
            {
                EndOfMonthCutscenes.CutsceneText = SokLoc.Translate(SokTerms.label_everyone_starved);

                yield return Cutscenes.WaitForContinueClicked(SokLoc.Translate(SokTerms.label_game_over));

                GameCanvas.instance.SetScreen<GameOverScreen>();
                wminst.currentAnimationRoutine = null;
            }
            else if (boid == Board.Island || boid == Board.Cities)
                yield return Cutscenes.EveryoneOnIslandDead();
            else if (boid == Board.Forest)
                yield return Cutscenes.EveryoneInForestDead();
            else if (wminst.CurrentBoard.BoardOptions.IsSpiritWorld)
                yield return Cutscenes.EveryoneInSpiritWorldDead(boid);
            else
                yield return Cutscenes.EveryoneOnIslandDead();
        }
    }
}