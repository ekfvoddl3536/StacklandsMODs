// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

#pragma warning disable IDE1006
using UnityEngine;

namespace SmartFactory;

partial class LogicBase
{
    public virtual void ConnectFrom(LogicBase inputCard)
    {
        OutputConnect(this, inputCard);

        InputConnect(inputCard, this);
    }

    public virtual void DisconnectFrom(LogicBase inputCard)
    {
        OutputDisconnect(this, inputCard);

        InputDisconnect(inputCard, this);
    }

    /// <summary>
    /// check connections is valid
    /// </summary>
    public override void StoppedDragging()
    {
        if (!(this is IComputable) && MyGameCard.Parent != (object)null)
            DisconnectAll();
    }

    protected void ConnectionUpdate(LogicBase[] nodes, LogicType[] types)
    {
        var myPos = MyGameCard.transform.position;

        // @DISABLE_NO_CHECK
        // ref var ntFirst = ref types.refdata();
        for (int i = 0; i < nodes.Length; ++i)
        {
            var item = nodes[i];
            if (item == (object)null ||
                _GetDistance(item, myPos) <= ModOptions.maxConnLen)
                continue;

            // @DISABLE_NO_CHECK
            // Unsafe.Add(ref ntFirst, i) &= ~LogicType.Connected;
            types[i] &= ~LogicType.Connected;

            nodes[i] = default;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected static float _GetDistance(CardData dst, Vector3 src)
    {
        var tmp = dst.MyGameCard.transform.position;

        float dx = tmp.x - src.x;
        float dy = tmp.z - src.z;

        return dx * dx + dy * dy;
    }
}
