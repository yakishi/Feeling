using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// バトルで使用するパラメータ
/// </summary>
public enum BattleParam
{
    HP,
    MP,
    Atk,
    Def,
    Agl,
}

public class BattleAction
{
    public BattleCharacter[] targets;
    public Dictionary<BattleParam, int> effects;
}
