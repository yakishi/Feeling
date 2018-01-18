using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill
{
    SingltonSkillManager.SkillInfo skill;

    /// <summary>
    /// スキルの種類
    /// </summary>
    //protected TargetRange range;
    //public TargetRange Range { get { return range; } }
    public SingltonSkillManager.Category Category
    {
        set
        {
            skill.myCategory = value;
        }
    }

    /// <summary>
    /// スキル範囲
    /// </summary>
    //protected TargetType target;
    //public TargetType Target { get { return target; } }
    public SingltonSkillManager.Scope Scope
    {
        set
        {
            skill.myScope = value;
        }
    }

    /// <summary>
    /// スキル対象
    /// </summary>
    public SingltonSkillManager.Target Target
    { 
        set
        {
            skill.myTarget = value;
        }
    }

    public Skill()
    {
        skill = new SingltonSkillManager.SkillInfo();
    }

    public Skill(SingltonSkillManager.SkillInfo info)
    {
        skill = info;
    }


    public virtual List<BattleAction> use(BattleCharacter from, BattleCharacter[] targets)
    {
        var ret = new List<BattleAction>();
        // 仮で攻撃力分のHPを減らす処理を作成
        // 防御力などが入った場合ここかアクションを処理するところで行う

        Debug.Log(skill.skill + ", " + skill.myCategory.ToString() + ", " + skill.myTarget + " = " + targets[0]);
        switch (skill.myCategory) {
            case SingltonSkillManager.Category.Damage:
                ret.Add(new BattleAction()
                {
                    targets = targets,
                    effects = new Dictionary<BattleParam, int>
                    {
                        { BattleParam.HP, -from.Atk }
                    }
                });
                break;

            case SingltonSkillManager.Category.Buff:
                ret.Add(new BattleAction()
                {
                    targets = targets,
                    effects = new Dictionary<BattleParam, int>
                    {
                        {skill.influence, 5 }
                    }
                });

                foreach(var c in targets) {
                    BattleCharacter.BuffManager tempManager = new BattleCharacter.BuffManager(skill);

                    if (!c.buffList.Contains(tempManager)) {
                        c.buffList.Add(tempManager);
                    }
                    else {
                        foreach(var s in c.buffList) {
                            if(s == tempManager) {
                                s.ResetBuff();
                            }
                        }
                    }
                }
                break;
            default:
                break;
        }
        return ret;
    }
}
