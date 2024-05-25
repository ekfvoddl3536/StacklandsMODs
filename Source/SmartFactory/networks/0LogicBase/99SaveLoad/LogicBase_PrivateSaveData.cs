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

#pragma warning disable IDE1006
using SuperComicLib.Runtime;
using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SmartFactory
{
    partial class LogicBase
    {
        [Serializable]
        private sealed class PrivateSaveData
        {
            public LogicType[] Inputs;
            public LogicType[] Outputs;

            public string[] InputNodeUniqueIds;
            public string[] OutputNodeUniqueIds;

            public PrivateSaveData(LogicBase me)
            {
                Inputs = me.Inputs;
                Outputs = me.Outputs;

                InputNodeUniqueIds = me.InputNodes.Select(x => x != (object)null ? x.UniqueId : string.Empty).ToArray();
                OutputNodeUniqueIds = me.OutputNodes.Select(x => x != (object)null ? x.UniqueId : string.Empty).ToArray();
            }

            public void LoadAll(LogicBase target)
            {
                target.Inputs = Inputs;
                target.Outputs = Outputs;

                ConvertToNodes(InputNodeUniqueIds, OutputNodeUniqueIds, target);
            }

            private static void ConvertToNodes(string[] ins, string[] outs, LogicBase target)
            {
                var allCards = WorldManager.instance.AllCards;

                var inres = new LogicBase[ins.Length];
                var outres = new LogicBase[outs.Length];

                allCards
                    .AsParallel()
                    .AsUnordered()
                    .Where(x => x.CardData is LogicBase)
                    .ForAll(x =>
                    {
                        // @DISABLE_NO_CHECK
                        // ref var inref = ref inres.refdata();
                        // ref var outref = ref outres.refdata();
                        // 
                        // var inidx = ins.AsSpan().IndexOf(x.CardData.UniqueId);
                        // if (inidx >= 0)
                        //     Unsafe.Add(ref inref, inidx) = (LogicBase)x.CardData;
                        // else
                        // {
                        //     var outidx = outs.AsSpan().IndexOf(x.CardData.UniqueId);
                        //     if (outidx >= 0)
                        //         Unsafe.Add(ref outref, outidx) = (LogicBase)x.CardData;
                        // }

                        var inidx = ins.AsSpan().IndexOf(x.CardData.UniqueId);
                        if (inidx >= 0)
                            inres[inidx] = (LogicBase)x.CardData;
                        else
                        {
                            var outidx = outs.AsSpan().IndexOf(x.CardData.UniqueId);
                            if (outidx >= 0)
                                outres[outidx] = (LogicBase)x.CardData;
                        }
                    });

                target.InputNodes = inres;
                target.OutputNodes = outres;
            }
        }
    }
}
