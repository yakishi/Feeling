using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapUI : MonoBehaviour {

    private GameObject[] dungeons = new GameObject[6];
    private GameObject[] dungeonList = new GameObject[6];
    //private GameObject[] dungeonDetail = new GameObject[6];
    private GameObject dungeonDetail;
    private GameObject[] goOrBack = new GameObject[2];

    private Vector3 defScale;
    private Vector3 choiceMap;

    int goBack;
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

        dungeonDetail = GameObject.Find("Detail");

        goOrBack[0] = GameObject.Find("Go");
        goOrBack[1] = GameObject.Find("Back");

        defScale = dungeons[0].transform.localScale;
        choiceMap = defScale * 1.4f;

        choiceElement = 0;
        goBack = 0;
        isOpen = false;

        dungeonDetail.SetActive(false);
	}

    // Update is called once per frame
    void Update () {
        if (isOpen) {
            Detail();
            return;
        }
        
        
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

        if (Input.GetKeyDown(KeyCode.Return)) {
            dungeonDetail.SetActive(true);

            isOpen = true;
        }

        ChoiceMap();
    }

    void Detail()
    {
        UIController.ChangeChoice(goOrBack, goBack);

        if (MyInput.isButtonDown()) {
            goBack += (int)MyInput.direction(false).x;

            if (goBack < 0) {
                goBack = 0;
            }
            if (goBack >= goOrBack.Length) {
                goBack = goOrBack.Length - 1;
            }
        }

        if (Input.GetKeyDown(KeyCode.Return)) {

            if (goBack == 1) {
                dungeonDetail.SetActive(false);
                isOpen = false;
                goBack = 0;
            }
        }
    }

    void ChoiceMap()
    {
        dungeons[choiceElement].transform.localScale = choiceMap;

        for(int i = 0; i < dungeons.Length; i++) {
            if(i != choiceElement) {
                dungeons[i].transform.localScale = defScale;
            }
        }
    }

}
