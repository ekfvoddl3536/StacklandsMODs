// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

namespace SuperComicLib.Stacklands.Collections;

public interface IHashedEquatable<T>
{
    bool EqualsHash(T other);
}
