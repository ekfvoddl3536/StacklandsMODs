// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

namespace SmartFactory;

public interface IComputable
{
    ComputeType ComputeType { get; }
    int StackPop { get; }
    int ComputeValue(in ComputeStack<int> stack);
}
