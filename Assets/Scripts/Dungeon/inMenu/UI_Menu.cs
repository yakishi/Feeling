using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Menu : MonoBehaviour {

    static GameObject[] MenuItems = new GameObject[5];      //メニューの項目
    static GameObject[] ItemWindows = new GameObject[5];    //項目ごとの開くべきウィンドウ
    int choiceElement;

    //ほかウィンドウが開いているか
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
        MenuItems[0] = GameObject.Find("Items");
        MenuItems[1] = GameObject.Find("Status");
        MenuItems[2] = GameObject.Find("Equipments");
        MenuItems[3] = GameObject.Find("MonsterList");
        MenuItems[4] = GameObject.Find("ItemList");

    }

    void Awake()
    {
        ItemWindows[1] = GameObject.Find("StatusWindow");
        ItemWindows[1].SetActive(false);
        ItemWindows[2] = GameObject.Find("EquipmentWindow");
        ItemWindows[2].SetActive(false);

    }

    void OnEnable()
    {
        choiceElement = 0;
    }

    // Update is called once per frame
    void Update () {

        if (isOpen)
            return;

        UIController.ChangeChoice(MenuItems,choiceElement);

        if (MyInput.isButtonDown()) {
            choiceElement -= (int)MyInput.direction().y;

            if(choiceElement < 0) {
                choiceElement = 0;
            }
            if(choiceElement >= MenuItems.Length) {
                choiceElement = MenuItems.Length - 1;
            }
        }

        if(Input.GetKeyDown(KeyCode.Return) && ItemWindows[choiceElement] != null) {
            ItemWindows[choiceElement].SetActive(true);

            isOpen = true;
        }

        if(Input.GetKeyDown(KeyCode.Backspace)) {
            this.gameObject.SetActive(false);
        }
	}
}
