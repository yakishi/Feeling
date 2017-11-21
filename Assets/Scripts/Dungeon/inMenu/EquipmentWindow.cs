using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentWindow : MonoBehaviour {

    private int choiceElement;
    private bool changeCharaMode;
    private GameObject[] selectIcon = new GameObject[4];

    private int equipElementX,equipElementY;
    private bool equipMode;
    private GameObject[,] equipItem = new GameObject[2,3];

    private Vector3 defScale;
    private Vector3 choiceChara;
    

    private UI_Menu menu;

	// Use this for initialization
	void Start () {
        choiceElement = 0;
        changeCharaMode = true;
        equipElementX = 0;
        equipElementY = 0;
        equipMode = false;

		for(int i = 0; i< selectIcon.Length; i++) {
            selectIcon[i] = GameObject.Find("CharacterIcon" + (i + 1));
        }

        equipItem[0,0] = GameObject.Find("WeaponName");
        equipItem[1,0] = GameObject.Find("HelmName");
        equipItem[0,1] = GameObject.Find("ArmorName");
        equipItem[1,1] = GameObject.Find("BootsName");
        equipItem[0,2] = GameObject.Find("AccessoriesName1");
        equipItem[1,2] = GameObject.Find("AccessoriesName2");

        defScale = selectIcon[0].transform.localScale;
        choiceChara = defScale * 1.3f;

        menu = GameObject.Find("Menu").GetComponent<UI_Menu>();
	}
	
	// Update is called once per frame
	void Update () {
        if (changeCharaMode) ChangeCharaMode();
        else if (equipMode) EquipmentMode();
    }

    void ChangeCharaMode()
    {
        ChoiceIcon();

        if (MyInput.isButtonDown()) {
            choiceElement += (int)MyInput.direction(false).x;

            if (choiceElement < 0) {
                choiceElement = 0;
            }
            if (choiceElement >= selectIcon.Length) {
                choiceElement = selectIcon.Length - 1;
            }
        }

        if (Input.GetKeyDown(KeyCode.Return)) {
            changeCharaMode = false;
            equipMode = true;
        }

        if (Input.GetKeyDown(KeyCode.Backspace)) {
            menu.IsOpen = false;
            this.gameObject.SetActive(false);
        }
    }
    void ChoiceIcon()
    {
        selectIcon[choiceElement].transform.localScale = choiceChara;
        for (int i = 0; i < selectIcon.Length; i++) {
            if (i != choiceElement) {
                selectIcon[i].transform.localScale = defScale;
            }
        }
    }

    void EquipmentMode()
    {
        UIController.ChangeChoice(equipItem,equipElementX,equipElementY);

        if (MyInput.isButtonDown()) {
            equipElementX += (int)MyInput.direction(false).x;
            equipElementY -= (int)MyInput.direction().y;

            if (equipElementX < 0) {
                equipElementX = 0;
            }
            else if(equipElementY < 0) {
                equipElementY = 0;
            }

            if (equipElementX >= equipItem.GetLength(0)) {
                equipElementX = equipItem.GetLength(0) - 1;
            }
            else if(equipElementY >= equipItem.GetLength(1)) {
                equipElementY = equipItem.GetLength(1) - 1;
            }
        }

        if (Input.GetKeyDown(KeyCode.Backspace)) {
            equipElementX = 0;
            equipElementY = 0;
            UIController.ChoiceClear(equipItem);

            equipMode = false;
            changeCharaMode = true;
        }
    }
}
