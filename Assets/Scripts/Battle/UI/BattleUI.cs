using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class BattleUI : MonoBehaviour
{

    public enum SelectMode
    {
        None,
        Behaviour,
        Skill,
        Item,
        Monster,
        Supporter
    }

    [SerializeField]
    GameObject canvas;
    public GameObject Canvas
    {
        get
        {
            return canvas;
        }
    }

    static private GameObject turnDisplay;
    List<BattleCharacter> characters;
    [SerializeField]
    BattleController battleController;
    BattleCharacter[] players;
    BattleCharacter[] monsters;
    BattleCharacter currentCharacter;
    public BattleCharacter target;

    GameObject[] monsterSelecter;
    [SerializeField]
    public GameObject monsterSelecterPrefab;

    /// <summary>
    /// スキルの各項目用
    /// </summary>
    public struct SkillButton
    {
        private SingltonSkillManager.SkillInfo skillInfo;
        private GameObject buttonObject;

        public SkillButton(SingltonSkillManager.SkillInfo info, GameObject button)
        {
            skillInfo = info;
            buttonObject = button;
        }

        public string SkillID
        {
            get
            {
                return skillInfo.ID;
            }
        }

        public SingltonSkillManager.SkillInfo SkillInfo
        {
            get
            {
                return skillInfo;
            }
            set
            {
                skillInfo = value;
            }
        }

        public GameObject setObject
        {
            set
            {
                buttonObject = value;
            }
        }

        public Button getButton
        {
            get
            {
                return buttonObject.GetComponent<Button>();
            }
        }
    }

    /// <summary>
    /// スキルの各項目用
    /// </summary>
    public struct ItemButton
    {
        private SingltonItemManager.ItemList itemInfo;
        private int itemNumber;
        private GameObject buttonObject;

        public ItemButton(SingltonItemManager.ItemList info,int number, GameObject button)
        {
            itemInfo = info;
            itemNumber = number;
            buttonObject = button;
        }

        public string ItemID
        {
            get
            {
                return itemInfo.id;
            }
        }

        public SingltonItemManager.ItemList ItemInfo
        {
            get
            {
                return itemInfo;
            }
            set
            {
                itemInfo = value;
            }
        }

        public GameObject setObject
        {
            set
            {
                buttonObject = value;
            }
        }

        public Button getButton
        {
            get
            {
                return buttonObject.GetComponent<Button>();
            }
        }
    }

    public List<SkillButton> skillButtonList;

    /// <summary>
    /// スキルウィンドウ
    /// </summary>
    [SerializeField]
    GameObject skillWindow;
    public GameObject SkillWindow
    {
        get
        {
            return skillWindow;
        }
    }

    /// <summary>
    /// スキルの説明画面
    /// </summary>
    [SerializeField]
    GameObject skillDetail;
    [SerializeField]
    public GameObject skillDetailText;
    public GameObject SkillDetail
    {
        get
        {
            return skillDetail;
        }

    }

    [SerializeField]
    public GameObject windowLine_Skill;
    [SerializeField]
    public GameObject windowLine_Item;

    /// <summary>
    /// スキルの各項目のPrefab
    /// </summary>
    [SerializeField]
    public GameObject skillPrefab;

    [SerializeField]
    public GameObject itemPrefab;

    public List<ItemButton> itemButtonList;

    [SerializeField]
    GameObject itemWindow;
    public GameObject ItemWindow
    {
        get
        {
            return itemWindow;
        }
    }

    [SerializeField]
    GameObject itemDetail;
    public GameObject ItemDetail
    {
        get
        {
            return itemDetail;
        }
    }

    /// <summary>
    /// 一時的ID格納
    /// </summary>
    string tempId;

    bool isSkillScopeAll;

    /// <summary>
    /// プレイヤーのHPバー
    /// </summary>
    Slider[] playerHP;
    

    /// <summary>
    /// プレイヤーのMPバー
    /// </summary>
    Slider[] playerMP;

    [SerializeField]
    private GameObject[] BackCharacters = new GameObject[2];
    
    [SerializeField]
    Text turnText;
    static private string currentState;

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

    public GameObject beforeGrid;
    public List<Button> beforeSelect;
    Vector3 defScale;

    // Use this for initialization
    void Start()
    {
        selectMode = SelectMode.None;
        characters = battleController.Characters;

        foreach (var character in characters) {
            character.setBattleUI(this);
        }

        skillButtonList = new List<SkillButton>();
        itemButtonList = new List<ItemButton>();

        tempId = "none";
        isSkillScopeAll = false;
        beforeSelect = new List<Button>();


        players = battleController.Players;
        monsters = battleController.Monsters;

        playerHP = new Slider[players.Length];
        playerMP = new Slider[players.Length];
        for(int i = 0; i < players.Length; i++) {
            if (!players[i].frontMember) continue;
            playerHP[i] = players[i].transform.Find("HPBar").GetComponent<Slider>();
            playerHP[i].maxValue = players[i].Hp;
            playerHP[i].enabled = false;
            playerMP[i] = players[i].transform.Find("MPBar").GetComponent<Slider>();
            playerMP[i].maxValue = players[i].Mp;
            playerMP[i].enabled = false;
        }

        foreach (var n in monsters) {
            var button = n.GetComponent<Button>();

            button.OnClickAsObservable()
                .Where(_ => selectMode == SelectMode.Monster && !isSkillScopeAll)
                .Subscribe(_ => {
                    target = n;
                    UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);

                    battleController.audioManager.AttackSE();
                    LeanTween.alpha(target.GetComponent<RectTransform>(), 1.0f, 0.3f).setFrom(0.0f).setLoopCount(3).setLoopType(LeanTweenType.pingPong).setOnComplete(() => {
                        playerAttack(tempId);
                    });
                });

            button.OnClickAsObservable()
                .Where(_ => selectMode == SelectMode.Monster && isSkillScopeAll)
                .Subscribe(_ => {
                    UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);

                    battleController.audioManager.AttackSE();
                    for (int i = 0; i < monsters.Length; ++i) {
                        LeanTween.alpha(monsters[i].GetComponent<RectTransform>(), 1.0f, 0.3f).setFrom(0.0f).setLoopCount(3).setLoopType(LeanTweenType.pingPong);

                    }
                    currentCharacter.GetComponent<BattlePlayer>().attackAction(monsters, tempId);
                    ClearSelecter();
                    isSkillScopeAll = false;
                    selectMode = SelectMode.Behaviour;
                });

            defScale = Vector3.zero;
        }

        foreach (var n in players) {
            if (!n.frontMember) continue;

            var button = n.GetComponent<Button>();

            button.OnClickAsObservable()
                .Where(_ => selectMode == SelectMode.Supporter && !isSkillScopeAll)
                .Subscribe(_ => {
                    target = n;

                    UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);

                    battleController.audioManager.AttackSE();
                    LeanTween.alpha(target.GetComponent<RectTransform>(), 1.0f, 0.3f).setFrom(0.0f).setLoopCount(3).setLoopType(LeanTweenType.pingPong).setOnComplete(() => {
                        playerAttack(tempId);
                    });
                });

            button.OnClickAsObservable()
                .Where(_ => selectMode == SelectMode.Supporter && isSkillScopeAll)
                .Subscribe(_ => {
                    UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);

                    battleController.audioManager.AttackSE();
                    for (int i = 0; i < monsters.Length; ++i) {
                        LeanTween.alpha(players[i].GetComponent<RectTransform>(), 1.0f, 0.3f).setFrom(0.0f).setLoopCount(3).setLoopType(LeanTweenType.pingPong);

                    }
                    currentCharacter.GetComponent<BattlePlayer>().attackAction(players, tempId);
                    ClearSelecter();
                    isSkillScopeAll = false;
                    selectMode = SelectMode.Behaviour;
                });
        }

        InitializeGrids();
    }

    void InitializeGrids()
    {
        NotActiveButton(SkillWindow);
        NotActiveButton(battleController.monsterZone);
        NotActiveButton(battleController.playerGrid);
        NotActiveButton(itemWindow);
        skillWindow.SetActive(false);
        windowLine_Skill.SetActive(false);
        skillDetail.SetActive(false);
        itemWindow.SetActive(false);
        windowLine_Item.SetActive(false);
        itemDetail.SetActive(false);
    }

    public void SkillMode()
    {
        beforeGrid = skillWindow;

        foreach (var n in skillButtonList) {
            var button = n.getButton;

            button.OnClickAsObservable()
                .Where(_ => selectMode == SelectMode.Skill)
                .Where(_ => currentCharacter.CurrentMp - n.SkillInfo.MP >= 0)
                .Subscribe(_ => {
                    beforeGrid = skillWindow;
                    beforeSelect.Add(button);
                    UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
                    currentCharacter.GetComponent<BattlePlayer>().playerSelect.DeSelect();
                    DecisionScope(n);
                    DecisionTarget(n.SkillInfo.myTarget);

                });
        }
    }

    public void ItemMode()
    {
        beforeGrid = itemWindow;

        foreach (var n in itemButtonList) {
            var button = n.getButton;

            button.OnClickAsObservable()
                .Where(_ => selectMode == SelectMode.Item)
                .Subscribe(_ => {
                    beforeGrid = itemWindow;
                    beforeSelect.Add(button);
                    UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
                    currentCharacter.GetComponent<BattlePlayer>().playerSelect.DeSelect();
                    DecisionScope(n);
                    DecisionTarget(n.ItemInfo.target);

                    cutNumber(n.ItemInfo);
                });
        }
    }

    // Update is called once per frame
    void Update()
    {
        ChangeBar();
        currentCharacter = battleController.CurrentActionCharacter;

        if(Input.GetKeyDown(KeyCode.Backspace) && selectMode == SelectMode.Skill) {
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
            battleController.combatGrid.SetActive(true);
            BattleUI.NotActiveButton(skillWindow);
            BattleUI.ActiveButton(battleController.combatGrid,beforeSelect[beforeSelect.Count - 1].gameObject);
            beforeSelect.RemoveAt(beforeSelect.Count - 1);
            ClearSelecter();
            skillWindow.SetActive(false);
            windowLine_Skill.SetActive(false);
            skillDetail.SetActive(false);

            selectMode = SelectMode.Behaviour;
        }
        if (Input.GetKeyDown(KeyCode.Backspace) && selectMode == SelectMode.Item) {
            battleController.combatGrid.SetActive(true);
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
            BattleUI.NotActiveButton(itemWindow);
            BattleUI.ActiveButton(battleController.combatGrid, beforeSelect[beforeSelect.Count - 1].gameObject);
            beforeSelect.RemoveAt(beforeSelect.Count - 1);
            ClearSelecter();
            itemWindow.SetActive(false);
            windowLine_Item.SetActive(false);
            itemDetail.SetActive(false);

            selectMode = SelectMode.Behaviour;
        }
        if (Input.GetKeyDown(KeyCode.Backspace) && selectMode == SelectMode.Monster) {
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
            BattleUI.NotActiveButton(battleController.monsterZone);
            ClearSelecter();
            if(beforeGrid == skillWindow) {
                skillWindow.SetActive(true);
                windowLine_Skill.SetActive(true);
                skillDetail.SetActive(true);
                selectMode = SelectMode.Skill;
            }
            else {
                battleController.combatGrid.SetActive(true);
                selectMode = SelectMode.Behaviour;
            }

            ClearSelecter();
            isSkillScopeAll = false;
            currentCharacter.GetComponent<BattlePlayer>().playerSelect.Select();
            BattleUI.ActiveButton(beforeGrid, beforeSelect[beforeSelect.Count - 1].gameObject);
            beforeSelect.RemoveAt(beforeSelect.Count - 1);

        }
        if(Input.GetKeyDown(KeyCode.Backspace) && selectMode == SelectMode.Supporter) {
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
            BattleUI.NotActiveButton(battleController.playerGrid);
            if (beforeGrid == skillWindow) {
                skillWindow.SetActive(true);
                windowLine_Skill.SetActive(true);
                skillDetail.SetActive(true);
                selectMode = SelectMode.Skill;
            }
            else {
                battleController.combatGrid.SetActive(true);
                selectMode = SelectMode.Behaviour;
            }

            ClearSelecter();
            isSkillScopeAll = false;
            currentCharacter.GetComponent<BattlePlayer>().playerSelect.Select();
            BattleUI.ActiveButton(beforeGrid,beforeSelect[beforeSelect.Count - 1].gameObject);
            beforeSelect.RemoveAt(beforeSelect.Count - 1);
        }

        if (currentState == null) return;
        turnText.text = currentState;
    }

    /// <summary>
    /// スキル範囲毎の処理
    /// </summary>
    void DecisionScope(SkillButton n)
    {
        if (n.SkillInfo.myScope == SingltonSkillManager.Scope.OverAll) {
            NotActiveButton(skillWindow);
            skillWindow.SetActive(false);
            windowLine_Skill.SetActive(false);
            skillDetail.SetActive(false);
            

            monsterSelecter = new GameObject[monsters.Length];
            isSkillScopeAll = true;

            if(n.SkillInfo.myTarget == SingltonSkillManager.Target.Supporter) {
                foreach (var p in players) {
                    p.gameObject.GetComponent<PlayerSelect>().enabled = false;
                    BattlePlayer player = p.GetComponent<BattlePlayer>();

                    defScale = player.img.transform.localScale;
                    player.img.glowSize = 10;
                    player.img.transform.localScale = player.img.transform.localScale * 1.2f;
                }
                
            }else if(n.SkillInfo.myTarget == SingltonSkillManager.Target.Enemy) {
                //モンスターセレクターを全モンスター上に表示
                for (int i = 0; i < monsters.Length; ++i) {
                    if (monsters[i].transform.childCount >= 1) continue;
                    monsterSelecter[i] = GameObject.Instantiate(monsterSelecterPrefab, monsters[i].transform);
                    monsterSelecter[i].transform.position = monsters[i].gameObject.transform.position + Vector3.up * 180.0f;
                }
            }
        }
        else if (n.SkillInfo.myScope == SingltonSkillManager.Scope.Simplex) {
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
            NotActiveButton(skillWindow);
            skillWindow.SetActive(false);
            windowLine_Skill.SetActive(false);
            skillDetail.SetActive(false);

            tempId = n.SkillID;
        }
    }

    void DecisionScope(ItemButton n)
    {
        if (n.ItemInfo.scope == SingltonSkillManager.Scope.OverAll) {
            NotActiveButton(itemWindow);
            itemWindow.SetActive(false);
            windowLine_Item.SetActive(false);
            itemDetail.SetActive(false);


            monsterSelecter = new GameObject[monsters.Length];
            isSkillScopeAll = true;

            if (n.ItemInfo.target == SingltonSkillManager.Target.Supporter) {
                foreach (var p in players) {
                    p.gameObject.GetComponent<PlayerSelect>().enabled = false;
                    BattlePlayer player = p.GetComponent<BattlePlayer>();

                    defScale = player.img.transform.localScale;
                    player.img.glowSize = 10;
                    player.img.transform.localScale = player.img.transform.localScale * 1.2f;
                }

            }
            else if (n.ItemInfo.target == SingltonSkillManager.Target.Enemy) {
                //モンスターセレクターを全モンスター上に表示
                for (int i = 0; i < monsters.Length; ++i) {
                    if (monsters[i].transform.childCount >= 1) continue;
                    monsterSelecter[i] = GameObject.Instantiate(monsterSelecterPrefab, monsters[i].transform);
                    monsterSelecter[i].transform.position = monsters[i].gameObject.transform.position + Vector3.up * 180.0f;
                }
            }
        }
        else if (n.ItemInfo.scope == SingltonSkillManager.Scope.Simplex) {
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
            NotActiveButton(itemWindow);
            itemWindow.SetActive(false);
            windowLine_Item.SetActive(false);
            itemDetail.SetActive(false);

            tempId = n.ItemID;
        }
    }

    /// <summary>
    /// スキル対象毎の処理
    /// </summary>
    /// <param name="n"></param>
    void DecisionTarget(SingltonSkillManager.Target target)
    {
        if (target == SingltonSkillManager.Target.Enemy) {
            ActiveButton(battleController.monsterZone);
            selectMode = SelectMode.Monster;
        }
        else if (target == SingltonSkillManager.Target.Supporter) {
            ActiveButton(battleController.playerGrid);
            selectMode = SelectMode.Supporter;
        }
        else if(target == SingltonSkillManager.Target.MySelf) {
            OnlyActiveButton(currentCharacter.gameObject);
        }
    }

    void cutNumber(SingltonItemManager.ItemList item)
    {
        List<string> tempList = new List<string>(battleController.testList.itemList.Keys);
        foreach (var i in tempList) {
            if(item.id == i) {
                battleController.testList.itemList[i] -= 1;
            }
        }
    }

    public void ClearTempSkillID()
    {
        tempId = "none";
    }

    public void SetSkill(SingltonSkillManager.SkillInfo info)
    {
        GameObject tempObject = Instantiate(skillPrefab, skillWindow.transform);
        tempObject.GetComponentInChildren<Text>().text = info.skill;
        SkillButton temp = new SkillButton(info, tempObject);
        skillButtonList.Add(temp);
    }

    public void SetItem(Dictionary<string,int> info)
    {
        foreach(string id in info.Keys) {
            foreach(var item in battleController.gameManager.ItemManager.CDItem) {
                if(id == item.id) {
                    GameObject tempObject = Instantiate(itemPrefab, itemWindow.transform);
                    foreach(Transform child in tempObject.transform) {
                        if (child.name == "itemName") child.GetComponent<Text>().text = item.name;
                        if (child.name == "itemNumber") child.GetComponent<Text>().text = info[id].ToString();
                    }

                    ItemButton temp = new ItemButton(item, info[id], tempObject);
                    itemButtonList.Add(temp);
                }
            }
        }

    }

 
    string DisplayStatus(BattleCharacter chara)
    {
        string str = chara.Param.Name + " : " + chara.CurrentHp + "/" + chara.Hp + "  " + chara.CurrentMp + "/" + chara.Mp + "  " + chara.Atk + "  " + chara.Def
                     + "  " + chara.MAtk + "   " + chara.MDef + "  " + chara.Agl + "  " + chara.Luc;

        return str;
    }

    public void ClearWindow()
    {
        if (skillButtonList.Count != 0) {
            skillButtonList.Clear();
            foreach (Transform n in skillWindow.transform) {
                GameObject.Destroy(n.gameObject);
            }
        }

        if (itemButtonList.Count != 0) {
            itemButtonList.Clear();
            foreach (Transform n in itemWindow.transform) {
                GameObject.Destroy(n.gameObject);
            }
        }
    }

    void playerAttack(string skillId)
    {
        currentCharacter.GetComponent<BattlePlayer>().attackAction(new BattleCharacter[] { target }, skillId);
    }

    public void ClearSelecter()
    {
        if (monsterSelecter != null) {
            for (int i = 0; i < monsterSelecter.Length; i++)
                DestroyObject(monsterSelecter[i]);
        }

        if(defScale != Vector3.zero) {
            foreach (var p in players) {
                p.gameObject.GetComponent<PlayerSelect>().enabled = true;
                BattlePlayer player = p.GetComponent<BattlePlayer>();

                player.img.transform.localScale = defScale;
                player.img.glowSize = 0;
            }
        }

        defScale = Vector3.zero;
    }

    static public void DontSelectableDeadCharacter(GameObject grid)
    {
        BattleCharacter chara;
        for (var i = 0; i < grid.transform.childCount; i++) {
            chara = grid.transform.GetChild(i).GetComponent<BattleCharacter>();

            if (chara.IsDead) {
                grid.transform.GetChild(i).GetComponent<Button>().enabled = false;
            }
        }
    }

    /// <summary>
    /// ターン表記（モンスター)
    /// </summary>
    /// <param name="name"> モンスター名</param>
    static public void DisplayMonsterTurn(string name)
    {
        if (turnDisplay == null) turnDisplay = GameObject.Find("Turn");
        turnDisplay.SetActive(true);
        currentState = name + "のターン";
    }

    /// <summary>
    /// ターン表記（プレイヤー)
    /// </summary>
    /// <param name="name">プレイヤー名</param>
    static public void DisplayPlayerTurn(string name)
    {
        if (turnDisplay == null) turnDisplay = GameObject.Find("Turn");
        turnDisplay.SetActive(false);
    }

    public void BattleEndDisplay(int totalExp)
    {
        if (turnDisplay == null) turnDisplay = GameObject.Find("Turn");

        if (battleController.DeadType == BattleController.GroupDeadType.AllEnemyDead) {
            turnDisplay.SetActive(true);
            currentState = "敵を殲滅しました";
            Observable.Timer(System.TimeSpan.FromMilliseconds(800.0))
            .Take(1)
            .Subscribe(_ => {
                currentState = totalExp + " の経験値";
            });
        }
        else if (battleController.DeadType == BattleController.GroupDeadType.AllPlayerDead) {
            turnDisplay.SetActive(true);
            currentState = "全滅しました";

        }
    }

    public void LevelUpDisplay(Dictionary<string,int> players)
    {
        turnDisplay.SetActive(true);

        //ToDO : レベルアップ表記
        
        //foreach (var p in players) {
        //    currentState = p.Key + "が Lv" + p.Value + "に上がった";
        //}
    }

    private void ChangeBar()
    {
        for (int i = 0; i < players.Length; i++) {
            if (!players[i].frontMember) continue;

            if (players[i].gameObject.activeSelf != false) {
                playerHP[i].value = players[i].CurrentHp;
                playerHP[i].gameObject.GetComponentInChildren<Text>().text = "HP : " + players[i].CurrentHp + " / " + players[i].Hp;
                playerMP[i].value = players[i].CurrentMp;
                playerMP[i].gameObject.GetComponentInChildren<Text>().text = "MP : " + players[i].CurrentMp + " / " + players[i].Mp;
            }
        }
    }

    static public void ActiveButton(GameObject grid,GameObject targetObj = null)
    {
        for (var i = 0; i < grid.transform.childCount; i++) {
            if (grid.transform.GetChild(i).gameObject.activeInHierarchy) {
                grid.transform.GetChild(i).GetComponent<Button>().enabled = true;
            }
        }

        
        for (var i = 0; i < grid.transform.childCount; ++i) {
            if (grid.transform.GetChild(i).tag == "Dead") continue;

            if(targetObj == null) {
                targetObj = grid.transform.GetChild(i).gameObject;
            }

            if (targetObj.activeInHierarchy) {
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(targetObj);
                break;
            }
            else {
                targetObj = null;
            }
        }
    }

    static public void NotActiveButton(GameObject grid)
    {
        for (var i = 0; i < grid.transform.childCount; i++) {
            grid.transform.GetChild(i).GetComponent<Button>().enabled = false;
        }
    }

    static public void OnlyActiveButton(GameObject targetObj)
    {
        targetObj.GetComponent<Button>().enabled = true;
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(targetObj);
    }
}
