using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class BattleUI : MonoBehaviour {

    public enum SelectMode
    {
        None,
        Behaviour,
        Monster
    }

    static private GameObject turnDisplay;
    List<BattleCharacter> characters;
    [SerializeField]
    BattleController battleController;
    BattleCharacter[] player;
    BattleCharacter[] monsters;
    BattleCharacter currentCharacter;
    public BattleCharacter target;

    [SerializeField]
    public GameObject monsterSelecter;

    /// <summary>
    /// プレイヤーのHPバー
    /// </summary>
    Slider[] playerHP;

    [SerializeField]
    Text turnText;
    static private string currentCharacterName;

    private bool isSelect;
    public bool IsSelect
    {
        get
        {
            return isSelect;
        }
    }

    private SelectMode selectMode;
    public SelectMode Mode
    {
        get
        {
            return selectMode;
        }

        set
        {
            selectMode = value;
        }
    }

    // Use this for initialization
    void Start () {
        selectMode = SelectMode.None;
        characters = battleController.Characters;

        foreach(var character in characters) {
            character.setBattleUI(this);
        }

        player = battleController.Players;
        monsters = battleController.Monsters;
        playerHP = new Slider[player.Length];
        for(int i = 0; i < player.Length; i++) {
            playerHP[i] = player[i].gameObject.GetComponent<Slider>();
            playerHP[i].enabled = false;
        }

        isSelect = false;
        monsterSelecter.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        ChangeHPBar();
        currentCharacter = battleController.CurrentActionCharacter;

        EndDisplay();

        foreach(var n in monsters) {
            var button = n.GetComponent<Button>();
            
            button.OnClickAsObservable()
                .Subscribe(_ => {
                    target = n;
                    isSelect = true;
                })
                .AddTo(this);
        }

        if (isSelect) {
            isSelect = false;
            monsterSelecter.SetActive(false);
            currentCharacter.GetComponent<BattlePlayer>().attackAction(target);
        }

        if (currentCharacterName == null) return;
        turnText.text = currentCharacterName;
    }

    /// <summary>
    /// ターン表記（モンスター)
    /// </summary>
    /// <param name="name"> モンスター名</param>
    static public void DisplayMonsterTurn(string name)
    {
        if(turnDisplay == null) turnDisplay = GameObject.Find("Turn");
        turnDisplay.SetActive(true);
        currentCharacterName = name + "のターン";
    }

    /// <summary>
    /// ターン表記（プレイヤー)
    /// </summary>
    /// <param name="name">プレイヤー名</param>
    static public void DisplayPlayerTurn(string name)
    {
        if(turnDisplay == null) turnDisplay = GameObject.Find("Turn");
        turnDisplay.SetActive(false);
    }

    private void EndDisplay()
    {
        if (battleController.DeadType == BattleController.GroupDeadType.AllEnemyDead) {
            turnDisplay.SetActive(true);
            turnDisplay.GetComponent<Text>().text = "敵を殲滅しました";
        }
        else if(battleController.DeadType == BattleController.GroupDeadType.AllPlayerDead) {
            turnDisplay.SetActive(true);
            turnDisplay.GetComponent<Text>().text = "全滅しました";

        }
    }

    private void ChangeHPBar()
    {
        for (int i = 0; i < player.Length; i++) {
            playerHP[i].value = player[i].CurrentHp;
        }
    }

    static public void ActiveButton(GameObject grid)
    {
        for(var i = 0; i < grid.transform.childCount; i++) {
		    grid.transform.GetChild(i).GetComponent<Button>().enabled = true;
	    }

        for (var i = 0; i < grid.transform.childCount; i++) {

            if (grid.transform.GetChild(i).gameObject.activeSelf == true) {
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(grid.transform.GetChild(i).gameObject);
                break;
            }
        }
    }

    static public void NotActiveButton(GameObject grid)
    {
        for (var i = 0; i < grid.transform.childCount; i++) {
            grid.transform.GetChild(i).GetComponent<Button>().enabled = false;
        }
    }

}
