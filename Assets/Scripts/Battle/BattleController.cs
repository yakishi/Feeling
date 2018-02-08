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
    [SerializeField]
    public AudioManager audioManager;

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
    public int MonsterCount {
        get
        {
            return monsterCount;
        }
    }

    BattleCharacter currentActionCharacter;
    public BattleCharacter CurrentActionCharacter {
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
    GameObject[] playerObjects;
    public GameObject[] PlayerObjects {
        get
        {
            return playerObjects;
        }
    }

    BattleCharacter[] monsters;
    public BattleCharacter[] Monsters {
        get
        {
            return monsters;
        }
    }

    int dropGolds = 0;

    /// <summary>
    /// 仮データ用モンスターイメージ
    /// </summary>
    [SerializeField]
    Sprite[] monsterImg = new Sprite[4];

    GroupDeadType groupDeadType = GroupDeadType.None;
    public GroupDeadType DeadType {
        get
        {
            return groupDeadType;
        }
    }

    //仮
    List<SingltonSkillManager.SkillInfo> skillList;
    public List<SingltonSkillManager.SkillInfo> SkillList {
        get
        {
            return skillList;
        }
    }

    public SingltonItemManager.ItemParam testList;

    [SerializeField]
    public CharacterImage charaImgList;

    void Start()
    {
    }

    public void Initialzie()
    {
        monsterZone = GameObject.Find("MonsterZone");

        characters = new List<BattleCharacter>();
        createDamyData();
        foreach (var character in characters) {
            // TODO: ロードIDはプレイヤーとモンスターで今後分ける
            // 仮ID読み込み
            //character.loadData(id++,gameManager);
            if (!character.frontMember) continue;
            character.setBattleController(this);
            character.battleStart();

            character.onEndActionAsObservable()
                .Where(c => character.frontMember)
                .Subscribe(c => {
                        //死亡処理
                        foreach (var p in players) {
                        if (!p.IsDead) continue;
                        p.gameObject.SetActive(false);
                    }

                    foreach (var m in monsters) {
                        if (!m.IsDead) continue;
                        Image image = m.gameObject.GetComponent<Image>();

                        image.color = new Color(252, 252, 252, 0);
                        m.frontMember = false;
                        m.gameObject.tag = "Dead";
                    }

                    groupDeadType = getGroupDeadType();
                    Debug.Log(groupDeadType);
                    if (groupDeadType == GroupDeadType.None) {
                        Observable.NextFrame().Subscribe(_ => {
                            currentActionCharacter = getNextActionCharacter();
                            if (currentActionCharacter == null) {
                                return;
                            }
                            currentActionCharacter.startAction();
                        });
                        return;
                    }
                    battleEnd(groupDeadType);
                })
                .AddTo(character);

        }


        currentActionCharacter = getNextActionCharacter();
        if (!currentActionCharacter.frontMember) return;
        currentActionCharacter.startAction();

        skillList = SingltonSkillManager.Instance.CDSkill;
    }

    /// <summary>
    /// 仮データの作成
    /// </summary>
    void createDamyData()
    {
        int id = 1;
        playerCount = 4;
        players = new BattlePlayer[playerCount];
        playerObjects = new GameObject[4];
        for (int i = 0; i < 4; ++i) {

            playerObjects[i] = Instantiate(battlePlayerPrefab, playerGrid.transform);
            players[i] = playerObjects[i].GetComponent<BattlePlayer>();

            players[i].enabled = false;
        }

        foreach (var player in players) {
            player.loadData("P" + id + "_0", gameManager);
            if (player != null) {
                player.img.sprite = charaImgList.charaImgList[id - 1];
            }
            if (id <= 4) {
                player.frontMember = true;
            }

            id++;
        }

        monsterCount = 3;
        monsters = new BattleMonster[monsterCount];
        for (int i = 0; i < monsters.Length; ++i) {
            var random = Random.Range(0, monsterImg.Length);
            GameObject temp = Instantiate(battleMonsterPrefab, monsterZone.transform, false);

            monsters[i] = temp.GetComponent<BattleMonster>();
            monsters[i].transform.position = new Vector3(monsterPosX(i, monsterCount), monsterZone.transform.position.y, 0);
            monsters[i].ID = random.ToString();
            monsters[i].GetComponent<Image>().sprite = monsterImg[random];
        }
        foreach (var monster in monsters) {
            monster.loadData(monster.ID, gameManager);
            dropGolds += monster.Hp;
        }

        BattleUI.NotActiveButton(monsterZone);

        characters = characters
            .Concat(players)
            .Concat(monsters)
            .ToList();

        for (int i = 0; i < characters.Count; ++i) {
            if (characters[i] == null) continue;
            characters[i].gameObject.name = i.ToString();
        }
        battleUI.Initialize();
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

        foreach (BattlePlayer p in players) {
            p.ChangeFeelingColor(HighestFeel(p.Param));
        }

        var ret = characters
            .Where(character => character.frontMember)
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
            SceneController.sceneTransition(SceneName.SceneNames.azito, 2.0f, SceneController.FadeType.Fade);
            return;
        }

        foreach (var m in monsters) {
            Destroy(m);
        }
        Debug.Log("敵殲滅");

        int totalEXP = 0;
        foreach (var m in monsters) {
            totalEXP += m.Param.TotalExp;
        }

        battleUI.BattleEndDisplay(totalEXP);

        int cnt = 1;
        //bool isLevelUp = false;

        Dictionary<string, int> lvUpPlayerList = new Dictionary<string, int>();
        foreach (var p in gameManager.PlayerList) {
            p.TotalExp += totalEXP;
            lvUpPlayerList.Add(p.Name, p.Lv);
            cnt++;
        }

        SingltonItemManager.Instance.SDItem.possessionGolds += dropGolds;
        battleUI.LevelUpDisplay(lvUpPlayerList);
        SceneController.startFade(fade => {
            Destroy(transform.parent.gameObject);
        }, 1.0f);
    }

    bool IsUpLevel(SingltonPlayerManager.PlayerParameters player, int cnt)
    {
        foreach (var p in gameManager.PlayerManager.GetCsvDataPlayerState) {
            if (p.ID == "P" + cnt + "_" + (player.Lv + 1)) {
                if (p.NeedTotalExp <= player.TotalExp) {
                    player.Lv += 1;

                    player.STATUS = UpStatus(player, HighestFeel(player));

                    return true;
                }
            }
        }

        return false;
    }

    SingltonSkillManager.Feel HighestFeel(SingltonPlayerManager.PlayerParameters player)
    {
        SingltonSkillManager.Feel tempFeel = SingltonSkillManager.Feel.Ki;
        int value = player.currentFeel[tempFeel];
        foreach (var f in player.currentFeel.Keys) {
            if (player.currentFeel[f] > value) {
                tempFeel = f;
                value = player.currentFeel[f];
            }
        }

        return tempFeel;
    }

    SingltonPlayerManager.Status UpStatus(SingltonPlayerManager.PlayerParameters player, SingltonSkillManager.Feel feel)
    {
        SingltonPlayerManager.Status status = player.STATUS;

        switch (feel) {
            case SingltonSkillManager.Feel.Ki:
                status.Atk = Mathf.FloorToInt(status.Def * 1.2f);
                status.Def = Mathf.FloorToInt(status.Def * 1.5f);
                status.Matk = Mathf.FloorToInt(status.Def * 1.2f);
                status.Mgr = Mathf.FloorToInt(status.Def * 1.2f);
                status.Agl = Mathf.FloorToInt(status.Def * 1.2f);
                status.Luc = Mathf.FloorToInt(status.Def * 1.2f);
                break;
            case SingltonSkillManager.Feel.Do:
                status.Atk = Mathf.FloorToInt(status.Def * 1.5f);
                status.Def = Mathf.FloorToInt(status.Def * 1.2f);
                status.Matk = Mathf.FloorToInt(status.Def * 1.2f);
                status.Mgr = Mathf.FloorToInt(status.Def * 1.2f);
                status.Agl = Mathf.FloorToInt(status.Def * 1.2f);
                status.Luc = Mathf.FloorToInt(status.Def * 1.2f);
                break;
            case SingltonSkillManager.Feel.Ai:
                status.Atk = Mathf.FloorToInt(status.Def * 1.2f);
                status.Def = Mathf.FloorToInt(status.Def * 1.2f);
                status.Matk = Mathf.FloorToInt(status.Def * 1.2f);
                status.Mgr = Mathf.FloorToInt(status.Def * 1.5f);
                status.Agl = Mathf.FloorToInt(status.Def * 1.2f);
                status.Luc = Mathf.FloorToInt(status.Def * 1.2f);
                break;
            case SingltonSkillManager.Feel.Raku:
                status.Atk = Mathf.FloorToInt(status.Def * 1.2f);
                status.Def = Mathf.FloorToInt(status.Def * 1.2f);
                status.Matk = Mathf.FloorToInt(status.Def * 1.2f);
                status.Mgr = Mathf.FloorToInt(status.Def * 1.2f);
                status.Agl = Mathf.FloorToInt(status.Def * 1.2f);
                status.Luc = Mathf.FloorToInt(status.Def * 1.5f);
                break;
            case SingltonSkillManager.Feel.Love:
                status.Atk = Mathf.FloorToInt(status.Def * 1.2f);
                status.Def = Mathf.FloorToInt(status.Def * 1.2f);
                status.Matk = Mathf.FloorToInt(status.Def * 1.5f);
                status.Mgr = Mathf.FloorToInt(status.Def * 1.2f);
                status.Agl = Mathf.FloorToInt(status.Def * 1.2f);
                status.Luc = Mathf.FloorToInt(status.Def * 1.2f);
                break;
            case SingltonSkillManager.Feel.Zou:
                status.Atk = Mathf.FloorToInt(status.Def * 1.2f);
                status.Def = Mathf.FloorToInt(status.Def * 1.2f);
                status.Matk = Mathf.FloorToInt(status.Def * 1.2f);
                status.Mgr = Mathf.FloorToInt(status.Def * 1.2f);
                status.Agl = Mathf.FloorToInt(status.Def * 1.5f);
                status.Luc = Mathf.FloorToInt(status.Def * 1.2f);
                break;
            default:
                break;

        }

        status.HP += 5;
        status.MP += 5;

        return status;
    }
}
