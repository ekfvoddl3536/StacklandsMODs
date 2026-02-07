// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

#pragma warning disable IDE1006
namespace SmartFactory;

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
