using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class ItemWindow : MonoBehaviour {

    public enum SelectMode
    {
        List,
        Use
    }
    SelectMode selectMode;

    [SerializeField]
    UI_Menu menu;

    public List<BattleUI.ItemButton> itemButtonList;

    [SerializeField]
    GameObject prefab;

    //テスト用
    SingltonItemManager.ItemParam testList;

    // Use this for initialization
    void Start () {
        
    }

    private void OnEnable()
    {
        itemButtonList = new List<BattleUI.ItemButton>();
        selectMode = SelectMode.List;

        testList = new SingltonItemManager.ItemParam();
        testList.itemList = new Dictionary<string, int>();
        testList.itemList.Add("I0", 5);
        testList.itemList.Add("I1", 10);
        testList.itemList.Add("I2", 3);
        testList.itemList.Add("I3", 4);
        testList.itemList.Add("I4", 10);
        testList.itemList.Add("I5", 6);

        SetItemList();

    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Backspace)) {
            switch (selectMode) {
                case SelectMode.List:
                    BattleUI.NotActiveButton(menu.itemGrid);
                    BattleUI.ActiveButton(menu.menuWindow, menu.beforeTarget[menu.beforeTarget.Count - 1]);
                    menu.beforeTarget.RemoveAt(menu.beforeTarget.Count - 1);
                    itemButtonList.Clear();

                    menu.itemWindow.SetActive(false);
                    break;
                default:
                    break;
            }
        }
	}

    void InitializeItemList()
    {
        if (itemButtonList == null) return;

        foreach(var item in itemButtonList) {
            Button button = item.getButton;

            button.OnClickAsObservable()
                .Subscribe(_ => {
                    

                })
                .AddTo(this);
        }
    }

   void SetItemList()
    {
        Dictionary<string, int> info = new Dictionary<string, int>();

        if(testList.itemList != null) info = testList.itemList;

        foreach (string id in info.Keys) {
            foreach (var item in menu.gameManager.ItemManager.CDItem) {
                if (id == item.id) {
                    GameObject tempObject = Instantiate(prefab, menu.itemGrid.transform);
                    foreach (Transform child in tempObject.transform) {
                        if (child.name == "itemName") child.GetComponent<Text>().text = item.name;
                        if (child.name == "itemNumber") child.GetComponent<Text>().text = info[id].ToString();
                    }

                    BattleUI.ItemButton temp = new BattleUI.ItemButton(item, info[id], tempObject);
                    itemButtonList.Add(temp);
                }
            }
        }
    }
}
