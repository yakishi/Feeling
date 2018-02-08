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


    // Use this for initialization
    void Start () {
        
    }

    private void OnEnable()
    {
        itemButtonList = new List<BattleUI.ItemButton>();
        selectMode = SelectMode.List;

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

        info = GV.Instance.GData.Items.itemList;

        foreach (string id in info.Keys) {
            foreach (var item in menu.gameManager.ItemManager.CDItem) {
                if (id == item.id) {
                    GameObject tempObject = Instantiate(prefab, menu.itemGrid.transform);
                    foreach (Transform child in tempObject.transform) {
                        if (child.name == "itemName") child.GetComponent<Text>().text = item.name;
                        if (child.name == "itemNumber") child.GetComponent<Text>().text = info[id].ToString();
                    }

                    BattleUI.ItemButton temp = new BattleUI.ItemButton(item, tempObject);
                    itemButtonList.Add(temp);
                }
            }
        }
    }
}
