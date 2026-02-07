// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

namespace SmartFactory;

partial class LogicBase
{
    public sealed override void UpdateCardText() { }

    public sealed override void UpdateCard()
    {
        if (MyGameCard.IsDemoCard || !MyGameCard.MyBoard.IsCurrent)
            return;

        base.UpdateCard();

        OnUpdateCard();
    }

    protected virtual void OnUpdateCard()
    {
        NetworkArrows.ReDraw(this);

        CheckNodeLength();
    }

    protected virtual void CheckNodeLength()
    {
        if (IsDraggingMe())
        {
            ConnectionUpdate(InputNodes, Inputs);

            ConnectionUpdate(OutputNodes, Outputs);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected bool IsDraggingMe()
    {
        var dragCard = WorldManager.instance.DraggingDraggable;
        return
            dragCard != (object)null && 
            dragCard == (object)MyGameCard;
    }

    public void NetworkUpdate()
    {
        // 초기 값
        int nextOutputValue = GetNextOutputValue();

        var outputs = Outputs;
        // @DISABLE_NO_CHECK
        // // next types
        // ref var ntFirst = ref outputs.refdata();
        // // next nodes
        // ref var nnFirst = ref OutputNodes.refdata();

        // 값 전파
        for (int i = 0; i < outputs.Length; ++i)
            if (outputs[i] < 0) // isConnected?
            {
                OutputNodes[i].SetInputValue(nextOutputValue);
            }

            // @DISABLE_NO_CHECK
            // if (Unsafe.Add(ref ntFirst, i) < 0) // isConnected?
            // {
            //     ref var item = ref Unsafe.Add(ref nnFirst, i);
            // 
            //     item.SetInputValue(nextOutputValue);
            // }
    }
}
