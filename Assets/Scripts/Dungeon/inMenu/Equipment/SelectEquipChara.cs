using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectEquipChara : MonoBehaviour {

    string playerID;

    [SerializeField]
    EquipmentWindow equipWindow;
    [SerializeField]
    Sprite charaImg;

    Vector3 defScale;

    //表示部分
    private Image CharaImg;
    private Text NAME;
    private Text HP;
    private Slider HPBar;
    private Text MP;
    private Slider MPBar;
    private Text ATK;
    private Text MATK;
    private Text DEF;
    private Text MDEF;
    private Text AGI;
    private Text LUK;

    // Use this for initialization
    void OnEnable () {
        playerID = "";

        defScale = new Vector3(1.0f, 1.0f, 1.0f);

        CharaImg = GameObject.Find("Icon").GetComponent<Image>();
        NAME = GameObject.Find("CharacterName").GetComponent<Text>();
        HP = GameObject.Find("HPvalue").GetComponent<Text>();
        HPBar = GameObject.Find("HPBar").GetComponent<Slider>();
        MP = GameObject.Find("MPvalue").GetComponent<Text>();
        MPBar = GameObject.Find("MPBar").GetComponent<Slider>();
        ATK = GameObject.Find("ATKvalue").GetComponent<Text>();
        MATK = GameObject.Find("MATKvalue").GetComponent<Text>();
        DEF = GameObject.Find("DEFvalue").GetComponent<Text>();
        MDEF = GameObject.Find("MDEFvalue").GetComponent<Text>();
        AGI = GameObject.Find("AGIvalue").GetComponent<Text>();
        LUK = GameObject.Find("LUKvalue").GetComponent<Text>();
    }
	
	public void Select(int playerNumber)
    {
        Vector3 scopeScale = new Vector3(this.gameObject.transform.localScale.x * 1.2f, this.gameObject.transform.localScale.y * 1.2f, 1.0f);

        this.gameObject.transform.localScale = scopeScale;

        if (equipWindow.gameManager == null) return;

        if (equipWindow.menu.playerList[playerNumber] == null) return;
        SingltonPlayerManager.PlayerParameters player = equipWindow.menu.playerList[playerNumber];

        playerID = player.ID;

        foreach(var p in equipWindow.gameManager.EquipmentManager.SaveDataPlayerEquipmentParam) {
            if(p.ID == playerID) {
                SetEquipInfo(p);
            }
        }

        UIController.DisplayImage(CharaImg, charaImg);
        UIController.DisplayText(NAME, player.Name);
        UIController.DisplayText(HP, player.STATUS.HP);
        UIController.DisplaySlider(HPBar, player.STATUS.HP);
        UIController.DisplayText(MP, player.STATUS.MP);
        UIController.DisplaySlider(MPBar, player.STATUS.MP);
        UIController.DisplayText(ATK, player.STATUS.Atk);
        UIController.DisplayText(MATK, player.STATUS.Matk);
        UIController.DisplayText(DEF, player.STATUS.Def);
        UIController.DisplayText(MDEF, player.STATUS.Mgr);
        UIController.DisplayText(AGI, player.STATUS.Agl);
        UIController.DisplayText(LUK, player.STATUS.Luc);
    }

    void SetEquipInfo(SingltonEquipmentManager.PlayerEquipmentParam info)
    {
        foreach(var e in equipWindow.gameManager.EquipmentManager.GetCsvDataPlayerState) {
            if(info.Arms == e.Name) {
                equipWindow.equipItem[0].ID = e.ID;
                equipWindow.equipItem[0].ButtonText(e.Name);
            }
            
            else if (info.Head == e.Name) {
                equipWindow.equipItem[1].ID = e.ID;
                equipWindow.equipItem[1].ButtonText(e.Name);
            }
            else if (info.Armor == e.Name) {
                equipWindow.equipItem[2].ID = e.ID;
                equipWindow.equipItem[2].ButtonText(e.Name);
            }
            else if (info.Shoes == e.Name) {
                equipWindow.equipItem[3].ID = e.ID;
                equipWindow.equipItem[3].ButtonText(e.Name);
            }
            else if (info.Accessory1 == e.Name) {
                equipWindow.equipItem[4].ID = e.ID;
                equipWindow.equipItem[4].ButtonText(e.Name);
            }
            else if (info.Accessory2 == e.Name) {
                equipWindow.equipItem[5].ID = e.ID;
                equipWindow.equipItem[5].ButtonText(e.Name);
            }
        }
    }

    public void DeSelect()
    {
        transform.localScale = defScale;
    }
}
