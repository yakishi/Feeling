using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectableText : MonoBehaviour {

    BattleUI battleUI;

	// Use this for initialization
	void Awake () {
        battleUI = GameObject.Find("Canvas").GetComponent<BattleUI>();
    }

    public void Select(GameObject button)
    {
        button.GetComponent<Text>().color = Color.red;
        if (battleUI == null) return;
        foreach(var skill in battleUI.skillButtonList) { 
            var name = button.GetComponentInChildren<Text>().text;
            if (name == skill.SkillInfo.skill) {
                battleUI.skillDetailText.GetComponent<Text>().text =
                    "消費MP :" + skill.SkillInfo.MP +
                    ", 種類 :" + Category(skill.SkillInfo.myCategory) +
                    ", 対象 :" + Target(skill.SkillInfo.myTarget) +
                    ", 範囲 :" + Scope(skill.SkillInfo.myScope);
            }
        }

    }

    public void DeSelect(GameObject button)
    {
        button.GetComponent<Text>().color = Color.white;
    }

    string Category(SingltonSkillManager.Category category)
    {
        string temp = "none";
        switch (category) {
            case SingltonSkillManager.Category.Damage:
                temp = "攻撃";
                break;
            case SingltonSkillManager.Category.Buff:
                temp = "バフ";
                break;
            default:
                break;
        }

        return temp;
    }

    string Scope(SingltonSkillManager.Scope scope)
    {
        string temp = "none";
        switch (scope) {
            case SingltonSkillManager.Scope.OverAll:
                temp = "全体";
                break;
            case SingltonSkillManager.Scope.Simplex:
                temp = "単体";
                break;
            default:
                break;
        }

        return temp;
    }

    string Target(SingltonSkillManager.Target target)
    {
        string temp = "none";
        switch (target) {
            case SingltonSkillManager.Target.Enemy:
                temp = "敵";
                break;
            case SingltonSkillManager.Target.MySelf:
                temp = "自身";
                break;
            case SingltonSkillManager.Target.Supporter:
                temp = "味方";
                break;
            default:
                break;
        }

        return temp;
    }
}
