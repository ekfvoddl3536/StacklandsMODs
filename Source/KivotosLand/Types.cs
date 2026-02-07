// The MIT License (MIT)
// Copyright (c) 2023-2026. Super Comic (ekfvoddl3535@naver.com)

using UnityEngine;

namespace KivotosLand;

public delegate void PerformSpecialHitDelegate(Combatable @this, SpecialHit hit, Combatable target, int dmg);
public delegate void ShowHitTextDelegate(Combatable @this, Combatable origin, Combatable effectTarget, Vector3 targetPosition, bool isHit, int damage, SpecialHitType? type);

public delegate void UpdateTabsDelegate(CardopediaScreen @this);
