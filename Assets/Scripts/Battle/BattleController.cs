using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;

public class BattleController : MonoBehaviour
{
    /// <summary>
    /// 味方グループ、敵グループの生存判定
    /// </summary>
    enum GroupDeadType
    {
        None,
        AllPlayerDead,
        AllEnemyDead
    }


    [SerializeField]
    GameObject battlePlayerPrefab;

    [SerializeField]
    GameObject battleMonsterPrefab;

    int playerCount;
    int monsterCount;

    BattleCharacter currentActionCharacter;

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

    void Start()
    {
        characters = new List<BattleCharacter>();
        createDamyData();

        int id = 0;
        foreach (var character in characters) {
            // TODO: ロードIDはプレイヤーとモンスターで今後分ける
            // 仮ID読み込み
            character.loadData(id++);
            character.setBattleController(this);
            character.battleStart();

            character.onEndActionAsObservable()
                .Subscribe(c => {
                    var groupDeadType = getGroupDeadType();
                    if (groupDeadType == GroupDeadType.None) {
                        Observable.NextFrame().Subscribe(_ => {
                            currentActionCharacter = getNextActionCharacter();
                            currentActionCharacter.startAction();
                            Debug.Log(currentActionCharacter.name);
                        });
                        return;
                    }
                    battleEnd(groupDeadType);
                })
                .AddTo(character);

        }

        currentActionCharacter = getNextActionCharacter();
        currentActionCharacter.startAction();
    }

    /// <summary>
    /// 仮データの作成
    /// </summary>
    void createDamyData()
    {
        playerCount = 2;
        players = new BattlePlayer[playerCount];
        for (int i = 0; i < players.Length; ++i) {
            players[i] = Instantiate(battlePlayerPrefab, transform).GetComponent<BattlePlayer>();
        }
        foreach (var player in players) {
            player.loadData(0);
        }

        for (int i = 0; i < playerCount; ++i) {
            players[i].gameObject.transform.position = Vector2.left * 20.0f;
        }

        monsterCount = 2;
        monsters = new BattleMonster[monsterCount];
        for (int i = 0; i < monsters.Length; ++i) {
            monsters[i] = Instantiate(battleMonsterPrefab, transform).GetComponent<BattleMonster>();
        }
        foreach (var monster in monsters) {
            monster.loadData(0);
        }

        characters = characters
            .Concat(players)
            .Concat(monsters)
            .ToList();

        for (int i = 0; i < characters.Count; ++i) {
            characters[i].gameObject.name = i.ToString();
        }
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

        foreach(var c in characters) {
            Debug.Log(c.CurrentHp);
        }

        var ret = characters
            .Where(character => character.CanAction)
            .MaxElement(character => { return character.Agl; });
        Debug.Log(ret.name);
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
