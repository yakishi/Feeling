using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class EquipmentWindow : MonoBehaviour {

    [SerializeField]
    public UI_Menu menu;

    public struct EquipmentButton
    {        
        private GameObject buttonObj;
        private Button button;
        public Button getButton
        {
            get
            {
                return button;
            }
        }
        private string id;
        public string ID
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }
        private Text buttonText;

        public EquipmentButton(GameObject obj,string setId)
        {
            buttonObj = obj;
            button = obj.GetComponent<Button>();
            id = setId;
            buttonText = buttonObj.transform.GetComponentInChildren<Text>();
        }

        public void ButtonText(string str)
        {
            buttonText.text = str;
        }
    }
    [System.NonSerialized]
    public GameManager gameManager;

    [SerializeField]
    public GameObject equipmentGrid;
    [SerializeField]
    public GameObject equipmentListGrid;

    private Button[] selectIcon = new Button[4];
    public EquipmentButton[] equipItem = new EquipmentButton[6];

    public enum SelectMode
    {
        Character,
        Equipment,
        List
    }

    SelectMode selectMode;

    private void Awake()
    {
        
    }

    // Use this for initialization
    void OnEnable () {

        BattleUI.NotActiveButton(equipmentGrid);
        BattleUI.NotActiveButton(equipmentListGrid);

        if (gameManager == null) gameManager = menu.gameManager;

        selectMode = SelectMode.Character;

        for(int i = 0;i < selectIcon.Length; i++) {
            selectIcon[i] = GameObject.Find("playerIcon" + (i + 1)).GetComponent<Button>();
        }

        equipItem[0] = new EquipmentButton(GameObject.Find("Weapon"), "");
        equipItem[1] = new EquipmentButton(GameObject.Find("Helm"), "");
        equipItem[2] = new EquipmentButton(GameObject.Find("Armor"), "");
        equipItem[3] = new EquipmentButton(GameObject.Find("Boots"), "");
        equipItem[4] = new EquipmentButton(GameObject.Find("Accessories1"), "");
        equipItem[5] = new EquipmentButton(GameObject.Find("Accessories2"), "");

        InitializeButton();

    }

    void InitializeButton()
    {
        if (selectIcon == null || equipItem == null) return;
        foreach (Button b in selectIcon) {
            b.OnClickAsObservable()
            .Subscribe(_ => {
                BattleUI.ActiveButton(equipmentGrid);
            BattleUI.NotActiveButton(menu.equipGrid);

                selectMode = SelectMode.Equipment;
                menu.beforeTarget.Add(b.gameObject);
            })
            .AddTo(this);
        }

        foreach(var eb in equipItem) {
            Button b = eb.getButton;

            b.OnClickAsObservable()
            .Subscribe(_ => {
                BattleUI.ActiveButton(equipmentListGrid);
                BattleUI.NotActiveButton(equipmentGrid);

                selectMode = SelectMode.List;
                menu.beforeTarget.Add(b.gameObject);
            })
            .AddTo(this);
        }
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Backspace)) {

            switch (selectMode) {
                case SelectMode.Equipment:
                    BattleUI.ActiveButton(menu.equipGrid, menu.beforeTarget[menu.beforeTarget.Count - 1]);
                    BattleUI.NotActiveButton(equipmentGrid);
                    menu.beforeTarget.RemoveAt(menu.beforeTarget.Count - 1);
                    break;
                case SelectMode.List:
                    BattleUI.ActiveButton(menu.equipGrid, menu.beforeTarget[menu.beforeTarget.Count - 1]);
                    BattleUI.NotActiveButton(equipmentGrid);
                    menu.beforeTarget.RemoveAt(menu.beforeTarget.Count - 1);
                    break;
                case SelectMode.Character:
                    BattleUI.ActiveButton(menu.menuWindow, menu.beforeTarget[menu.beforeTarget.Count - 1]);
                    BattleUI.NotActiveButton(menu.equipGrid);
                    menu.beforeTarget.RemoveAt(menu.beforeTarget.Count - 1);
                    break;
                default:
                    break;
                    
            }
        }
    }
}
