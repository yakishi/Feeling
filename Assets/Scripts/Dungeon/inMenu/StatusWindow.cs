using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusWindow : MonoBehaviour {

    [SerializeField]
    private PlayerManagerCSV playerManager;
    private PlayerManagerCSV.PlayerParameters[] playerList = new PlayerManagerCSV.PlayerParameters[4];    //  キャラクター情報
    public PlayerManagerCSV.PlayerParameters getPlayerParam(int choiceElement)
    {
        if (choiceElement > playerList.Length) choiceElement = playerList.Length;

        return playerList[choiceElement];
    }

    //表示部分
    private Slider HP;
    private Slider MP;
    private Slider EXP;
    private Text TotalEXP;
    private Text ATK;
    private Text MATK;
    private Text DEF;
    private Text MDEF;
    private Text AGI;
    private Text LUK;

    int choiceElement;

    private GameObject[] selectIcon = new GameObject[4];        //キャラクター切り替えオブジェクト
    private Vector3 defScale;

    private UI_Menu menu;

    // Use this for initialization
    void Start ()
    {
        playerList[0] = playerManager.Player1;
        playerList[1] = playerManager.Player2;
        playerList[2] = playerManager.Player3;
        playerList[3] = playerManager.Player4;

        HP       = GameObject.Find("HPBar").GetComponent<Slider>();
        MP       = GameObject.Find("MPBar").GetComponent<Slider>();
        EXP      = GameObject.Find("EXPBar").GetComponent<Slider>();
        TotalEXP = GameObject.Find("Value").GetComponent<Text>();
        ATK      = GameObject.Find("ATKValue").GetComponent<Text>();
        MATK     = GameObject.Find("MATKValue").GetComponent<Text>();
        DEF      = GameObject.Find("DEFValue").GetComponent<Text>();
        MDEF     = GameObject.Find("MDEFValue").GetComponent<Text>();
        AGI      = GameObject.Find("AGIValue").GetComponent<Text>();
        LUK      = GameObject.Find("LUKValue").GetComponent<Text>();

        choiceElement = 0;

        for(int i = 0; i < selectIcon.Length; i++) {
            selectIcon[i] = GameObject.Find("playerIcon" + (i + 1));
        }

        defScale = selectIcon[0].transform.localScale;

        menu = GameObject.Find("Menu").GetComponent<UI_Menu>();
    }

    // Update is called once per frame
    void Update ()
    {
        SelectPlayer(playerList[choiceElement]);

        if (MyInput.isButtonDown()) {
            choiceElement += (int)MyInput.direction(false).x;

            if (choiceElement < 0) {
                choiceElement = 0;
            }
            if (choiceElement >= playerList.Length) {
                choiceElement = playerList.Length - 1;
            }
        }

        if(Input.GetKeyDown(KeyCode.Backspace)) {
            menu.IsOpen = false;
            this.gameObject.SetActive(false);
        }
    }

    void SelectPlayer(PlayerManagerCSV.PlayerParameters player)
    {
        //DisplaySlider(HP, player.HP);
        //DisplaySlider(MP, player.MP);
        //DisplaySlider(MP, player.MP);
        //DisplayText(TotalEXP, player.TotalEXP)
        DisplayText(ATK, player.ATK);
        DisplayText(MATK, player.MATK);
        DisplayText(DEF, player.DEF);
        DisplayText(MDEF, player.MDEF);
        DisplayText(AGI, player.SPD);
        DisplayText(LUK, player.LUCKY);

        selectIcon[choiceElement].transform.localScale = new Vector3(1.2f, 1.2f, 1.0f);

        for (int i = 0; i < selectIcon.Length; i++){
            if (i != choiceElement) {
                selectIcon[i].transform.localScale = defScale;
            }
        }
    }

    void DisplayText(Text item, int value)
    {
        item.text =value.ToString();
    }
    
    void DisplaySlider(Slider item, int value)
    {
        
    }
}
