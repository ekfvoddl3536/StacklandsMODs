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

namespace SuperComicLib.Stacklands
{
    [HarmonyPatch(typeof(SokLoc), nameof(SokLoc.Translate), new[] { typeof(string) })]
    internal static class SokLoc_Translation_PATCH01
    {
        public static bool Prefix(ref string __result, string termId)
        {
            termId = termId.ToLowerCached();

            var instance = SokLoc.instance;
            if (instance != null && 
                instance.CurrentLocSet.TermLookup.TryGetValue(termId, out var sokTerm))
            {
                var text = sokTerm.GetText();
                if (!string.IsNullOrWhiteSpace(text))
                {
                    __result = text;
                    return false;
                }
            }

            __result =
                SokLoc.FallbackSet.TermLookup.TryGetValue(termId, out var fallback_sokTerm)
                ? fallback_sokTerm.GetText()
                : "---MISSING---";
                
            return false;
        }
    }
}
