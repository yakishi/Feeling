using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class BattlePlayer : BattleCharacter
{
    /// <summary>
    /// 現在プレイヤーの行動選択か
    /// </summary>
    bool isPlayerAction;
    
    /// <summary>
    /// UI コントローラー的なの実装する
    /// </summary>
    public Button actionButton;

    public override void battleStart()
    {
        base.battleStart();
        actionButton.OnClickAsObservable()
            .Where(_ => isPlayerAction)
            .Subscribe(_ => {
                // 仮実装
                // 一番HPの高いキャラクターを攻撃
                var target = battleController.Monsters.MaxElement(player => player.CurrentHp);
                var skill = new Skill();

                isPlayerAction = false;
                playAction(skill.use(this, new BattleCharacter[] { target }));
            })
            .AddTo(this);
    }
    public override void startAction()
    {
        isPlayerAction = true;
    }
}
