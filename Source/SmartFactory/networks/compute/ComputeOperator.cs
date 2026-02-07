// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

using UnityEngine;
using UnityEngine.InputSystem;

namespace SmartFactory;

public class ComputeOperator : CardData, IComputable, IJsonSaveLoadable
{
    public OperatorType opType;

    [ExtraData("operatorType")]
    public int m_saveData_opType;

    protected override void Awake()
    {
        // random select
        opType = (OperatorType)UnityEngine.Random.Range(0, (int)OperatorType.Abs + 1);

        UpdateDisplayName();

        base.Awake();
    }

    protected void UpdateDisplayName()
    {
        nameOverride =
            (uint)opType <= (uint)OperatorType.Abs
            ? OPNameTable.Names[(int)opType]
            : GetNameFallback(opType);
    }

    public override void Clicked()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null)
            return;

        if (keyboard.ctrlKey.isPressed)
        {
            if (keyboard.shiftKey.isPressed)
                opType = GetNextOpType(opType, 5);
            else
                opType = GetNextOpType(opType);

            UpdateDisplayName();
        }
    }

    protected virtual OperatorType GetNextOpType(OperatorType old, int shift = 1) =>
        old + shift <= OperatorType.Abs
        ? (old + shift)
        : OperatorType.Add;

    protected override bool CanHaveCard(CardData otherCard) => ConstantCard._CanHaveCard(otherCard);
    protected virtual string GetNameFallback(OperatorType t) => "NOP";

    #region interface (+method)
    ComputeType IComputable.ComputeType => ComputeType.Constant;
    int IComputable.StackPop => GetStackPopCount(opType);
    int IComputable.ComputeValue(in ComputeStack<int> stack) => ComputeValue(in stack, opType);

    protected virtual int GetStackPopCount(OperatorType t) =>
        (uint)t < (uint)OperatorType.Not
        ? 2
        : 1;

    protected virtual int ComputeValue(in ComputeStack<int> stack, OperatorType t)
    {
        int y =
            (uint)t < (uint)OperatorType.Not
            ? stack.Pop()
            : 0;
        int x = stack.Pop();

        return t switch
        {
            // def
            OperatorType.Add => x + y,
            OperatorType.Sub => x - y,
            OperatorType.Mul => x * y,
            OperatorType.Div => x / y,
            OperatorType.Mod => x % y,

            // bit
            OperatorType.And => x & y,
            OperatorType.Or => x | y,
            OperatorType.Xor => x ^ y,

            // compare
            OperatorType.CmpEQ => ToInt(x == y),
            OperatorType.CmpNE => ToInt(x != y),
            OperatorType.CmpLE => ToInt(x <= y),
            OperatorType.CmpGE => ToInt(x >= y),
            OperatorType.CmpLT => ToInt(x < y),
            OperatorType.CmpGT => ToInt(x > y),

            // unary
            OperatorType.Not => ~x,
            OperatorType.Neg => -x,
            OperatorType.Abs => Mathf.Abs(x),

            // NOP
            _ => 0
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected static int ToInt(bool v) => v ? 1 : 0;

    void IJsonSaveLoadable.OnSave()
    {
        m_saveData_opType = (int)opType;
    }

    void IJsonSaveLoadable.OnLoad()
    {
        var v = (OperatorType)m_saveData_opType;
        if (v != opType)
        {
            opType = v;
            UpdateDisplayName();
        }
    }
    #endregion
}
