using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill
{
    /// <summary>
    /// スキル範囲
    /// </summary>
    public enum TargetRange
    {
        Unit,  // 単体
        Group, // グループ
        Field  // 全体
    }

    /// <summary>
    /// スキル対象
    /// </summary>
    public enum TargetType
    {
        Enemy, // 相手を対象
        Ally,  // 味方を対象
        Self   // 自分自身
    }

    /// <summary>
    /// スキルタイプ
    /// </summary>
    public enum SkillType
    {
        Damage,
        Buff,
        Debuff
    }

    /// <summary>
    /// スキルの範囲
    /// </summary>
    protected TargetRange range;
    public TargetRange Range { get { return range; } }

    /// <summary>
    /// スキル対象
    /// </summary>
    protected TargetType target;
    public TargetType Target { get { return target; } }

    public virtual List<BattleAction> use(BattleCharacter from, BattleCharacter[] targets,SkillType type)
    {
        var ret = new List<BattleAction>();
        // 仮で攻撃力分のHPを減らす処理を作成
        // 防御力などが入った場合ここかアクションを処理するところで行う
        switch (type) {
            case SkillType.Damage:
                ret.Add(new BattleAction()
                {
                    targets = targets,
                    effects = new Dictionary<BattleParam, int>
                    {
                        { BattleParam.HP, -from.Atk }
                    }
                });
                break;

            case SkillType.Buff:
                ret.Add(new BattleAction()
                {
                    targets = targets,
                    effects = new Dictionary<BattleParam, int>
                    {
                        {BattleParam.Def, 5 }
                    }
                });
                break;
            case SkillType.Debuff:
                break;
            default:
                break;
        }
        return ret;
    }
}
