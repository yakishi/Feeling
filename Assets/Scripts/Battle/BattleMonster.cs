using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BattleMonster : BattleCharacter
{
    /// <summary>
    /// id からデータを読み込み
    /// </summary>
    /// <param name="id"></param>
    public override void loadData(int id)
    {
        base.loadData(id);
        GetComponentInChildren<Text>().text = "Monst" + id;
    }

    public override void startAction()
    {
        // 一番HPの高いキャラクターを攻撃
        var target = battleController.Players.MaxElement(player => player.CurrentHp);
        var skill = new Skill();

        playAction(skill.use(this, new BattleCharacter[] { target }));
    }
}
