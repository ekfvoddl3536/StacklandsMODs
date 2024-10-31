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

using HarmonyLib;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace FasterEndOfMonths.Patchs;

internal static class CitiesDLC
{
    internal static IEnumerator EndOfMonthRoutine(WorldManager wm, EndOfMonthParameters param)
    {
        AudioManager.me.PlaySound2D(AudioManager.me.EndOfMoon, 0.8f, 0.2f);

        // don't change view
        // skip wellbeing bar

        var allCards = wm.AllCards.Where(static c => c.MyBoard.IsCurrent);

        var requirementsCards = allCards.Where(static c =>
        {
            var d = c.CardData;
            if (d.RequirementHolders == null || d.RequirementHolders.Count <= 0) return false;

            var s = c.GetCardWithStatusInStack();
            return s == null || s.TimerActionId != "finish_blueprint";
        }).Select(static x => x.CardData);

        TrySatisfiedCards(requirementsCards);
        
        Phase01();

        var rootWithResults = allCards.Where(static x => x.CardData.MonthlyRequirementResult != null);

        bool lostGame = false;
        var cminst = CitiesManager.instance;
        if (cminst.Wellbeing > 0)
        {
            foreach (var gc in rootWithResults)
            {
                int index = 0;
                foreach (var result in gc.CardData.MonthlyRequirementResult.results)
                {
                    var v = result.Value;
                    if (v.Amount != 0)
                        wm.CreateFloatingText(
                            gc,
                            v.Amount > 0,
                            v.Amount,
                            v.Card.CardData.GetRequirementDescription(v.Card, v.CardAmount),
                            wm.GetIconStringFromRequirementType(v.Type),
                            v.Amount > 0, 
                            index, 0f);
                    
                    index++;
                }
            }

            wm.CutsceneBoardView = true;
            yield return EndOfMonthCutscenes.MaxCardCount();
            wm.CutsceneBoardView = false;

            if (cminst.Wellbeing > 25 && !wm.CurrentRunOptions.IsPeacefulMode && wm.CurrentMonth == cminst.NextConflictMonth && cminst.NextConflictMonth != -1)
            {
                var rsp = wm.GetRandomSpawnPosition();
                GameCamera.instance.TargetPositionOverride = rsp;
                wm.CutsceneTitle = SokLoc.Translate(SokTerms.label_goblin_conflict_title);
                wm.CutsceneText = SokLoc.Translate(SokTerms.label_goblin_conflict_text);
                wm.CreateCard(rsp, Cards.event_goblin_attack);
                yield return wm.WaitForContinueClicked(SokLoc.Translate(SokTerms.label_uh_oh));
                GameCamera.instance.TargetPositionOverride = null;
            }
        }
        else
        {
            lostGame = true;

            wm.CutsceneTitle = SokLoc.Translate(SokTerms.label_cities_game_over_title);
            wm.CutsceneText = SokLoc.Translate(SokTerms.label_cities_game_over_text);
            yield return wm.WaitForContinueClicked(SokLoc.Translate(SokTerms.label_uh_oh));

            wm.CutsceneText = SokLoc.Translate(SokTerms.label_cities_game_over_text_1);
            yield return wm.WaitForContinueClicked(SokLoc.Translate(SokTerms.label_okay));

            GameCamera.instance.TargetPositionOverride = null;
        }

        CutsceneScreen.instance.CanMoveScreen = false;
        if (!lostGame)
        {
            wm.CloseAllFloatingTextObjects();
            TrySatisfiedCards(requirementsCards.Where(static k => k != null));

            rootWithResults.Do(static k => k.CardData.MonthlyRequirementResult = null);

            if ((wm.CurrentMonth & 3) == 0) // mod 4
            {
                Boosterpack pack = wm.CreateBoosterpack(wm.MiddleOfBoard(), "cities_weather");
                AudioManager.me.PlaySound2D(AudioManager.me.CardCreate, 1f, 0.5f);
                
                for (int i = 0; i < pack.TotalCardsInPack; ++i)
                {
                    pack.Clicked();
                    yield return null; // wait just a frame
                }
            }
        }

        QuestManager.instance.SpecialActionComplete("cities_wellbeing_changed");
        GameCanvas.instance.SetScreen<GameScreen>();

        param.OnDone?.Invoke();

        wm.currentAnimationRoutine = null;
        wm.CutsceneBoardView = false;
        GameCamera.instance.TargetPositionOverride = null;
        
        wm.CutsceneTitle = string.Empty;
        wm.CutsceneText = string.Empty;

        if (lostGame)
        {
            var board = wm.GetCurrentBoardSafe();
            wm.GoToBoard(wm.GetBoardWithId(Board.Mainland), () =>
            {
                wm.RemoveAllBoostersFromBoard(board.Id);
                wm.ResetBoughtBoostersOnLocation(board.Location);
                wm.ResetCityVariables();
            }, Board.Cities);
        }
        else
        {
            EOMHelpers.Autosave();
            DebugScreen.instance?.AutoSave();
            wm.QueueCutsceneIfNotPlayed("cities_first_moon");
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Phase01()
    {
        var cminst = CitiesManager.instance;

        int prevWellbeing = cminst.Wellbeing;

        CutsceneScreen.instance.WellbeingAmount = cminst.Wellbeing;

        var wellbeing_diff = cminst.Wellbeing - prevWellbeing;
        if (wellbeing_diff > 0)
        {
            AudioManager.me.PlaySound2D(AudioManager.me.AddWellbeing, 1f, 0.5f);
            if (wellbeing_diff >= 5)
                QuestManager.instance.SpecialActionComplete("cities_wellbeing_gained_5");
        }
        else if (wellbeing_diff < 0)
        {
            AudioManager.me.PlaySound2D(AudioManager.me.LostWellbeing, 1f, 0.5f);
            if (wellbeing_diff <= -5)
                QuestManager.instance.SpecialActionComplete("cities_wellbeing_lost_5");
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void TrySatisfiedCards(IEnumerable<CardData> requirementsCards)
    {
        foreach (var rcd in requirementsCards)
            foreach (var reqHolder in rcd.RequirementHolders)
            {
                foreach (var creq in reqHolder.CardRequirements)
                    if (!creq.Satisfied(rcd.MyGameCard))
                    {
                        foreach (var negative in reqHolder.NegativeResults)
                            PerformUntil(negative.Perform(rcd.MyGameCard));

                        goto _nextLoop;
                    }

                foreach (var positive in reqHolder.PositiveResults)
                    PerformUntil(positive.Perform(rcd.MyGameCard));

                _nextLoop:;
            }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void PerformUntil(IEnumerator e)
    {
        if (e != null)
            while (e.MoveNext()) ;
    }
}