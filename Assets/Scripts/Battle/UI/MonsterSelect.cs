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


        if(transform.childCount < 1) { 
            monsterSelecter = GameObject.Instantiate(prefab, gameObject.transform);
        }
    }

    public void Deselect()
    {
        DestroyObject(monsterSelecter);
    }
}
