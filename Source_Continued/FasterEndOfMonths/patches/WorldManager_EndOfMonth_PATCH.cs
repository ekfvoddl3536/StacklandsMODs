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

namespace FasterEndOfMonths.Patchs;

[HarmonyPatch(typeof(WorldManager), EndOfMonth)]
internal static class WorldManager_EndOfMonth_PATCH
{
    public const string EndOfMonth = nameof(EndOfMonth);

    public static bool Prefix(WorldManager __instance, EndOfMonthParameters param)
    {
        param ??= new EndOfMonthParameters();
        param.SkipEndConfirmation = true;

        GameCanvas.instance.SetScreen<CutsceneScreen>();

        __instance.CloseOpenInventories();

        var routine =
            __instance.CurrentBoard.Id != Board.Cities
            ? new NoWaitCoroutine(__instance.EndOfMonthRoutine(param))
            : CitiesDLC.EndOfMonthRoutine(__instance, param);

        __instance.currentAnimationRoutine = __instance.StartCoroutine(new NoWaitCoroutine(routine));

        if (GameScreen.instance.ControllerIsInUI)
            GameScreen.instance.SetControllerInUI(false);

        QuestManager.instance.SpecialActionComplete("month_end");

        return false;
    }
}
