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
                battleUI.beforeGrid = battleController.combatGrid;

                battleUI.Mode = BattleUI.SelectMode.Monster;
            })
            .AddTo(this);

        combatButtons[1].OnClickAsObservable()
            .Where(_ => isPlayerAction)
            .Subscribe(_ => {
                BattleCharacter targets = this;
                var skill = new Skill();

                isPlayerAction = false;
                playAction(skill.use(this, new BattleCharacter[] { targets }));
            })
            .AddTo(this);

        combatButtons[2].OnClickAsObservable()
            .Where(_ => isPlayerAction)
            .Subscribe(_ =>{

                if (battleUI.skillButtonList.Count == 0) {
                    foreach (var i in skillList) {
                        battleUI.SetSkill(i);
                    }
                }

                battleUI.SkillWindow.SetActive(true);
                battleUI.SkillDetail.SetActive(true);
                battleUI.SkillMode();

                BattleUI.NotActiveButton(battleController.combatGrid);
                BattleUI.ActiveButton(battleUI.SkillWindow);
                battleUI.Mode = BattleUI.SelectMode.Skill;
            })
            .AddTo(this);

        combatButtons[5].OnClickAsObservable()
            .Where(_ => isPlayerAction)
            .Subscribe(_ => {
                Application.Quit();
            })
            .AddTo(this);
    }

    public void attackAction(BattleCharacter[] targets, string id = "none")
    {
        var skill = new Skill();
        if (id == "none") {
            skill.Target = SingltonSkillManager.Target.Enemy;
            skill.Scope = SingltonSkillManager.Scope.Simplex;
            skill.Category = SingltonSkillManager.Category.Damage;
        }
        else{
            skill = searchSkill(id);
        }
        isPlayerAction = false;

        playAction(skill.use(this, targets));
    }

    Skill searchSkill(string id)
    {
        SingltonSkillManager.SkillInfo skill = new SingltonSkillManager.SkillInfo();

        foreach(var s in battleController.SkillList) {
            if(s.ID == id) {
                skill = s;
            }
        }

        return new Skill(skill);
    }

    public override void startAction()
    {
        BattleUI.DisplayPlayerTurn(this.name);

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
