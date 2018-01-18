using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSelect : MonoBehaviour{

    BattleUI battleUI;
    GameObject monsterSelecter;
    GameObject prefab;

    private void Start()
    {
        battleUI = GameObject.Find("Canvas").GetComponent<BattleUI>();
        prefab = battleUI.monsterSelecterPrefab;
    }

    public void Select(GameObject monster)
    {
        if (monster == null) return;

        monsterSelecter = GameObject.Instantiate(prefab,battleUI.Canvas.transform);
        monsterSelecter.transform.position = this.gameObject.transform.position + Vector3.up * 180.0f;

    }

    public void Deselect()
    {
        DestroyObject(monsterSelecter);
    }
}
