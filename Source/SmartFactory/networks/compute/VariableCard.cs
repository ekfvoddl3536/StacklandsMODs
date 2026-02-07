// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

namespace SmartFactory;

public sealed class VariableCard : LogicBase, IComputable
{
    public int varValue;

    #region awake
    protected override void Awake()
    {
        if (Inputs == null || Inputs.Length != 1)
            Inputs = new LogicType[1] { LogicType.Any };

        Outputs = Array.Empty<LogicType>();

        base.Awake();
    }
    #endregion

    #region interface
    ComputeType IComputable.ComputeType => ComputeType.Variable;
    int IComputable.StackPop => 0;
    int IComputable.ComputeValue(in ComputeStack<int> stack) => varValue;
    #endregion

    #region override
    protected override bool CanHaveCard(CardData otherCard) => ConstantCard._CanHaveCard(otherCard);

    protected override void SetInputValue(int newInputValue) => varValue = newInputValue;
    protected override int GetNextOutputValue() => 0;
    #endregion
}
