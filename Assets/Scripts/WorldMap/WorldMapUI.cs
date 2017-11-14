using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapUI : MonoBehaviour {

    private GameObject[] dungeons = new GameObject[4];
    private GameObject[] dungeonDetail = new GameObject[4];
    private GameObject[] dungeonList = new GameObject[4];

    private GameObject choiceMarker;

    int choiceElement;
    private bool isOpen;

    public bool IsOpen
    {
        get
        {
            return isOpen;
        }
        set
        {
            isOpen = value;
        }
    }

    // Use this for initialization
    void Start () {
		for(int i = 0;i < dungeons.Length;i++) {
            dungeons[i] = GameObject.Find("Dungeon" + (i + 1));
            dungeonList[i] = GameObject.Find("DungeonList" + (i + 1));
        }

        

        choiceMarker = GameObject.Find("choice");

        choiceElement = 0;
        isOpen = false;

        choiceMarker.transform.position = dungeons[choiceElement].transform.position;
	}

    // Update is called once per frame
    void Update () {

        UIController.ChangeChoice(dungeonList, choiceElement);

        if (MyInput.isButtonDown()) {
            choiceElement -= (int)MyInput.direction().y;

            if (choiceElement < 0) {
                choiceElement = 0;
            }
            if (choiceElement >= dungeonList.Length) {
                choiceElement = dungeonList.Length - 1;
            }
        }

        if (Input.GetKeyDown(KeyCode.Return) && ItemWindows[choiceElement] != null) {
            
            isOpen = true;
        }

        choiceMap();
    }

    void choiceMap()
    {
        choiceMarker.transform.position = dungeons[choiceElement].transform.position;
    }
}
