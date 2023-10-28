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
using UnityEngine;

namespace SuperComicLib.Stacklands
{
    public static class ConfigFile_Extensions
    {
        /// <summary>
        /// Reads the <see cref="ConfigEntry{T}"/> of the configuration value with the specified name from the <see cref="ConfigFile"/>.
        /// <para/>
        /// The name and tooltip of the UI are retrieved by TermId with the following pattern:<br/>
        /// <br/>
        /// name     : <c>"op_" + <paramref name="name"/> + "_name"</c><br/>
        /// tooltipe : <c>"op_" + <paramref name="name"/> + "_tooltip"</c><br/>
        /// <br/>
        /// If a TermId in the current language cannot be found, English is the default.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ConfigEntry<T> GetEntry<T>(this ConfigFile conf, string name, T defaultValue, bool restartAfterChanged = false)
        {
            var term = "op_" + name;

            var entry = conf.GetEntry<T>(name, defaultValue, new ConfigUI
            {
                RestartAfterChange = restartAfterChanged,
                NameTerm = term + "_name",
                TooltipTerm = term + "_tooltip"
            });

            if (!SokLoc.instance.CurrentLocSet.ContainsTerm(entry.UI.NameTerm))
            {
                Debug.LogWarning("The provided string is not available in the current language. Attempting to substitute in English...");

                var fallback = SokLoc.FallbackSet;

                var ui = entry.UI;
                ui.Name = fallback.TranslateTerm(ui.NameTerm);
                ui.Tooltip = fallback.TranslateTerm(ui.TooltipTerm);

                ui.NameTerm = null;
                ui.TooltipTerm = null;
            }

            return entry;
        }

        /// <summary>
        /// Reads the <see cref="ConfigEntry{T}.Value"/> of the configuration value with the specified name from the <see cref="ConfigFile"/>.
        /// <para/>
        /// The name and tooltip of the UI are retrieved by TermId with the following pattern:<br/>
        /// <br/>
        /// name     : <c>"op_" + <paramref name="name"/> + "_name"</c><br/>
        /// tooltipe : <c>"op_" + <paramref name="name"/> + "_tooltip"</c><br/>
        /// <br/>
        /// If a TermId in the current language cannot be found, English is the default.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetValue<T>(this ConfigFile conf, string name, T defaultValue, bool restartAfterChanged = false) =>
            GetEntry(conf, name, defaultValue, restartAfterChanged).Value;
    }
}
