// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

using SuperComicLib.Stacklands.Collections;

namespace SmartFactory;

public abstract partial class LogicBase : CardData, ILogicGate, IHashedEquatable<LogicBase>, IJsonSaveLoadable
{
    public LogicType[] Inputs;
    public LogicType[] Outputs;

    public LogicBase[] InputNodes;
    public LogicBase[] OutputNodes;

    // use card texture rectangle background?
    //  FALSE = default, circle background
    //  TRUE  = rectangle background
    public bool RectBackground;

    internal int _inputConnections;
    internal int _hasInputValueCount;

    [ExtraData("exdata_json")]
    public string m_saveDataJson;
}
