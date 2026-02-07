// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

namespace SmartFactory;

public sealed class ComputeField : LogicBase
{
    private ComputeStack<int> _computeStack;

    // 0    :   lastError
    // 1    :   existsVariables
    private int stateFlags;

    public bool LastError => (stateFlags & 1) != 0;
    /// <summary>
    /// <see cref="GetNextOutputValue"/>에 의해서 갱신되는 값.
    /// </summary>
    public bool ExistsVariables => (stateFlags & 2) != 0;

    internal int StateFlags => stateFlags;

    protected override void Awake()
    {
        Inputs = Array.Empty<LogicType>();

        if (Outputs == null || Outputs.Length != 1)
            Outputs = new LogicType[1] { LogicType.Any };

        _computeStack = new ComputeStack<int>(ModOptions.maxComputeStack);

        base.Awake();
    }

    protected override bool CanHaveCard(CardData otherCard) => ConstantCard._CanHaveCard(otherCard);

    public override bool CanConnectFrom(LogicBase inputCard) => false;
    protected override void SetInputValue(int newInputValue) { }

    protected override int GetNextOutputValue()
    {
        stateFlags = 0;

        ref readonly var compStack = ref _computeStack;
        compStack.Reset();

        int rFlags = 0;

        var child = MyGameCard.Child;
        for (; child != (object)null; child = child.Child)
        {
            var item = (IComputable)child.CardData;

            if (item.StackPop > compStack.Count ||
                compStack.Count >= ModOptions.maxComputeStack)
            {
                stateFlags = 1;
                return 0;
            }

            rFlags |= (item.ComputeType == ComputeType.Variable) ? 1 : 0;

            int v = item.ComputeValue(in compStack);
            compStack.Push(v);
        }

        stateFlags = rFlags << 1;
        if (compStack.Count == 1)
            return compStack.PopFast();

        // set lastError = true
        stateFlags |= 1;
        return 0;
    }

    public bool CheckIfHasAnyVariablesImmediate()
    {
        var child = MyGameCard.Child;
        for (; child != (object)null; child = child.Child)
            if (((IComputable)child.CardData).ComputeType == ComputeType.Variable)
                return true;

        return false;
    }
}
