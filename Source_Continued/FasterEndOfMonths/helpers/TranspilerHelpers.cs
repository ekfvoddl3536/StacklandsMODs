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
using System.Reflection;
using UnityEngine;
// -- alias --
using acs = HarmonyLib.AccessTools;
using R0 = HarmonyLib.CodeMatcher; // return first(0) <-- no mean. haha
using cm = HarmonyLib.CodeMatch;
using op = System.Reflection.Emit.OpCodes;
using arg0 = System.Collections.Generic.IEnumerable<HarmonyLib.CodeInstruction>;

namespace FasterEndOfMonths;

internal static class TranspilerHelpers
{
    public static readonly ConstructorInfo _waitForSecondsCtor;
    public static readonly FieldInfo _saveManagerInst;
    public static readonly MethodInfo _saveManagerSave;

    public static readonly MethodInfo _EOMHelpersAutosave;

    static TranspilerHelpers()
    {
        _waitForSecondsCtor = typeof(WaitForSeconds).GetConstructor([typeof(float)]);
        _saveManagerInst = acs.Field(typeof(SaveManager), nameof(SaveManager.instance));
        _saveManagerSave = acs.Method(typeof(SaveManager), nameof(SaveManager.Save), [typeof(bool)]);

        _EOMHelpersAutosave = acs.Method(typeof(EOMHelpers), nameof(EOMHelpers.Autosave));
    }

    public static R0 NewobjWaitForSecondsMatcher(arg0 @is) =>
        new R0(@is)
        .MatchStartForward(
            new cm(op.Ldc_R4),
            new cm(op.Newobj, _waitForSecondsCtor)
        );

    public static R0 CallvirtSaveManagerSaveMatcher(arg0 @is) =>
        new R0(@is)
        .MatchStartForward(
            new cm(op.Ldsfld, _saveManagerInst),
            new cm(op.Ldc_I4_1),
            new cm(op.Callvirt, _saveManagerSave)
        );

    public static void ReduceAutosaveCallReplace(R0 m)
    {
        m.SetInstructionAndAdvance(new CodeInstruction(op.Call, _EOMHelpersAutosave));

        m.SetInstructionAndAdvance(new CodeInstruction(op.Nop));
        m.SetInstructionAndAdvance(new CodeInstruction(op.Nop));
    }
}