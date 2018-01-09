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
    public Button[] combatButtons = new Button[6];

    public override void battleStart()
    {
        base.battleStart();
        GetComponentInChildren<Text>().text = this.name;
        attachButton();
        combatButtons[0].OnClickAsObservable()
            .Where(_ => isPlayerAction)
            .Subscribe(_ => {
                BattleUI.NotActiveButton(battleController.combatGrid);
                BattleUI.ActiveButton(battleController.monsterZone);

                battleUI.Mode = BattleUI.SelectMode.Monster;
            })
            .AddTo(this);

        combatButtons[1].OnClickAsObservable()
            .Where(_ => isPlayerAction)
            .Subscribe(_ => {
                var target = this;
                var skill = new Skill();

                isPlayerAction = false;
                playAction(skill.use(this, new BattleCharacter[] { target }, Skill.SkillType.Buff));
            })
            .AddTo(this);
    }

    public void attackAction(BattleCharacter target)
    {
        var skill = new Skill();

        Debug.Log("Hit" + target.name);

        isPlayerAction = false;
        playAction(skill.use(this, new BattleCharacter[] { target }, Skill.SkillType.Damage));
    }

    public override void startAction()
    {
        BattleUI.DisplayPlayerTurn(this.name);
        Debug.Log(this.name + " Turn");

        isPlayerAction = true;
        battleController.combatGrid.SetActive(true);
        
        BattleUI.ActiveButton(battleController.combatGrid);
        BattleUI.NotActiveButton(battleController.monsterZone);

        combatButtons[0].Select();

        if(battleUI != null)
            battleUI.Mode = BattleUI.SelectMode.Behaviour;
    }

    private void attachButton()
    {
        combatButtons[0] = GameObject.Find("AtkButton").GetComponent<Button>();          //攻撃
        combatButtons[1] = GameObject.Find("DefButton").GetComponent<Button>();          //防御
        combatButtons[2] = GameObject.Find("SkillButton").GetComponent<Button>();        //スキル
        combatButtons[3] = GameObject.Find("ItemButton").GetComponent<Button>();         //アイテム
        combatButtons[4] = GameObject.Find("ReplacementButton").GetComponent<Button>();  //入れ替え
        combatButtons[5] = GameObject.Find("EscapeButton").GetComponent<Button>();       //逃げる
    }
}
