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

    /// <summary>
    /// スキルの各項目のPrefab
    /// </summary>
    [SerializeField]
    GameObject skillItemPrefab;

    /// <summary>
    /// 一時的スキルID格納
    /// </summary>
    string tempSkillid;

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
        NotActiveButton(SkillWindow);
        ActiveButton(battleController.combatGrid);
        NotActiveButton(battleController.monsterZone);
        NotActiveButton(battleController.playerGrid);
        skillWindow.SetActive(false);
        skillDetail.SetActive(false);
        tempSkillid = "none";
        isSkillScopeAll = false;
        beforeSelect = new List<Button>();


        players = battleController.Players;
        monsters = battleController.Monsters;

        playerHP = new Slider[players.Length];
        playerMP = new Slider[players.Length];
        for(int i = 0; i < players.Length; i++) {
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

                    LeanTween.alpha(target.GetComponent<RectTransform>(), 1.0f, 0.3f).setFrom(0.0f).setLoopCount(3).setLoopType(LeanTweenType.pingPong).setOnComplete(() => {
                        playerAttack(tempSkillid);
                    });
                });

            button.OnClickAsObservable()
                .Where(_ => selectMode == SelectMode.Monster && isSkillScopeAll)
                .Subscribe(_ => {
                    UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);

                    for (int i = 0; i < monsters.Length; ++i) {
                        LeanTween.alpha(monsters[i].GetComponent<RectTransform>(), 1.0f, 0.3f).setFrom(0.0f).setLoopCount(3).setLoopType(LeanTweenType.pingPong);

                    }
                    currentCharacter.GetComponent<BattlePlayer>().attackAction(monsters, tempSkillid);
                    ClearSelecter();
                    isSkillScopeAll = false;
                    selectMode = SelectMode.Behaviour;
                });

            defScale = Vector3.zero;
        }

        foreach (var n in players) {
            var button = n.GetComponent<Button>();

            button.OnClickAsObservable()
                .Where(_ => selectMode == SelectMode.Supporter && !isSkillScopeAll)
                .Subscribe(_ => {
                    target = n;

                    UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);

                    LeanTween.alpha(target.GetComponent<RectTransform>(), 1.0f, 0.3f).setFrom(0.0f).setLoopCount(3).setLoopType(LeanTweenType.pingPong).setOnComplete(() => {
                        playerAttack(tempSkillid);
                    });
                });

            button.OnClickAsObservable()
                .Where(_ => selectMode == SelectMode.Supporter && isSkillScopeAll)
                .Subscribe(_ => {
                    UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);

                    for (int i = 0; i < monsters.Length; ++i) {
                        LeanTween.alpha(players[i].GetComponent<RectTransform>(), 1.0f, 0.3f).setFrom(0.0f).setLoopCount(3).setLoopType(LeanTweenType.pingPong);

                    }
                    currentCharacter.GetComponent<BattlePlayer>().attackAction(players, tempSkillid);
                    ClearSelecter();
                    isSkillScopeAll = false;
                    selectMode = SelectMode.Behaviour;
                });
        }

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
                    DecisionTarget(n);

                });
        }
    }

    // Update is called once per frame
    void Update()
    {
        ChangeBar();
        currentCharacter = battleController.CurrentActionCharacter;

        EndDisplay();

        if(Input.GetKeyDown(KeyCode.Escape) && selectMode == SelectMode.Skill) {
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
            BattleUI.NotActiveButton(skillWindow);
            BattleUI.ActiveButton(battleController.combatGrid,beforeSelect[beforeSelect.Count - 1].gameObject);
            beforeSelect.RemoveAt(beforeSelect.Count - 1);
            ClearSelecter();
            skillWindow.SetActive(false);
            skillDetail.SetActive(false);

            selectMode = SelectMode.Behaviour;
        }
        if (Input.GetKeyDown(KeyCode.Escape) && selectMode == SelectMode.Monster) {
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
            BattleUI.NotActiveButton(battleController.monsterZone);
            ClearSelecter();
            if(beforeGrid == skillWindow) {
                skillWindow.SetActive(true);
                skillDetail.SetActive(true);
                selectMode = SelectMode.Skill;
            }
            else {
                selectMode = SelectMode.Behaviour;
            }

            ClearSelecter();
            currentCharacter.GetComponent<BattlePlayer>().playerSelect.Select();
            BattleUI.ActiveButton(beforeGrid, beforeSelect[beforeSelect.Count - 1].gameObject);
            beforeSelect.RemoveAt(beforeSelect.Count - 1);

        }
        if(Input.GetKeyDown(KeyCode.Escape) && selectMode == SelectMode.Supporter) {
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
            BattleUI.NotActiveButton(battleController.playerGrid);
            if (beforeGrid == skillWindow) {
                skillWindow.SetActive(true);
                skillDetail.SetActive(true);
                selectMode = SelectMode.Skill;
            }
            else {
                selectMode = SelectMode.Behaviour;
            }

            ClearSelecter();
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
            skillDetail.SetActive(false);

            tempSkillid = n.SkillID;
        }
    }

    /// <summary>
    /// スキル対象毎の処理
    /// </summary>
    /// <param name="n"></param>
    void DecisionTarget(SkillButton n)
    {
        if (n.SkillInfo.myTarget == SingltonSkillManager.Target.Enemy) {
            ActiveButton(battleController.monsterZone);
            selectMode = SelectMode.Monster;
        }
        else if (n.SkillInfo.myTarget == SingltonSkillManager.Target.Supporter) {
            ActiveButton(battleController.playerGrid);
            selectMode = SelectMode.Supporter;
        }
        else if(n.SkillInfo.myTarget == SingltonSkillManager.Target.MySelf) {
            OnlyActiveButton(currentCharacter.gameObject);
        }
    }

    public void ClearTempSkillID()
    {
        tempSkillid = "none";
    }

    public void SetSkill(SingltonSkillManager.SkillInfo info)
    {
        GameObject tempObject = Instantiate(skillItemPrefab, skillWindow.transform);
        tempObject.GetComponentInChildren<Text>().text = info.skill;
        SkillButton temp = new SkillButton(info, tempObject);
        skillButtonList.Add(temp);
    }

    public void ClearSkillWindow()
    {
        if (skillButtonList.Count != 0) {
            skillButtonList.Clear();
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

    private void EndDisplay()
    {
        if (turnDisplay == null) turnDisplay = GameObject.Find("Turn");

        if (battleController.DeadType == BattleController.GroupDeadType.AllEnemyDead) {
            turnDisplay.SetActive(true);
            currentState = "敵を殲滅しました";
        }
        else if (battleController.DeadType == BattleController.GroupDeadType.AllPlayerDead) {
            turnDisplay.SetActive(true);
            currentState = "全滅しました";

        }
    }

    private void ChangeBar()
    {
        for (int i = 0; i < players.Length; i++) {
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
            grid.transform.GetChild(i).GetComponent<Button>().enabled = true;
        }

        
        for (var i = 0; i < grid.transform.childCount; i++) {
            if(targetObj == null) {
                targetObj = grid.transform.GetChild(i).gameObject;
            }

            if (grid.transform.GetChild(i).gameObject.activeSelf == true) {
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(targetObj);
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

    static public void OnlyActiveButton(GameObject targetObj)
    {
        targetObj.GetComponent<Button>().enabled = true;
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(targetObj);
    }
}
