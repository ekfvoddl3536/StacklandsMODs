// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

#pragma warning disable IDE1006
namespace SmartFactory;

partial class LogicBase
{
    /// <summary>
    /// <paramref name="this"/>에서 출력 노드에 <paramref name="outputCard"/> 개체를 연결한다.
    /// </summary>
    protected static void OutputConnect(LogicBase outputCard, LogicBase @this) =>
        _Connect(outputCard, @this.OutputNodes, @this.Outputs);

    /// <summary>
    /// <paramref name="this"/>에서 입력 노드에 <paramref name="inputCard"/> 개체를 연결한다.
    /// </summary>
    protected static void InputConnect(LogicBase inputCard, LogicBase @this) =>
        _Connect(inputCard, @this.InputNodes, @this.Inputs);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected static void _Connect(LogicBase otherCard, LogicBase[] nodes, LogicType[] types)
    {
        for (int i = 0; i < nodes.Length; ++i)
            if (nodes[i] == (object)null)
            {
                nodes[i] = otherCard;

                // @DISABLE_NO_CHECK
                // types.refdata(i) |= LogicType.Connected;
                types[i] |= LogicType.Connected;

                break;
            }
    }
}
