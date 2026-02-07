// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

namespace SmartFactory;

partial class LogicBase 
{
    public bool CanInputsConnect => _CanConnect(Inputs);
    public bool CanOutputsConnect => _CanConnect(Outputs);

    public int InputConnections => _GetConnections(Inputs);
    public int OutputConnections => _GetConnections(Outputs);

    public bool HasInputNode(LogicBase input) => _HasNode(InputNodes, input);
    public bool HasOutputNode(LogicBase output) => _HasNode(OutputNodes, output);
}
