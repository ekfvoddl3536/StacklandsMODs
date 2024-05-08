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

using System;
using SuperComicLib.Runtime;
using SuperComicLib.Stacklands;

namespace SmartFactory
{
    public sealed unsafe class ModLoad : Mod
    {
        internal static ConfigFile CurrentConfig;

        public override void Ready()
        {
            Logger.Log(nameof(SmartFactory) + " MOD Loading... by 'SuperComic (ekfvoddl3535@naver.com)'");

            try
            {
                LogPhase(1, "Load fallback terms");
                this.LoadFallbackTerms();


                LogPhase(2, "Load dependencies");
                this.PatchAllWithDependencies(Harmony, false);


                LogPhase(3, "Patch");
                Harmony.PatchAll();


                LogPhase(4, "Read MOD Options");
                LoadMODOptions();


                LogPhase(5, "Post Load");
                PostLoad();


                Logger.Log(nameof(SmartFactory) + " MOD Loaded!");
            }
            catch (Exception e)
            {
                LogFail(e);
            }
        }

        private void LoadMODOptions()
        {
            CurrentConfig = Config;
            ModOptions.Load(Logger);
            CurrentConfig = null;
        }

        private void PostLoad()
        {
            WorldManager.instance.gameObject.AddComponent<MOD_SMARTFACTORY_BY_SUPERCOMIC_MAIN_SYSTEM>();

            ModPostLoad.Ready();
        }

        private void LogPhase(int step, string title) =>
            Logger.Log(nameof(SmartFactory) + $" / Phase {step}: {title}");

        private void LogFail(Exception e) =>
            Logger.LogException(nameof(SmartFactory) + " MOD Load FAIL! -> " + e.ToString());
    }
}
