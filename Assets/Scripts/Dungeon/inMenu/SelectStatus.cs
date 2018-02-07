using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectStatus : MonoBehaviour
{

    //表示部分
    private Image CharaImg;
    private Text NAME;
    private Text Lv;
    private Slider HP;
    private Slider MP;
    private Text TotalEXP;
    private Text ATK;
    private Text MATK;
    private Text DEF;
    private Text MDEF;
    private Text AGI;
    private Text LUK;

    Vector3 defScale;

    [SerializeField]
    StatusWindow statusWindow;

    [SerializeField]
    Sprite charaImg;

    // Use this for initialization
    void Awake()
    {
        defScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

    private void OnEnable()
    {
        CharaImg = GameObject.Find("Character").GetComponent<Image>();
        NAME = GameObject.Find("CharacterName").GetComponent<Text>();
        Lv = NAME.transform.Find("Level").GetComponent<Text>();
        HP = GameObject.Find("HPBar").GetComponent<Slider>();
        MP = GameObject.Find("MPBar").GetComponent<Slider>();
        TotalEXP = GameObject.Find("Value").GetComponent<Text>();
        ATK = GameObject.Find("ATKValue").GetComponent<Text>();
        MATK = GameObject.Find("MATKValue").GetComponent<Text>();
        DEF = GameObject.Find("DEFValue").GetComponent<Text>();
        MDEF = GameObject.Find("MDEFValue").GetComponent<Text>();
        AGI = GameObject.Find("AGIValue").GetComponent<Text>();
        LUK = GameObject.Find("LUKValue").GetComponent<Text>();

        defScale = new Vector3(1.0f,1.0f,1.0f);
    }

    public void Select(int i)
    {
        SingltonPlayerManager.PlayerParameters player = statusWindow.menu.playerList[i];

        UIController.DisplayImage(CharaImg, charaImg);
        UIController.DisplayText(NAME, player.Name);
        UIController.DisplayText(Lv, "Lv" + player.Lv);
        UIController.DisplaySlider(HP, player.STATUS.HP);
        UIController.DisplaySlider(MP, player.STATUS.MP);
        UIController.DisplayText(TotalEXP, player.TotalExp);
        UIController.DisplayText(ATK, player.STATUS.Atk);
        UIController.DisplayText(MATK, player.STATUS.Matk);
        UIController.DisplayText(DEF, player.STATUS.Def);
        UIController.DisplayText(MDEF, player.STATUS.Mgr);
        UIController.DisplayText(AGI, player.STATUS.Agl);
        UIController.DisplayText(LUK, player.STATUS.Luc);

        Vector3 scopeScale = new Vector3(this.gameObject.transform.localScale.x * 1.2f, this.gameObject.transform.localScale.y * 1.2f, 1.0f);
        
        this.gameObject.transform.localScale = scopeScale;

    }

    public void DeSelect()
    {
        this.gameObject.transform.localScale = defScale;
    }
    
}
