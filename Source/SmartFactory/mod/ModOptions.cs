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

using SuperComicLib.Stacklands;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SmartFactory
{
    internal static class ModOptions
    {
        public static readonly float updateInterval;
        public static readonly bool onDebug;

        public static readonly float maxConnLen;

        public static readonly int maxComputeStack;

        // WIP
        // public static readonly bool onBatchUpdates;
        // public static readonly int maxBatchSize;

        static ModOptions()
        {
            // 25fps
            const int FPS = 25;

            var config = ModLoad.CurrentConfig;

            updateInterval = config.GetValue(nameof(updateInterval), 1f / FPS, true);
            // clamp
            updateInterval = Mathf.Clamp(updateInterval, 0f, 1f);

            // onDebug = config.GetValue(nameof(onDebug), false, true);
            onDebug = config.GetValue(nameof(onDebug), true, true);

            maxConnLen = config.GetValue(nameof(maxConnLen), 3.5f, true);
            maxConnLen = Mathf.Clamp(maxConnLen, 0, 20f);
            maxConnLen *= maxConnLen;

            maxComputeStack = config.GetValue(nameof(maxComputeStack), 30, true);
            maxComputeStack = Mathf.Clamp(maxComputeStack, 8, 120);

            // WIP
            // onBatchUpdates = config.GetValue(nameof(onBatchUpdates), false, true);
            // 
            // maxBatchSize = config.GetValue(nameof(maxBatchSize), 1_000, true);
            // maxBatchSize = Mathf.Max(maxBatchSize, 100);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Load(ModLogger logger)
        {
            if (onDebug)
                logger.LogWarning(" ======!!! DEBUG FEATURES ENABLED !!!====== ");

            // WIP
            // if (onBatchUpdates)
            // {
            //     logger.LogWarning(" Experimental feature has been activated.");
            //     logger.LogWarning(" Max Batch Size: " + maxBatchSize);
            // }
        }
    }
}
