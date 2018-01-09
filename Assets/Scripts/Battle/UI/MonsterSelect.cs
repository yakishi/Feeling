using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSelect : MonoBehaviour{

    BattleUI battleUI;
    GameObject monsterSelecter;

    private void Start()
    {
        battleUI = GameObject.Find("Canvas").GetComponent<BattleUI>();
        monsterSelecter = battleUI.monsterSelecter;
    }

    public void Select(GameObject monster)
    {
        if (monster == null) return;

        monsterSelecter.SetActive(true);

        monsterSelecter.transform.position = this.gameObject.transform.position + Vector3.up * 180.0f;

    }

    public void Deselect()
    {
        monsterSelecter.SetActive(false);
    }
}
