using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class BattleMonster : BattleCharacter
{
    /// <summary>
    /// id からデータを読み込み
    /// </summary>
    /// <param name="id"></param>
    public override void loadData(string id,GameManager gameManager, bool dummy = false)
    {
        base.loadData(id,gameManager,true);
    }

    public override void startAction()
    {
        BattleUI.DisplayMonsterTurn(this.name);
        battleController.combatGrid.SetActive(false);

        // 一番HPの高いキャラクターを攻撃
        var target = battleController.Players.MaxElement(player => player.CurrentHp);
        var skill = new Skill();

        Observable.Timer(System.TimeSpan.FromMilliseconds(800.0))
            .Take(1)
            .Subscribe(_ => {
                playAction(skill.use(this, new BattleCharacter[] { target}));

            })
            .AddTo(this);

    }
}
