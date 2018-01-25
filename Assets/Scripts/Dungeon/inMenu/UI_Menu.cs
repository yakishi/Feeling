using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class UI_Menu : MonoBehaviour {
    [SerializeField]
    public GameManager gameManager;

    [SerializeField]
    public GameObject menuWindow;

    [SerializeField]
    public GameObject statusWindow;
    [SerializeField]
    public GameObject statusGrid;

    [SerializeField]
    public GameObject equipmentWindow;
    [SerializeField]
    public GameObject equipGrid;

    [SerializeField]
    public GameObject itemWindow;
    [SerializeField]
    public GameObject itemGrid;

    [System.NonSerialized]
    public SingltonPlayerManager.PlayerParameters[] playerList = new SingltonPlayerManager.PlayerParameters[4];
    private Button[] MenuItems = new Button[4];      //メニューの項目

    public List<GameObject> beforeTarget;

	// Use this for initialization
	void Start () {
        playerList[0] = gameManager.PlayerList[0];
        playerList[1] = gameManager.PlayerList[1];
        playerList[2] = gameManager.PlayerList[2];
        playerList[3] = gameManager.PlayerList[3];

        MenuItems[0] = GameObject.Find("Items").GetComponent<Button>();
        MenuItems[1] = GameObject.Find("Status").GetComponent<Button>();
        MenuItems[2] = GameObject.Find("Equipments").GetComponent<Button>();
        MenuItems[3] = GameObject.Find("BackTitle").GetComponent<Button>();

        Initialize();
        beforeTarget = new List<GameObject>();


        BattleUI.ActiveButton(menuWindow);
        BattleUI.NotActiveButton(statusGrid);
        BattleUI.NotActiveButton(equipGrid);
        BattleUI.NotActiveButton(itemGrid);
        statusWindow.SetActive(false);
        equipmentWindow.SetActive(false);
        itemWindow.SetActive(false);

    }

    void Initialize()
    {
        MenuItems[0].OnClickAsObservable()
            .Subscribe(_ => {
                itemWindow.SetActive(true);
                BattleUI.NotActiveButton(menuWindow);
                BattleUI.ActiveButton(itemGrid);

                beforeTarget.Add(MenuItems[0].gameObject);
            })
            .AddTo(this);

        MenuItems[1].OnClickAsObservable()
            .Subscribe(_ => {
                statusWindow.SetActive(true);
                BattleUI.NotActiveButton(menuWindow);
                BattleUI.ActiveButton(statusGrid);

                beforeTarget.Add(MenuItems[1].gameObject);
            })
            .AddTo(this);

        MenuItems[2].OnClickAsObservable()
            .Subscribe(_ => {
                equipmentWindow.SetActive(true);
                BattleUI.NotActiveButton(menuWindow);
                BattleUI.ActiveButton(equipGrid);

                beforeTarget.Add(MenuItems[2].gameObject);
            })
            .AddTo(this);

        MenuItems[3].OnClickAsObservable()
            .Subscribe(_ => {

            })
            .AddTo(this);
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Backspace)) {
            menuWindow.SetActive(false);
        }
	}
}
