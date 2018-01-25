using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class BattleController : MonoBehaviour
{
    [SerializeField]
    public GameManager gameManager;
    private Transform[] playerIconPos = new Transform[4];
    /// <summary>
    /// モンスターが配置されるグリッド
    /// </summary>
    public GameObject monsterZone;

    /// <summary>
    /// コマンドが配置されるグリッド
    /// </summary>
    [SerializeField]
    public GameObject combatGrid;

    /// <summary>
    /// プレイヤーが配置されるグリッド
    /// </summary>
    [SerializeField]
    public GameObject playerGrid;

    /// <summary>
    /// 味方グループ、敵グループの生存判定
    /// </summary>
    public enum GroupDeadType
    {
        None,
        AllPlayerDead,
        AllEnemyDead
    }


    [SerializeField]
    GameObject battlePlayerPrefab;

    [SerializeField]
    GameObject battleMonsterPrefab;

    [SerializeField]
    BattleUI battleUI;

    int playerCount;
    int monsterCount;
    public int MonsterCount
    {
        get
        {
            return monsterCount;
        }
    }

    BattleCharacter currentActionCharacter;
    public BattleCharacter CurrentActionCharacter
    {
        get
        {
            return currentActionCharacter;
        }
    }

    List<BattleCharacter> characters;
    public List<BattleCharacter> Characters {
        get
        {
            return characters;
        }
    }

    BattleCharacter[] players;
    public BattleCharacter[] Players {
        get
        {
            return players;
        }
    }
    BattleCharacter[] monsters;
    public BattleCharacter[] Monsters {
        get
        {
            return monsters;
        }
    }
    /// <summary>
    /// 仮データ用モンスターイメージ
    /// </summary>
    [SerializeField]
    Sprite[] monsterImg = new Sprite[4];

    GroupDeadType groupDeadType = GroupDeadType.None;
    public GroupDeadType DeadType
    {
        get
        {
            return groupDeadType;
        }
    }

    //仮
    List<SingltonSkillManager.SkillInfo> skillList;
    public List<SingltonSkillManager.SkillInfo> SkillList
    {
        get
        {
            return skillList;
        }
    }

    public SingltonItemManager.ItemParam testList;

    void Start()
    {
        //for(int i = 0; i < playerIconPos.Length; i++) {
        //    playerIconPos[i] = GameObject.Find("PlayerIcon" + (i + 1)).transform;
        //}
        monsterZone = GameObject.Find("MonsterZone");

        characters = new List<BattleCharacter>();
        createDamyData();
        foreach (var character in characters) {
            // TODO: ロードIDはプレイヤーとモンスターで今後分ける
            // 仮ID読み込み
            //character.loadData(id++,gameManager);
            character.setBattleController(this);
            character.battleStart();

            character.onEndActionAsObservable()
                .Subscribe(c => {
                    //死亡処理
                    foreach(var p in players) {
                        if (!p.IsDead) continue;
                        p.gameObject.SetActive(false);
                    }

                    foreach (var m in monsters) {
                        if (!m.IsDead) continue;
                        m.gameObject.SetActive(false);
                    }

                    groupDeadType = getGroupDeadType();
                    if (groupDeadType == GroupDeadType.None) {
                        Observable.NextFrame().Subscribe(_ => {
                            currentActionCharacter = getNextActionCharacter();
                            currentActionCharacter.startAction();
                        });
                        return;
                    }
                    battleEnd(groupDeadType);
                })
                .AddTo(character);

        }

        currentActionCharacter = getNextActionCharacter();
        currentActionCharacter.startAction();

        skillList = SingltonSkillManager.Instance.CDSkill;
        testList = new SingltonItemManager.ItemParam();
        testList.itemList = new Dictionary<string, int>();
        testList.itemList.Add("I0", 5);
        testList.itemList.Add("I1", 10);
        testList.itemList.Add("I2", 3);
        testList.itemList.Add("I3", 4);
        testList.itemList.Add("I4", 10);
        testList.itemList.Add("I5", 6);
    }

    /// <summary>
    /// 仮データの作成
    /// </summary>
    void createDamyData()
    {
        int id = 1;
        playerCount = 4;
        players = new BattlePlayer[playerCount];
        for (int i = 0; i < players.Length; ++i) {
            players[i] = Instantiate(battlePlayerPrefab,playerGrid.transform).GetComponent<BattlePlayer>();
            
            players[i].enabled = false;
        }
        foreach(var player in players) {
            player.loadData("P" + id + "_0",gameManager);
            id++;
        }

        monsterCount = 3;
        monsters = new BattleMonster[monsterCount];
        for (int i = 0; i < monsters.Length; ++i) {
            var random = Random.Range(0, monsterImg.Length);
            GameObject temp = Instantiate(battleMonsterPrefab, monsterZone.transform, false);

            monsters[i] = temp.GetComponent<BattleMonster>();
            monsters[i].transform.position = new Vector3(monsterPosX(i, monsterCount), monsterZone.transform.position.y, 0);
            monsters[i].GetComponent<Image>().sprite = monsterImg[random];
        }
        foreach (var monster in monsters) {
            monster.loadData("0",gameManager,true);
        }

        BattleUI.NotActiveButton(monsterZone);

        characters = characters
            .Concat(players)
            .Concat(monsters)
            .ToList();

        for (int i = 0; i < characters.Count; ++i) {
            characters[i].gameObject.name = i.ToString();
        }
    }

    /// <summary>
    /// モンスターの均等配置
    /// </summary>
    /// <param name="i">何番目のモンスターか</param>
    /// <param name="monsterCount">モンスターの最大数</param>
    /// <returns></returns>
    float monsterPosX(float i, int monsterCount)
    {
        float posX;
        float zoneWidth = monsterZone.transform.position.x * 2; 
        posX = (zoneWidth / monsterCount) / 2;

        return posX + (zoneWidth / monsterCount) * i;
    }
    /// <summary>
    /// GameDataから読み込む
    /// </summary>
    void loadData()
    {

    }

    /// <summary>
    /// バトル終了の種類を取得
    /// </summary>
    /// <returns></returns>
    GroupDeadType getGroupDeadType()
    {
        if (players.All(character => character.IsDead)) {
            return GroupDeadType.AllPlayerDead;
        }
        if (monsters.All(character => character.IsDead)) {
            return GroupDeadType.AllEnemyDead;
        }

        return GroupDeadType.None;
    }

    /// <summary>
    /// 次に行動するキャラクターを取得
    /// </summary>
    /// <returns>行動するキャラクター</returns>
    BattleCharacter getNextActionCharacter()
    {
        if (characters.All(character => !character.CanAction)) {
            nextTurn();
        }

        battleUI.ClearTempSkillID();

        var ret = characters
            .Where(character => character.CanAction)
            .MaxElement(character => { return character.Agl; });
        return ret;
    }

    /// <summary>
    /// 次のターンにする
    /// </summary>
    void nextTurn()
    {

        foreach (var character in characters) {
            character.beginTurn();
        }
    }

    /// <summary>
    /// バトル終了
    /// </summary>
    void battleEnd(GroupDeadType deadType)
    {
        if (deadType == GroupDeadType.AllPlayerDead) {
            Debug.Log("プレイヤー全滅");
            return;
        }

        Debug.Log("敵殲滅");
    }
}
