// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

#pragma warning disable IDE1006
namespace SmartFactory;

partial class LogicBase
{
    protected override void Awake()
    {
        if (_EnsureNodeSizes(Inputs, ref InputNodes))
            OnReadyInputs();

        if (_EnsureNodeSizes(Outputs, ref OutputNodes))
            OnReadyOutputs();

        Inputs ??= Array.Empty<LogicType>();
        Outputs ??= Array.Empty<LogicType>();

        InputNodes ??= Array.Empty<LogicBase>();
        OutputNodes ??= Array.Empty<LogicBase>();

        LogicNetworkManager.NextVersion();
    }

    private static bool _EnsureNodeSizes(LogicType[] types, ref LogicBase[] nodes)
    {
        if (types != null && types.Length > 0)
        {
            // reset connected info
            _ResetConnect(types);

            var vs = nodes;
            if (vs == null || vs.Length != types.Length)
                nodes = new LogicBase[types.Length];

            // okay ready.
            return true;
        }
        
        // no
        return false;
    }

    /// <summary>
    /// 이 논리 카드에는 입력이 하나 이상 존재합니다. 필요한 초기화를 수행하세요.
    /// </summary>
    protected virtual void OnReadyInputs() { }
    /// <summary>
    /// 이 논리 카드에는 출력이 하나 이상 존재합니다. 필요한 초기화를 수행하세요.
    /// </summary>
    protected virtual void OnReadyOutputs() { }
}
