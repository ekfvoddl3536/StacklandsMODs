// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

#pragma warning disable IDE1006
namespace SmartFactory;

partial class LogicBase
{
    protected void DisconnectAll()
    {
        _ResetConnect(Inputs);
        _ResetConnect(Outputs);

        InputDisconnectAll();
        OutputDisconnectAll();

        OnDisconnectedAll();
    }

    protected virtual void OnDisconnectedAll() { }

    protected void InputDisconnectAll()
    {
        var nodes = InputNodes;
        for (int i = 0; i < nodes.Length; ++i)
            if (nodes[i] != (object)null)
            {
                OutputDisconnect(this, nodes[i]);
                nodes[i] = null;
            }
    }

    protected void OutputDisconnectAll()
    {
        var nodes = OutputNodes;
        for (int i = 0; i < nodes.Length; ++i)
            if (nodes[i] != (object)null)
            {
                InputDisconnect(this, nodes[i]);
                nodes[i] = null;
            }
    }

    /// <summary>
    /// <paramref name="this"/>에서 입력 노드에 연결된 <paramref name="outputCard"/> 개체를 끊는다.
    /// </summary>
    protected static void OutputDisconnect(LogicBase outputCard, LogicBase @this) =>
        _Disconnect(outputCard, @this.OutputNodes, @this.Outputs);

    /// <summary>
    /// <paramref name="this"/>에서 입력 노드에 연결된 <paramref name="inputCard"/> 개체를 끊는다.
    /// </summary>
    protected static void InputDisconnect(LogicBase inputCard, LogicBase @this) =>
        _Disconnect(inputCard, @this.InputNodes, @this.Inputs);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected static void _Disconnect(LogicBase otherCard, LogicBase[] nodes, LogicType[] types)
    {
        for (int i = 0; i < nodes.Length; ++i)
            if (nodes[i] == (object)otherCard)
            {
                nodes[i] = null!;

                // @DISABLE_NO_CHECK
                // types.refdata(i) &= ~LogicType.Connected;
                types[i] &= ~LogicType.Connected;

                break;
            }
    }
}
