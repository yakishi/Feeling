using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectDungeon : MonoBehaviour{

    WorldMapUI worldMap;

	// Use this for initialization
	void Start () {
        worldMap = GameObject.Find("Canvas").GetComponent<WorldMapUI>();
	}

    public void Select(int dungeonNumber)
    {
        worldMap.EnlargementDungeon(dungeonNumber);

    }

    public void Select(GameObject button)
    {
        SelectText.Select(button);
    }


    public void DeSelect(int dungeonNumber)
    {
        worldMap.ReductionDungeon(dungeonNumber);

    }

    public void DeSelect(GameObject button)
    {
        SelectText.DeSelect(button);
    }
}
